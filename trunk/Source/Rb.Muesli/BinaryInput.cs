using System;
using System.Collections.Generic;
using System.IO;

namespace Rb.Muesli
{
    public class BinaryInput : IInput
    {
        public BinaryInput( Stream stream )
        {
            m_TypeReader.ReadHeader( stream );
            m_Reader = new BinaryReader( stream );
        }



        #region IInput Members

        public ITypeReader TypeReader
        {
            get { return m_TypeReader; }
        }


        public SerializationInfo ReadSerializationInfo( )
        {
            SerializationInfo info = new SerializationInfo( );

            int memberCount;
            Read( out memberCount );

            for ( int readCount = 0; readCount < memberCount; ++readCount )
            {
                string key;
                object value;

                Read( out key );
                m_TypeReader.Read( out value );

                info[ key ] = value;
            }

            return info;
        }

        public void Read( out bool val )
        {
            val = m_Reader.ReadBoolean( );
        }

        public void Read( out byte val )
        {
            val = m_Reader.ReadByte( );
        }

        public void Read( out sbyte val )
        {
            val = m_Reader.ReadSByte( );
        }

        public void Read( out char val )
        {
            val = m_Reader.ReadChar( );
        }

        public void Read( out short val )
        {
            val = m_Reader.ReadInt16( );
        }

        public void Read( out ushort val )
        {
            val = m_Reader.ReadUInt16( );
        }

        public void Read( out int val )
        {
            val = m_Reader.ReadInt32( );
        }

        public void Read( out uint val )
        {
            val = m_Reader.ReadUInt32( );
        }

        public void Read( out long val )
        {
            val = m_Reader.ReadInt64( );
        }

        public void Read( out ulong val )
        {
            val = m_Reader.ReadUInt64( );
        }

        public void Read( out float val )
        {
            val = m_Reader.ReadSingle( );
        }

        public void Read( out double val )
        {
            val = m_Reader.ReadDouble( );
        }

        public void Read( out decimal val )
        {
            val = m_Reader.ReadDecimal( );
        }

        public void Read( out Guid val )
        {
            val = new Guid( m_Reader.ReadBytes( 16 ) );
        }

        public void Read( out string val )
        {
            val = m_Reader.ReadString( );
        }

        /*
        public void Read( out System.Collections.ArrayList val )
        {
            val
        }

        public void Read<T>(out T[] val)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read<T>(out ICollection<T> val)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read<Key, Val>(out IDictionary<Key, Val> dictionary)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read(out System.Runtime.Serialization.ISerializable obj)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        */

        public void Read( out object obj )
        {
            obj = m_TypeReader.Read( this );
        }

        #endregion
        
        #region Private stuff

        private BinaryTypeReader    m_TypeReader = new BinaryTypeReader( );
        private BinaryReader        m_Reader;

        #endregion
    }
}
