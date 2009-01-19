using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Rb.Core.Graphs.Tests
{
	/// <summary>
	/// Tests <see cref="IGraphDataSources"/>
	/// </summary>
	public abstract class TestIGraphDataSources
	{
		/// <summary>
		/// Adding a null data source to an <see cref="IGraphDataSources"/> object should throw
		/// </summary>
		[Test, ExpectedException( typeof( ArgumentNullException ) )]
		public void TestAddNullSourceThrows( )
		{
			CreateDataSources( ).Add( null );
		}

		/// <summary>
		/// Removing a null data source from an <see cref="IGraphDataSources"/> object should throw
		/// </summary>
		[Test, ExpectedException( typeof( ArgumentNullException ) )]
		public void TestRemoveNullSourceThrows( )
		{
			CreateDataSources( ).Remove( null );
		}

		/// <summary>
		/// Tests retrieving a named data source
		/// </summary>
		[Test]
		public void TestRetrieveNamedSource( )
		{
			IGraphDataSources sources = CreateDataSources( );
			IGraphDataSource source = new GraphDataSource( "test" );
			sources.Add( source );
			Assert.AreEqual( source, sources[ source.Name ] );
		}

		/// <summary>
		/// Tests adding a named data source, then removing it
		/// </summary>
		[Test, ExpectedException( typeof( KeyNotFoundException ) )]
		public void TestRemoveThenGetNamedSourceShouldThrow( )
		{
			IGraphDataSources sources = CreateDataSources( );
			IGraphDataSource source = new GraphDataSource( "test" );
			sources.Add( source );
			Assert.AreEqual( source, sources[ source.Name ] );
			sources.Remove( source );

			source = sources[ source.Name ];
		}

		#region Protected Members

		/// <summary>
		/// Creates a  graph data sources object
		/// </summary>
		protected abstract IGraphDataSources CreateDataSources( );

		#endregion

		#region Private Members

		private class GraphDataSource : IGraphDataSource
		{
			public GraphDataSource( string name )
			{
				m_Name = name;
			}

			#region IGraphDataSource Members

			public string Name
			{
				get { return m_Name; }
			}

			public void UpdateTarget( IGraphDataTarget target )
			{
			}

			#endregion

			#region Private Members

			private readonly string m_Name;

			#endregion

		}

		#endregion
	}
}
