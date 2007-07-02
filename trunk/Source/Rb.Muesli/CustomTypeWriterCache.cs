using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Text;

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

        public CustomTypeWriterCache( )
        {
            m_WriterMap[ typeof( bool ) ]       = WriteBool;
            m_WriterMap[ typeof( byte ) ]       = WriteByte;
            m_WriterMap[ typeof( sbyte ) ]      = WriteSByte;
            m_WriterMap[ typeof( char ) ]       = WriteChar;
            m_WriterMap[ typeof( short ) ]      = WriteInt16;
            m_WriterMap[ typeof( ushort ) ]     = WriteUInt16;
            m_WriterMap[ typeof( int ) ]        = WriteInt32;
            m_WriterMap[ typeof( uint ) ]       = WriteUInt32;
            m_WriterMap[ typeof( long ) ]       = WriteInt64;
            m_WriterMap[ typeof( ulong ) ]      = WriteUInt64;
            m_WriterMap[ typeof( float ) ]      = WriteSingle;
            m_WriterMap[ typeof( double ) ]     = WriteDouble;
            m_WriterMap[ typeof( decimal ) ]    = WriteDecimal;
            m_WriterMap[ typeof( string ) ]     = WriteString;
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

        private CustomWriterDelegate CreateWriter( Type type )
        {
            if ( type.IsPrimitive )
            {
                throw new ArgumentException( string.Format( "No writer available for primitive type \"{0}\"", type ) );
            }

            DynamicMethod method = new DynamicMethod( "CustomWriter", typeof( void ), new Type[] { typeof( IOutput ), typeof( IObject ) }, type );
            ILGenerator generator = method.GetILGenerator( );

            generator.Emit( OpCodes.Ldarg_0 );                      //  Load the IOutput onto the stack
            generator.Emit( OpCodes.Call, IOutput_GetTypeWriter);   //  Get the TypeWriter from the IOutput
            generator.Emit( OpCodes.Stloc_0 );                      //  Store it at local variable zero

            BuildCustomWriterDelegate( generator, type );

            generator.Emit( OpCodes.Ret );

            return ( CustomWriterDelegate )method.CreateDelegate( typeof( CustomWriterDelegate ) );
        }

        private static MethodInfo IOutput_GetTypeWriter = typeof(IOutput).GetProperty( "TypeWriter" ).GetGetMethod( );
        private static MethodInfo ITypeWriter_Write     = typeof( ITypeWriter ).GetMethod( "Write" );

        private void BuildCustomWriterDelegate( ILGenerator generator, Type objType )
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

                generator.Emit( OpCodes.Ldloc_0 );                      //  Load the TypeWriter local variable
                generator.Emit( OpCodes.Ldarg_1 );                      //  Load the object being serialized
                generator.Emit( OpCodes.Ldfld, field );                 //  Load the current field

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

        #region Custom writer methods for pre-defined value types

        private static void WriteBool( IOutput output, object val ) { output.Write( ( bool )val ); }
        private static void WriteByte( IOutput output, object val ) { output.Write( ( byte )val ); }
        private static void WriteSByte( IOutput output, object val ) { output.Write( ( sbyte )val ); }
        private static void WriteChar( IOutput output, object val ) { output.Write( ( char )val ); }
        private static void WriteInt16( IOutput output, object val ) { output.Write( ( short )val ); }
        private static void WriteUInt16( IOutput output, object val ) { output.Write( ( ushort )val ); }
        private static void WriteInt32( IOutput output, object val ) { output.Write( ( int )val ); }
        private static void WriteUInt32( IOutput output, object val ) { output.Write( ( uint )val ); }
        private static void WriteInt64( IOutput output, object val ) { output.Write( ( long )val ); }
        private static void WriteUInt64( IOutput output, object val ) { output.Write( ( ulong )val ); }
        private static void WriteSingle( IOutput output, object val ) { output.Write( ( float )val ); }
        private static void WriteDouble( IOutput output, object val ) { output.Write( ( double )val ); }
        private static void WriteDecimal( IOutput output, object val ) { output.Write( ( decimal )val ); }
        private static void WriteString( IOutput output, object val ) { output.Write( ( string )val ); }

        #endregion
    }
}
