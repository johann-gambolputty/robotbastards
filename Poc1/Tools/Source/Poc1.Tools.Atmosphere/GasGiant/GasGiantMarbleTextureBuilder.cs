using System;
using System.Drawing;
using System.Drawing.Imaging;
using Rb.Core.Maths;

namespace Poc1.Tools.Atmosphere.GasGiant
{
	/// <summary>
	/// Builds gas giant textures
	/// </summary>
	public static class GasGiantMarbleTextureBuilder
	{
		public static Bitmap Generate( GasGiantModel model )
		{
			if ( model == null )
			{
				throw new ArgumentNullException( "model" );
			}
			Bitmap bmp = new Bitmap( model.Width, model.Height, PixelFormat.Format24bppRgb );
			for ( int y = 0; y < model.Height; ++y )
			{
				float fY = ( float )y / ( model.Height - 1 );
				for ( int x = 0; x < model.Width; ++x )
				{
					float fX = ( float )x / ( model.Width - 1 );
					float wrapfX = fX - 1;
					float invfX = 1 - fX;

					float v0 = GetValue( model, fX, fY ) * invfX;
					float v1 = GetValue( model, wrapfX, fY ) * fX;
					float v = ( v0 + v1 );

					bmp.SetPixel( x, y, MapValueToColour( model, v ) );
				}
			}
			return bmp;
		}

		private static Color MapValueToColour( GasGiantModel model, float value )
		{
			float cF = value * model.BandColours.Length;
			int c0In = Math.Min( ( int )cF, model.BandColours.Length - 1 );
			Color c0 = model.BandColours[ c0In ];
			Color c1 = model.BandColours[ Math.Min( c0In + 1, model.BandColours.Length - 1 ) ];
			float t = ( cF - c0In );
			float r = c0.R + ( c1.R - c0.R ) * t;
			float g = c0.G + ( c1.G - c0.G ) * t;
			float b = c0.B + ( c1.B - c0.B ) * t;
			return Color.FromArgb( ( int )r, ( int )g, ( int )b );
		}

		private static float GetValue( GasGiantModel model, float x, float y )
		{
			float n0 = s_Noise.GetNoise( 2.0098f + x * model.Pass0XNoiseScale, 5.0973502375f + y * model.Pass0YNoiseScale, 0 );
			float res = s_Noise.GetNoise( 2.0098f + n0 * model.Pass1XNoiseMultiplier + x * model.Pass1XNoiseScale, n0 * n0 * model.Pass1YNoiseMultiplier + y * model.Pass1YNoiseScale, 0 );
			res = ( res + 1.0f ) / 2.0f;
			return res;
		}

		private static Noise s_Noise = new Noise( );
	}
}
