
namespace Rb.Common.Controls.Graphs.Interfaces
{
	/// <summary>
	/// The terribly named graph data interface, for graphs that are parameterized by X
	/// </summary>
	public interface IGraphX2dSource : IGraph2dSource
	{
		/// <summary>
		/// Evaluates the graph function with input x
		/// </summary>
		float Evaluate( float x );
	}
}
