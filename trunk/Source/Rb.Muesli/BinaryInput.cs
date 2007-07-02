using System;
using System.Collections.Generic;
using System.IO;

namespace Rb.Muesli
{
    public class BinaryInput : IInput
    {
        public BinaryInput( Stream stream )
        {
            
        }

        #region IInput Members

        public void Read(out byte val)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read(out sbyte val)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read(out char val)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read(out short val)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read(out ushort val)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read(out int val)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read(out uint val)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read(out long val)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read(out ulong val)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read(out float val)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read(out double val)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read(out Guid val)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read(out string val)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read(out System.Collections.ArrayList val)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read<T>(out T[] val)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read<T>(out ICollection<T> val)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read<Key, Val>(out IDictionary<Key, Val> dictionary)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read(out System.Runtime.Serialization.ISerializable obj)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read(out object obj)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
