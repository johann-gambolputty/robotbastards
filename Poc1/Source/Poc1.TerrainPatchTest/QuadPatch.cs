
using System;
using System.Collections.Generic;
using System.Drawing;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Cameras;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.TerrainPatchTest
{
	class QuadPatch : IDisposable
	{
		public const int PatchResolution = 17;
		public const int PatchArea = PatchResolution * PatchResolution;
		public const float ErrorThreshold = 4;

		#region Public Construction

		public QuadPatch( QuadPatch parent, Terrain terrain, QuadPatchVertices vertices, Color patchColour, float x, float z, float w, float d, float maxWidth, float maxHeight )
		{
			m_Parent = parent;

			m_Rs.FaceRenderMode = PolygonRenderMode.Lines;
			m_Rs.Colour = patchColour;

			m_MaxWidth = maxWidth;
			m_MaxHeight = maxHeight;

			m_Terrain = terrain;
			m_Vertices = vertices;
			m_X = x;
			m_Z = z;
			m_Width = w;
			m_Depth = d;

			m_IncreaseDetailDistance = float.MaxValue;
			m_VbIndex = vertices.Allocate( );

			m_BuildVertices = true;
			m_BuildIndices = true;
		}

		#endregion

		#region Public Properties

		public Point3 PatchCentre
		{
			get { return Point3.Origin; }
		}

		#endregion

		#region Public LOD updates

		public void UpdateLod( float distance )
		{
			if ( distance < m_IncreaseDetailDistance )
			{
				if ( m_Children == null )
				{
					IncreaseDetail( );
				}
				else
				{
					foreach ( QuadPatch childPatch in m_Children )
					{
						childPatch.UpdateLod( distance );
					}
				}
			}
			else if ( distance > m_IncreaseDetailDistance )
			{
				if ( m_Children != null )
				{
					ReduceDetail( );
				}
			}
		}

		public void UpdateLod( Point3 pt )
		{
			UpdateLod( pt.DistanceTo( PatchCentre ) );
		}

		#endregion

		#region Updates and rendering

		public void Update( IProjectionCamera camera )
		{
			if ( m_Children != null )
			{
				foreach ( QuadPatch childPatch in m_Children )
				{
					childPatch.Update( camera );
				}
			}
			else
			{
				if ( m_BuildVertices )
				{
					m_IncreaseDetailDistance = BuildVertices( camera );
					m_BuildVertices = false;
				}
				if ( m_BuildIndices )
				{
					BuildIndices( );
					m_BuildIndices = false;
				}	
			}
		}

		public void Render( )
		{
			if ( m_Children == null )
			{
				m_Rs.Begin( );
				m_Ib.Draw( PrimitiveType.TriList );
				m_Rs.End( );
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

		private readonly float m_MaxWidth;
		private readonly float m_MaxHeight;
		private readonly QuadPatch m_Parent;
		private readonly Terrain m_Terrain;
		private readonly QuadPatchVertices m_Vertices;
		private readonly IRenderState m_Rs = Graphics.Factory.CreateRenderState( );
		private readonly IIndexBuffer m_Ib = Graphics.Factory.CreateIndexBuffer( );
		private float m_IncreaseDetailDistance;
		private int m_VbIndex;
		private readonly float m_X;
		private readonly float m_Z;
		private readonly float m_Width;
		private readonly float m_Depth;
		private QuadPatch[] m_Children;
		private bool m_BuildVertices;
		private bool m_BuildIndices;

		private void IncreaseDetail( )
		{
			m_Vertices.Deallocate( m_VbIndex );

			float hW = m_Width / 2;
			float hD = m_Depth / 2;

			try
			{
				m_Children = new QuadPatch[ 4 ]
					{
						new QuadPatch( this, m_Terrain, m_Vertices, Color.Red, m_X, m_Z, hW, hD, m_MaxWidth, m_MaxHeight ),
						new QuadPatch( this, m_Terrain, m_Vertices, Color.Black, m_X + hW, m_Z, hW, hD, m_MaxWidth, m_MaxHeight ),
						new QuadPatch( this, m_Terrain, m_Vertices, Color.Black, m_X, m_Z + hD, hW, hD, m_MaxWidth, m_MaxHeight ),
						new QuadPatch( this, m_Terrain, m_Vertices, Color.Red, m_X + hW, m_Z + hD, hW, hD, m_MaxWidth, m_MaxHeight )
					};
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
			int res = PatchResolution;
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

		private unsafe float BuildVertices( IProjectionCamera camera )
		{
			float maxWidth = m_MaxWidth;
			float maxHeight = m_MaxHeight;
			float maxError = 0;
			using ( IVertexBufferLock vbLock = m_Vertices.VertexBuffer.Lock( m_VbIndex, PatchArea, false, true ) )
			{
				PatchVertex* curVertex = ( PatchVertex* )vbLock.Bytes;
				float incX = m_Width / ( PatchResolution - 1 );
				float incZ = m_Depth / ( PatchResolution - 1 );
				float z = m_Z;
				for ( int row = 0; row < PatchResolution; ++row, z += incZ )
				{
					float x = m_X;
					for ( int col = 0; col < PatchResolution; ++col, x += incX )
					{
						float nX = ( x / maxWidth ) + 0.5f;
						float nY = ( z / maxHeight ) + 0.5f;
						float nXInc = incX / maxWidth;
						float nYInc = incZ / maxHeight;

						float curHeight = m_Terrain.GetHeight( nX, nY );
						float nextHeightX = m_Terrain.GetHeight( nX + nXInc, nY );
						float nextHeightY = m_Terrain.GetHeight( nX, nY + nYInc );

						float estXHeight = ( curHeight + nextHeightX ) / 2;
						float estYHeight = ( curHeight + nextHeightY ) / 2;
						float xError = Math.Abs( m_Terrain.GetHeight( nX + nXInc / 2, nY ) - estXHeight );
						float yError = Math.Abs( m_Terrain.GetHeight( nX, nY + nYInc / 2 ) - estYHeight );
						float error = Utils.Max( xError, yError );
						maxError = error > maxError ? error : maxError;

						curVertex->Position = new Point3( x, curHeight, z );
						++curVertex;
					}
				}
			}
			return DistanceFromError( camera, maxError );
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
