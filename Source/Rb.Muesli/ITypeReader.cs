using System;
using System.IO;

namespace Rb.Muesli
{
    public interface ITypeReader
    {
        void ReadHeader( Stream stream );
        Type ReadType( IInput input );
    }
}
