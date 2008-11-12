using System.Collections.Generic;
using Poc1.Universe.Interfaces.Planets.Renderers.Patches;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Core.Maths;
using Rb.Core.Threading;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects.Cameras;
using ITerrainPatch=Poc1.Universe.Interfaces.Planets.Renderers.Patches.ITerrainPatch;

namespace Poc1.Universe.Planets.Spherical.Renderers.Patches
{
	/// <summary>
	/// Builds terrain patch geometry
	/// </summary>
	/// <remarks>
	/// Build items are allocated from a free list. Access to this list is not thread-safe, so allocations
	/// and deallocations must take place on the same thread.
	/// When its time for a build item to be built (<see cref="StartBuild"/>), it allocates vertex data from another
	/// free list. This it then fills, using the terrain model to generate the vertex data. Once this operation is
	/// complete, <see cref="FinishBuild"/> passes the results back to the patch that created the build item.
	/// </remarks>
	public class TerrainPatchBuildItem : IWorkItem
	{

		/// <summary>
		/// Runs the build. This can be run asynchronously. Call FinishBuild() on completion to commit the results
		/// to the target terrain patch.
		/// </summary>
		public unsafe void StartBuild( )
		{
			m_VertexData = AllocateVertexData( );
			fixed ( byte* vertexDataMem = m_VertexData )
			{
				TerrainVertex* firstVertex = ( TerrainVertex* )vertexDataMem;

				if ( m_CalculatePatchError )
				{
					float error;
					m_Generator.GenerateTerrainPatchVertices( m_Patch, TerrainPatchConstants.PatchResolution, firstVertex, out error );
					CreateSkirtVertices( firstVertex );
					m_Patch.PatchError = error;
				}
				else
				{
					m_Generator.GenerateTerrainPatchVertices( m_Patch, TerrainPatchConstants.PatchResolution, firstVertex );
					CreateSkirtVertices( firstVertex );
				}
			}

			m_IncreaseDetailDistance = DistanceFromError( m_Camera, m_Patch.PatchError );
		}

		/// <summary>
		/// Calls the synchronous part of the build
		/// </summary>
		/// <remarks>
		/// This is a bit crap - when FinishBuild() is done, this build item is placed onto the build item free list
		/// </remarks>
		public unsafe void FinishBuild( )
		{
			int[] srcIndices = DebugInfo.DisableTerainSkirts ? s_BaseIndexArray : s_BaseSkirtIndexArray;
			int[] indices = DebugInfo.DisableTerainSkirts ? s_IndexArray : s_SkirtIndexArray;

			for ( int index = 0; index < indices.Length; ++index )
			{
				indices[ index ] = srcIndices[ index ];
			}

			fixed ( byte* vertexDataMem = m_VertexData )
			{
				m_Patch.OnBuildComplete( vertexDataMem, m_IncreaseDetailDistance, indices );
			}
			FreeVertexData( m_VertexData );
			Deallocate( this );
		}

		/// <summary>
		/// Blocking build
		/// </summary>
		public unsafe void Build( )
		{
			StartBuild( );
			FinishBuild( );
		}

		/// <summary>
		/// Allocates a terrain patch build item from an internal pool
		/// </summary>
		public static TerrainPatchBuildItem Allocate( IProjectionCamera camera, ITerrainPatchGenerator generator, ITerrainPatch patch, bool calculatePatchError )
		{
			if ( s_BuildItems.Count == 0 )
			{
				return new TerrainPatchBuildItem( camera, generator, patch, calculatePatchError );
			}
			TerrainPatchBuildItem item = s_BuildItems[ 0 ];
			s_BuildItems.RemoveAt( 0 );

			item.m_Camera = camera;
			item.m_Generator = generator;
			item.m_Patch = patch;
			item.m_CalculatePatchError = calculatePatchError;

			return item;
		}

		/// <summary>
		/// Returns a build item to the internal pool
		/// </summary>
		public static void Deallocate( TerrainPatchBuildItem buildItem )
		{
			s_BuildItems.Add( buildItem );
		}

		#region Private Members

