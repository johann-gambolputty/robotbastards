
namespace Rb.Rendering.RenderGraph
{
	/// <summary>
	/// Interface for objects that can visit nodes in a render graph
	/// </summary>
	/// <remarks>
	/// See <see cref="RenderGraphNodeWalker"/> for mechanism to call visitor for nodes in a graph
	/// </remarks>
	public interface IRenderNodeVisitor
	{
		/// <summary>
		/// Called when the visiting process starts
		/// </summary>
		void StartVisiting( );

		/// <summary>
		/// Visits a render node
		/// </summary>
		/// <param name="node">Render node to visit</param>
		void Visit( IRenderNode node );

		/// <summary>
		/// Called when the visiting process finished
		/// </summary>
		void FinishVisiting( );
	}
}
