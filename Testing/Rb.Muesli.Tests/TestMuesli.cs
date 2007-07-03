using System;
using System.IO;
using NUnit.Framework;

namespace Rb.Muesli.Tests
{
    [TestFixture]
    public class TestMuesli
    {
        [Serializable]
        public class TestObject : IEquatable< TestObject >
        {
            public byte     m_Value0    = 0;
            public sbyte    m_Value1    = 1;
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

            #region IEquatable<TestObject> Members

            public bool Equals( TestObject other )
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

        [Serializable]
        public class WrapperTest : IEquatable< WrapperTest >
        {
            public TestObject m_Object0 = new TestObject( );
            public DateTime m_Now = DateTime.Now;

            #region IEquatable<WrapperTest> Members

            public bool Equals( WrapperTest other )
            {
                return ( m_Now == other.m_Now ) && ( m_Object0.Equals( other.m_Object0 ) );
            }

            #endregion
        }

        [Test]
        public void TestSimpleIntegerArrayIo( )
        {
            MemoryStream    stream  = new MemoryStream( );
            BinaryOutput    output  = new BinaryOutput( stream );

            for ( int i = 0; i < 10; ++i )
            {
                output.Write( i );
            }

            output.Finish( );

            stream.Seek( 0, SeekOrigin.Begin );
            BinaryInput input = new BinaryInput( stream );

            for ( int i = 0; i < 10; ++i )
            {
                int value;
                input.Read( out value );
                Assert.AreEqual( value, i );
            }
        }
        
        [Test]
        public void TestSimpleObjectIo( )
        {
            MemoryStream    stream  = new MemoryStream( );
            BinaryOutput    output  = new BinaryOutput( stream );

            TestObject outputObject = new TestObject( );
            output.Write( outputObject );

            output.Finish( );

            stream.Seek( 0, SeekOrigin.Begin );
            BinaryInput input = new BinaryInput( stream );

            object inputObject;
            input.Read( out inputObject );

            Assert.IsTrue( outputObject.Equals( ( TestObject )inputObject ) );
        }


        [Test]
        public void TestNestedObjectIo()
        {
            MemoryStream stream = new MemoryStream( );
            BinaryOutput output = new BinaryOutput( stream );

            WrapperTest outputObject = new WrapperTest( );
            output.Write( new WrapperTest( ) );

            output.Finish( );
            
            stream.Seek( 0, SeekOrigin.Begin );
            BinaryInput input = new BinaryInput( stream );

            object inputObject;
            input.Read( out inputObject );

            Assert.IsTrue( outputObject.Equals( ( WrapperTest )inputObject ) );
        }
    }
}
