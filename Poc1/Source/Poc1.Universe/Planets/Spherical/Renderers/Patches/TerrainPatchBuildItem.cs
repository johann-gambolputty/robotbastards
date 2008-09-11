using System.Collections.Generic;
using Poc1.Universe.Interfaces.Planets.Renderers.Patches;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects.Cameras;
using ITerrainPatch=Poc1.Universe.Interfaces.Planets.Renderers.Patches.ITerrainPatch;

namespace Poc1.Universe.Planets.Spherical.Renderers.Patches
{
	/// <summary>
	/// Builds terrain patch geometry
	/// </summary>
	public class TerrainPatchBuildItem
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="camera">Camera, used to determine patch error</param>
		/// <param name="generator">Object used to generate the patch vertices</param>
		/// <param name="patch">Terrain patch</param>
		/// <param name="calculatePatchError">If true, the maximum error between the patch geometry and the terrain model is calculated</param>
		public TerrainPatchBuildItem( IProjectionCamera camera, ITerrainPatchGenerator generator, ITerrainPatch patch, bool calculatePatchError )
		{
			m_Camera = camera;
			m_Generator = generator;
			m_Patch = patch;
			m_CalculatePatchError = calculatePatchError;
		}

		/// <summary>
		/// Builds the object
		/// </summary>
		public unsafe void Build( )
		{
			byte[] vertexData = AllocateVertexData( );
			float increaseDetailDistance = DistanceFromError( m_Camera, m_Patch.MaximumPatchError );
			fixed ( byte* vertexDataMem = vertexData )
			{
				TerrainVertex* firstVertex = ( TerrainVertex* )vertexDataMem;

				if ( m_CalculatePatchError )
				{
					float error;
					m_Generator.GenerateTerrainPatchVertices( m_Patch, TerrainPatchConstants.PatchWidth, m_Patch.UvResolution, firstVertex, out error );
					CreateSkirtVertices( firstVertex );
					m_Patch.MaximumPatchError = error;
				}
				else
				{
					m_Generator.GenerateTerrainPatchVertices( m_Patch, TerrainPatchConstants.PatchWidth, m_Patch.UvResolution, firstVertex );
					CreateSkirtVertices( firstVertex );
				}
				m_Patch.OnBuildComplete( vertexDataMem, increaseDetailDistance );
			}
			FreeVertexData( vertexData );
		}

		#region Private Members

		private readonly IProjectionCamera		m_Camera;
		private readonly ITerrainPatchGenerator m_Generator;
		private readonly ITerrainPatch			m_Patch;
		private readonly bool					m_CalculatePatchError;

		private static List<byte[]>				VertexDataPool = new List<byte[]>( );

		/// <summary>
		/// Determines the distance at which a patch LOD should be increased, from the patch error
		/// </summary>
		private static float DistanceFromError( IProjectionCamera camera, float error )
		{
			//	Extract frustum from projection matrix:
			//	http://www.opengl.org/resources/faq/technical/transformations.htm
			//	http://www.opengl.org/discussion_boards/ubbthreads.php?ubb=showflat&Number=209274
			float near = camera.PerspectiveZNear;
			float top = Functions.Tan( camera.PerspectiveFovDegrees * Constants.DegreesToRadians * 0.5f ) * near;
			float a = near / top;
			float t = ( TerrainPatchConstants.ErrorThreshold * 2 ) / Graphics.Renderer.ViewportHeight;
			float c = a / t;
			float d = error * c;

			return d;
		}

		/// <summary>
		/// Frees a vertex data array, returning it to the pool
		/// </summary>
		private static void FreeVertexData( byte[] data )
		{
			lock ( VertexDataPool )
			{
				VertexDataPool.Add( data );
			}
		}

		/// <summary>
		/// Allocates a byte array from the static vertex data pool
		/// </summary>
		private unsafe static byte[] AllocateVertexData( )
		{
			lock ( VertexDataPool )
			{
				if ( VertexDataPool.Count == 0 )
				{
					return new byte[ TerrainPatchConstants.PatchTotalVertexCount * sizeof( TerrainVertex ) ];
				}
				byte[] result = VertexDataPool[ 0 ];
				VertexDataPool.RemoveAt( 0 );
				return result;
			}
		}

		/// <summary>
		/// Creates patch skirt vertices
		/// </summary>
		/// <param name="firstVertex">First vertex in the patch</param>
		private unsafe static void CreateSkirtVertices( TerrainVertex* firstVertex )
		{
			if ( DebugInfo.DisableTerainSkirts )
			{
				return;
			}

			int vRes = TerrainPatchConstants.PatchWidth;

			//	First horizontal skirt
			TerrainVertex* skirtVertex = firstVertex + TerrainPatchConstants.PatchArea;
			CreateSkirtVertices( firstVertex, 1, skirtVertex );

			//	First vertical skirt
			skirtVertex += TerrainPatchConstants.PatchWidth;
			CreateSkirtVertices( firstVertex, TerrainPatchConstants.PatchWidth, skirtVertex );

			//	Last horizontal skirt
			skirtVertex += TerrainPatchConstants.PatchWidth;
			CreateSkirtVertices( firstVertex + vRes * TerrainPatchConstants.PatchWidth, 1, skirtVertex );

			//	Last vertical skirt
			skirtVertex += TerrainPatchConstants.PatchWidth;
			CreateSkirtVertices( firstVertex + vRes, TerrainPatchConstants.PatchWidth, skirtVertex );
		}

		/// <summary>
		/// Creates a strip of patch skirt vertices
		/// </summary>
		/// <param name="srcVertex"></param>
		/// <param name="srcOffset"></param>
		/// <param name="dstVertex"></param>
		private unsafe static void CreateSkirtVertices( TerrainVertex* srcVertex, int srcOffset, TerrainVertex* dstVertex )
		{
			float skirtSize = -10;
			for ( int i = 0; i < TerrainPatchConstants.PatchWidth; ++i )
			{
				Vector3 offset = srcVertex->Normal * skirtSize;
				srcVertex->CopyTo( dstVertex, offset );

				srcVertex += srcOffset;
				++dstVertex;
			}
		}

		#endregion
	}
}
