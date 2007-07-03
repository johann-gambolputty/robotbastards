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

        static private CustomTypeReaderCache ms_Instance = new CustomTypeReaderCache( );

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

        private static MethodInfo IOutput_GetTypeReader             = typeof( IInput ).GetProperty( "TypeReader" ).GetGetMethod( );
        private static MethodInfo ITypeReader_Read                  = typeof( ITypeReader ).GetMethod( "Read" );
        private static MethodInfo ITypeReader_ReadSerializationInfo = typeof( ITypeReader ).GetMethod("ReadSerializationInfo");

        private CustomReaderDelegate CreateSerializableReader( Type type )
        {
            ConstructorInfo constructor = type.GetConstructor( new Type[] { typeof( SerializationInfo ), typeof( StreamingContext ) } );
            if ( constructor == null )
            {
                throw new ArgumentException( string.Format( "Type {0} had no serializing constructor", type ) );
            }

            DynamicMethod method = new DynamicMethod( "CustomSerializableReader", typeof( object ), new Type[] { typeof( IInput ) }, type );

            ILGenerator generator = method.GetILGenerator( );
            
            generator.Emit( OpCodes.Ldarg_0 );                                  //  Load the IInput onto the stack
            generator.Emit( OpCodes.Call, ITypeReader_ReadSerializationInfo );  //  Read serialization info
            generator.Emit( OpCodes.Ldnull );                                   //  Load null (streaming context)
            generator.Emit( OpCodes.Newobj, constructor );                      //  Create object by calling serializing constructor
            generator.Emit( OpCodes.Ret );                                      //  Return the object

            return ( CustomReaderDelegate )method.CreateDelegate( typeof( CustomReaderDelegate ) );
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

            //  TODO: AP: This breaks traditional serialisation... need to able to create the class without calling the constructor
            ConstructorInfo constructor = type.GetConstructor( new Type[] { } );
            if ( constructor == null )
            {
                throw new ArgumentException( string.Format( "Type {0} had no default constructor", type ) );
            }

            DynamicMethod method = new DynamicMethod( "CustomReader", typeof( object ), new Type[] { typeof( IInput ) }, type );
            ILGenerator generator = method.GetILGenerator( );

            generator.DeclareLocal( typeof( ITypeReader ) );
            generator.DeclareLocal( typeof( object ) );

            generator.Emit( OpCodes.Ldarg_0 );                      //  Load the IInput onto the stack
            generator.Emit( OpCodes.Call, IOutput_GetTypeReader );  //  Get the TypeReader from the IInput
            generator.Emit( OpCodes.Stloc_0 );                      //  Store it at local variable zero

            generator.Emit( OpCodes.Newobj, constructor );          //  Create a new object of the specified type
            generator.Emit( OpCodes.Stloc_1 );                      //  Store it in local variable one

            BuildCustomReaderDelegate( generator, type );           //  Generate bytecode to 

            generator.Emit( OpCodes.Ldloc_1 );                      //  Load the new object from local variable one
            generator.Emit( OpCodes.Ret );                          //  Return it

            return ( CustomReaderDelegate )method.CreateDelegate( typeof( CustomReaderDelegate ) );
        }

        private void BuildCustomReaderDelegate( ILGenerator generator, Type objType )
        {
            //  The base type has to be serializable
            if ( objType.GetCustomAttributes( typeof( SerializableAttribute ), false ).Length == 0 )
            {
                throw new SerializationException( string.Format( "Type \"{0}\" cannot be serialized (no SerializableAttribute)", objType ) );
            }

            FieldInfo[] fields = objType.GetFields( BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public );
            foreach ( FieldInfo field in fields )
            {
                //  TODO: AP: Handle events, etc.?
                if ( field.GetCustomAttributes( typeof( NonSerializedAttribute ), false ).Length == 0 )
                {
                    continue;
                }

                //  TODO: AP: Hardcoded primitive type write
                generator.Emit( OpCodes.Ldarg_1 );                     //  Load the object being serialized
                generator.Emit( OpCodes.Ldloc_0 );                     //  Load the TypeReader local variable
                generator.Emit( OpCodes.Call, ITypeReader_Read );      //  Write the identifier using the output interface
                generator.Emit( OpCodes.Stfld, field );                //  Store the read object in the current field
            }

            objType = objType.BaseType;
            if ( objType != typeof( object ) )
            {
                BuildCustomReaderDelegate( generator, objType );
            }
        }

        private Dictionary< Type, CustomReaderDelegate > m_ReaderMap = new Dictionary< Type, CustomReaderDelegate >( );
    }
}
