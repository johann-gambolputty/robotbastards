using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;

namespace Rb.Muesli
{
    public delegate void CustomWriterDelegate( IOutput output, object obj );

    public class CustomTypeWriterCache
    {
        public static CustomTypeWriterCache Instance
        {
            get { return ms_Instance; }
        }

        public CustomWriterDelegate GetWriter( Type type )
        {
            CustomWriterDelegate writer;
            if ( !m_WriterMap.TryGetValue( type, out writer ) )
            {
                writer = CreateWriter( type );
                m_WriterMap[ type ] = writer;
            }
            return writer;
        }

		
        private readonly static CustomTypeWriterCache ms_Instance		= new CustomTypeWriterCache( );

		private readonly static MethodInfo IOutput_GetStreamingContext	= typeof( IOutput ).GetProperty( "Context" ).GetGetMethod( );
        private readonly static MethodInfo IOutput_GetTypeWriter 		= typeof( IOutput ).GetProperty( "TypeWriter" ).GetGetMethod( );
        private readonly static MethodInfo ITypeWriter_Write     		= typeof( ITypeWriter ).GetMethod( "Write" );
		private readonly static MethodInfo This_SaveNamedField			= typeof( CustomTypeWriterCache ).GetMethod( "SaveNamedField" );

        private readonly Dictionary< Type, CustomWriterDelegate > m_WriterMap = new Dictionary< Type, CustomWriterDelegate >( );

        private static void SerializableWriter( IOutput output, object obj )
        {
            SerializationInfo info = new SerializationInfo( obj.GetType( ), new FormatterConverter( ) );
            ( ( ISerializable )obj ).GetObjectData( info, output.Context );

			output.WriteSerializationInfo( info, info.FullTypeName != obj.GetType().FullName );
        }

        private CustomWriterDelegate CreateWriter( Type type )
        {
			if ( type.IsInterface )
			{
				//	Not an error, because CreateWriter() is used to create delegate for CustomWriter objects representing
				//	interfaces, so they can have a unique type ID...
				return null;
			}

            if ( type.IsPrimitive )
            {
                throw new ArgumentException( string.Format( "No writer available for primitive type \"{0}\"", type ) );
            }
            if ( type.GetInterface( "ISerializable" ) != null )
            {
                return SerializableWriter;
            }

            DynamicMethod method = new DynamicMethod( "CustomWriter", typeof( void ), new Type[] { typeof( IOutput ), typeof( object ) }, type, true );
            
            ILGenerator generator = method.GetILGenerator( );
            generator.DeclareLocal( typeof( ITypeWriter ) );
            generator.DeclareLocal( typeof( StreamingContext ) );

            generator.Emit( OpCodes.Ldarg_0 );                      		//  Load the IOutput onto the stack
            generator.Emit( OpCodes.Call, IOutput_GetTypeWriter );  		//  Get the TypeWriter from the IOutput
            generator.Emit( OpCodes.Stloc_0 );                      		//  Store it at local variable zero

            generator.Emit( OpCodes.Ldarg_0 );                      		//  Load the IOutput onto the stack
            generator.Emit( OpCodes.Call, IOutput_GetStreamingContext );	//  Get the StreamingContext from the IOutput
            generator.Emit( OpCodes.Stloc_1 );								//  Store it at local variable zero

			TypeIoUtils.CallSerializationEventMethod( generator, OpCodes.Ldarg_1, OpCodes.Ldloc_1, type, typeof( OnSerializingAttribute ) );

            BuildCustomWriterDelegate( generator, type );

			TypeIoUtils.CallSerializationEventMethod( generator, OpCodes.Ldarg_1, OpCodes.Ldloc_1, type, typeof( OnSerializedAttribute ) );

            generator.Emit( OpCodes.Ret );

            return ( CustomWriterDelegate )method.CreateDelegate( typeof( CustomWriterDelegate ) );
        }

		/// <summary>
		/// Saves a named field, by accessing its value using reflection
		/// </summary>
		public static void SaveNamedField( IOutput output, RuntimeTypeHandle objTypeHandle, object obj, string fieldName )
		{
			Type objType = Type.GetTypeFromHandle( objTypeHandle );
			FieldInfo fieldInfo = objType.GetField( fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );
			object field = fieldInfo.GetValue( obj  );
			output.Write( field );
		}

		/// <summary>
		/// Writes code that calls <see cref="SaveNamedField"/>
		/// </summary>
		private static void WriteNamedFieldSaveCode( ILGenerator generator, Type objType, FieldInfo fieldInfo )
		{
			//	NOTE: AP: There's a problem accessing fields of generic types, so for the moment, we'll call the reflection
			//	method to get around it
			//	see http://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=221225
			//	This is also used for accessing event fields
			generator.Emit( OpCodes.Ldarg_0 );
			generator.Emit( OpCodes.Ldtoken, objType );
			generator.Emit( OpCodes.Ldarg_1 );
			generator.Emit( OpCodes.Ldstr, fieldInfo.Name );
			generator.Emit( OpCodes.Call, This_SaveNamedField );
		}

        private void BuildCustomWriterDelegate( ILGenerator generator, Type objType )
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
					//	Special case for saving fields using SaveNamedField()
					//	Loading generic-typed fields doesn't work (.NET problem), and accessing events
					//	must be done either through reflection, or via the owning class...
					WriteNamedFieldSaveCode( generator, objType, field );
					continue;
				}

                //  TODO: AP: Hardcoded primitive type write
                generator.Emit( OpCodes.Ldloc_0 );                      //  Load the ITypeWriter local variable
                generator.Emit( OpCodes.Ldarg_0 );                      //  Load the IOutput local variable
				generator.Emit( OpCodes.Ldarg_1 );                      //  Load the object being serialized
				generator.Emit( OpCodes.Ldfld, field );					//  Load the current field

                if ( field.FieldType.IsValueType )
                {
                    //  Must box the field value before it can be written
                    generator.Emit( OpCodes.Box, field.FieldType );
                }

                //  TODO: AP: Should call method appropriate to field type if possible
                generator.Emit( OpCodes.Call, ITypeWriter_Write );      //  Write the identifier using the output interface
            }

            objType = objType.BaseType;
            if ( ( objType != typeof( object ) ) && ( objType != null ) )
            {
                BuildCustomWriterDelegate( generator, objType );
            }
        }
    }
}
