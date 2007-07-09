using System;
using System.IO;
using System.Runtime.Serialization;

namespace Rb.Muesli
{
    public class BinaryTypeReader : ITypeReader
    {
        #region ITypeReader Members

        public void ReadHeader( Stream stream )
        {
            BinaryReader reader = new BinaryReader( stream );
            m_OtherOffset = reader.ReadByte( );
            m_Objects = new object[ reader.ReadInt32( ) ];
            ushort numTypes = reader.ReadUInt16( );
            m_TypeTable = new Type[ numTypes ];
            for ( ushort typeIndex = 0; typeIndex < numTypes; ++typeIndex )
            {
                m_TypeTable[ typeIndex ] = ReadType( reader );
            }
        }

        public Type ReadType( BinaryReader reader )
        {
			if ( reader.ReadBoolean( ) )
			{
				return SerializationIdMap.Instance.GetType( reader.ReadUInt32( ) );
			}

            string typeName = reader.ReadString( );
            return Type.GetType( typeName );
        }

		public Type ReadType( IInput input )
		{
			int typeId = ReadTypeId( input );
			//	TODO: AP: Cope with primitive types
			return m_TypeTable[ typeId - m_OtherOffset ];
		}

        public object Read( IInput input )
        {
            int typeId = ReadTypeId( input );
            switch( typeId )
            {
                case ( int )TypeId.Null     : return null;
                case ( int )TypeId.Bool     : return Input.ReadBoolean( input );;
                case ( int )TypeId.Byte     : return Input.ReadByte( input );
                case ( int )TypeId.SByte    : return Input.ReadSByte( input );
                case ( int )TypeId.Char     : return Input.ReadChar( input );
                case ( int )TypeId.Int16    : return Input.ReadInt16( input );
                case ( int )TypeId.UInt16   : return Input.ReadUInt16( input );
                case ( int )TypeId.Int32    : return Input.ReadInt32( input );
                case ( int )TypeId.UInt32   : return Input.ReadUInt32( input );
                case ( int )TypeId.Int64    : return Input.ReadInt64( input );
                case ( int )TypeId.UInt64   : return Input.ReadUInt64( input );
                case ( int )TypeId.Single   : return Input.ReadSingle( input );
                case ( int )TypeId.Double   : return Input.ReadDouble( input );
                case ( int )TypeId.Decimal  : return Input.ReadDecimal( input );
                case ( int )TypeId.DateTime : return Input.ReadDateTime( input );
                case ( int )TypeId.String   : return Input.ReadString( input );
                case ( int )TypeId.Array    :
                    {
						int length = Input.ReadInt32( input );
						Type elementType = ReadType( input );
						Array array = Array.CreateInstance( elementType, length );
						for ( int i = 0; i < length; ++i  )
						{
							array.SetValue( Read( input ), i );
						}
						return array;
                    }

                case ( int )TypeId.Existing :
                    {
                        return m_Objects[ Input.ReadInt32( input ) ];
                    }
            }

            int typeIndex = typeId - m_OtherOffset;

            Type objType = m_TypeTable[ typeIndex ];

            object result = CustomTypeReaderCache.Instance.GetReader( objType )( input );

            m_Objects[ m_ObjectIndex++ ] = result;

            return result;
        }

        #endregion

        #region Private stuff

        private object[]	m_Objects;
        private int			m_ObjectIndex;
		private byte		m_OtherOffset;
		private Type[]		m_TypeTable;

        private static int ReadTypeId( IInput input )
        {
            int shift = 0;
            int typeId = 0;
            bool followOn;
            do
            {
                byte curByte;
                input.Read( out curByte );
                followOn = ( curByte & 0x80 ) != 0;
                typeId |= ( curByte & ~0x80 ) << shift;
                shift += 7;
            } while ( followOn );

            return typeId;
        }

        #endregion
    }
}
