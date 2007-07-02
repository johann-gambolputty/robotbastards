using System;
using System.IO;
using System.Runtime.Serialization;

namespace Rb.Muesli
{
    public interface ITypeWriter
    {
        void WriteHeader( Stream stream );
        void Write( IOutput output, object obj );
    }
}
