using System;
using System.Collections.Generic;
using System.IO;

namespace Rb.Muesli
{
    /*
     * Design:
     *  - ISerializable objects should be able to write using standard techniques
     *      - Reading and writing a specific type to the SerializationContext should be in order
     *      - Writing a specific value type should not write the type ID to the stream
     *      - Writing a generic object should write a type ID to the stream
     *  - [Serializable] objects should work as normal
     */
    public class BinaryTypeWriter : ITypeWriter
    {
        #region ITypeWriter Members

        /// <summary>
        /// Writes header information to the stream
        /// </summary>
        public void WriteHeader( Stream stream )
        {
            //  Write type GUIDs
            BinaryWriter output = new BinaryWriter( stream );

            //  Write the value of the "Other" object ID. This gives some version change protection
            output.Write( ( byte )TypeId.Other );
            output.Write( m_Objects.Count );
            output.Write( ( ushort )m_Writers.Count );
            foreach ( CustomWriter writer in m_Writers )
            {
                WriteType( output, writer.m_Type );
            }
        }

		/// <summary>
		/// Writes type information
		/// </summary>
        private static void WriteType( BinaryWriter writer, Type type )
        {
        	uint id = SerializationIdAttribute.GetTypeId( type );
			if( id != SerializationIdAttribute.NoId )
			{
				writer.Write( true );
				writer.Write( id );
			}
			else
			{
				writer.Write( false );
				writer.Write( type.AssemblyQualifiedName );
			}
        }

		/// <summary>
		/// Gets the unique integer identifier for a type
		/// </summary>
		private int GetTypeId( Type type )
		{
			if ( type.IsArray )
			{
				return ( int )TypeId.Array;
			}
			switch ( type.Name )
			{
				case "Type"		: return ( int )TypeId.Type;
				case "Boolean"	: return ( int )TypeId.Bool;
				case "Byte"		: return ( int )TypeId.Byte;
				case "SByte"	: return ( int )TypeId.SByte;
				case "Char"		: return ( int )TypeId.Char;
				case "Int16"	: return ( int )TypeId.Int16;
				case "UInt16"	: return ( int )TypeId.UInt16;
				case "Int32"	: return ( int )TypeId.Int32;
				case "UInt32"	: return ( int )TypeId.UInt32;
				case "Int64"	: return ( int )TypeId.Int64;
				case "UInt64"	: return ( int )TypeId.UInt64;
				case "Single"	: return ( int )TypeId.Single;
				case "Double"	: return ( int )TypeId.Double;
				case "Decimal"	: return ( int )TypeId.Decimal;
				case "DateTime"	: return ( int )TypeId.DateTime;
				case "String"	: return ( int )TypeId.String;
			}
			return GetCustomWriter( type ).m_TypeId;
		}

		/// <summary>
		/// Writes a type
		/// </summary>
		/// <remarks>
		/// Does not write the TypeId.Type identifier prefix
		/// </remarks>
		public void WriteType( IOutput output, Type type )
		{
			WriteTypeId( output, GetTypeId( type ) );
		}
        
