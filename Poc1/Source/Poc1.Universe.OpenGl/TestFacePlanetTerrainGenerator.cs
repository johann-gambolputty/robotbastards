
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Poc1.Universe.OpenGl
{
	/// <summary>
	/// Colours planet map faces according to their face value
	/// </summary>
	internal class TestFacePlanetTerrainGenerator : IPlanetTerrainGenerator
	{
		#region IPlanetTerrainGenerator Members

		public unsafe void GenerateSide( PlanetMapFace face, byte* pixels, int width, int height, int stride )
		{
			for ( int row = 0; row < height; ++row )
			{
				byte* curPixel = pixels + row * stride;
				for ( int col = 0; col < width; ++col )
				{
					ColourFace( face, curPixel );
					curPixel += 3;
				}
			}

			using ( Bitmap bmp = new Bitmap( width, height, stride, PixelFormat.Format24bppRgb, ( IntPtr )pixels ) )
			{
				using ( Graphics g = Graphics.FromImage( bmp ) )
				{
					g.DrawString( face.ToString( ), new Font( "Arial", 8 ), Brushes.White, width / 2, height / 2 );
				}
			}
		}

		#endregion

		private static unsafe void ColourFace( PlanetMapFace face, byte* pixel )
		{
			switch ( face )
			{
				case PlanetMapFace.NegX: pixel[ 0 ] = 0xff; pixel[ 1 ] = 0x80; pixel[ 2 ] = 0x80; break;
				case PlanetMapFace.PosX: pixel[ 0 ] = 0xff; pixel[ 1 ] = 0x00; pixel[ 2 ] = 0x00; break;
				case PlanetMapFace.NegY: pixel[ 0 ] = 0x80; pixel[ 1 ] = 0xff; pixel[ 2 ] = 0x80; break;
				case PlanetMapFace.PosY: pixel[ 0 ] = 0x00; pixel[ 1 ] = 0xff; pixel[ 2 ] = 0x00; break;
				case PlanetMapFace.NegZ: pixel[ 0 ] = 0x80; pixel[ 1 ] = 0x80; pixel[ 2 ] = 0xff; break;
				case PlanetMapFace.PosZ: pixel[ 0 ] = 0x00; pixel[ 1 ] = 0x00; pixel[ 2 ] = 0xff; break;
			}
		}

	}
}
