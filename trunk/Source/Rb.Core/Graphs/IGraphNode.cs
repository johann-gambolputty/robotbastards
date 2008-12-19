using System;
using System.Collections.ObjectModel;

namespace Rb.Core.Graphs
{
	/// <summary>
	/// Graph node interface
	/// </summary>
	public interface IGraphNode
	{
		/// <summary>
		/// Event, invoked when an input node is added to this node
		/// </summary>
		event Action<IGraphNode> InputNodeAdded;

		/// <summary>
		/// Event, invoked when an output node is added to this node
		/// </summary>
		event Action<IGraphNode> OutputNodeAdded;


		/// <summary>
		/// Gets the unique, zero-based identifier of this node
		/// </summary>
		/// <remarks>
		/// ID is only unique in the context of the graph that this node is a part of.
		/// </remarks>
		int Id
		{
			get;
		}

		/// <summary>
		/// Gets the name of this node
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Gets the render targets belonging to this node
		/// </summary>
		ReadOnlyCollection<IGraphDataTarget> Targets
		{
			get;
		}

		/// <summary>
		/// Gets the inputs to this node. A node can't be processed until all its inputs have been processed
		/// </summary>
		ReadOnlyCollection<IGraphNode> InputNodes
		{
			get;
		}

		/// <summary>
		/// Gets the outputs of this node
		/// </summary>
		ReadOnlyCollection<IGraphNode> OutputNodes
		{
			get;
		}

		/// <summary>
		/// Gets the outputs of this node
		/// </summary>
		IGraphDataSources Outputs
		{
			get;
		}

		/// <summary>
		/// Adds an output node to this node
		/// </summary>
		/// <param name="node">Output node to add</param>
		void AddOutputNode( IGraphNode node );

		/// <summary>
		/// Adds an input node to this node
		/// </summary>
		/// <param name="node">Input node to add</param>
		void AddInputNode( IGraphNode node );

		/// <summary>
		/// Binds a source to a target
		/// </summary>
		/// <param name="source">Data source</param>
		/// <param name="target">Target to bind to. Must belong to this node</param>
		void Bind( IGraphDataSource source, IGraphDataTarget target );

		/// <summary>
		/// Processes this node
		/// </summary>
		/// <param name="context">Processing context</param>
		void Process( IGraphProcessContext context );
	}
}


