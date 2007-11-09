using System;
using System.Runtime.Serialization;

namespace Rb.Muesli
{
    public interface IOutput
    {
        ITypeWriter TypeWriter
        {
            get;
        }

		StreamingContext Context
		{
			get;
		}

        #region Writing single values

        void WriteNull( );
        
        void WriteSerializationInfo( SerializationInfo info, bool writeType );

		void Write( bool val );

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

        void Write( decimal val );
        
        void Write( DateTime val);

        void Write( string val );

        void Write( Array val );

		void Write( Type val );

        void Write( object obj );

        #endregion

        void Finish( );
    }
}
