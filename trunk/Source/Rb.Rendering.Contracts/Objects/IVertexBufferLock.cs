using System;
using System.Collections.Generic;
using System.IO;

namespace Rb.Rendering.Contracts.Objects
{
	/// <summary>
	/// A vertex buffer lock provides access to the contents of a vertex buffer, for reading or writing.
	/// </summary>
	/// <remarks>
	/// Changes are not commited to the vertex buffer until the lock is disposed.
	/// The standard usage pattern is therefore:
	/// <code>
	/// using (IVertexBufferLock lock = buffer.Lock(0, 100)) // Lock first 100 vertices in the buffer
	/// {
	///     // Make changes/read data/whatever
	/// } // Implicit Dispose() will commit changes to buffer
	/// </code>
	/// </remarks>
	public unsafe interface IVertexBufferLock : IDisposable
	{
		/// <summary>
		/// Gets a pointer to the locked memory 
		/// </summary>
		byte* Bytes
		{
			get;
		}

		/// <summary>
		/// Writes a series of vertices to the locked region
		/// </summary>
		/// <typeparam name="T">Vertex type</typeparam>
		/// <param name="offset">Offset to the first vertex to write to</param>
		/// <param name="vertices">Vertices</param>
		void Write< T >( int offset, IEnumerable< T > vertices );
		
		/// <summary>
		/// Creates a stream from the locked region
		/// </summary>
		Stream ToStream( );

	}
}
