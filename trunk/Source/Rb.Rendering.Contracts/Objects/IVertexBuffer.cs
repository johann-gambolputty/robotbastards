
using Rb.Rendering.Contracts;

namespace Rb.Rendering.Contracts.Objects
{
	/// <summary>
	/// Vertex buffer interface
	/// </summary>
	/// <remarks>
	/// Vertex buffers are classified as passes - "beginning" a vertex buffer passes vertex data 
	/// to the rendering engine, "ending" a vertex buffer reverts changes.
	/// Vertex buffers are created by passing an <see cref="VertexBufferData"/> object to
	/// <see cref="IGraphicsFactory.CreateVertexBuffer"/>.
	/// </remarks>
	public interface IVertexBuffer : IPass
	{
	}
}
