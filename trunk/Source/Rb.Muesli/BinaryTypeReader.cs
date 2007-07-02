using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Muesli
{
    public class BinaryTypeReader : ITypeReader
    {
        #region ITypeReader Members

        public void ReadHeader(System.IO.Stream stream)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Type ReadType(IInput input)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
