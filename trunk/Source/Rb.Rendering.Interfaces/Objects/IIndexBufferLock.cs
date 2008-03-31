using System.IO;

namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// An index buffer lock provides access to the contents of an index buffer, for reading or writing.
	/// </summary>
	/// <remarks>
	/// Changes are not commited to the index buffer until the lock is disposed.
	/// The standard usage pattern is therefore:
	/// <code>
	/// using (IIndexBufferLock lock = buffer.Lock(0, 100)) // Lock first 100 indices in the buffer
	/// {
	///     // Make changes/read data/whatever
	/// } // Implicit Dispose() will commit changes to buffer
	/// </code>
	/// </remarks>
	public unsafe interface IIndexBufferLock
	{
		/// <summary>
		/// Gets a pointer to the locked memory 
		/// </summary>
		byte* Bytes
		{
			get;
		}

		/// <summary>
		/// Writes a series of indices to the locked region
		/// </summary>
		void Write( int offset, int[] indices );

		/// <summary>
		/// Creates a stream from the locked region
		/// </summary>
		Stream ToStream( );
	}
}
