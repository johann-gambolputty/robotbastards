using System;
using System.Drawing;
using System.Drawing.Imaging;
using Exocortex.DSP;
using Rb.Core.Maths;

namespace Poc1.Tools.Waves
{
	/// <summary>
	/// Generates
	/// </summary>
	public class WaveAnimationGenerator
	{
		/// <summary>
		/// Generates an animation sequence of wave heightmaps
		/// </summary>
		public Bitmap[] GenerateHeightmapSequence( WaveAnimationParameters parameters )
		{
			if ( parameters == null )
			{
				throw new ArgumentNullException( "parameters" );
			}
			m_Map = null;
			m_InvMap = null;
			Bitmap[] results = new Bitmap[ parameters.Frames ];
			for ( int i = 0; i < parameters.Frames; ++i )
			{
				results[ i ] = GenerateHeightMap( parameters, parameters.Time * ( ( float )i / ( parameters.Frames - 1 ) ), parameters.Time );
			}
			return results;
		}

		/// <summary>
		/// Generates an animation sequence of wave normal maps
		/// </summary>
		public Bitmap[] GenerateNormalMapSequence( WaveAnimationParameters parameters )
		{
			if ( parameters == null )
			{
				throw new ArgumentNullException( "parameters" );
			}
			m_Map = null;
			m_InvMap = null;
			Bitmap[] results = new Bitmap[ parameters.Frames ];
			for ( int i = 0; i < parameters.Frames; ++i )
			{
				results[ i ] = GenerateNormalMap( parameters, parameters.Time * ( ( float )i / ( parameters.Frames - 1 ) ), parameters.Time );
			}
			return results;
		}

		private ComplexF[] 	m_Map;
		private ComplexF[] 	m_InvMap;

		private const float Pi = ( float )Math.PI;
		private static readonly float InvRoot2 = 1.0f / ( float )Math.Sqrt( 2 );

		private static ComplexF ComplexFExp( float iY )
		{
			return new ComplexF( ( float )( Math.Cos( iY ) * Math.E ), ( float )( Math.Sin( iY ) * Math.E ) );
		}

		private static Bitmap CreateNormalMapFromHeightMap( Bitmap heightMap )
		{
			Bitmap normalMap = new Bitmap( heightMap.Width, heightMap.Height, PixelFormat.Format24bppRgb );
			for ( int y = 0; y < normalMap.Height; ++y )
			{
				int uY = y == 0 ? normalMap.Height - 1 : y - 1;
				int dY = ( y + 1 ) % normalMap.Height;

				int lX = normalMap.Width - 1;
				int rX = 1;
				for ( int x = 0; x < normalMap.Width; ++x )
				{
					byte uH = heightMap.GetPixel( x, uY ).R;
					byte dH = heightMap.GetPixel( x, dY ).R;
					byte lH = heightMap.GetPixel( lX, y ).R;
					byte rH = heightMap.GetPixel( rX, y ).R;

					lX = x;
					rX = ( x + 1 ) % normalMap.Width;

					byte slopeX = ( byte )( 128.0f + ( rH - lH ) * 8.0f );
					byte slopeY = ( byte )( 128.0f + ( dH - uH ) * 8.0f );

					normalMap.SetPixel( x, y, Color.FromArgb( slopeX, 0, slopeY ) );
				}
			}
			return normalMap;
		}

		private ComplexF[] GetInvFrequencyMap( WaveAnimationParameters parameters )
		{
			if ( m_InvMap == null )
			{
				ComplexF[] frequencyMap = new ComplexF[ parameters.Width * parameters.Height ];
				GenerateInitialFrequencyMap( parameters, frequencyMap, -1 );				
				m_InvMap = frequencyMap;
			}
			return m_InvMap;
		}

		private ComplexF[] GetFrequencyMap( WaveAnimationParameters parameters )
		{
			if ( m_Map == null )
			{
				ComplexF[] frequencyMap = new ComplexF[ parameters.Width * parameters.Height ];
				GenerateInitialFrequencyMap( parameters, frequencyMap, 1 );
				m_Map = frequencyMap;
			}

			return m_Map;
		}
		
		private Bitmap GenerateNormalMap( WaveAnimationParameters parameters, float t, float maxT )
		{
			int width = parameters.Width;
			int height = parameters.Height;
			ComplexF[] frequencyMap = GetFrequencyMap( parameters );
			ComplexF[] invFrequencyMap = GetInvFrequencyMap( parameters );

			Bitmap bmp = new Bitmap( width, height, PixelFormat.Format24bppRgb );
			ComplexF[] resMap = Generate( frequencyMap, invFrequencyMap, width, height, t, maxT, true );

			Fourier.FFT2( resMap, width, height, FourierDirection.Backward );
			for ( int y = 0; y < height; ++y)
			{
				for ( int x = 0; x < width; ++x )
				{
					byte nX = ( byte )( Math.Max( 0, Math.Min( 256, 128 + resMap[ x + y * width ].Re * 8 ) ) );
					byte nY = ( byte )( Math.Max( 0, Math.Min( 256, 128 + resMap[ x + y * width ].Im * 8 ) ) );
					bmp.SetPixel( x, y, Color.FromArgb( nX, 0, nY ) );
				}
			}

			return bmp;
		}

