using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Rb.Core.Utils;
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
		/// Event, invoked when an input node is added to this node
		/// </summary>
		public event Action<IRenderNode> InputNodeAdded;

		/// <summary>
		/// Event, invoked when an output node is added to this node
		/// </summary>
		public event Action<IRenderNode> OutputNodeAdded;

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
		/// Gets the render targets belonging to this node
		/// </summary>
		public virtual ReadOnlyCollection<IRenderGraphDataTarget> Targets
		{
			get
			{
				List<IRenderGraphDataTarget> targets = new List<IRenderGraphDataTarget>( );
				foreach ( PropertyDescriptor descriptor in TypeDescriptor.GetProperties( this ) )
				{
					targets.Add( new RenderGraphPropertyDataTarget( this, descriptor ) );
				}
				return targets.AsReadOnly( );
			}
		}


		/// <summary>
		/// Gets the inputs to this node. A node can't be processed until all its inputs have been processed
		/// </summary>
		public ReadOnlyCollection<IRenderNode> InputNodes
		{
			get { return m_InputNodes.AsReadOnly( ); }
		}

		/// <summary>
		/// Gets the outputs of this node
		/// </summary>
		public ReadOnlyCollection<IRenderNode> OutputNodes
		{
			get { return m_OutputNodes.AsReadOnly( ); }
		}

		/// <summary>
		/// Gets the outputs of this node
		/// </summary>
		public IRenderGraphDataSources Outputs
		{
			get { return m_Outputs; }
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
				if ( OutputNodeAdded != null )
				{
					OutputNodeAdded( node );
				}
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
				if ( InputNodeAdded != null )
				{
					InputNodeAdded( node );
				}
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
		/// Binds a source to a target
		/// </summary>
		/// <param name="source">Data source</param>
		/// <param name="target">Target to bind to. Must belong to this node</param>
		public void Bind( IRenderGraphDataSource source, IRenderGraphDataTarget target )
		{
			Arguments.CheckNotNull( source, "source" );
			Arguments.CheckNotNull( target, "target" );

			m_Bindings.Add( new Binding( source, target ) );
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

			//	Update all bindings
			foreach ( Binding binding in m_Bindings )
			{
				binding.Update( );
			}

			//	Render the object
			renderable.Render( context );
		}

		#endregion

		#region Private Members

		#region Binding class

		private class Binding
		{
			/// <summary>
			/// Stores a binding between a render data source and a render data target
			/// </summary>
			public Binding( IRenderGraphDataSource source, IRenderGraphDataTarget target )
			{
				m_Source = source;
				m_Target = target;
			}

			/// <summary>
			/// Updates the target
			/// </summary>
			public void Update( )
			{
				m_Source.UpdateTarget( m_Target );
			}

			#region Private Members

			private readonly IRenderGraphDataSource m_Source;
			private readonly IRenderGraphDataTarget m_Target;

			#endregion
		}

		#endregion

		private int m_Id;
		private string m_Name;
		private readonly List<IRenderNode> m_OutputNodes = new List<IRenderNode>( );
		private readonly List<IRenderNode> m_InputNodes = new List<IRenderNode>( );
		private readonly IRenderGraphDataSources m_Outputs = new RenderGraphDataSources( );
		private readonly List<Binding> m_Bindings = new List<Binding>( );

		#endregion
	}
}
