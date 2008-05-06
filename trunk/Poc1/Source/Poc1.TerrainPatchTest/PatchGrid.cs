using System.Drawing;
using Rb.Rendering.Cameras;
using Rb.Rendering.Interfaces.Objects;
using RbGraphics = Rb.Rendering.Graphics;

namespace Poc1.TerrainPatchTest
{
	internal class PatchGrid : IRenderable
	{
		private const float PatchWidth = 32;
		private const float PatchDepth = 32;

		public PatchGrid( Terrain terrain, int gridWidth, int gridHeight )
		{
			int highestRes = Patch.GetLevelResolution( Patch.HighestDetailLod );
			highestRes *= highestRes;


			VertexBufferFormat format = new VertexBufferFormat( );
			format.Add( VertexFieldSemantic.Position, VertexFieldElementTypeId.Float32, 3 );
			m_Vb = RbGraphics.Factory.CreateVertexBuffer( );
			m_Vb.Create( format, gridWidth * gridHeight * highestRes );
			
			m_Patches = new Patch[ gridWidth, gridHeight ];

			float z = -PatchDepth * ( gridHeight / 2 );
			float xInc = PatchWidth;
			float zInc = PatchDepth;

			float maxWidth = gridWidth * PatchWidth;
			float maxHeight = gridWidth * PatchDepth;

			terrain.SetTerrainArea( maxWidth, maxHeight );

			int vbOffset = 0;

			for ( int row = 0; row < gridHeight; ++row, z += zInc )
			{
				float x = -PatchWidth * ( gridWidth / 2 );
				for ( int col = 0; col < gridWidth; ++col, x += xInc )
				{
					Color c = ( ( col + row ) % 2 ) == 0 ? Color.Black : Color.White;

					m_Patches[ col, row ] = new Patch( terrain, vbOffset, x, z, PatchWidth, PatchDepth, c );
					vbOffset += highestRes;
				}
			}

			int maxCol = gridWidth - 1;
			int maxRow = gridHeight - 1;
			for ( int row = 0; row < gridHeight; ++row )
			{
				for ( int col = 0; col < gridWidth; ++col )
				{
					Patch left	= ( col == 0 ) ? null : ( m_Patches[ col - 1, row ] );
					Patch right	= ( col == maxCol ) ? null : ( m_Patches[ col + 1, row ] );
					Patch up	= ( row == 0 ) ? null : ( m_Patches[ col, row - 1] );
					Patch down	= ( row == maxRow ) ? null : ( m_Patches[ col, row + 1 ] );

					m_Patches[ col, row ].Link( left, right, up, down );
				}
			}
		}

		public float CameraDistanceToPatch
		{
			get { return m_CameraDistanceToPatch; }
			set { m_CameraDistanceToPatch = value; }
		}

		public bool UseCameraDist
		{
			get { return m_UseCameraDist; }
			set { m_UseCameraDist = value; }
		}

		#region IRenderable Members

		private void SetLodCheckboard( int lowLod, int highLod )
		{
			for ( int row = 0; row <= m_Patches.GetUpperBound( 1 ); ++row )
			{
				for ( int col = 0; col <= m_Patches.GetUpperBound( 0 ); ++col )
				{
					m_Patches[ col, row ].SetLod( ( col + row ) % 2 == 0 ? lowLod : highLod );
				}
			}
		}

		public void Render( IRenderContext context )
		{
			if ( !UseCameraDist )
			{
				RbGraphics.Fonts.DebugFont.Write( 0, 0, Color.White, "Using simulated camera distance {0}", CameraDistanceToPatch );
			}
			else
			{
				RbGraphics.Fonts.DebugFont.Write( 0, 0, Color.White, "Using actual camera distance" );	
			}

			foreach ( Patch patch in m_Patches )
			{
			    if ( UseCameraDist )
			    {
			        patch.DetermineLod( ( ( Camera3 )RbGraphics.Renderer.Camera ).Frame.Translation );
			    }
			    else
			    {
			        patch.DetermineLod( CameraDistanceToPatch );
			    }
			}
		//	SetLodCheckboard( 3, 5 );
			
			foreach ( Patch patch in m_Patches )
			{
				patch.Update( m_Vb );
			}

			m_Vb.Begin( );
			foreach ( Patch patch in m_Patches )
			{
				patch.Render( );
			}
			m_Vb.End( );
		}

		#endregion

		#region Private Members

		private float m_CameraDistanceToPatch = 360.0f;
		private bool m_UseCameraDist;
		private readonly Patch[,] m_Patches;
		private readonly IVertexBuffer m_Vb;

		#endregion
	}
}
