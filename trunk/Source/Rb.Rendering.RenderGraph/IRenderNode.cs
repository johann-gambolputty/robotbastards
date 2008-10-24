using System;
using System.Collections.Generic;
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
		/// ID is set by the constructor, or by <see cref="RenderGraph.AddNode"/>.
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
		/// Gets the inputs to this node. A node can't be processed until all its inputs have been processed
		/// </summary>
		IEnumerable<IRenderNode> InputNodes
		{
			get;
		}

		/// <summary>
		/// Gets the outputs of this node
		/// </summary>
		IEnumerable<IRenderNode> OutputNodes
		{
			get;
		}

		/// <summary>
		/// Returns the number of input nodes
		/// </summary>
		int NumInputNodes
		{
			get;
		}

		/// <summary>
		/// Returns the number of output nodes
		/// </summary>
		int NumOutputNodes
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
		/// Runs this render node
		/// </summary>
		/// <param name="context">Rendering context</param>
		/// <param name="renderable">Object to render</param>
		void Run( IRenderContext context, IRenderable renderable );
	}
}


