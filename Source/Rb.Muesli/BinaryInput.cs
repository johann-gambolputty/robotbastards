using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Rb.Muesli
{
    public class BinaryInput : IInput
    {
        public BinaryInput( Stream stream )
        {
			m_Context = new StreamingContext( );
            m_TypeReader.ReadHeader( stream );
            m_Reader = new BinaryReader( stream );
        }

        #region IInput Members

        public ITypeReader TypeReader
        {
            get { return m_TypeReader; }
        }

    	public StreamingContext Context
    	{
    		get { return m_Context; }
    	}

        public SerializationInfo ReadSerializationInfo( Type type )
        {
            SerializationInfo info = new SerializationInfo( type, new FormatterConverter( ) );

            int memberCount;
            Read( out memberCount );

            for ( int readCount = 0; readCount < memberCount; ++readCount )
            {
                string key;
                Read( out key );
                object value = m_TypeReader.Read( this );
                info.AddValue( key, value );
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

        public void Read( out DateTime val )
        {
            val = DateTime.FromBinary( m_Reader.ReadInt64( ) );
        }

        public void Read< T >( out T[] val )
        {
			int length = m_Reader.ReadInt32( );
			val = new T[ length ];

			for ( int index = 0; index < length; ++index )
			{
				object value;
				Read( out value );
				val[ index ] = ( T )value;
			}
        }

		public void Read( out Guid val )
        {
            val = new Guid( m_Reader.ReadBytes( 16 ) );
        }

        public void Read( out string val )
        {
            val = m_Reader.ReadString( );
        }

        public void Read( out object obj )
        {
            obj = m_TypeReader.Read( this );
        	IDeserializationCallback listener = obj as IDeserializationCallback;
			if ( listener != null )
			{
				m_DeserializationListeners.Add( listener );
			}
        }

		public void Finish( )
		{
			foreach ( IDeserializationCallback listener in m_DeserializationListeners )
			{
				listener.OnDeserialization( null );
			}
		}

        #endregion
        
        #region Private stuff

		private StreamingContext				m_Context;
        private BinaryTypeReader    			m_TypeReader = new BinaryTypeReader( );
        private BinaryReader        			m_Reader;
    	private List<IDeserializationCallback>	m_DeserializationListeners = new List< IDeserializationCallback >( );

        #endregion
    }
}
