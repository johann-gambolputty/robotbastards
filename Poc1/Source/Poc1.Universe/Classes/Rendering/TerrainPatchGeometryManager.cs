
using Poc1.Universe.Interfaces.Rendering;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Manages terrain geometry for different levels of detail
	/// </summary>
	/// <seealso cref="ITerrainPatchGeometry"/>
	public class TerrainPatchGeometryManager : ITerrainPatchGeometryManager
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public TerrainPatchGeometryManager( )
		{
			int numVertices = 0;
			for ( int level = 0; level < MaxLodLevels; ++level )
			{
				numVertices += LodLevelPoolSize * NumberOfLevelVertices( level );
			}

			VertexBufferFormat format = new VertexBufferFormat( );
			format.Add( VertexFieldSemantic.Position, VertexFieldElementTypeId.Float32, 3 );
			format.Add( VertexFieldSemantic.Normal, VertexFieldElementTypeId.Float32, 3 );

			GraphicsLog.Info( "Allocating terrain patch builder VBO: {0} vertices in format {1}", numVertices, format );

			m_ManagedVb.Create( format, numVertices );
			//	BABY P SAYS HELLO
		}

		/// <summary>
		/// Allocates a range of vertices and indices from the associated planet buffers
		/// </summary>
		/// <param name="lod">Level of detail to allocate buffers for</param>
		/// <returns>Returns a new ITerrainPatchGeometry object for the specified level of detail</returns>
		public ITerrainPatchGeometry CreateGeometry( int lod )
		{
			int res = GetLevelSize(lod);
			int numVertices = NumberOfLevelVertices( lod );
			ManagedVertexBuffer.VbHandle handle = m_ManagedVb.CreateHandle(numVertices);

			return new TerrainPatchGeometry( lod, res, handle );
		}

		/// <summary>
		/// Releases terrain patch geometry allocated by <see cref="CreateGeometry"/>
		/// </summary>
		/// <param name="geometry">Geometry object to release</param>
		public void ReleaseGeometry( ITerrainPatchGeometry geometry )
		{
			geometry.Dispose( );
		}

		/// <summary>
		/// Sets up the rendering pipeline for patches using geometry created by this manager
		/// </summary>
		public void BeginPatchRendering( )
		{
			m_ManagedVb.Begin( );
		}

		/// <summary>
		/// Reverts the rendering pipeline after <see cref="EndPatchRendering"/>
		/// </summary>
		public void EndPatchRendering( )
		{
			m_ManagedVb.End( );
		}

		#region Private Members

		public const int LowestLodLevel = MaxLodLevels - 1;

		private const int LodLevelPoolSize = 32;
		private const int MaxLodLevels = 5;
		private readonly static int MaxSize = ( int )Functions.Pow( 2, MaxLodLevels + 1 );

		private readonly ManagedVertexBuffer m_ManagedVb = new ManagedVertexBuffer( );

		/// <summary>
		/// Returns the resolution of a patch using the specified level of detail
		/// </summary>
		private static int GetLevelSize( int level )
		{
			return ( MaxSize / ( int )Functions.Pow( 2, level ) ) + 1;
		}

		/// <summary>
		/// Returns the number of vertices required by a patch with the specified level of detail
		/// </summary>
		private static int NumberOfLevelVertices( int level )
		{
			int size = GetLevelSize( level );
			return size * size;
		}

		#endregion
	}
}