
using System.Collections.Generic;
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

			m_VertexBuffer = Graphics.Factory.CreateVertexBuffer( );
			m_VertexBuffer.Create( format, numVertices );

			int curVertexIndex = 0;
			for ( int level = 0; level < MaxLodLevels; ++level )
			{
				Lod newLod = new Lod( );
				m_LodLevels[ level ] = newLod;

				int numLevelVertices = NumberOfLevelVertices( level );
				int poolSize = LodLevelPoolSize * ( level + 1 );
				for ( int poolIndex = 0; poolIndex < poolSize; ++poolIndex )
				{
					newLod.VbPool.Add( curVertexIndex );
					curVertexIndex += numLevelVertices;
				}
				//	BABY P SAYS HELLO
			}
		}

		/// <summary>
		/// Allocates a range of vertices and indices from the associated planet buffers
		/// </summary>
		/// <param name="lod">Level of detail to allocate buffers for</param>
		/// <returns>Returns a new ITerrainPatchGeometry object for the specified level of detail</returns>
		public ITerrainPatchGeometry CreateGeometry( int lod )
		{
			if ( m_LodLevels[ lod ].VbPool.Count == 0 )
			{
				return null;
			}
			List<int> levelIndexes = m_LodLevels[ lod ].VbPool;
			int firstIndex = levelIndexes[ 0 ];
			levelIndexes.RemoveAt( 0 );

			int res = GetLevelSize( lod ); 

			return new TerrainPatchGeometry( lod, res, m_VertexBuffer, firstIndex, NumberOfLevelVertices( lod ) );
		}

		/// <summary>
		/// Releases terrain patch geometry allocated by <see cref="CreateGeometry"/>
		/// </summary>
		/// <param name="geometry">Geometry object to release</param>
		public void ReleaseGeometry( ITerrainPatchGeometry geometry )
		{
			m_LodLevels[ geometry.LevelOfDetail ].VbPool.Add( ( ( TerrainPatchGeometry )geometry ).FirstVertexIndex );
		}

		/// <summary>
		/// Sets up the rendering pipeline for patches using geometry created by this manager
		/// </summary>
		public void BeginPatchRendering( )
		{
			m_VertexBuffer.Begin( );
		}

		/// <summary>
		/// Reverts the rendering pipeline after <see cref="EndPatchRendering"/>
		/// </summary>
		public void EndPatchRendering( )
		{
			m_VertexBuffer.End( );
		}

		#region Private Members

		#region Lod Private Class

		/// <summary>
		/// Manages a vertex index pool for a given level of detail
		/// </summary>
		private class Lod
		{
			/// <summary>
			/// Gets the list of available vertex indices
			/// </summary>
			public List<int> VbPool
			{
				get { return m_VbPool; }
			}

			#region Private Members

			private readonly List<int> m_VbPool = new List<int>( );

			#endregion
		}

		#endregion

		public const int LowestLodLevel = MaxLodLevels - 1;

		private const int LodLevelPoolSize = 32;
		private const int MaxLodLevels = 5;
		private readonly static int MaxSize = ( int )Functions.Pow( 2, MaxLodLevels + 1 );

		private readonly IVertexBuffer m_VertexBuffer;
		private readonly Lod[] m_LodLevels = new Lod[ MaxLodLevels ];

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