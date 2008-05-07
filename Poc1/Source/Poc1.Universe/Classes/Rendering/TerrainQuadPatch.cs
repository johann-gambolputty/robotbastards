
using System;
using System.Collections.Generic;
using System.Drawing;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Cameras;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.Universe.Classes.Rendering
{
	//	TODO: AP: Error metric (half root error per tier)
	//	

	class TerrainQuadPatch
	{
		public const int VertexResolution = 17;
		public const int VertexArea = VertexResolution * VertexResolution;

		public const float ErrorThreshold = 4;


		#region Public Construction

		public TerrainQuadPatch( TerrainQuadPatchVertices vertices, Color patchColour, Point3 origin, Vector3 uAxis, Vector3 vAxis ) :
			this( vertices, patchColour, origin, uAxis, vAxis, float.MaxValue )
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

		#endregion

		#region Private Members

		private readonly TerrainQuadPatchVertices m_Vertices;
		private readonly IIndexBuffer m_Ib = Graphics.Factory.CreateIndexBuffer( );
		private readonly Point3 m_Origin;
		private readonly Vector3 m_UAxis;
		private readonly Vector3 m_VAxis;
		private Point3 m_Centre;
		private float m_PatchError;
		private float m_IncreaseDetailDistance;
		private int m_VbIndex;
		private TerrainQuadPatch[] m_Children;
		private bool m_BuildVertices;
		private bool m_BuildIndices;

		private void IncreaseDetail( )
		{
			m_Vertices.Deallocate( m_VbIndex );

			Vector3 uOffset = m_UAxis * 0.5f;
			Vector3 vOffset = m_VAxis * 0.5f;
			float error = m_PatchError / 2;

			try
			{
				TerrainQuadPatch tl = new TerrainQuadPatch( m_Vertices, Color.Red, m_Origin, uOffset, vOffset, error );
				TerrainQuadPatch tr = new TerrainQuadPatch( m_Vertices, Color.Black, m_Origin + uOffset, uOffset, vOffset, error );
				TerrainQuadPatch bl = new TerrainQuadPatch( m_Vertices, Color.Black, m_Origin + vOffset, uOffset, vOffset, error );
				TerrainQuadPatch br = new TerrainQuadPatch( m_Vertices, Color.Red, m_Origin + uOffset + vOffset, uOffset, vOffset, error );

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
			m_BuildVertices = true;
			m_BuildIndices = true;
		}

		private void BuildIndices( )
		{
			int res = VertexResolution;
			List<int> indices = new List<int>( res * res * 3 );

			//	Add connecting strips to patches of lower levels of detail
			bool addedLeftStrip = false; // BuildConnectingStripIndexBuffer( m_Left, Side.Left, indices );
			bool addedTopStrip = false; //BuildConnectingStripIndexBuffer( m_Up, Side.Top, indices );
			bool addedRightStrip = false; //BuildConnectingStripIndexBuffer( m_Right, Side.Right, indices );
			bool addedBottomStrip = false; //BuildConnectingStripIndexBuffer( m_Down, Side.Bottom, indices );

			int startRow = addedTopStrip ? 1 : 0;
			int endRow = addedBottomStrip ? res - 2 : res - 1;
			int startCol = addedLeftStrip ? 1 : 0;
			int endCol = addedRightStrip ? res - 2 : res - 1;

			for ( int row = startRow; row < endRow; ++row )
			{
				int index = m_VbIndex + ( row * res ) + startCol;
				for ( int col = startCol; col < endCol; ++col )
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

			m_Ib.Create( indices.ToArray( ), true );
		}

		private unsafe void BuildVertices( IProjectionCamera camera, IPlanetTerrain terrain )
		{
			using ( IVertexBufferLock vbLock = m_Vertices.VertexBuffer.Lock( m_VbIndex, VertexArea, false, true ) )
			{
				TerrainVertex* firstVertex = ( TerrainVertex* )vbLock.Bytes;

				//Vector3 uStep = m_UAxis / ( VertexResolution - 1 );
				//Vector3 vStep = m_VAxis / ( VertexResolution - 1 );

				Vector3 uStep = m_UAxis / ( VertexResolution );
				Vector3 vStep = m_VAxis / ( VertexResolution );	//	Leave a border for debugging
				
				if ( m_PatchError == float.MaxValue )
				{
				    m_Centre = terrain.GenerateTerrainPatchVertices( m_Origin, uStep, vStep, VertexResolution, firstVertex, out m_PatchError );
				}
				else
				{
				    m_Centre = terrain.GenerateTerrainPatchVertices( m_Origin, uStep, vStep, VertexResolution, firstVertex );
				}

				m_IncreaseDetailDistance = DistanceFromError( camera, m_PatchError );
			}
		}

		private static float DistanceFromError( IProjectionCamera camera, float error )
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

		private TerrainQuadPatch( TerrainQuadPatchVertices vertices, Color patchColour, Point3 origin, Vector3 uAxis, Vector3 vAxis, float patchError )
		{
			m_Vertices = vertices;
			m_Origin = origin;
			m_UAxis = uAxis;
			m_VAxis = vAxis;

			m_PatchError = patchError;
			m_IncreaseDetailDistance = float.MaxValue;
			m_VbIndex = vertices.Allocate( );

			m_BuildVertices = true;
			m_BuildIndices = true;
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