		private IProjectionCamera		m_Camera;
		private ITerrainPatchGenerator	m_Generator;
		private ITerrainPatch			m_Patch;
		private bool					m_CalculatePatchError;

		private float					m_IncreaseDetailDistance;
		private byte[]					m_VertexData;

		private static int[]								s_BaseIndexArray = BuildBaseIndexArray( true );
		private static int[]								s_BaseSkirtIndexArray = BuildBaseIndexArray( true );
		
		private static int[]								s_IndexArray;
		private static int[]								s_SkirtIndexArray;

		private static readonly List<byte[]>				s_VertexDataPool = new List<byte[]>( );
		private static readonly List<TerrainPatchBuildItem> s_BuildItems = new List<TerrainPatchBuildItem>( );

		/// <summary>
		/// Frees a vertex data array, returning it to the pool
		/// </summary>
		private static void FreeVertexData( byte[] data )
		{
			lock ( s_VertexDataPool )
			{
				s_VertexDataPool.Add( data );
			}
		}

		/// <summary>
		/// Allocates a byte array from the static vertex data pool
		/// </summary>
		private unsafe static byte[] AllocateVertexData( )
		{
			lock ( s_VertexDataPool )
			{
				if ( s_VertexDataPool.Count == 0 )
				{
					return new byte[ TerrainPatchConstants.PatchTotalVertexCount * sizeof( TerrainVertex ) ];
				}
				byte[] result = s_VertexDataPool[ 0 ];
				s_VertexDataPool.RemoveAt( 0 );
				return result;
			}
		}

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
		/// Creates patch skirt vertices
		/// </summary>
		/// <param name="firstVertex">First vertex in the patch</param>
		private unsafe static void CreateSkirtVertices( TerrainVertex* firstVertex )
		{
			if ( DebugInfo.DisableTerainSkirts )
			{
				return;
			}

			int vRes = TerrainPatchConstants.PatchResolution - 1;

			//	First horizontal skirt
			TerrainVertex* skirtVertex = firstVertex + TerrainPatchConstants.PatchArea;
			CreateSkirtVertices( firstVertex, 1, skirtVertex );

			//	First vertical skirt
			skirtVertex += TerrainPatchConstants.PatchResolution;
			CreateSkirtVertices( firstVertex, TerrainPatchConstants.PatchResolution, skirtVertex );

			//	Last horizontal skirt
			skirtVertex += TerrainPatchConstants.PatchResolution;
			CreateSkirtVertices( firstVertex + vRes * TerrainPatchConstants.PatchResolution, 1, skirtVertex );

			//	Last vertical skirt
			skirtVertex += TerrainPatchConstants.PatchResolution;
			CreateSkirtVertices( firstVertex + vRes, TerrainPatchConstants.PatchResolution, skirtVertex );
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
			for ( int i = 0; i < TerrainPatchConstants.PatchResolution; ++i )
			{
				Vector3 offset = srcVertex->Normal * skirtSize;
				srcVertex->CopyTo( dstVertex, offset );

				srcVertex += srcOffset;
				++dstVertex;
			}
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="camera">Camera, used to determine patch error</param>
		/// <param name="generator">Object used to generate the patch vertices</param>
		/// <param name="patch">Terrain patch</param>
		/// <param name="calculatePatchError">If true, the maximum error between the patch geometry and the terrain model is calculated</param>
		private unsafe TerrainPatchBuildItem( IProjectionCamera camera, ITerrainPatchGenerator generator, ITerrainPatch patch, bool calculatePatchError )
		{
			m_Camera = camera;
			m_Generator = generator;
			m_Patch = patch;
			m_CalculatePatchError = calculatePatchError;
		}

		/// <summary>
		/// Builds index data for a patch
		/// </summary>
		private static int[] BuildBaseIndexArray( bool addSkirts )
		{
			int res = TerrainPatchConstants.PatchResolution;
			int triRes = res - 1;
			List<int> indices = new List<int>( triRes * triRes * 6 + triRes * 12 );

			//	Add connecting strips to patches of lower levels of detail
			int endRow = triRes;
			int endCol = triRes;

			//   *--*--*
			//   | /| /|
			//   *--*--*
			//   | \| \|
			//   *--*--*

			//	TODO: AP: Change to strips?
			for ( int row = 0; row < endRow; ++row )
			{
				bool upSlope = ( row % 2 ) != 0;
				int index = ( row * res );
				for ( int col = 0; col < endCol; ++col )
				{
					if ( upSlope )
					{
						indices.Add( index );
						indices.Add( index + 1 );
						indices.Add( index + res );

						indices.Add( index + 1 );
						indices.Add( index + 1 + res );
						indices.Add( index + res );
					}
					else
					{
						indices.Add( index );
						indices.Add( index + 1 );
						indices.Add( index + res + 1 );

						indices.Add( index );
						indices.Add( index + 1 + res );
						indices.Add( index + res );	
					}

					++index;
				}
			}

			if ( addSkirts )
			{
				//	First horizontal skirt
				int skirtIndex = TerrainPatchConstants.PatchArea;
				BuildSkirtIndexBuffer( indices, 0, 1, skirtIndex, false );

				//	First vertical skirt
				skirtIndex += TerrainPatchConstants.PatchResolution;
				BuildSkirtIndexBuffer( indices, 0, TerrainPatchConstants.PatchResolution, skirtIndex, true );

				//	Last horizontal skirt
				skirtIndex += TerrainPatchConstants.PatchResolution;
				BuildSkirtIndexBuffer( indices, TerrainPatchConstants.PatchResolution * ( TerrainPatchConstants.PatchResolution - 1 ), 1, skirtIndex, true );

				//	Last vertical skirt
				skirtIndex += TerrainPatchConstants.PatchResolution;
				BuildSkirtIndexBuffer( indices, TerrainPatchConstants.PatchResolution - 1, TerrainPatchConstants.PatchResolution, skirtIndex, false );
			}

			return indices.ToArray( );
		}

		/// <summary>
		/// Builds a skirt index buffer
		/// </summary>
		private static void BuildSkirtIndexBuffer( ICollection<int> indices, int srcIndex, int srcIndexOffset, int dstIndex, bool flip )
		{
			int res = TerrainPatchConstants.PatchResolution - 1;
			for ( int i = 0; i < res; ++i )
			{
				indices.Add( srcIndex );
				if ( flip )
				{
					indices.Add( srcIndex + srcIndexOffset );
					indices.Add( dstIndex );
				}
				else
				{
					indices.Add( dstIndex );
					indices.Add( srcIndex + srcIndexOffset );
				}

				indices.Add( dstIndex );

				if ( flip )
				{
					indices.Add( srcIndex + srcIndexOffset );
					indices.Add( dstIndex + 1 );
				}
				else
				{
					indices.Add( dstIndex + 1 );
					indices.Add( srcIndex + srcIndexOffset );
				}

				++dstIndex;
				srcIndex += srcIndexOffset;
			}
		}

		static TerrainPatchBuildItem( )
		{
			s_BaseIndexArray = BuildBaseIndexArray( true );
			s_BaseSkirtIndexArray = BuildBaseIndexArray( true );

			s_IndexArray = new int[ s_BaseIndexArray.Length ];
			s_SkirtIndexArray = new int[ s_BaseSkirtIndexArray.Length ];
		}

		#endregion

		#region IWorkItem Members

		/// <summary>
		/// Work item name
		/// </summary>
		public string Name
		{
			get { return "Terrain Patch Build Item"; }
		}

		/// <summary>
		/// Builds the terrain patch
		/// </summary>
		public void DoWork( IProgressMonitor progress )
		{
			StartBuild( );
		}

		/// <summary>
		/// Called when work has failed
		/// </summary>
		public void WorkFailed( IProgressMonitor progress, System.Exception ex )
		{
			//	Should never happen... just eat the exception
		}

		/// <summary>
		/// Called when work is complete
		/// </summary>
		public void WorkComplete( IProgressMonitor progress )
		{
			FinishBuild( );
		}

		#endregion
	}
}
