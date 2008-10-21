using System;
using System.Collections.Generic;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.RenderGraph
{
	/// <summary>
	/// Handy base class implementation of <see cref="IRenderNode"/>
	/// </summary>
	public class RenderNode : IRenderNode
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="name">Node name</param>
		/// <param name="id">Node unique, zero-based identifier</param>
		public RenderNode( string name, int id )
		{
			m_Name = name;
			m_Id = id;
		}

		#region IRenderNode Members

		/// <summary>
		/// Gets the ID of this node
		/// </summary>
		public int Id
		{
			get { return m_Id; }
		}

		/// <summary>
		/// Gets the name of this node
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Gets the inputs to this node. A node can't be processed until all its inputs have been processed
		/// </summary>
		public IEnumerable<IRenderNode> InputNodes
		{
			get { return m_InputNodes; }
		}

		/// <summary>
		/// Gets the outputs of this node
		/// </summary>
		public IEnumerable<IRenderNode> OutputNodes
		{
			get { return m_OutputNodes; }
		}

		/// <summary>
		/// Adds an output node to this node
		/// </summary>
		/// <param name="node">Output node to add</param>
		/// <exception cref="ArgumentNullException">Thrown if node is null</exception>
		/// <remarks>
		/// If the output node does not contain this node as an iput, then this node is added to the
		/// output node (<see cref="AddInputNode"/>)
		/// </remarks>
		public void AddOutputNode( IRenderNode node )
		{
			if ( node == null )
			{
				throw new ArgumentNullException( "node" );
			}
			if ( !m_OutputNodes.Contains( node ) )
			{
				m_OutputNodes.Add( node );
			}
			bool outputContainsThis = false;
			foreach ( IRenderNode inputNode in node.InputNodes )
			{
				if ( inputNode == this )
				{
					outputContainsThis = true;
					break;
				}
			}
			if ( !outputContainsThis )
			{
				node.AddInputNode( this );
			}
		}

		/// <summary>
		/// Adds an input node to this node
		/// </summary>
		/// <param name="node">Input node to add</param>
		/// <exception cref="ArgumentNullException">Thrown if node is null</exception>
		/// <remarks>
		/// If the input node does not contain this node as an output, then this node is added to the
		/// input node (<see cref="AddOutputNode"/>)
		/// </remarks>
		public void AddInputNode( IRenderNode node )
		{
			if ( node == null )
			{
				throw new ArgumentNullException( "node" );
			}
			if ( !m_InputNodes.Contains( node ) )
			{
				m_InputNodes.Add( node );
			}
			bool inputContainsThis = false;
			foreach ( IRenderNode outputNode in node.OutputNodes )
			{
				if ( outputNode == this )
				{
					inputContainsThis = true;
					break;
				}
			}
			if ( !inputContainsThis )
			{
				node.AddOutputNode( this );
			}
		}

		/// <summary>
		/// Runs this node
		/// </summary>
		/// <param name="context">Rendering context</param>
		/// <param name="renderable">Renderable object</param>
		public virtual void Run( IRenderContext context, IRenderable renderable )
		{
			if ( renderable == null )
			{
				return;
			}
			renderable.Render( context );
		}

		#endregion

		#region Private Members

		private string m_Name;
		private int m_Id;
		private readonly List<IRenderNode> m_OutputNodes = new List<IRenderNode>( );
		private readonly List<IRenderNode> m_InputNodes = new List<IRenderNode>( );

		#endregion


	}
}
