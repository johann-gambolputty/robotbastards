using System;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Noise generation
	/// </summary>
	/// <remarks>
	/// From http://mrl.nyu.edu/~perlin/paper445.pdf
	/// </remarks>
	public class Noise
	{
		/// <summary>
		/// Noise singleton
		/// </summary>
		public static Noise Instance
		{
			get { return s_Instance; }
		}

		/// <summary>
		/// Noise constructor
		/// </summary>
		public Noise( ) :
			this( new Random( ) )
		{
		}

		/// <summary>
		/// Noise constructor
		/// </summary>
		/// <param name="seed">Noise seed value</param>
		public Noise( int seed ) :
			this( new Random( seed ) )
		{
		}

		/// <summary>
		/// Returns a noise value given the input value x
		/// </summary>
		/// <returns>Noise value in the range [-1,1]</returns>
		public float GetNoise( float x )
		{
			int iX = ( int )x;
			return NoiseBasis( iX ) / 2 + NoiseBasis( iX - 1 ) / 4 + NoiseBasis( iX + 1 ) / 4;
		}

		/// <summary>
		/// Returns a noise value given the input vector (x,y)
		/// </summary>
		/// <returns>Noise value in the range [-1,1]</returns>
		public float GetNoise( float x, float y )
		{
			int iX = ( int )x;
			float fX = x - iX;
			int iY = ( int )y;
			float fY = y - iY;
			iX = iX & 0xff;
			iY = iY & 0xff;

			float u = Fade( fX );
			float v = Fade( fY );

			int py0 = Perm( iY ) + iX;
			int py1 = Perm( iY + 1 ) + iX;
			int x0y0 = Perm( py0 );
			int x1y0 = Perm( py0 + 1 );
			int x1y1 = Perm( py1 + 1 );
			int x0y1 = Perm( py1 );

			float res =
				Lerp
				(
					v,
					Lerp( u, Gradient( x0y0, fX, fY ), Gradient( x1y0, fX - 1, fY ) ),
					Lerp( u, Gradient( x0y1, fX, fY - 1 ), Gradient( x1y1, fX - 1, fY - 1 ) )
				);

			return res / 0.8f;
		}

		/// <summary>
		/// Returns a noise value given the input position (x,y,z). 
		/// </summary>
		/// <returns>Noise value in the range [-1,1]</returns>
		public float GetNoise( float x, float y, float z )
		{
			int X = ( int )Math.Floor( x ) & 255,                  // FIND UNIT CUBE THAT
				Y = ( int )Math.Floor( y ) & 255,                  // CONTAINS POINT.
				Z = ( int )Math.Floor( z ) & 255;
			x -= ( float )Math.Floor( x );                                // FIND RELATIVE X,Y,Z
			y -= ( float )Math.Floor( y );                                // OF POINT IN CUBE.
			z -= ( float )Math.Floor( z );
			float u = Fade( x ),                                // COMPUTE FADE CURVES
				   v = Fade( y ),                                // FOR EACH OF X,Y,Z.
				   w = Fade( z );
			int A = Perm( X ) + Y,
				AA = Perm( A ) + Z,
				AB = Perm( A + 1 ) + Z,      // HASH COORDINATES OF
				B = Perm( X + 1 ) + Y,
				BA = Perm( B ) + Z,
				BB = Perm( B + 1 ) + Z;      // THE 8 CUBE CORNERS,

			float res = Lerp( w,
							Lerp( v,
								Lerp( u,
									grad( Perm( AA ), x, y, z ),
									grad( Perm( BA ), x - 1, y, z ) ), // BLENDED
								Lerp( u,
									grad( Perm( AB ), x, y - 1, z ),  // RESULTS
									grad( Perm( BB ), x - 1, y - 1, z ) ) ),// FROM  8
							Lerp( v,
								Lerp( u,
									grad( Perm( AA + 1 ), x, y, z - 1 ),
									grad( Perm( BA + 1 ), x - 1, y, z - 1 ) ), // OF CUBE
								Lerp( u,
									grad( Perm( AB + 1 ), x, y - 1, z - 1 ),
									grad( Perm( BB + 1 ), x - 1, y - 1, z - 1 ) ) ) );

			//	Experimental hand-wavy normalization
			return ( res );
		}

		#region Private Members

		private int Perm( int x )
		{
		    return m_Perms[ x ];
		}
		//private static int Perm( int x )
		//{
		//    uint n = ( uint )( x );
		//    n = ( n << 13 ) ^ n;
		//    return ( int )( ( n * ( n * n * 15731 + 789221 ) + 1376312589 ) & 0xff );
		//}

		private readonly int[] m_Perms = new int[ 512 ];
		private readonly static Noise s_Instance = new Noise( );

		/// <summary>
		/// Fade curve
		/// </summary>
		private static float Fade( float t )
		{
			return t * t * t * ( t * ( t * 6 - 15 ) + 10 );
		}

		/// <summary>
		/// Linear interpolation
		/// </summary>
		private static float Lerp( float t, float a, float b )
		{
			return a + t * ( b - a );
		}

		/// <summary>
		/// Gradient selection for 2d noise
		/// </summary>
		private static float Gradient( int hash, float x, float y )
		{
			switch ( hash & 3 )
			{
				case 0: return x;
				case 1: return -x;
				case 2: return y;
			}
			return -y;
		}

		/// <summary>
		/// Gradient selection for 3d noise
		/// </summary>
		private static float grad( int hash, float x, float y, float z )
		{
			int h = hash & 15;                      // CONVERT LO 4 BITS OF HASH CODE
			float u = h < 8 ? x : y,                 // INTO 12 GRADIENT DIRECTIONS.
			     v = h < 4 ? y : h == 12 || h == 14 ? x : z;
			     //v = h < 4 ? y : z;
			return ( ( h & 1 ) == 0 ? u : -u ) + ( ( h & 2 ) == 0 ? v : -v );
		}

		private static float SmoothNoise( int iX, int iY )
		{
			float corners = ( NoiseBasis( iX - 1, iY - 1 ) + NoiseBasis( iX + 1, iY - 1 ) + NoiseBasis( iX - 1, iY + 1 ) + NoiseBasis( iX + 1, iY + 1 ) ) / 16.0f;
			float sides = ( NoiseBasis( iX - 1, iY ) + NoiseBasis( iX + 1, iY ) + NoiseBasis( iX, iY - 1 ) + NoiseBasis( iX, iY + 1 ) ) / 8.0f;
			float centre = NoiseBasis( iX, iY ) / 4;

			return corners + sides + centre;
		}

		private static float NoiseBasis( int x, int y )
		{
			uint n = ( uint )( x + y * 57 );
			n = ( n << 13 ) ^ n;
			return ( 1.0f - ( ( n * ( n * n * 15731 + 789221 ) + 1376312589 ) & 0x7fffffff ) / 1073741824.0f );
		}

		/// <summary>
		/// Basis function for single-valued noise
		/// </summary>
		private static float NoiseBasis( int x )
		{
			uint n = ( uint )( x );
			n = ( n << 13 ) ^ n;
			return ( 1.0f - ( ( n * ( n * n * 15731 + 789221 ) + 1376312589 ) & 0x7fffffff ) / 1073741824.0f );
		}

		private Noise( Random rnd )
		{
			int[] perms = new int[ 256 ];

			for ( int i = 0; i < perms.Length; ++i )
			{
				perms[ i ] = i;
			}
			for ( int i = 0; i < perms.Length; ++i )
			{
				int index0 = rnd.Next( perms.Length );
				int index1 = rnd.Next( perms.Length );

				Utils.Swap( ref perms[ index0 ], ref perms[ index1 ] );
			}

			for ( int i = 0; i < perms.Length; ++i )
			{
				m_Perms[ i ] = m_Perms[ i + 256 ] = perms[ i ];
			}
		}

		#endregion

	}
}
