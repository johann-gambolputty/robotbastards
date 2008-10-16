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
		TriFan,
		QuadList
	}

	/// <summary>
	/// Index buffer interface
	/// </summary>
	public interface IIndexBuffer : IDisposable
	{
		#region Index buffer properties

		/// <summary>
		/// Gets the length of the index buffer
		/// </summary>
		int Length
		{
			get;
		}

		#endregion

		#region Index buffer creation

		/// <summary>
		/// Creates this index buffer
		/// </summary>
		void Create( IndexBufferFormat indexSize, bool staticBuffer, int numIndices );

		/// <summary>
		/// Creates this index buffer from an index array
		/// </summary>
		void Create( int[] indices, bool staticBuffer );

		/// <summary>
		/// Creates this index buffer from an index array
		/// </summary>
		void Create( ushort[] indices, bool staticBuffer );

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

		#endregion

		#region Index buffer application

		/// <summary>
		/// Draws elements of the specified primitive type using the index buffer
		/// </summary>
		void Draw( PrimitiveType primType );

		/// <summary>
		/// Draws elements of the specified primitive type, using a range within the index buffer
		/// </summary>
		void Draw( PrimitiveType primType, int firstIndex, int count );

		#endregion

	}
}
