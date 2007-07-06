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

        static private CustomTypeWriterCache ms_Instance = new CustomTypeWriterCache( );

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

        private static void SerializableWriter( IOutput output, object obj )
        {
            SerializationInfo info = new SerializationInfo( obj.GetType( ), new FormatterConverter( ) );
            StreamingContext context = new StreamingContext( );
            ( ( ISerializable )obj ).GetObjectData( info, context );

			output.WriteSerializationInfo( info );
        }

        private CustomWriterDelegate CreateWriter( Type type )
        {
            if ( type.IsPrimitive )
            {
                throw new ArgumentException( string.Format( "No writer available for primitive type \"{0}\"", type ) );
            }
            if ( type.GetInterface( "ISerializable" ) != null )
            {
                return SerializableWriter;
            }

            DynamicMethod method = new DynamicMethod( "CustomWriter", typeof( void ), new Type[] { typeof( IOutput ), typeof( object ) }, type );
            
            ILGenerator generator = method.GetILGenerator( );
            generator.DeclareLocal( typeof( ITypeWriter ) );

            generator.Emit( OpCodes.Ldarg_0 );                      //  Load the IOutput onto the stack
            generator.Emit( OpCodes.Call, IOutput_GetTypeWriter );  //  Get the TypeWriter from the IOutput
            generator.Emit( OpCodes.Stloc_0 );                      //  Store it at local variable zero

            BuildCustomWriterDelegate( generator, type );

            generator.Emit( OpCodes.Ret );

            return ( CustomWriterDelegate )method.CreateDelegate( typeof( CustomWriterDelegate ) );
        }

        private static MethodInfo IOutput_GetTypeWriter = typeof(IOutput).GetProperty( "TypeWriter" ).GetGetMethod( );
        private static MethodInfo ITypeWriter_Write     = typeof( ITypeWriter ).GetMethod( "Write" );
		private static MethodInfo This_SaveGenericField	= typeof( CustomTypeWriterCache ).GetMethod( "SaveGenericField" );

		public static void SaveGenericField( IOutput output, object obj, string fieldName )
		{
			FieldInfo fieldInfo = obj.GetType( ).GetField( fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );
			object field = fieldInfo.GetValue( obj  );
			output.Write( field );
		}

		private static void WriteGenericFieldSaveCode( ILGenerator generator, Type objType, FieldInfo fieldInfo )
		{
			//	NOTE: AP: There's a problem accessing fields of generic types, so for the moment, we'll call the reflection
			//	method to get around it
			//	see http://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=221225
			generator.Emit( OpCodes.Ldarg_0 );
			generator.Emit( OpCodes.Ldarg_1 );
			generator.Emit( OpCodes.Ldstr, fieldInfo.Name );
			generator.Emit( OpCodes.Call, This_SaveGenericField );
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
					WriteGenericFieldSaveCode( generator, objType, field );
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
            if ( objType != typeof( object ) )
            {
                BuildCustomWriterDelegate( generator, objType );
            }
        }

        private Dictionary< Type, CustomWriterDelegate > m_WriterMap = new Dictionary< Type, CustomWriterDelegate >( );
    }
}