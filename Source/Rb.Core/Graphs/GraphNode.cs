using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Rb.Core.Utils;

namespace Rb.Core.Graphs
{
	/// <summary>
	/// Handy base class implementation of <see cref="IGraphNode"/>
	/// </summary>
	public class GraphNode : IGraphNode
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="name">Node name</param>
		/// <param name="id">Node unique, zero-based identifier</param>
		public GraphNode( string name, int id )
		{
			m_Name = name;
			m_Id = id;
		}

		#region IRenderNode Members

		/// <summary>
		/// Event, invoked when an input node is added to this node
		/// </summary>
		public event Action<IGraphNode> InputNodeAdded;

		/// <summary>
		/// Event, invoked when an output node is added to this node
		/// </summary>
		public event Action<IGraphNode> OutputNodeAdded;

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
		public virtual ReadOnlyCollection<IGraphDataTarget> Targets
		{
			get
			{
				List<IGraphDataTarget> targets = new List<IGraphDataTarget>( );
				foreach ( PropertyDescriptor descriptor in TypeDescriptor.GetProperties( this ) )
				{
					targets.Add( new GraphPropertyDataTarget( this, descriptor ) );
				}
				return targets.AsReadOnly( );
			}
		}


		/// <summary>
		/// Gets the inputs to this node. A node can't be processed until all its inputs have been processed
		/// </summary>
		public ReadOnlyCollection<IGraphNode> InputNodes
		{
			get { return m_InputNodes.AsReadOnly( ); }
		}

		/// <summary>
		/// Gets the outputs of this node
		/// </summary>
		public ReadOnlyCollection<IGraphNode> OutputNodes
		{
			get { return m_OutputNodes.AsReadOnly( ); }
		}

		/// <summary>
		/// Gets the outputs of this node
		/// </summary>
		public IGraphDataSources Outputs
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
		public void AddOutputNode( IGraphNode node )
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
			if ( !node.InputNodes.Contains( this ) )
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
		public void AddInputNode( IGraphNode node )
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
			if ( !node.OutputNodes.Contains( this ) )
			{
				node.AddOutputNode( this );
			}
		}

		/// <summary>
		/// Binds a source to a target
		/// </summary>
		/// <param name="source">Data source</param>
		/// <param name="target">Target to bind to. Must belong to this node</param>
		public void Bind( IGraphDataSource source, IGraphDataTarget target )
		{
			Arguments.CheckNotNull( source, "source" );
			Arguments.CheckNotNull( target, "target" );

			m_Bindings.Add( new Binding( source, target ) );
		}

		/// <summary>
		/// Runs this node
		/// </summary>
		/// <param name="context">Processing context</param>
		public virtual void Process( IGraphProcessContext context )
		{
			UpdateAllDataBindings( );
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Updates all targets from their data sources
		/// </summary>
		protected void UpdateAllDataBindings( )
		{
			//	Update all bindings
			foreach ( Binding binding in m_Bindings )
			{
				binding.Update( );
			}	
		}

		#endregion

		#region Private Members

		#region Binding class

		private class Binding
		{
			/// <summary>
			/// Stores a binding between a render data source and a render data target
			/// </summary>
			public Binding( IGraphDataSource source, IGraphDataTarget target )
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

			private readonly IGraphDataSource m_Source;
			private readonly IGraphDataTarget m_Target;

			#endregion
		}

		#endregion

		private int m_Id;
		private string m_Name;
		private readonly List<IGraphNode> m_OutputNodes = new List<IGraphNode>( );
		private readonly List<IGraphNode> m_InputNodes = new List<IGraphNode>( );
		private readonly IGraphDataSources m_Outputs = new GraphDataSources( );
		private readonly List<Binding> m_Bindings = new List<Binding>( );

		#endregion
	}
}
