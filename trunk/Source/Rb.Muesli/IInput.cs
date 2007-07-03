using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rb.Muesli
{
    public interface IInput
    {
        ITypeReader TypeReader
        {
            get;
        }

        SerializationInfo ReadSerializationInfo( );

        void Read( out bool val );

        void Read( out byte val );

        void Read( out sbyte val );

        void Read( out char val );

        void Read( out short val );

        void Read( out ushort val );

        void Read( out int val );

        void Read( out uint val );

        void Read( out long val );

        void Read( out ulong val );

        void Read( out float val );

        void Read( out double val );
        
        void Read( out decimal val );

        void Read( out Guid val );

        void Read( out string val );

        /*
        void Read( out ArrayList val );

        void Read< T >( out T[] val );

        void Read< T >( out ICollection< T > val );

        void Read< Key, Val >( out IDictionary< Key, Val > dictionary );

        void Read( out ISerializable obj );
        */

        void Read( out object obj );
    }
}
