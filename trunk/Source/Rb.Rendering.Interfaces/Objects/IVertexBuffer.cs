
using Rb.Rendering.Interfaces;

namespace Rb.Rendering.Interfaces.Objects
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
		/// <summary>
		/// Locks a region of the vertex buffer
		/// </summary>
		/// <param name="firstIndex">Index of the first vertex in the buffer to lock</param>
		/// <param name="count">Number of vertices to lock after the first</param>
		/// <param name="read">If true, lock provides read access to the buffer</param>
		/// <param name="write">If true, lock provides write access to the buffer</param>
		/// <returns>Returns a lock object that provides access to the buffer</returns>
		/// <remarks>
		/// Dispose of the lock object to commit the changes to the buffer
		/// </remarks>
		IVertexBufferLock Lock( int firstIndex, int count, bool read, bool write );
	}
}
