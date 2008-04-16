
using Poc1.Universe.Interfaces.Rendering;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Interface for a handle into a vertex or index buffer, managed by <see cref="ITerrainPatchGeometryManager"/>
	/// </summary>
	public unsafe class TerrainPatchGeometry : ITerrainPatchGeometry
	{
		#region Construction

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="lodLevel">Patch geometry level of detail</param>
		/// <param name="res">Resolution of this patch</param>
		/// <param name="vertexBuffer">Vertex buffer</param>
		/// <param name="firstVertexIndex">Index of the first vertex in the vertex buffer used by this object</param>
		/// <param name="numVertices">Number of vertices in the vertex buffer used by this object</param>
		public TerrainPatchGeometry( int lodLevel, int res, IVertexBuffer vertexBuffer, int firstVertexIndex, int numVertices )
		{
			m_Lod = lodLevel;
			m_Resolution = res;
			m_VertexBuffer = vertexBuffer;
			m_FirstVertexIndex = firstVertexIndex;
			m_VertexCount = numVertices;

			m_IndexBuffer = Graphics.Factory.CreateIndexBuffer( );
		}

		#endregion

		#region Public Members

		/// <summary>
		/// Gets the index of the first vertex used by this patch
		/// </summary>
		public int FirstVertexIndex
		{
			get { return m_FirstVertexIndex; }
		}

		#endregion

		#region ITerrainPatchBuffers Members

		/// <summary>
		/// Gets the level of detail forthis patch
		/// </summary>
		public int LevelOfDetail
		{
			get { return m_Lod; }
		}

		/// <summary>
		/// Gets the resolution of this patch
		/// </summary>
		public int Resolution
		{
			get { return m_Resolution; }
		}

		/// <summary>
		/// Sets up the patch index buffer
		/// </summary>
		public void SetIndexBuffer( int[] indices )
		{
			m_IndexBuffer.Create( indices, true );
		}

		/// <summary>
		/// Locks the vertex buffer
		/// </summary>
		/// <returns>Returns a pointer into the vertex buffer</returns>
		public TerrainVertex* LockVertexBuffer( bool read, bool write )
		{
			UnlockVertexBuffer( );
			m_VertexBufferLock = m_VertexBuffer.Lock( m_FirstVertexIndex, m_VertexCount, read, write );
			return ( TerrainVertex* )m_VertexBufferLock.Bytes;
		}

		/// <summary>
		/// Unlocks the vertex buffer, commiting the changes made by the last <see cref="LockVertexBuffer"/>
		/// </summary>
		public void UnlockVertexBuffer( )
		{
			if ( m_VertexBufferLock != null )
			{
				m_VertexBufferLock.Dispose( );
				m_VertexBufferLock = null;
			}
		}

		#endregion

		#region Private Members

		private readonly int m_Lod;
		private readonly int m_Resolution;
		private readonly int m_FirstVertexIndex;
		private readonly int m_VertexCount;
		private readonly IIndexBuffer m_IndexBuffer;
		private readonly IVertexBuffer m_VertexBuffer;
		private IVertexBufferLock m_VertexBufferLock;

		#endregion
	}
}