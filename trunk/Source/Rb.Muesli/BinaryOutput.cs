using System;
using System.IO;
using System.Runtime.Serialization;

namespace Rb.Muesli
{
    public class BinaryOutput : IOutput
    {
        public ITypeWriter TypeWriter
        {
            get { return m_TypeWriter;  }
        }

        public BinaryOutput( Stream stream )
        {
            m_Stream        = stream;
            m_WriterStream  = new MemoryStream( );
            m_Writer        = new BinaryWriter( m_WriterStream );
        }

        public void Finish( )
        {
            m_TypeWriter.WriteHeader( m_Stream );

            byte[] writerMem = m_WriterStream.ToArray( );
            m_Stream.Write( writerMem, 0, writerMem.Length );
        }

        public void WriteNull( )
        {
            m_Writer.Write( ( byte )TypeId.Null );
        }

        public void WriteSerializationInfo( SerializationInfo info )
        {
            int memberCount = info.MemberCount;
            Write( memberCount );

            SerializationInfoEnumerator e = info.GetEnumerator( );
            while ( e.MoveNext( ) )
            {
                m_Writer.Write( e.Name );
                m_TypeWriter.Write( this, e.Value );
            }
        }

        public void Write( byte val )
        {
            m_Writer.Write( val );
        }

        public void Write( sbyte val )
        {
            m_Writer.Write( val );
        }

        public void Write( char val )
        {
            m_Writer.Write( val );
        }

        public void Write( short val )
        {
            m_Writer.Write( val );
        }

        public void Write( ushort val )
        {
            m_Writer.Write( val );
        }

        public void Write( int val )
        {
            m_Writer.Write( val );
        }

        public void Write( uint val )
        {
            m_Writer.Write( val );
        }

        public void Write( long val )
        {
            m_Writer.Write( val );
        }
        
        public void Write( ulong val )
        {
            m_Writer.Write( val );
        }

        public void Write( float val )
        {
            m_Writer.Write( val );
        }

        public void Write( double val )
        {
            m_Writer.Write( val );
        }

        public void Write( decimal val )
        {
            m_Writer.Write( val );
        }

        public void Write( DateTime val )
        {
            m_Writer.Write( val.ToBinary( ) );
        }

        public void Write( Guid val )
        {
            m_Writer.Write( val.ToByteArray( ) );
        }
        
        public void Write( string val )
        {
            m_Writer.Write( val );
        }
        
        public void Write( Array val )
        {
            m_Writer.Write( val.Length );
			Write( val.GetType( ).GetElementType( ) );
            foreach ( object element in val )
            {
                Write( element );
            }
        }

		public void Write( Type type )
		{
			m_TypeWriter.WriteType( this, type );
		}

        public void Write( object obj )
        {
            m_TypeWriter.Write( this, obj );
        }
        
        /*
        #region Writing collections

        public void Write( ArrayList val )
        {
            Write( val.Count );
            foreach ( object obj in val )
            {
                Write( obj );
            }
        }

        void Write< T >( T[] val )
        {
        }

        void Write< T >( ICollection< T > val );

        void Write< Key, Val >( IDictionary< Key, Val > dictionary );

        #endregion
        */

        private Stream          m_Stream;
        private MemoryStream    m_WriterStream;
        private BinaryWriter    m_Writer;
        private ITypeWriter     m_TypeWriter = new BinaryTypeWriter( );
    }
}
