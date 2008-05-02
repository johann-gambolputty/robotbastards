
using System.Drawing;
using System.Drawing.Imaging;
using Rb.Core.Maths;

namespace Poc1.TerrainPatchTest
{
	/// <summary>
	/// Stores generated terrain
	/// </summary>
	internal class Terrain
	{
		public Terrain( int res )
		{
			m_Res = res;
			m_Heights = new float[ res, res ];

			float width = 9;
			float height = 9;

			Bitmap bmp = new Bitmap( res, res, PixelFormat.Format24bppRgb );

			float incX = width / ( res - 1 );
			float incZ = height / ( res - 1 );
			float z = 2.35f;
			for ( int row = 0; row < res; ++row, z += incZ )
			{
				float x = 2.35f;
				for ( int col = 0; col < res; ++col, x += incX )
				{
				//	float h = Fractals.SimpleFractal( x, 0.35f, z, 1.3f, 8, 0.8f, Fractals.Noise3dBasis );
					float h = Fractals.RidgedFractal( x, 0.35f, z, 1.3f, 8, 0.8f, Fractals.Noise3dBasis );
					m_Heights[ col, row ] = h;
					byte hC = ( byte )( h * 255.0f );
					bmp.SetPixel( col, row, Color.FromArgb( hC, hC, hC ) );
				}
			}
			bmp.Save( "Terrain.bmp", ImageFormat.Bmp );
		}

		public float GetHeight( float x, float y )
		{
			int iX = Utils.Clamp( ( int )( x * m_Res ), 0, m_Res - 1 );
			int iY = Utils.Clamp( ( int )( y * m_Res ), 0, m_Res - 1 );

			return m_Heights[ iX, iY ] * 20;
		}

		private readonly int m_Res;
		private readonly float[,] m_Heights;
	}
}
