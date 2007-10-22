
namespace Rb.Rendering
{
	/// <summary>
	/// Vertex buffer interface
	/// </summary>
	/// <remarks>
	/// Vertex buffers are classified as passes - "beginning" a vertex buffer passes vertex data 
	/// to the rendering engine, "ending" a vertex buffer reverts these changes.
	/// </remarks>
	public interface IVertexBuffer : IPass
	{
	}
}
