using System;
using System.Collections.ObjectModel;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.RenderGraph
{
	/// <summary>
	/// Rendering node interface
	/// </summary>
	public interface IRenderNode
	{
		/// <summary>
		/// Event, invoked when an input node is added to this node
		/// </summary>
		event Action<IRenderNode> InputNodeAdded;

		/// <summary>
		/// Event, invoked when an output node is added to this node
		/// </summary>
		event Action<IRenderNode> OutputNodeAdded;


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
		ReadOnlyCollection<IRenderGraphDataTarget> Targets
		{
			get;
		}

		/// <summary>
		/// Gets the inputs to this node. A node can't be processed until all its inputs have been processed
		/// </summary>
		ReadOnlyCollection<IRenderNode> InputNodes
		{
			get;
		}

		/// <summary>
		/// Gets the outputs of this node
		/// </summary>
		ReadOnlyCollection<IRenderNode> OutputNodes
		{
			get;
		}

		/// <summary>
		/// Gets the outputs of this node
		/// </summary>
		IRenderGraphDataSources Outputs
		{
			get;
		}

		/// <summary>
		/// Adds an output node to this node
		/// </summary>
		/// <param name="node">Output node to add</param>
		void AddOutputNode( IRenderNode node );

		/// <summary>
		/// Adds an input node to this node
		/// </summary>
		/// <param name="node">Input node to add</param>
		void AddInputNode( IRenderNode node );

		/// <summary>
		/// Binds a source to a target
		/// </summary>
		/// <param name="source">Data source</param>
		/// <param name="target">Target to bind to. Must belong to this node</param>
		void Bind( IRenderGraphDataSource source, IRenderGraphDataTarget target );

		/// <summary>
		/// Runs this render node
		/// </summary>
		/// <param name="context">Rendering context</param>
		/// <param name="renderable">Object to render</param>
		void Run( IRenderContext context, IRenderable renderable );
	}
}


