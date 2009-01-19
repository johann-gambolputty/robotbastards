using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Rb.Core.Graphs.Tests
{
	[TestFixture]
	public class TestGraphNodeWalker
	{
		/// <summary>
		/// Calling <see cref="GraphNodeWalker.Walk(IGraphNode[], IGraphNodeVisitor)"/> with a null startNodes parameter should throw
		/// </summary>
		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void TestWalkNullStartNodesShouldThrow( )
		{
			GraphNodeWalker.Walk( null, null );
		}

		/// <summary>
		/// Calling <see cref="GraphNodeWalker.Walk(IGraphNode[], IGraphNodeVisitor)"/> with a null visitor parameter should throw
		/// </summary>
		[Test, ExpectedException( typeof( ArgumentNullException ) )]
		public void TestWalkNullVisitorShouldThrow( )
		{
			GraphNodeWalker.Walk( new IGraphNode[ 0 ], null );
		}

		/// <summary>
		/// Calling <see cref="GraphNodeWalker.Walk(IGraphNode[], IGraphNodeVisitor)"/> with an
		/// empty start node array should succeed
		/// </summary>
		[Test]
		public void TestWalkEmptyStartNodes( )
		{
			GraphNodeWalker.Walk( new IGraphNode[ 0 ], new MockVisitor( ) );
		}

		/// <summary>
		/// Calling <see cref="GraphNodeWalker.Walk(IGraphNode[], IGraphNodeVisitor)"/> with a
		/// single start node array should succeed
		/// </summary>
		[Test]
		public void TestWalkOneNodeGraph( )
		{
			IGraphNode node = new GraphNode( "", 0 );
			GraphNodeWalker.Walk( new IGraphNode[ ] { node }, new MockVisitor( node ) );
		}

		/// <summary>
		/// Calling <see cref="GraphNodeWalker.Walk(IGraphNode[], IGraphNodeVisitor)"/> with a
		/// tree graph should succeed
		/// </summary>
		[Test]
		public void TestWalkTreeGraph( )
		{
			IGraphNode root = new GraphNode( "root", 0 );
			IGraphNode n1 = new GraphNode( "n1", 1 );
			IGraphNode n2 = new GraphNode( "n2", 2 );
			IGraphNode n3 = new GraphNode( "n3", 3 );
			IGraphNode n4 = new GraphNode( "n4", 4 );
			IGraphNode n5 = new GraphNode( "n5", 5 );
			IGraphNode n6 = new GraphNode( "n6", 6 );
			root.AddOutputNode( n1 );
			root.AddOutputNode( n2 );
			n1.AddOutputNode( n3 );
			n1.AddOutputNode( n4 );
			n2.AddOutputNode( n5 );
			n2.AddOutputNode( n6 );
			GraphNodeWalker.Walk( new IGraphNode[] { root }, new MockVisitor( root, n1, n2, n3, n4, n5, n6 ) );
		}

		/// <summary>
		/// Calling <see cref="GraphNodeWalker.Walk(IGraphNode[], IGraphNodeVisitor)"/> with a
		/// directed acyclic graph should succeed
		/// </summary>
		[Test]
		public void TestWalkDirectedAcyclicGraph( )
		{
			IGraphNode n0 = new GraphNode( "n0", 0 );
			IGraphNode n1 = new GraphNode( "n1", 1 );
			IGraphNode n2 = new GraphNode( "n2", 2 );
			IGraphNode n3 = new GraphNode( "n3", 3 );
			IGraphNode n4 = new GraphNode( "n4", 4 );
			IGraphNode n5 = new GraphNode( "n5", 5 );
			IGraphNode n6 = new GraphNode( "n6", 6 );
			n0.AddOutputNode( n2 );
			n1.AddOutputNode( n2 );	//	n2 has two inputs (n0,n1). n1 is not in the start node list
			
			n3.AddInputNode( n2 );	//	Should be the same as n2.AddOutputNode( n3 )
			n4.AddInputNode( n3 );
			n5.AddInputNode( n4 );
			n5.AddInputNode( n1 );
			n6.AddInputNode( n5 );
			GraphNodeWalker.Walk( new IGraphNode[] { n0 }, new MockVisitor( n0, n1, n2, n3, n4, n5, n6 ) );
		}

		/// <summary>
		/// Calling <see cref="GraphNodeWalker.Walk(IGraphNode[], IGraphNodeVisitor)"/> with a
		/// directed cyclical graph should succeed
		/// </summary>
		[Test]
		public void TestWalkDirectedCyclicGraph( )
		{
			IGraphNode n0 = new GraphNode( "n0", 0 );
			IGraphNode n1 = new GraphNode( "n1", 1 );
			IGraphNode n2 = new GraphNode( "n2", 2 );
			n0.AddOutputNode( n1 );
			n1.AddOutputNode( n2 );
			n2.AddOutputNode( n0 );

			GraphNodeWalker.Walk( new IGraphNode[] { n0 }, new MockVisitor( n0, n1, n2 ) );
		}


		#region MockVisitor Private Class

		//	TODO: AP: Switch to rhino mocks 

		private class MockVisitor : IGraphNodeVisitor
		{
			public MockVisitor( params IGraphNode[] expectedNodes )
			{
				m_Expected = expectedNodes;
			}

			#region IGraphNodeVisitor Members

			public void StartVisiting( )
			{
			}

			public void Visit( IGraphNode node )
			{
				Assert.IsFalse( m_Actual.Contains( node ), "Each node should be visited only once" );
				m_Actual.Add( node );
				Console.WriteLine( "Visiting node {0}", node.Id );
			}

			public void FinishVisiting( )
			{
				foreach ( IGraphNode expected in m_Expected )
				{
					IGraphNode foundNode = m_Actual.Find( delegate( IGraphNode node ) { return node.Id == expected.Id; } );
					Assert.IsNotNull( foundNode );
				}
				Assert.AreEqual( m_Actual.Count, m_Expected.Length );
			}

			#endregion

			#region Private Members

			private readonly List<IGraphNode> m_Actual = new List<IGraphNode>( );
			private readonly IGraphNode[] m_Expected;

			#endregion

		}
		 
		#endregion
	}
}
