using System;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Noise generation
	/// </summary>
	/// <remarks>
	/// From http://mrl.nyu.edu/~perlin/paper445.pdf
	/// 
	/// </remarks>
	public class Noise
	{
		/// <summary>
		/// Noise singleton
		/// </summary>
		public static Noise Instance
		{
			get { return ms_Instance; }
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

			float v1 = SmoothNoise( iX, iY );
			float v2 = SmoothNoise( iX + 1, iY );
			float v3 = SmoothNoise( iX, iY + 1 );
			float v4 = SmoothNoise( iX + 1, iY + 1 );

			float i1 = v1 + ( v2 - v1 ) * fX;
			float i2 = v3 + ( v4 - v3 ) * fX;

			float r = i1 + ( i2 - i1 ) * fY;
			return ( r / 0.8f ); // TODO: AP: Handy-wavy experimental normalization values
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
			float u = fade( x ),                                // COMPUTE FADE CURVES
				   v = fade( y ),                                // FOR EACH OF X,Y,Z.
				   w = fade( z );
			int A = m_Perms[ X ] + Y, AA = m_Perms[ A ] + Z, AB = m_Perms[ A + 1 ] + Z,      // HASH COORDINATES OF
				B = m_Perms[ X + 1 ] + Y, BA = m_Perms[ B ] + Z, BB = m_Perms[ B + 1 ] + Z;      // THE 8 CUBE CORNERS,

			float res = lerp( w, lerp( v, lerp( u, grad( m_Perms[ AA ], x, y, z ),  // AND ADD
										   grad( m_Perms[ BA ], x - 1, y, z ) ), // BLENDED
								   lerp( u, grad( m_Perms[ AB ], x, y - 1, z ),  // RESULTS
										   grad( m_Perms[ BB ], x - 1, y - 1, z ) ) ),// FROM  8
						   lerp( v, lerp( u, grad( m_Perms[ AA + 1 ], x, y, z - 1 ),  // CORNERS
										   grad( m_Perms[ BA + 1 ], x - 1, y, z - 1 ) ), // OF CUBE
								   lerp( u, grad( m_Perms[ AB + 1 ], x, y - 1, z - 1 ),
										   grad( m_Perms[ BB + 1 ], x - 1, y - 1, z - 1 ) ) ) );

			//	Experimental hand-wavy normalization
			return ( res / 0.7f );
		}

		#region Private Members

		private readonly int[] m_Perms = new int[ 512 ];
		private readonly static Noise ms_Instance = new Noise( );

		private static float fade( float t )
		{
			return t * t * t * ( t * ( t * 6 - 15 ) + 10 );

		//	float t2 = t * t;
		//	return ( 3 * t2 ) - 2 * t2 * t;
		}

		private static float lerp( float t, float a, float b )
		{
			return a + t * ( b - a );
		}

		private static float grad( int hash, float x, float y, float z )
		{
			int h = hash & 15;                      // CONVERT LO 4 BITS OF HASH CODE
			float u = h < 8 ? x : y,                 // INTO 12 GRADIENT DIRECTIONS.
				   v = h < 4 ? y : h == 12 || h == 14 ? x : z;
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