		/// <summary>
		/// Generates a height map at a given time step
		/// </summary>
		private Bitmap GenerateHeightMap( WaveAnimationParameters parameters, float t, float maxT )
		{
			int width = parameters.Width;
			int height = parameters.Height;
			ComplexF[] frequencyMap = GetFrequencyMap( parameters );
			ComplexF[] invFrequencyMap = GetInvFrequencyMap( parameters );

			Bitmap bmp = new Bitmap( width, height, PixelFormat.Format24bppRgb );
			ComplexF[] resMap = Generate( frequencyMap, invFrequencyMap, width, height, t, maxT, false );

			Fourier.FFT2( resMap, width, height, FourierDirection.Backward );
			for ( int y = 0; y < height; ++y)
			{
				for ( int x = 0; x < width; ++x )
				{
					byte h = ( byte )( Math.Max( 0, Math.Min( 256, 128 + resMap[ x + y * width ].Re ) ) );
					bmp.SetPixel( x, y, Color.FromArgb( h, h, h ) );
				}
			}

			return bmp;
		}
		

		private static ComplexF[] Generate( ComplexF[] frequencyMap, ComplexF[] invFrequencyMap, int width, int height, float t, float maxT, bool normals )
		{
			float repW = 2 * Pi / maxT;
			ComplexF[] resMap = new ComplexF[ frequencyMap.Length ];
			for ( int y = 0; y < height; ++y )
			{
				float kY = GetKComponent( y, height ); //Pi * y / height;
				float kYSqr = kY * kY;
				for ( int x = 0; x < width; ++x )
				{
					float kX = GetKComponent( x, width ); // Pi * x / width;

					//	Calculate dispersion w(k)
					float kLen = ( float )Math.Sqrt( kX * kX + kYSqr );
					float w = ( float )Math.Sqrt( 9.81f * kLen ) + repW;
					w = ( float )Math.Floor( w / repW ) * repW;	//	This creates a continuous loop over time

					ComplexF c = frequencyMap[x + y * width];
					// If wave vectors are distributed over a square, this can be simplified to a mirrored lookup of frequencyMap
					ComplexF invC = invFrequencyMap[ x + y * width ].GetConjugate( );

					//	Multiplied by polar form. Actual value is e^(iw(k)t) :: e^(x+iy) = (cos y + i.sin y).e^x
					ComplexF res = ( c * ComplexFExp( w * t ) ) +( invC * ComplexFExp( -w * t ) );

					if ( normals )
					{
						res *= new ComplexF( kX, kY );
					}

					resMap[ x + y * width ] = res;
				}
			}
			return resMap;
		}

		private static float GetKComponent( int val, int maxVal )
		{
			float cVal = 2 * val;
			return Pi * ( cVal / ( maxVal - 1 ) );
		}

		private static void GenerateInitialFrequencyMap( WaveAnimationParameters parameters, ComplexF[] frequencyMap, float kMul )
		{
			int width = parameters.Width;
			int height = parameters.Height;
			float angle = parameters.WindDirectionDegrees * Constants.DegreesToRadians;
			Vector2 windDir = new Vector2( Functions.Sin( angle ), Functions.Cos( angle ) );
			Random rnd = new Random( );
			for ( int y = 0; y < height; ++y )
			{
				float ky = GetKComponent( y, height ); //Pi * y / height;
				for ( int x = 0; x < width; ++x )
				{
					float kx = GetKComponent( x, width ); //Pi * ( x / width );

					float Er = ( 2.0f * ( float )rnd.NextDouble( ) ) - 1.0f;
					float Ei = ( 2.0f * ( float )rnd.NextDouble( ) ) - 1.0f;

					float P = ( float )Math.Sqrt( GetWaveSpectrum( parameters, windDir, new Vector2( kx * kMul, ky * kMul ) ) );

					float re = InvRoot2 * Er * P;
					float im = InvRoot2 * Ei * P;

					frequencyMap[ x + y * width ] = new ComplexF( re, im );
				}
			}
		}

		private static float GetWaveSpectrum( WaveAnimationParameters parameters, Vector2 windDir, Vector2 k )
		{
			float lenK = k.Length;
			if ( lenK < 0.000001f )
			{
				return 0;
			}

			Vector2 nK = k / lenK;
			float a = parameters.WaveModifier;
			float l = parameters.WindSpeed * parameters.WindSpeed / 10;
			float lenK2 = lenK * lenK;
			float f = ( float )Math.Exp( -1 / ( lenK2 * l * l ) ) / ( lenK2 * lenK2 );
			float wDotK = windDir.Dot( nK );

			return f * wDotK * wDotK * a;
		}
	}
}
