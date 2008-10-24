using System;
using System.Collections.Generic;

namespace Rb.Rendering.RenderGraph
{
	/// <summary>
	/// Invokes a visitor (<see cref="IRenderNodeVisitor"/>) for each node in a render graph
	/// </summary>
	public class RenderGraphNodeWalker
	{
		/// <summary>
		/// Visits all the nodes in a render node graph. Used when the maximum node identifier is known
		/// </summary>
		/// <param name="startNodes">Initial nodes</param>
		/// <param name="visitor">Visitor object</param>
		/// <param name="maxNodeId">Maximum node identifier value in the graph</param>
		/// <remarks>
		/// A node is only visited once all its input nodes have been visited first.
		/// </remarks>
		public static void Walk( IEnumerable<IRenderNode> startNodes, IRenderNodeVisitor visitor, int maxNodeId )
		{
			if ( startNodes == null )
			{
				throw new ArgumentNullException( "startNodes" );
			}
			if ( visitor == null )
			{
				throw new ArgumentNullException( "visitor" );
			}
			visitor.StartVisiting( );

			NodeStatus[] nodeStatuses = new NodeStatus[ 32 ];
			Queue<IRenderNode> nodeQueue = new Queue<IRenderNode>( );
			Enqueue( nodeQueue, startNodes, ref nodeStatuses );

			try
			{
				while ( nodeQueue.Count > 0 )
				{
					IRenderNode node = nodeQueue.Dequeue( );
					if ( nodeStatuses[ node.Id ] == NodeStatus.Visited )
					{
						//	Guards against cyclic graphs
						continue;
					}

					//	Check that the current node can be visited
					if ( !CheckNodeStatus( nodeQueue, node, ref nodeStatuses ) )
					{
						//	It can't (an input node has not been visited yet)
						nodeQueue.Enqueue( node );
						continue;
					}

					//	Enqueue all output nodes of the current node
					Enqueue( nodeQueue, node.OutputNodes, ref nodeStatuses );

					//	Visit the current node
					visitor.Visit( node );
					nodeStatuses[ node.Id ] = NodeStatus.Visited;
				}
			}
			finally
			{
				visitor.FinishVisiting( );
			}

		}

		/// <summary>
		/// Visits all the nodes in a render node graph.
		/// </summary>
		/// <param name="startNodes">Initial nodes</param>
		/// <param name="visitor">Visitor object</param>
		/// <remarks>
		/// A node is only visited once all its input nodes have been visited first.
		/// </remarks>
		public static void Walk( IRenderNode[] startNodes, IRenderNodeVisitor visitor )
		{
			Walk( startNodes, visitor, 32 );
		}

		#region Private Members

		/// <summary>
		/// Enumerates the status a node can have during graph traversal
		/// </summary>
		private enum NodeStatus
		{
			NotInQueue,
			Pending,
			Visited
		}

		/// <summary>
		/// Grows a array of node status values to ensure that it includes an entry for the specified node
		/// </summary>
		private static void GrowNodeStatusArrayToIncludeNode( IRenderNode node, ref NodeStatus[] nodeStatuses )
		{
			if ( node.Id >= nodeStatuses.Length )
			{
				Array.Resize( ref nodeStatuses, ( node.Id + 1 ) * 2 );
			}
		}

		/// <summary>
		/// Sets a node status value
		/// </summary>
		private static void SetNodeStatus( IRenderNode node, NodeStatus status, ref NodeStatus[] nodeStatuses )
		{
			GrowNodeStatusArrayToIncludeNode( node, ref nodeStatuses );
			nodeStatuses[ node.Id ] = status;
		}

		/// <summary>
		/// Checks that a node can be visited. Ensures all inputs to the node are in the node queue
		/// </summary>
		private static bool CheckNodeStatus( Queue<IRenderNode> nodeQueue, IRenderNode node, ref NodeStatus[] nodeStatuses )
		{
			bool canVisit = true;
			foreach ( IRenderNode inputNode in node.InputNodes )
			{
				GrowNodeStatusArrayToIncludeNode( inputNode, ref nodeStatuses );
				if ( nodeStatuses[ inputNode.Id ] == NodeStatus.NotInQueue )
				{
					//	Input node is not in the queue - add it
					Enqueue( nodeQueue, inputNode, ref nodeStatuses );
					canVisit = false;
				}
				else if ( nodeStatuses[ inputNode.Id ] == NodeStatus.Pending )
				{
					canVisit = false;
				}
			}

			return canVisit;
		}

		/// <summary>
		/// Enqueues a single node onto the render queue. Updates the node's status to NodeStatus.Pending
		/// </summary>
		private static void Enqueue( Queue< IRenderNode > nodeQueue, IRenderNode node, ref NodeStatus[] nodeStatuses )
		{
			nodeQueue.Enqueue( node );
			SetNodeStatus( node, NodeStatus.Pending, ref nodeStatuses );
		}

		/// <summary>
		/// Enqueues a range of nodes onto the render queue. Updates each node's status to NodeStatus.Pending
		/// </summary>
		private static void Enqueue( Queue<IRenderNode> nodeQueue, IEnumerable<IRenderNode> nodes, ref NodeStatus[] nodeStatuses )
		{
			foreach ( IRenderNode node in nodes )
			{
				Enqueue( nodeQueue, node, ref nodeStatuses );
			}
		}

		#endregion
	}
}
