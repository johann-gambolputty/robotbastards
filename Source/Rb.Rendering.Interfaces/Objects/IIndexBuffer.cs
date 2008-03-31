namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Index buffer interface
	/// </summary>
	/// <remarks>
	/// Index buffers are classified as passes - "beginning" an index buffer passes inedx data 
	/// to the rendering engine, "ending" an index buffer disables index buffering.
	/// Index buffers are created by passing an <see cref="IndexBufferData"/> object to
	/// <see cref="IGraphicsFactory.CreateIndexBuffer"/>.
	/// </remarks>
	public interface IIndexBuffer : IPass
	{
		/// <summary>
		/// Locks a region of the index buffer
		/// </summary>
		/// <param name="firstIndex">Index of the first index in the buffer to lock</param>
		/// <param name="count">Number of vertices to lock after the first</param>
		/// <returns>Returns a lock object that provides access to the buffer</returns>
		/// <remarks>
		/// Dispose of the lock object to commit the changes to the buffer
		/// </remarks>
		IIndexBufferLock Lock( int firstIndex, int count );
	}
}
