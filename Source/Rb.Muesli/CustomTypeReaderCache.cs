using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;

namespace Rb.Muesli
{
    public delegate object CustomReaderDelegate( IInput input );

    public class CustomTypeReaderCache
    {
        public static CustomTypeReaderCache Instance
        {
            get { return ms_Instance; }
        }

        public CustomReaderDelegate GetReader( Type type )
        {
            CustomReaderDelegate Reader;
            if ( !m_ReaderMap.TryGetValue( type, out Reader ) )
            {
                Reader = CreateReader( type );
                m_ReaderMap[ type ] = Reader;
            }
            return Reader;
        }
		
        private readonly static CustomTypeReaderCache ms_Instance = new CustomTypeReaderCache( );

		private readonly static MethodInfo IInput_GetContext						= typeof( IInput ).GetProperty( "Context" ).GetGetMethod( );
        private readonly static MethodInfo IInput_GetTypeReader						= typeof( IInput ).GetProperty( "TypeReader" ).GetGetMethod( );
		private readonly static MethodInfo IInput_ReadSerializationInfo				= typeof( IInput ).GetMethod( "ReadSerializationInfo" );
        private readonly static MethodInfo ITypeReader_Read              			= typeof( ITypeReader ).GetMethod( "Read" );
		private readonly static MethodInfo Type_TypeFromHandle						= typeof( Type ).GetMethod( "GetTypeFromHandle" );
		private readonly static MethodInfo FormatterServices_GetUninitializedObject	= typeof( FormatterServices ).GetMethod( "GetUninitializedObject" );
		private readonly static MethodInfo This_LoadGenericField					= typeof( CustomTypeReaderCache ).GetMethod( "LoadGenericField" );
		
        private readonly Dictionary< Type, CustomReaderDelegate > m_ReaderMap = new Dictionary< Type, CustomReaderDelegate >( );

        private static CustomReaderDelegate CreateSerializableReader( Type type )
        {
            ConstructorInfo constructor = type.GetConstructor( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof( SerializationInfo ), typeof( StreamingContext ) }, null );
            if ( constructor == null )
            {
                throw new ArgumentException( string.Format( "Type \"{0}\" had no serializing constructor", type ) );
            }

            DynamicMethod method = new DynamicMethod( "CustomSerializableReader", typeof( object ), new Type[] { typeof( IInput ) }, type );

            ILGenerator generator = method.GetILGenerator( );

			//	TODO: AP: Test ISerialization read for structure

			//	Create a StreamingContext
			//	TODO: AP: Make this a property of the IInput object
			generator.DeclareLocal( typeof( StreamingContext ) );
			generator.Emit( OpCodes.Ldloca_S, 0 );								//	Load streaming context address
			generator.Emit( OpCodes.Initobj, typeof( StreamingContext ) );		//	Initialise streaming context

            generator.Emit( OpCodes.Ldarg_0 );                                  //  Load the IInput onto the stack
			generator.Emit( OpCodes.Ldtoken, type );
			generator.Emit( OpCodes.Call, Type_TypeFromHandle );
            generator.Emit( OpCodes.Call, IInput_ReadSerializationInfo );		//  Read serialization info (pushes info onto stack for object constructor)

			generator.Emit(OpCodes.Ldloc_0);									//	Load streaming context

            generator.Emit( OpCodes.Newobj, constructor );                      //  Create object by calling serializing constructor

            generator.Emit( OpCodes.Ret );                                      //  Return the object

            return ( CustomReaderDelegate )method.CreateDelegate( typeof( CustomReaderDelegate ) );
        }
		
		public static void LoadGenericField( IInput input, object obj, string fieldName )
		{
			object value;
			input.Read( out value );

			FieldInfo fieldInfo = obj.GetType( ).GetField( fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );
			fieldInfo.SetValue( obj, value );
		}

		private static void WriteGenericFieldLoadCode( ILGenerator generator, Type objType, FieldInfo fieldInfo )
		{
			//	NOTE: AP: There's a problem accessing fields of generic types, so for the moment, we'll call the reflection
			//	method to get around it
			//	see http://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=221225
			generator.Emit( OpCodes.Ldarg_0 );
			if ( objType.IsValueType )
			{
				generator.Emit( OpCodes.Ldloc_1 );
			}
			else
			{
				generator.Emit( OpCodes.Ldloc_1 );
			}
			generator.Emit( OpCodes.Ldstr, fieldInfo.Name );
			generator.Emit( OpCodes.Call, This_LoadGenericField );
		}

