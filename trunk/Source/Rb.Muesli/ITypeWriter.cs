using System;
using System.IO;

namespace Rb.Muesli
{
    public interface ITypeWriter
    {
        void WriteHeader( Stream stream );
		void WriteType( IOutput output, Type type );
        void Write( IOutput output, object obj );
    }
}
