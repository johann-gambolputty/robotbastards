
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
		/// <param name="handle">Manager vertex buffer handle</param>
		public TerrainPatchGeometry( int lodLevel, int res, ManagedVertexBuffer.VbHandle handle )
		{
			m_Lod = lodLevel;
			m_Resolution = res;
			m_VbHandle = handle;
			m_IndexBuffer = Graphics.Factory.CreateIndexBuffer( );
		}

		#endregion

		#region Public Members

		public ManagedVertexBuffer.VbHandle VbHandle
		{
			get { return m_VbHandle; }
		}

		#endregion

		#region ITerrainPatchGeometry Members

		/// <summary>
		/// Gets the index of the first vertex used by this patch
		/// </summary>
		public int FirstVertexIndex
		{
			get { return m_VbHandle.FirstVertexIndex; }
		}

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
		public void SetIndexBuffer( PrimitiveType primType, int[] indices )
		{
			m_PrimitiveType = primType;
			m_IndexBuffer.Create( indices, true );
		}

		/// <summary>
		/// Locks the vertex buffer
		/// </summary>
		/// <returns>Returns a pointer into the vertex buffer</returns>
		public TerrainVertex* LockVertexBuffer( bool read, bool write )
		{
			return ( TerrainVertex* )m_VbHandle.Lock( read, write );
		}

		/// <summary>
		/// Unlocks the vertex buffer, commiting the changes made by the last <see cref="LockVertexBuffer"/>
		/// </summary>
		public void UnlockVertexBuffer( )
		{
			m_VbHandle.Unlock( );
		}

		/// <summary>
		/// Renders this patch geometry
		/// </summary>
		public void Draw( )
		{
			m_IndexBuffer.Draw( m_PrimitiveType );
		}

		#endregion
		
		#region IDisposable Members

		/// <summary>
		/// Disposes of this object
		/// </summary>
		public void Dispose( )
		{
			m_VbHandle.Dispose( );
		}

		#endregion

		#region Private Members

		private readonly int m_Lod;
		private readonly int m_Resolution;
		private readonly ManagedVertexBuffer.VbHandle m_VbHandle;
		private PrimitiveType m_PrimitiveType;
		private readonly IIndexBuffer m_IndexBuffer;

		#endregion


	}
}