        private CustomReaderDelegate CreateReader( Type type )
        {
            if ( type.IsPrimitive )
            {
                throw new ArgumentException( string.Format( "No reader available for primitive type \"{0}\"", type ) );
            }
            if ( type.GetInterface( "ISerializable" ) != null )
            {
                return CreateSerializableReader( type );
            }

            DynamicMethod method = new DynamicMethod( "CustomReader", typeof( object ), new Type[] { typeof( IInput ) }, type );
            ILGenerator generator = method.GetILGenerator( );

            generator.DeclareLocal( typeof( ITypeReader ) );
			generator.DeclareLocal( typeof( object ) );
			generator.DeclareLocal( typeof( StreamingContext ) );

            generator.Emit( OpCodes.Ldarg_0 );                      //  Load the IInput onto the stack
            generator.Emit( OpCodes.Call, IInput_GetTypeReader );   //  Get the ITypeReader from the IInput
            generator.Emit( OpCodes.Stloc_0 );                      //  Store it at local variable zero
			
            generator.Emit( OpCodes.Ldarg_0 );                      //  Load the IInput onto the stack
            generator.Emit( OpCodes.Call, IInput_GetContext );		//  Get the StreamingContext from the IInput
            generator.Emit( OpCodes.Stloc_2 );                      //  Store it at local variable zero

            if ( type.IsValueType )
            {
				generator.DeclareLocal( type );
				generator.Emit( OpCodes.Ldloca_S, 3 );
				generator.Emit( OpCodes.Initobj, type );
				generator.Emit( OpCodes.Ldloc_3 );
				generator.Emit( OpCodes.Box, type );
				generator.Emit( OpCodes.Stloc_1 );
            }
            else
            {
				generator.Emit( OpCodes.Ldtoken, type );
				generator.Emit( OpCodes.Call, Type_TypeFromHandle );
				generator.Emit( OpCodes.Call, FormatterServices_GetUninitializedObject );
	            generator.Emit( OpCodes.Stloc_1 );					//  Store it in local variable one
            }

			TypeIoUtils.CallSerializationEventMethod( generator, OpCodes.Ldloc_1, OpCodes.Ldloc_2, type, typeof( OnDeserializingAttribute ) );

            BuildCustomReaderDelegate( generator, type );           //  Generate bytecode to 
			
			TypeIoUtils.CallSerializationEventMethod( generator, OpCodes.Ldloc_1, OpCodes.Ldloc_2, type, typeof( OnDeserializedAttribute ) );

			generator.Emit( OpCodes.Ldloc_1 );                      //  Load the new object from local variable one
            generator.Emit( OpCodes.Ret );                          //  Return it

            return ( CustomReaderDelegate )method.CreateDelegate( typeof( CustomReaderDelegate ) );
        }

        private void BuildCustomReaderDelegate( ILGenerator generator, Type objType )
        {
            //  The base type has to be serializable
            if ( !objType.IsSerializable )
            {
                throw new SerializationException( string.Format( "Type \"{0}\" cannot be serialized (no SerializableAttribute)", objType ) );
            }

            FieldInfo[] fields = objType.GetFields( BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public );
            foreach ( FieldInfo field in fields )
            {
                //  TODO: AP: Handle events, etc.?
                if ( field.IsNotSerialized )
                {
                    continue;
                }
				if ( objType.IsGenericType )
				{
					WriteGenericFieldLoadCode( generator, objType, field );
					continue;
				}

                //  TODO: AP: Hardcoded primitive type write
				generator.Emit( OpCodes.Ldloc_1 );						//  Load the object being deserialized

				if ( !field.FieldType.IsValueType )
				{
					generator.Emit( OpCodes.Castclass, objType );
				}

				generator.Emit( OpCodes.Ldloc_0 );						//  Load the ITypeReader local variable
                generator.Emit( OpCodes.Ldarg_0 );						//  Load the IInput argument

                generator.Emit( OpCodes.Call, ITypeReader_Read );		//  Read the identifier using the input interface

                if ( field.FieldType.IsValueType )
				{
                    generator.Emit( OpCodes.Unbox_Any, field.FieldType );
				}

                generator.Emit( OpCodes.Stfld, field );					//  Store the read object in the current field
            }

            objType = objType.BaseType;
            if ( objType != typeof( object ) )
            {
                BuildCustomReaderDelegate( generator, objType );
            }
        }
    }
}
