using System.Collections.Generic;
using System.ComponentModel;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects.Cameras;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Builds terrain quad patches in a separate thread
	/// </summary>
	internal class TerrainQuadPatchBuilder
	{
		/// <summary>
		/// Gets the singleton instance of this class
		/// </summary>
		public static TerrainQuadPatchBuilder Instance
		{
			get { return ms_Instance; }
		}

		/// <summary>
		/// Builds vertices for a terrain patch
		/// </summary>
		public void BuildVertices( TerrainQuadPatch patch, IPlanetTerrain terrain, IProjectionCamera camera, bool calculatePatchError )
		{
			RequestResult result = HandleBuildVertices( patch, terrain, camera, calculatePatchError );
			result.BuildPatch( );
			ReleaseResult( result );
		}
		
		private static float DistanceFromError( IProjectionCamera camera, float error )
		{
			//	Extract frustum from projection matrix:
			//	http://www.opengl.org/resources/faq/technical/transformations.htm
			//	http://www.opengl.org/discussion_boards/ubbthreads.php?ubb=showflat&Number=209274
			float near = camera.PerspectiveZNear;
			float top = Functions.Tan( camera.PerspectiveFovDegrees * Constants.DegreesToRadians * 0.5f ) * near;
			float a = near / top;
			float t = ( TerrainQuadPatch.ErrorThreshold * 2 ) / Graphics.Renderer.ViewportHeight;
			float c = a / t;
			float d = error * c;

			return d;
		}

		/// <summary>
		/// Adds a request to generate vertices for 4 child patches of a given patch
		/// </summary>
		public void AddRequest( TerrainQuadPatch patch, IPlanetTerrain terrain, IProjectionCamera camera, bool calculatePatchError )
		{
			lock ( m_RequestList )
			{
				m_RequestList.Add( new Request( patch, terrain, camera, calculatePatchError ) );

				if ( !m_Worker.IsBusy )
				{
					m_Worker.RunWorkerAsync( );
				}
			}
		}

		#region Private Members

		#region RequestResult class

		private class RequestResult
		{
			public unsafe RequestResult( TerrainQuadPatch patch )
			{
				m_Patch = patch;
				m_VertexData = new byte[ TerrainQuadPatch.TotalVerticesPerPatch * sizeof( TerrainVertex ) ];
			}

			public byte[] VertexData
			{
				get { return m_VertexData; }
			}

			public TerrainQuadPatch Patch
			{
				set { m_Patch = value; }
			}

			public void Setup( float increaseDetailDistance )
			{
				m_IncreaseDetailDistance = increaseDetailDistance;
			}

			public unsafe void BuildPatch( )
			{
				fixed ( byte* vertexData = m_VertexData )
				{
					m_Patch.OnBuildComplete( vertexData, m_IncreaseDetailDistance );
				}
			}

			private float m_IncreaseDetailDistance;
			private TerrainQuadPatch m_Patch;
			private readonly byte[] m_VertexData;
		}

		#endregion

		#region Request Class

		private class Request
		{
			public Request( TerrainQuadPatch patch, IPlanetTerrain terrain, IProjectionCamera camera, bool calculatePatchError )
			{
				m_Patch = patch;
				m_Terrain = terrain;
				m_Camera = camera;
				m_CalculatePatchError = calculatePatchError;
			}

			public IPlanetTerrain Terrain
			{
				get { return m_Terrain; }
			}

			public TerrainQuadPatch Patch
			{
				get { return m_Patch; }
			}

			public bool CalculatePatchError
			{
				get { return m_CalculatePatchError; }
			}

			public IProjectionCamera Camera
			{
				get { return m_Camera; }
			}

			#region Private Members

			private readonly TerrainQuadPatch m_Patch;
			private readonly IPlanetTerrain m_Terrain;
			private readonly bool m_CalculatePatchError;
			private readonly IProjectionCamera m_Camera;

			#endregion
		}

		#endregion

		private readonly static TerrainQuadPatchBuilder ms_Instance = new TerrainQuadPatchBuilder( );
		private readonly List<Request> m_RequestList = new List<Request>( );
		private readonly List<RequestResult> m_FreeResults = new List<RequestResult>();
		private readonly BackgroundWorker m_Worker;

		private RequestResult GetNewResult( TerrainQuadPatch patch )
		{
			lock ( m_FreeResults )
			{
				if ( m_FreeResults.Count == 0 )
				{
					return new RequestResult( patch );
				}
				RequestResult result = m_FreeResults[ 0 ];
				m_FreeResults.RemoveAt( 0 );
				result.Patch = patch;
				return result;
			}
		}

		private void ReleaseResult( RequestResult result )
		{
			lock ( m_FreeResults )
			{
				m_FreeResults.Add( result );
			}
		}

		private TerrainQuadPatchBuilder( )
		{
			BackgroundWorker worker = new BackgroundWorker( );
			worker.DoWork += ProcessRequest;
			worker.RunWorkerCompleted += RequestComplete;
			m_Worker = worker;
		}

		private void ProcessRequest( object sender, DoWorkEventArgs args )
		{
			Request request;
			lock ( m_RequestList )
			{
				request = m_RequestList[ 0 ];
				m_RequestList.RemoveAt( 0 );
			}

			args.Result = HandleBuildVertices( request.Patch, request.Terrain, request.Camera, request.CalculatePatchError );
		}

		private void RequestComplete( object sender, RunWorkerCompletedEventArgs args )
		{
			RequestResult completed = ( RequestResult )args.Result;
			completed.BuildPatch( );
			ReleaseResult( completed );
			if ( m_RequestList.Count > 0 )
			{
				m_Worker.RunWorkerAsync( );
			}
		}
		
		/// <summary>
		/// Builds vertices for a terrain patch
		/// </summary>
		private unsafe RequestResult HandleBuildVertices( TerrainQuadPatch patch, IPlanetTerrain terrain, IProjectionCamera camera, bool calculatePatchError )
		{
			RequestResult result = GetNewResult( patch );
			fixed ( byte* vertexBytes = result.VertexData )
			{
				TerrainVertex* firstVertex = ( TerrainVertex* )vertexBytes;

				if ( calculatePatchError )
				{
					float error;
					terrain.GenerateTerrainPatchVertices( patch, TerrainQuadPatch.VertexResolution, patch.UvResolution, firstVertex, out error );
					CreateSkirtVertices( firstVertex );
					patch.PatchError = error * 4;
				}
				else
				{
					terrain.GenerateTerrainPatchVertices( patch, TerrainQuadPatch.VertexResolution, patch.UvResolution, firstVertex );
					CreateSkirtVertices( firstVertex );
				}
				result.Setup( DistanceFromError( camera, patch.PatchError ) );
			}

			return result;
		}

		private unsafe static void CreateSkirtVertices( TerrainVertex* firstVertex )
		{
			int vRes = TerrainQuadPatch.VertexResolution - 1;

			//	First horizontal skirt
			TerrainVertex* skirtVertex = firstVertex + TerrainQuadPatch.VertexArea;
			CreateSkirtVertices( firstVertex, 1, skirtVertex );

			//	First vertical skirt
			skirtVertex += TerrainQuadPatch.VertexResolution;
			CreateSkirtVertices(firstVertex, TerrainQuadPatch.VertexResolution, skirtVertex);
			
			//	Last horizontal skirt
			skirtVertex += TerrainQuadPatch.VertexResolution;
			CreateSkirtVertices( firstVertex + vRes * TerrainQuadPatch.VertexResolution, 1, skirtVertex );
			
			//	Last vertical skirt
			skirtVertex += TerrainQuadPatch.VertexResolution;
			CreateSkirtVertices( firstVertex + vRes, TerrainQuadPatch.VertexResolution, skirtVertex );
		}

		private unsafe static void CreateSkirtVertices( TerrainVertex* srcVertex, int srcOffset, TerrainVertex* dstVertex )
		{
			float skirtSize = 0;
			for ( int i = 0; i < TerrainQuadPatch.VertexResolution; ++i )
			{
				Vector3 offset = srcVertex->Position.ToVector3( ).MakeNormal( ) * -skirtSize;
				srcVertex->CopyTo( dstVertex, offset );

				srcVertex += srcOffset;
				++dstVertex;
			}
		}

		#endregion
	}
}
