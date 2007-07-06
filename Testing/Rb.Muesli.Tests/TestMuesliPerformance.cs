using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using NUnit.Framework;
using Rb.Core.Utils;

namespace Rb.Muesli.Tests
{
    [TestFixture]
    public class TestMuesliPerformance
    {
        struct Stats
        {
            public Stats( double time, Stream stream )
            {
                byte[] testBytes = ( ( MemoryStream )stream ).ToArray( );

                m_Time = time;
                m_BytesWritten = stream.Length;
            }

            public void Dump( string prefix )
            {
                System.Diagnostics.Trace.WriteLine( string.Format( "{0}: time: {1}, bytesWritten: {2}", prefix, m_Time, m_BytesWritten ) );
            }

            private double m_Time;
            private long m_BytesWritten;
        }

        [Test]
        public void TestMuesliVsBinaryFormatter( )
        {
            Primitives objToStore = new Primitives( );

            ProfileSerialization( new BinaryFormatter( ), new MemoryStream( ), objToStore, 10000 ).Dump( "Muesli" );
            ProfileSerialization( new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter( ), new MemoryStream( ), objToStore, 10000 ).Dump( "BF" );
        }

        [Test]
        public void TestBf( )
        {
            List< Primitives > list = new List< Primitives >( );
            list.Add( new Primitives( ) );
            list.Add( new Primitives( ) );

            IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter( );
            MemoryStream stream = new MemoryStream( );
            formatter.Serialize( stream, list );

            //byte[] bytes = stream.ToArray( );
        }

        private static Stats ProfileSerialization( IFormatter formatter, Stream stream, object objToStore, int numIterations )
        {

            long startTime = TinyTime.CurrentTime;

            for ( int count = 0; count < numIterations; ++count )
            {
                formatter.Serialize( stream, objToStore );
            }

            long endTime = TinyTime.CurrentTime;
            stream.Flush( );

            return new Stats( TinyTime.ToSeconds( startTime, endTime ), stream );
        }
    }
}
