using System;

namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Types of primitive that can be rendered by the <see cref="IIndexBuffer.Draw"/> method
	/// </summary>
	public enum PrimitiveType
	{
		TriList,
		TriStrip,
		TriFan
	}

	/// <summary>
	/// Index buffer interface
	/// </summary>
	/// <remarks>
	/// Index buffers are created by passing an <see cref="IndexBufferData"/> object to
	/// <see cref="IGraphicsFactory.CreateIndexBuffer(IndexBufferData)"/>.
	/// </remarks>
	public interface IIndexBuffer : IDisposable
	{
		/// <summary>
		/// Gets the length of the index buffer
		/// </summary>
		int Length
		{
			get;
		}

		/// <summary>
		/// Draws elements of the specified primitive type using the index buffer
		/// </summary>
		void Draw( PrimitiveType primType );

		/// <summary>
		/// Draws elements of the specified primitive type, using a range within the index buffer
		/// </summary>
		void Draw( PrimitiveType primType, int firstIndex, int count );

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
