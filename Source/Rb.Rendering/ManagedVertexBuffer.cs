using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering
{
	/// <summary>
	/// Manages memory in a vertex buffer
	/// </summary>
	public class ManagedVertexBuffer : IPass, IDisposable
	{
		#region VbHandle Public Class

		/// <summary>
		/// A handle into the managed vertex buffer memory
		/// </summary>
		public class VbHandle : IDisposable
		{
			#region Public Members

			/// <summary>
			/// Gets the index of the first vertex used by this handle
			/// </summary>
			public int FirstVertexIndex
			{
				get { return m_FirstVertexIndex; }
				internal set { m_FirstVertexIndex = value; }
			}

			/// <summary>
			/// Gets the number of vertices owned by this handle
			/// </summary>
			public int VertexCount
			{
				get { return m_VertexCount; }
			}

			/// <summary>
			/// Locks a region of the vertex buffer
			/// </summary>
			/// <returns>Returns a pointer into the buffer memory</returns>
			/// <remarks>
			/// Dispose of the lock object to commit the changes to the buffer
			/// </remarks>
			public unsafe void* Lock( bool read, bool write )
			{
				Unlock( );
				m_VertexBufferLock = m_Owner.VertexBuffer.Lock( m_FirstVertexIndex, m_VertexCount, read, write );
				return m_VertexBufferLock.Bytes;
			}

			/// <summary>
			/// Unlocks
			/// </summary>
			public void Unlock( )
			{
				if ( m_VertexBufferLock != null )
				{
					m_VertexBufferLock.Dispose( );
					m_VertexBufferLock = null;
				}
			}

			#endregion

			#region IDisposable Members

			/// <summary>
			/// Releases the memory owned by the handle, back to the managed VB owner
			/// </summary>
			public void Dispose( )
			{
				m_Owner.ReleaseHandle( this );
			}

			#endregion
			
			#region Internal Members

			/// <summary>
			/// Sets the owner of this handle
			/// </summary>
			internal VbHandle( ManagedVertexBuffer owner, int firstVertexIndex, int vertexCount )
			{
				m_Owner = owner;
				m_FirstVertexIndex = firstVertexIndex;
				m_VertexCount = vertexCount;
			} 

			#endregion

			#region Private Members

			private readonly ManagedVertexBuffer m_Owner;
			private IVertexBufferLock m_VertexBufferLock;
			private int m_FirstVertexIndex;
			private readonly int m_VertexCount;

			#endregion
		} 

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor. Call <see cref="Create"/> before attempting to allocate memory
		/// </summary>
		public ManagedVertexBuffer( )
		{
		}

		/// <summary>
		/// Setup constructor. Creates the underlying vertex buffer whose memory will be managed by this class
		/// </summary>
		public ManagedVertexBuffer( VertexBufferFormat format, int numVertices )
		{
			Create( format, numVertices );
		}

		#endregion

		#region Public Members

		/// <summary>
		/// Gets the vertex buffer that is being managed by this object
		/// </summary>
		public IVertexBuffer VertexBuffer
		{
			get { return m_Vb; }
		}

		/// <summary>
		/// Creates the underlying vertex buffer whose memory will be managed by this class
		/// </summary>
		public void Create( VertexBufferFormat format, int numVertices )
		{
			m_Vb.Create( format, numVertices );
			m_FreeList.Clear( );
			m_FreeList.Add( new FreeRange( 0, numVertices ) );
			m_Handles.Clear( );
		}

		public VbHandle CreateHandle( int numVertices )
		{
			FreeRange range = GetFreeRange( numVertices );
			VbHandle handle = new VbHandle( this, range.Start, range.Size );
			m_Handles.Add( handle );
			return handle;
		}

		#endregion

		#region IPass Members

		/// <summary>
		/// Applies the vertex buffer
		/// </summary>
		public void Begin( )
		{
			m_Vb.Begin( );
		}
		
		/// <summary>
		/// Un-applies the vertex buffer
		/// </summary>
		public void End( )
		{
			m_Vb.End( );
		}

		#endregion

		#region IDisposable Members

		public void Dispose( )
		{
			m_Vb.Dispose( );
		}

		#endregion
		
		#region Protected Members

		/// <summary>
		/// Creates a new VB handle
		/// </summary>
		protected virtual VbHandle CreateHandle( int start, int size )
		{
			return new VbHandle( this, start, size );
		}
		
		#endregion

		#region FreeRange Private Struct

		private struct FreeRange
		{
			public FreeRange( int start, int count )
			{
				m_Start = start;
				m_Count = count;
			}

			public int Start
			{
				get { return m_Start; }
			}

			public int Size
			{
				get { return m_Count; }
			}

			private readonly int m_Start;
			private readonly int m_Count;
		}

		#endregion


		#region Private Members

		private readonly IVertexBuffer m_Vb = Graphics.Factory.CreateVertexBuffer( );
		private readonly List<VbHandle> m_Handles = new List<VbHandle>( );
		private readonly List<FreeRange> m_FreeList = new List<FreeRange>( );

		/// <summary>
		/// Acquires a free range from the free list
		/// </summary>
		private FreeRange GetFreeRange( int size )
		{
			if ( m_FreeList.Count == 0 )
			{
				//	No space free - compact or die
				Compact( );
				if ( m_FreeList.Count == 0 )
				{
					//	Still empty of free space - AAARGHHHH
					throw new OutOfMemoryException( "Managed vertex buffer ran out of memory. Poor show." );
				}
			}
			FreeRange? range = FindFreeRange( size );
			if ( range == null )
			{
				//	oh dear - no range was big enough to hold the required size.
				Compact( );
				range = FindFreeRange( size );
				if ( range == null )
				{
					throw new OutOfMemoryException( "Managed vertex buffer ran out of memory. Bad luck." );
				}
			}
			return range.Value;
		}

		/// <summary>
		/// Finds a free range that can satisfy the specified size requirement
		/// </summary>
		private FreeRange? FindFreeRange( int size )
		{
			for ( int rangeIndex = 0; rangeIndex < m_FreeList.Count; ++rangeIndex )
			{
				FreeRange range = m_FreeList[ rangeIndex ];
				if ( range.Size == size )
				{
					m_FreeList.RemoveAt( rangeIndex );
					return range;
				}
				else if ( range.Size > size )
				{
					FreeRange lowRange = new FreeRange( range.Start, size );
					FreeRange hiRange = new FreeRange( range.Start + size, range.Size - size );
					m_FreeList.RemoveAt( rangeIndex );
					m_FreeList.Insert( rangeIndex, hiRange );
					return lowRange;
				}
			}
			return null;
		}

		/// <summary>
		/// Compacts the free range heap
		/// </summary>
		private unsafe void Compact( )
		{
			if ( m_Handles.Count == 0 )
			{
				return;
			}
			GraphicsLog.Warning( "Compacting managed vertex buffer..." );
			m_Handles.Sort
			(
				delegate( VbHandle h0, VbHandle h1 )
				{
					return h0.FirstVertexIndex < h1.FirstVertexIndex ? -1 : ( h0.FirstVertexIndex > h1.FirstVertexIndex ? 1 : 0 );
				}
			);
			int start = 0;
			using ( IVertexBufferLock vbLock = m_Vb.Lock( 0, m_Vb.Count, true, true ) )
			{
				byte* mem = vbLock.Bytes;
				int vertexSize = 0;
				for ( int handleIndex = 0; handleIndex < m_Handles.Count; ++handleIndex )
				{
					VbHandle curHandle = m_Handles[ handleIndex ];
					if ( curHandle.FirstVertexIndex > start )
					{
						int amount = curHandle.VertexCount * vertexSize;
						memmove( mem + start * vertexSize, mem + curHandle.FirstVertexIndex * vertexSize, amount );
						curHandle.FirstVertexIndex = start;
					}
					start = curHandle.FirstVertexIndex + curHandle.VertexCount;
				}
			}

			m_FreeList.Clear( );
			m_FreeList.Add( new FreeRange( start, m_Vb.Count - start ) );
		}


		/// <summary>
		/// Releases a VB handle
		/// </summary>
		private void ReleaseHandle( VbHandle handle )
		{
			if ( m_Handles.Remove( handle ) )
			{
				m_FreeList.Add( new FreeRange( handle.FirstVertexIndex, handle.VertexCount ) ); 
			}
		}

		#endregion

		#region Private P/Invoke

		[DllImport("msvcrt.dll")]
		private unsafe static extern byte* memmove( byte* dest, byte* src, int count );

		#endregion

	}
}
