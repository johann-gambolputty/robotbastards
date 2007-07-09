using System;
using System.Runtime.Serialization;

namespace Rb.Muesli
{
    public interface IInput
    {
        ITypeReader TypeReader
        {
            get;
        }

		StreamingContext Context
		{
			get;
		}

		SerializationInfo ReadSerializationInfo( Type type );

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
        
        void Read( out DateTime val );

        void Read( out Guid val );

        void Read( out string val );

		void Read< T >( out T[] val );

        void Read( out object obj );
		
		void Finish( );
    }
}
