
using System;
using System.Collections.Generic;
using System.Drawing;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Cameras;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.Universe.Classes.Rendering
{
	//	TODO: AP: Error metric (half root error per tier)
	//	

	class TerrainQuadPatch
	{
		public const int VertexResolution = 33;
		public const int VertexArea = VertexResolution * VertexResolution;
		public const int TotalVerticesPerPatch = VertexArea + VertexResolution * 4;

		public const float ErrorThreshold = 6;

		#region Public Construction

		public TerrainQuadPatch( TerrainQuadPatchVertices vertices, Color patchColour, Point3 origin, Vector3 uAxis, Vector3 vAxis ) :
			this( vertices, patchColour, origin, uAxis, vAxis, float.MaxValue, 128.0f )
		{
		}

		#endregion

		#region Public Properties

		public Point3 PatchCentre
		{
			get { return m_Centre; }
		}

		#endregion

		#region Updates and rendering

		public void RequestComplete( )
		{
		}
		
		/// <summary>
		/// Updates the level of detail of this patch. 
		/// </summary>
		public void UpdateLod( Point3 cameraPos )
		{
			if ( m_IncreaseDetailDistance == float.MaxValue )
			{
				//	Detail up distance has not been calculated for this patch yet - exit
				return;
			}
			float distanceToPatch = AccurateDistance( cameraPos, PatchCentre );
			m_DistToPatch = distanceToPatch;
			if ( distanceToPatch < m_IncreaseDetailDistance )
			{
				if ( m_Children == null )
				{
					IncreaseDetail( );
				}
				else
				{
					foreach ( TerrainQuadPatch childPatch in m_Children )
					{
						childPatch.UpdateLod( cameraPos );
					}
				}
			}
			else if ( distanceToPatch > m_IncreaseDetailDistance )
			{
				if ( m_Children != null )
				{
					ReduceDetail( );
				}
			}
		}

		/// <summary>
		/// Updates this patch
		/// </summary>
		public void Update( IProjectionCamera camera, IPlanetTerrain terrain )
		{
			if ( m_Children != null )
			{
				foreach ( TerrainQuadPatch childPatch in m_Children )
				{
					childPatch.Update( camera, terrain );
				}
			}
			else
			{
				if ( m_BuildVertices )
				{
					BuildVertices( camera, terrain );
					m_BuildVertices = false;
				}
				if ( m_BuildIndices )
				{
					BuildIndices( );
					m_BuildIndices = false;
				}
			}
		}

		/// <summary>
		/// Renders this patch
		/// </summary>
		public void Render( )
		{
			if ( m_Children == null )
			{
				m_Ib.Draw( PrimitiveType.TriList );
			}
			else
			{
				for ( int i = 0; i < m_Children.Length; ++i )
				{
					m_Children[ i ].Render( );
				}
			}
		}

		/// <summary>
		/// Renders debug information for this patch
		/// </summary>
		public void DebugRender( )
		{
			if ( m_Children == null )
			{
				Graphics.Fonts.DebugFont.Write( m_Centre.X, m_Centre.Y, m_Centre.Z, FontAlignment.TopRight, Color.White, "{0:F2}/{1:F2}", m_DistToPatch, m_IncreaseDetailDistance );
				Graphics.Draw.Billboard( ms_Brush, m_Centre, 100.0f, 100.0f );
			}
			else
			{
				for ( int i = 0; i < m_Children.Length; ++i )
				{
					m_Children[ i ].DebugRender( );
				}
			}
		}

		#endregion

		#region Private Members

		private readonly TerrainQuadPatchVertices m_Vertices;
		private readonly IIndexBuffer m_Ib = Graphics.Factory.CreateIndexBuffer( );
		private readonly Point3 m_Origin;
		private readonly Vector3 m_UAxis;
		private readonly Vector3 m_VAxis;
		private Point3 m_Centre;
		private float m_PatchError;
		private float m_DistToPatch;
		private float m_IncreaseDetailDistance;
		private int m_VbIndex;
		private TerrainQuadPatch[] m_Children;
		private bool m_BuildVertices;
		private bool m_BuildIndices;
		private readonly float m_UvRes;
		
		private readonly static DrawBase.IBrush ms_Brush;

		static TerrainQuadPatch( )
		{
			ms_Brush = Graphics.Draw.NewBrush( Color.Blue );
			ms_Brush.State.DepthTest = false;
			ms_Brush.State.DepthWrite = false;
		}

		private void IncreaseDetail( )
		{


			m_Vertices.Deallocate( m_VbIndex );

			Vector3 uOffset = m_UAxis * 0.5f;
			Vector3 vOffset = m_VAxis * 0.5f;
			float error = m_PatchError / 2;
		//	float error = float.MaxValue;
			float uvRes = m_UvRes / 2;

			try
			{
				TerrainQuadPatch tl = new TerrainQuadPatch( m_Vertices, Color.Red, m_Origin, uOffset, vOffset, error, uvRes );
				TerrainQuadPatch tr = new TerrainQuadPatch( m_Vertices, Color.Black, m_Origin + uOffset, uOffset, vOffset, error, uvRes );
				TerrainQuadPatch bl = new TerrainQuadPatch( m_Vertices, Color.Black, m_Origin + vOffset, uOffset, vOffset, error, uvRes );
				TerrainQuadPatch br = new TerrainQuadPatch( m_Vertices, Color.Red, m_Origin + uOffset + vOffset, uOffset, vOffset, error, uvRes );

				m_Children = new TerrainQuadPatch[] { tl, tr, bl, br };
			}
			catch ( OutOfMemoryException )
			{
				m_Children = null;
			}
		}

		private void ReduceDetail( )
		{
			for ( int i = 0; i < m_Children.Length; ++i )
			{
				m_Children[ i ].Dispose( );
			}
			m_Children = null;
			m_VbIndex = m_Vertices.Allocate( );
			if ( m_VbIndex != -1 )
			{
				m_BuildVertices = true;
				m_BuildIndices = true;
			}
		}

		private static void BuildSkirtIndexBuffer( ICollection<int> indices, int srcIndex, int srcIndexOffset, int dstIndex, bool flip )
		{
			int res = VertexResolution - 1;
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

		private void BuildIndices( )
		{
			int res = VertexResolution;
			int triRes = res - 1;
			List<int> indices = new List<int>( triRes * triRes * 6 + triRes * 12 );

			//	Add connecting strips to patches of lower levels of detail
			int endRow = triRes;
			int endCol = triRes;

			for ( int row = 0; row < endRow; ++row )
			{
				int index = m_VbIndex + ( row * res );
				for ( int col = 0; col < endCol; ++col )
				{
					indices.Add( index );
					indices.Add( index + 1 );
					indices.Add( index + res );

					indices.Add( index + 1 );
					indices.Add( index + 1 + res );
					indices.Add( index + res );

					++index;
				}
			}

			//	First horizontal skirt
			int skirtIndex = m_VbIndex + VertexArea;
			BuildSkirtIndexBuffer( indices, m_VbIndex, 1, skirtIndex, false );

			//	First vertical skirt
			skirtIndex += VertexResolution;
			BuildSkirtIndexBuffer( indices, m_VbIndex, VertexResolution, skirtIndex, true );

			//	Last horizontal skirt
			skirtIndex += VertexResolution;
			BuildSkirtIndexBuffer( indices, m_VbIndex + VertexResolution * ( VertexResolution - 1 ), 1, skirtIndex, true );
			
			//	Last vertical skirt
			skirtIndex += VertexResolution;
			BuildSkirtIndexBuffer( indices, m_VbIndex + VertexResolution - 1, VertexResolution, skirtIndex, false );

			m_Ib.Create( indices.ToArray( ), true );
		}

		private unsafe void BuildVertices( IProjectionCamera camera, IPlanetTerrain terrain )
		{
			using ( IVertexBufferLock vbLock = m_Vertices.VertexBuffer.Lock( m_VbIndex, VertexArea, false, true ) )
			{
				TerrainVertex* firstVertex = ( TerrainVertex* )vbLock.Bytes;

				Vector3 uStep = m_UAxis / ( VertexResolution - 1 );
				Vector3 vStep = m_VAxis / ( VertexResolution - 1 );

			//	Vector3 uStep = m_UAxis / ( VertexResolution );
			//	Vector3 vStep = m_VAxis / ( VertexResolution );	//	Leave a border for debugging
				
				if ( m_PatchError == float.MaxValue )
				{
					m_Centre = terrain.GenerateTerrainPatchVertices( m_Origin, uStep, vStep, VertexResolution, m_UvRes, firstVertex, out m_PatchError );
					CreateSkirtVertices( firstVertex );
				}
				else
				{
				    m_Centre = terrain.GenerateTerrainPatchVertices( m_Origin, uStep, vStep, VertexResolution, m_UvRes, firstVertex );
					CreateSkirtVertices( firstVertex );
				}

				//	Comment out this line to disable all LOD
				m_IncreaseDetailDistance = DistanceFromError( camera, m_PatchError );
			}
		}

		private unsafe static void CreateSkirtVertices( TerrainVertex* firstVertex )
		{
			int vRes = VertexResolution - 1;

			//	First horizontal skirt
			TerrainVertex* skirtVertex = firstVertex + VertexArea;
			CreateSkirtVertices( firstVertex, 1, skirtVertex );

			//	First vertical skirt
			skirtVertex += VertexResolution;
			CreateSkirtVertices( firstVertex, VertexResolution, skirtVertex );
			
			//	Last horizontal skirt
			skirtVertex += VertexResolution;
			CreateSkirtVertices( firstVertex + vRes * VertexResolution, 1, skirtVertex );
			
			//	Last vertical skirt
			skirtVertex += VertexResolution;
			CreateSkirtVertices( firstVertex + vRes, VertexResolution, skirtVertex );
		}

		private unsafe static void CreateSkirtVertices( TerrainVertex* srcVertex, int srcOffset, TerrainVertex* dstVertex )
		{
			float skirtSize = 100;
			for ( int i = 0; i < VertexResolution; ++i )
			{
				Vector3 offset = srcVertex->Position.ToVector3( ).MakeNormal( ) * -skirtSize;
				srcVertex->CopyTo( dstVertex, offset );

				srcVertex += srcOffset;
				++dstVertex;
			}
		}

		private static float DistanceFromError(IProjectionCamera camera, float error)
		{
			//	Extract frustum from projection matrix:
			//	http://www.opengl.org/resources/faq/technical/transformations.htm
			//	http://www.opengl.org/discussion_boards/ubbthreads.php?ubb=showflat&Number=209274
			float near = camera.PerspectiveZNear;
			float top = Functions.Tan( camera.PerspectiveFovDegrees * Constants.DegreesToRadians * 0.5f ) * near;
			float a = near / top;
			float t = ( ErrorThreshold * 2 ) / Graphics.Renderer.ViewportHeight;
			float c = a / t;
			float d = error * c;

			return d;
		}

		private static float AccurateDistance( Point3 pos0, Point3 pos1 )
		{
			double x = pos0.X - pos1.X;
			double y = pos0.Y - pos1.Y;
			double z = pos0.Z - pos1.Z;
			return ( float )Math.Sqrt( x * x + y * y + z * z );
		}

		private TerrainQuadPatch( TerrainQuadPatchVertices vertices, Color patchColour, Point3 origin, Vector3 uAxis, Vector3 vAxis, float patchError, float uvRes )
		{
			m_Vertices = vertices;
			m_Origin = origin;
			m_UAxis = uAxis;
			m_VAxis = vAxis;

			m_PatchError = patchError;
			m_IncreaseDetailDistance = float.MaxValue;
			m_VbIndex = vertices.Allocate( );
			m_UvRes = uvRes;

			if ( m_VbIndex != -1 )
			{
				m_BuildVertices = true;
				m_BuildIndices = true;
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose( )
		{
			if ( m_VbIndex != -1 )
			{
				m_Vertices.Deallocate( m_VbIndex );
				m_VbIndex = -1;
			}
		}

		#endregion
	}
}
