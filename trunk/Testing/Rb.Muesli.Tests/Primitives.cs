using System;

namespace Rb.Muesli.Tests
{
    [Serializable]
	[SerializationId(0)]
    public class Primitives : IEquatable< Primitives >
    {
        private byte    m_Value0    = 0;
        private sbyte   m_Value1    = 1;
        public char     m_Value2    = 'a';
        public short    m_Value3    = 2;
        public ushort   m_Value4    = 3;
        public int      m_Value5    = 4;
        public uint     m_Value6    = 5;
        public long     m_Value7    = 6;
        public ulong    m_Value8    = 7;
        public float    m_Value9    = 8;
        public double   m_Value10   = 9;
        public decimal  m_Value11   = 10;
        public string   m_Value12   = "jam";

		public override bool Equals( object obj )
		{
			Primitives rhs = obj as Primitives;
			return ( rhs == null )? false : Equals( rhs );
		}

        #region IEquatable<TestObject> Members

        public bool Equals( Primitives other )
        {
            return m_Value0 == other.m_Value0 &&
                   m_Value1 == other.m_Value1 &&
                   m_Value2 == other.m_Value2 &&
                   m_Value3 == other.m_Value3 &&
                   m_Value4 == other.m_Value4 &&
                   m_Value5 == other.m_Value5 &&
                   m_Value6 == other.m_Value6 &&
                   m_Value7 == other.m_Value7 &&
                   m_Value8 == other.m_Value8 &&
                   m_Value9 == other.m_Value9 &&
                   m_Value10 == other.m_Value10 &&
                   m_Value11 == other.m_Value11 &&
                   m_Value12 == other.m_Value12;
        }

        #endregion
    }
}
