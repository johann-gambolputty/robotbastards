
namespace Rb.Core.Graphs
{
	/// <summary>
	/// Interface for objects that can visit nodes in a render graph
	/// </summary>
	/// <remarks>
	/// See <see cref="GraphNodeWalker"/> for mechanism to call visitor for nodes in a graph
	/// </remarks>
	public interface IGraphNodeVisitor
	{
		/// <summary>
		/// Called when the visiting process starts
		/// </summary>
		void StartVisiting( );

		/// <summary>
		/// Visits a render node
		/// </summary>
		/// <param name="node">Render node to visit</param>
		void Visit( IGraphNode node );

		/// <summary>
		/// Called when the visiting process finished
		/// </summary>
		void FinishVisiting( );
	}
}