        /// <summary>
        /// Writes an object to the specified output
        /// </summary>
        public void Write( IOutput output, object obj )
        {
            if ( obj == null )
            {
                output.WriteNull();
                return;
            }

            Type objType = obj.GetType( );
            if ( objType.IsArray )
            {
                output.Write( ( byte )TypeId.Array );
				output.Write( ( Array )obj );
				return;
            }
            switch ( objType.Name )
            {
				case "Type" :
					output.Write( ( byte )TypeId.Type );
					WriteType( output, ( Type )obj );
            		return;

                case "Boolean" :
                    output.Write( ( byte )TypeId.Bool );
                    output.Write( ( Boolean )obj );
                    return;

                case "Byte" :
                    output.Write( ( byte )TypeId.Byte );
                    output.Write( ( Byte )obj );
                    return;
                    
                case "SByte" :
                    output.Write( ( byte )TypeId.SByte );
                    output.Write( ( SByte )obj );
                    return;
                    
                case "Char" :
                    output.Write( ( byte )TypeId.Char );
                    output.Write( ( Char )obj );
                    return;
                    
                case "Int16" :
                    output.Write( ( byte )TypeId.Int16 );
                    output.Write( ( Int16 )obj );
                    return;
                    
                case "UInt16" :
                    output.Write( ( byte )TypeId.UInt16 );
                    output.Write( ( UInt16 )obj );
                    return;
                    
                case "Int32" :
                    output.Write( ( byte )TypeId.Int32 );
                    output.Write( ( Int32 )obj );
                    return;
                    
                case "UInt32" :
                    output.Write( ( byte )TypeId.UInt32 );
                    output.Write( ( UInt32 )obj );
                    return;
                    
                case "Int64" :
                    output.Write( ( byte )TypeId.Int64 );
                    output.Write( ( Int64 )obj );
                    return;

                case "UInt64" :
                    output.Write( ( byte )TypeId.UInt64 );
                    output.Write( ( UInt64 )obj );
                    return;

                case "Single" :
                    output.Write( ( byte )TypeId.Single );
                    output.Write( ( Single )obj );
                    return;
                    
                case "Double" :
                    output.Write( ( byte )TypeId.Double );
                    output.Write( ( Double )obj );
                    return;

                case "Decimal" :
                    output.Write( ( byte )TypeId.Decimal );
                    output.Write( ( Decimal )obj );
                    return;

                case "DateTime" :
                    output.Write( ( byte )TypeId.DateTime );
                    output.Write( ( DateTime )obj );
                    return;

                case "String" :
                    output.Write( ( byte )TypeId.String );
                    output.Write( ( String )obj );
                    return;
            }

			int objIndex = m_Objects.Find( obj );
			if ( objIndex == -1 )
			{
				m_Objects.Add( obj );
				CustomWriter writer = GetCustomWriter( objType );
				WriteTypeId( output, writer.m_TypeId );
				writer.m_Writer( output, obj );
			}
			else
			{
				WriteTypeId( output, ( int )TypeId.Existing );
				output.Write( objIndex );
			}
		}

        #endregion

        #region Private stuff
        
        private readonly ObjectTable                         m_Objects = new ObjectTable( );
        private readonly List< CustomWriter >                m_Writers = new List< CustomWriter >( );
        private readonly Dictionary< Type, CustomWriter >    m_TypeIds = new Dictionary< Type, CustomWriter >( );

        /// <summary>
        /// Stores a type, its identifier, and a delegate from <see cref="CustomTypeWriterCache"/> that is responsible for writing it
        /// </summary>
        private struct CustomWriter
        {
            public readonly Type					m_Type;
			public readonly CustomWriterDelegate	m_Writer;
			public readonly int						m_TypeId;

            /// <summary>
            /// Setup constructor
            /// </summary>
            public CustomWriter( Type type, int typeId, CustomWriterDelegate writer )
            {
                m_Type = type;
                m_Writer = writer;
                m_TypeId = typeId;
            }
        }

        /// <summary>
        /// Writes a type identifier to the specified output
        /// </summary>
        private static void WriteTypeId( IOutput output, int typeId )
        {
            while ( typeId > 127 )
            {
                byte bitsWithFollowOn = ( byte )( 0x80 | ( typeId & ~0x80 ) );
                output.Write( bitsWithFollowOn );

                typeId >>= 7;
            }

            //  No follow-on bits required
            output.Write( unchecked( ( byte )( typeId ) ) );
        }

        /// <summary>
        /// Gets a CustomWriter for a given type
        /// </summary>
        private CustomWriter GetCustomWriter( Type type )
        {
            CustomWriter writer;
            if ( !m_TypeIds.TryGetValue( type, out writer ) )
            {
                int typeId = ( int )TypeId.Other + m_Writers.Count;

                writer = new CustomWriter( type, typeId, CustomTypeWriterCache.Instance.GetWriter( type ) );

                m_Writers.Add( writer );
                m_TypeIds[ type ] = writer;
            }
            return writer;
        }

        #endregion
    }
}
