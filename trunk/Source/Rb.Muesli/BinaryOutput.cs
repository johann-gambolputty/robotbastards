using System;
using System.Collections;
using System.Collections.Generic;
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
            m_Writer = new BinaryWriter( stream );
        }

        public void Finish( )
        {
        }

        public void WriteTypeId( int typeId )
        {
            while ( typeId > 127 )
            {
                byte bitsWithFollowOn = ( byte )( 0x80 | ( typeId & ~0x80 ) );
                Write( bitsWithFollowOn );

                typeId >>= 7;
            }

            //  No follow-on bits required
            Write( unchecked( ( byte )( typeId ) ) );
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

        public void Write( Guid val )
        {
            m_Writer.Write( val.ToByteArray( ) );
        }
        
        public void Write( string val )
        {
            m_Writer.Write( val );
        }

        public void Write( object obj )
        {

            if ( obj is byte )      { Write( ( byte )obj ); return; }
            if ( obj is sbyte )     { Write( ( sbyte )obj ); return; }
            if ( obj is char )      { Write( ( char )obj ); return; }
            if ( obj is short )     { Write( ( short )obj ); return; }
            if ( obj is ushort )    { Write( ( ushort )obj ); return; }
            if ( obj is int )       { Write( ( int )obj ); return; }
            if ( obj is uint )      { Write( ( uint )obj ); return; }
            if ( obj is long )      { Write( ( long )obj ); return; }
            if ( obj is ulong )     { Write( ( long )obj ); return; }
            if ( obj is float )     { Write( ( float )obj ); return; }
            if ( obj is double )    { Write( ( double )obj ); return; }
            if ( obj is string )    { Write( ( string )obj ); return; }
            if ( obj is ArrayList ) { Write( );}

            IPersistent persistentObject = obj as IPersistent;
            if ( persistentObject != null )
            {
                Write( persistentObject );
                return;
            }

            //  TODO: ... serialize obj ...
        }
        
        #region Writing collections

        public void Write( ArrayList val )
        {
            Write( val.Count );
            foreach ( object obj in val )
            {
                Write( obj );
            }
        }

        void Write< T >( T[] val );

        void Write< T >( ICollection< T > val );

        void Write< Key, Val >( IDictionary< Key, Val > dictionary );

        #endregion
        
        #region Writing special collections

        void WriteFixedTypeCollection( ArrayList val );

        void WriteFixedTypeCollection( Array val );

        void WriteFixedTypeCollection< T >( T[] val );

        void WriteFixedTypeCollection< T >( ICollection< T > val );

        void WriteFixedTypeCollection< Key, Val >( IDictionary< Key, Val > dictionary );

        #endregion


        #region Writing special objects

        void Write( IPersistent persistentObject )
        {
            if ( persistentObject == null )
            {
                m_Writer.Write( ( byte )0 );
            }
            else
            {
                m_TypeIo.WriteType( this, persistentObject.GetType( ) );
                persistentObject.Write( this );
            }
        }

        void Write( IAutoPersistent persistentObject );

        void Write( ISerializable serializableObject );

        #endregion

        private BinaryWriter    m_Writer;
        private ITypeWriter     m_TypeWriter = new BinaryTypeWriter( );
    }
}
