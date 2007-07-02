using System;
using System.Collections.Generic;
using System.Text;

using Rb.Log;

using NUnit.Framework;

namespace Rb.Log.Tests
{
	[TestFixture]
	public class TestLog
	{
		[Test]
		public void TestLogging( )
		{
			WriteStuff( AppLog.Tag );
			WriteStuff( TestTag.Tag );
		}


		[Test]
		public void TestMultithreadedLogging( )
		{
			System.Threading.Thread[] threads = new System.Threading.Thread[]
			{
				new System.Threading.Thread( new System.Threading.ThreadStart( TestMessageGenerator ) )
				, new System.Threading.Thread( new System.Threading.ThreadStart( TestMessageGenerator ) )
				, new System.Threading.Thread( new System.Threading.ThreadStart( TestMessageGenerator ) )
			};

			for ( int i = 0; i < threads.Length; ++i )
			{
				threads[ i ].Name = "LogThread " + i;
				threads[ i ].Start( );
			}

			for ( int i = 0; i < threads.Length; ++i )
			{
				threads[ i ].Join( );
			}
		}

		private class TestTag : StaticTag<TestTag>
		{
			public override string TagName
			{
				get { return "Test"; }
			}
		}

		private static void TestMessageGenerator( )
		{
			Random rnd = new Random( );
			for ( int i = 0; i < 10; ++i )
			{
				Severity severity = ( Severity )( rnd.Next( ) % ( int )Severity.Count );

				AppLog.GetSource( severity ).Write( "badgers {0}\r\ntest", i );

				System.Threading.Thread.Sleep( rnd.Next( ) % 100 );
			}
		}

		private static void WriteStuff( Tag t )
		{
			for ( int i = 0; i < ( int )Severity.Count; ++i )
			{
				t.GetSource( ( Severity )i ).Write( "blah" );
				t.GetDebugSource( ( Severity )i ).Write( "blah" );
			}
		}

	}
}
