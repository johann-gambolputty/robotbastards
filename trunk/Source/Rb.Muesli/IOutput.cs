using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rb.Muesli
{
    public interface IOutput
    {
        ITypeWriter TypeWriter
        {
            get;
        }

        #region Writing single values

        void WriteTypeId( int typeId );

        void WriteNull( );

        void Write( byte val );

        void Write( sbyte val );

        void Write( char val );

        void Write( short val );

        void Write( ushort val );

        void Write( int val );

        void Write( uint val );

        void Write( long val );

        void Write( ulong val );

        void Write( float val );

        void Write( double val );

        void Write( Guid val );

        void Write( string val );

        void Write( object obj );

        #endregion

        #region Writing collections

        void Write( ArrayList val );

        void Write< T >( T[] val );

        void Write< T >( ICollection< T > val );

        void Write< Key, Val >( IDictionary< Key, Val > dictionary );

        #endregion
        
        #region Writing special collections

        void WriteFixedTypeCollection( ArrayList val );

        void WriteFixedTypeCollection< T >( T[] val );

        void WriteFixedTypeCollection< T >( ICollection< T > val );

        void WriteFixedTypeCollection< Key, Val >( IDictionary< Key, Val > dictionary );

        #endregion

        #region Writing special objects

        void Write( ISerializable persistentObject );

        #endregion

        void Finish( );
    }
}
