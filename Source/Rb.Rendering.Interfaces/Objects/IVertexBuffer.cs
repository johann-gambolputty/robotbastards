using System;

namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Vertex buffer interface
	/// </summary>
	/// <remarks>
	/// Vertex buffers are classified as passes - "beginning" a vertex buffer passes vertex data 
	/// to the rendering engine, "ending" a vertex buffer reverts changes.
	/// </remarks>
	public interface IVertexBuffer : IPass, IDisposable
	{
		#region Vertex buffer creation

		/// <summary>
		/// Creates this vertex buffer with a given format and size
		/// </summary>
		/// <param name="format"></param>
		/// <param name="numVertices"></param>
		/// <remarks>
		/// Buffer must be filled using <see cref="Lock(bool,bool)"/> or <see cref="Lock(int,int,bool,bool)"/> 
		/// prior to use.
		/// </remarks>
		void Create( VertexBufferFormat format, int numVertices );

		/// <summary>
		/// Creates this vertex buffer from a <see cref="VertexBufferData"/> object
		/// </summary>
		/// <param name="data">Vertex buffer data</param>
		void Create( VertexBufferData data );

		/// <summary>
		/// Creates this vertex buffer from an array of vertices
		/// </summary>
		/// <typeparam name="T">Vertex type</typeparam>
		/// <param name="vertices">Vertex array</param>
		/// <remarks>
		/// The vertex type T must have one or more fields tagged with the <see cref="VertexFieldAttribute"/> attribute.
		/// At least one of those fields must have the semantic <see cref="VertexFieldSemantic.Position"/>
		/// </remarks>
		void Create<T>( T[] vertices );

		#endregion

		/// <summary>
		/// Returns the number of vertices allocated to this buffer
		/// </summary>
		int Count
		{
			get;
		}

		/// <summary>
		/// Gets the size of a single vertex
		/// </summary>
		int VertexSizeInBytes
		{
			get;
		}

		/// <summary>
		/// Draws the contents of the vertex buffer directly
		/// </summary>
		void Draw( PrimitiveType primType );

		/// <summary>
		/// Locks the entire vertex buffer
		/// </summary>
		/// <param name="read">If true, lock provides read access to the buffer</param>
		/// <param name="write">If true, lock provides write access to the buffer</param>
		/// <returns>Returns a lock object that provides access to the buffer</returns>
		IVertexBufferLock Lock( bool read, bool write );

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
