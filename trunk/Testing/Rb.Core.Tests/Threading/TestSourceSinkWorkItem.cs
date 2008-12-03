using System;
using NUnit.Framework;
using Rb.Core.Threading;

namespace Rb.Core.Tests.Threading
{
	/// <summary>
	/// Tests the <see cref="SourceSinkWorkItem"/> class
	/// </summary>
	[TestFixture]
	public class TestSourceSinkWorkItem
	{
		/// <summary>
		/// Attempting to build a source-sink work item without a source delegate should throw
		/// </summary>
		[Test, ExpectedException( typeof( MissingFieldException ) )]
		public void TestBuildWithoutSourceShouldThrow( )
		{
			new SourceSinkWorkItem.Builder<int>( ).Build( "test" );
		}

		/// <summary>
		/// Attempting to build a source-sink work item without a sink delegate should throw
		/// </summary>
		[Test, ExpectedException( typeof( MissingFieldException ) )]
		public void TestBuildWithoutSinkShouldThrow( )
		{
			SourceSinkWorkItem.Builder<int> builder = new SourceSinkWorkItem.Builder<int>( );
			builder.SetSource( delegate { return 0; } );
			builder.Build( "test" );
		}

		/// <summary>
		/// Test a simple source-sink
		/// </summary>
		[Test]
		public void TestSimpleSourceSink( )
		{
			//	TODO: AP: Use mock framework to ensure that source and sink are called
			SourceSinkWorkItem.Builder<int> builder = new SourceSinkWorkItem.Builder<int>( );
			builder.SetSource( delegate { return 10; } );
			builder.SetSink( delegate( int res ) { Assert.AreEqual( 10, res ); } );
			builder.Build( "test" ).DoWork( ProgressMonitor.Null );
		}

	}
}
