using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Rb.Rendering.RenderGraph.Tests
{
	[TestFixture]
	public class RenderGraphNodeWalkerTest
	{
		/// <summary>
		/// Calling <see cref="RenderGraphNodeWalker.Walk(IRenderNode[], IRenderNodeVisitor)"/> with a null startNodes parameter should throw
		/// </summary>
		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void TestWalkNullStartNodesShouldThrow( )
		{
			RenderGraphNodeWalker.Walk( null, null );
		}

		/// <summary>
		/// Calling <see cref="RenderGraphNodeWalker.Walk(IRenderNode[], IRenderNodeVisitor)"/> with a null visitor parameter should throw
		/// </summary>
		[Test, ExpectedException( typeof( ArgumentNullException ) )]
		public void TestWalkNullVisitorShouldThrow( )
		{
			RenderGraphNodeWalker.Walk( new IRenderNode[ 0 ], null );
		}

		/// <summary>
		/// Calling <see cref="RenderGraphNodeWalker.Walk(IRenderNode[], IRenderNodeVisitor)"/> with an
		/// empty start node array should succeed
		/// </summary>
		[Test]
		public void TestWalkEmptyStartNodes( )
		{
			RenderGraphNodeWalker.Walk( new IRenderNode[ 0 ], new MockVisitor( ) );
		}

		/// <summary>
		/// Calling <see cref="RenderGraphNodeWalker.Walk(IRenderNode[], IRenderNodeVisitor)"/> with a
		/// single start node array should succeed
		/// </summary>
		[Test]
		public void TestWalkOneNodeGraph( )
		{
			IRenderNode node = new RenderNode( "", 0 );
			RenderGraphNodeWalker.Walk( new IRenderNode[ ] { node }, new MockVisitor( node ) );
		}

		/// <summary>
		/// Calling <see cref="RenderGraphNodeWalker.Walk(IRenderNode[], IRenderNodeVisitor)"/> with a
		/// tree graph should succeed
		/// </summary>
		[Test]
		public void TestWalkTreeGraph( )
		{
			IRenderNode root = new RenderNode( "root", 0 );
			IRenderNode n1 = new RenderNode( "n1", 1 );
			IRenderNode n2 = new RenderNode( "n2", 2 );
			IRenderNode n3 = new RenderNode( "n3", 3 );
			IRenderNode n4 = new RenderNode( "n4", 4 );
			IRenderNode n5 = new RenderNode( "n5", 5 );
			IRenderNode n6 = new RenderNode( "n6", 6 );
			root.AddOutputNode( n1 );
			root.AddOutputNode( n2 );
			n1.AddOutputNode( n3 );
			n1.AddOutputNode( n4 );
			n2.AddOutputNode( n5 );
			n2.AddOutputNode( n6 );
			RenderGraphNodeWalker.Walk( new IRenderNode[] { root }, new MockVisitor( root, n1, n2, n3, n4, n5, n6 ) );
		}

		/// <summary>
		/// Calling <see cref="RenderGraphNodeWalker.Walk(IRenderNode[], IRenderNodeVisitor)"/> with a
		/// directed acyclic graph should succeed
		/// </summary>
		[Test]
		public void TestWalkDirectedAcyclicGraph( )
		{
			IRenderNode n0 = new RenderNode( "n0", 0 );
			IRenderNode n1 = new RenderNode( "n1", 1 );
			IRenderNode n2 = new RenderNode( "n2", 2 );
			IRenderNode n3 = new RenderNode( "n3", 3 );
			IRenderNode n4 = new RenderNode( "n4", 4 );
			IRenderNode n5 = new RenderNode( "n5", 5 );
			IRenderNode n6 = new RenderNode( "n6", 6 );
			n0.AddOutputNode( n2 );
			n1.AddOutputNode( n2 );	//	n2 has two inputs (n0,n1). n1 is not in the start node list
			
			n3.AddInputNode( n2 );	//	Should be the same as n2.AddOutputNode( n3 )
			n4.AddInputNode( n3 );
			n5.AddInputNode( n4 );
			n5.AddInputNode( n1 );
			n6.AddInputNode( n5 );
			RenderGraphNodeWalker.Walk( new IRenderNode[] { n0 }, new MockVisitor( n0, n1, n2, n3, n4, n5, n6 ) );
		}


		#region MockVisitor Private Class

		//	TODO: AP: Switch to rhino mocks 

		private class MockVisitor : IRenderNodeVisitor
		{
			public MockVisitor( params IRenderNode[] expectedNodes )
			{
				m_Expected = expectedNodes;
			}

			#region IRenderNodeVisitor Members

			public void StartVisiting( )
			{
			}

			public void Visit( IRenderNode node )
			{
				Assert.IsFalse( m_Actual.Contains( node ), "Each node should be visited only once" );
				m_Actual.Add( node );
				Console.WriteLine( "Visiting node {0}", node.Id );
			}

			public void FinishVisiting( )
			{
				foreach ( IRenderNode expected in m_Expected )
				{
					IRenderNode foundNode = m_Actual.Find( delegate( IRenderNode node ) { return node.Id == expected.Id; } );
					Assert.IsNotNull( foundNode );
				}
				Assert.AreEqual( m_Actual.Count, m_Expected.Length );
			}

			#endregion

			#region Private Members

			private readonly List<IRenderNode> m_Actual = new List<IRenderNode>( );
			private readonly IRenderNode[] m_Expected;

			#endregion

		}
		 
		#endregion
	}
}
