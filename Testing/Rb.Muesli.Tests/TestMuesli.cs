using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using NUnit.Framework;

namespace Rb.Muesli.Tests
{
    [TestFixture]
    public class TestMuesli
    {
        private static string BuildRandomString( Random rnd, Dictionary< string, string > uniqueMap )
        {
            int length = rnd.Next( 5, 20 );

            StringBuilder sb = new StringBuilder( length + 1 );
            string result;
            do
            {
                for ( int index = 0; index < length; ++index )
                {
                    sb.Append( ( char )rnd.Next( 'a', 'z' ) );
                }
                result = sb.ToString( );
            } while ( uniqueMap.ContainsKey( result ) );
            uniqueMap[ result ] = result;
            return result;
        }

        [Test]
        public void EnsureSerializationInfoWorksAsExpected( )
        {
            //  Muesli assumes that SerializationInfo stores members in order added, not in some crazy map

            Random rnd = new Random( );
            Dictionary< string, string > uniqueMap = new Dictionary< string, string >( );
            string[] dummyNames = new string[ 100 ];
            for ( int nameIndex = 0; nameIndex < dummyNames.Length; ++nameIndex )
            {
                dummyNames[ nameIndex ] = BuildRandomString( rnd, uniqueMap );
            }

            SerializationInfo info = new SerializationInfo( typeof( TestMuesli ), new FormatterConverter( ) );
            for ( int nameIndex = 0; nameIndex < dummyNames.Length; ++nameIndex )
            {
                info.AddValue( dummyNames[ nameIndex ], null );
            }

            {
                int nameIndex = 0;
                SerializationInfoEnumerator e = info.GetEnumerator();
                while ( e.MoveNext( ) )
                {
                    Assert.AreEqual( dummyNames[ nameIndex++ ], e.Name );
                }
            }
         }

        [Test]
        public void TestSimpleIntegerArrayIo()
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
        public void Test1dArrayIo( )
        {
            MemoryStream    stream  = new MemoryStream( );
            BinaryOutput    output  = new BinaryOutput( stream );

            Primitives[] outputObject = new Primitives[ 2 ];
            outputObject[ 0 ] = new Primitives( );
            outputObject[ 1 ] = null;
            output.Write( ( object )outputObject );

            output.Finish( );

            stream.Seek( 0, SeekOrigin.Begin );
            BinaryInput input = new BinaryInput( stream );

            object inputObject;
            input.Read( out inputObject );

            Assert.AreEqual( outputObject, inputObject );
        }
        
        [Test]
        public void TestListIo( )
        {
            MemoryStream    stream  = new MemoryStream( );
            BinaryOutput    output  = new BinaryOutput( stream );

            List< Primitives > outputObject = new List< Primitives >( );
            outputObject.Add( new Primitives( ) );
            outputObject.Add( new Primitives( ) );
            output.Write( outputObject );

            output.Finish( );

            stream.Seek( 0, SeekOrigin.Begin );
            BinaryInput input = new BinaryInput( stream );

            object inputObject;
            input.Read( out inputObject );
        }

        [Test]
        public void TestSimpleObjectIo( )
        {
            MemoryStream    stream  = new MemoryStream( );
            BinaryOutput    output  = new BinaryOutput( stream );

            Primitives outputObject = new Primitives( );
            outputObject.m_Value12 = "badgers";
            output.Write( outputObject );

            output.Finish( );

            stream.Seek( 0, SeekOrigin.Begin );
            BinaryInput input = new BinaryInput( stream );

            object inputObject;
            input.Read( out inputObject );

            Assert.IsTrue( outputObject.Equals( ( Primitives )inputObject ) );
        }


        [Test]
        public void TestNestedObjectIo()
        {
            MemoryStream stream = new MemoryStream( );
            BinaryOutput output = new BinaryOutput( stream );

            Wrapper outputObject = new Wrapper( );
            output.Write( outputObject );

            output.Finish( );

            stream.Seek( 0, SeekOrigin.Begin );
            BinaryInput input = new BinaryInput( stream );

            object inputObject;
            input.Read( out inputObject );

            Assert.IsTrue( outputObject.Equals( ( Wrapper )inputObject ) );
        }

        [Serializable]
        private class RingTest
        {
            private static int ms_ValueInit = 0;
            public int m_Value = ms_ValueInit++;
            public RingTest m_Next;
        }
        
        [Test]
        public void TestRingIo()
        {
            MemoryStream stream = new MemoryStream( );
            BinaryOutput output = new BinaryOutput( stream );

            RingTest outputObject = new RingTest( );
            outputObject.m_Next = new RingTest( );
            outputObject.m_Next.m_Next = outputObject;

            output.Write( outputObject );

            output.Finish( );

            stream.Seek( 0, SeekOrigin.Begin );
            BinaryInput input = new BinaryInput( stream );

            object inputObject;
            input.Read( out inputObject );

            RingTest curOutput = outputObject;
            RingTest curInput = ( RingTest )inputObject;

            do
            {
                Assert.AreEqual( curOutput.m_Value, curInput.m_Value );
                curOutput = curOutput.m_Next;
                curInput = curInput.m_Next;
            } while ( curOutput != outputObject );
        }
    }
}
