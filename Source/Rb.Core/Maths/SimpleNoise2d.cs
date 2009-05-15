using System;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Simple 2d noise
	/// </summary>
	public class SimpleNoise2d
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SimpleNoise2d( ) :
			this( new Random( ) )
		{
		}

		/// <summary>
		/// Gets noise value at position (x,y)
		/// </summary>
		public float GetNoise( float x, float y )
		{
			float sum = 0.0f;
			int a = ( int )Math.Floor( x );
			int b = ( int )Math.Floor( y );

			for ( int i = 0; i < 2; ++i )
			{
				for ( int j = 0; j < 2; ++j )
				{
					int n = m_Perm[ ( a + i + m_Perm[ ( b + j ) % m_Perm.Length ] ) % m_Perm.Length ];
					float curX = x - a - i;
					float curY = y - b - j;
					sum += Weight( curX ) * Weight( curY ) * ( m_GradX[ n ] * curX + m_GradY[ n ] * curY );
				}
			}

			return sum;
		}

		/// <summary>
		/// Gets tiling noise value at position (x,y), for a tile size (w,h)
		/// </summary>
		public float GetTilingNoise( float x, float y, float tileWidth, float tileHeight )
		{
			int xclip = ( int )tileWidth;
			int yclip = ( int )tileHeight;
			float sum = 0.0f;
			int a = ( int )Math.Floor( x );
			int b = ( int )Math.Floor( y );

			for ( int i = 0; i < 2; ++i )
			{
				for ( int j = 0; j < 2; ++j )
				{
					int n = m_Perm[ ( ( ( a + i ) % xclip ) + m_Perm[ ( ( b + j ) % yclip ) % m_Perm.Length ] ) % m_Perm.Length ];
					float curX = x - a - i;
					float curY = y - b - j;
					sum += Weight( curX ) * Weight( curY ) * ( m_GradX[ n ] * curX + m_GradY[ n ] * curY );
				}
			}

			return sum;
		}

		#region Private Members

		private int[] m_Perm;
		private float[] m_GradX;
		private float[] m_GradY;

		/// <summary>
		/// Weighting function
		/// </summary>
		private static float Weight( float v )
		{
			return ( 2.0f * Math.Abs( v ) - 3.0f ) * v * v + 1.0f;
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		private SimpleNoise2d( Random rnd )
		{
			m_Perm = new int[ 64 ];
			for ( int permIndex = 0; permIndex < m_Perm.Length; ++permIndex )
			{
				m_Perm[ permIndex ] = permIndex;
			}
			for ( int permIndex = 0; permIndex < m_Perm.Length; ++permIndex )
			{
				int swapIndex0 = rnd.Next( m_Perm.Length );
				int swapIndex1 = rnd.Next( m_Perm.Length );
				int tmpValue = m_Perm[ swapIndex0 ];
				m_Perm[ swapIndex0 ] = m_Perm[ swapIndex1 ];
				m_Perm[ swapIndex1 ] = tmpValue;
			}
			m_GradX = new float[ 64 ];
			m_GradY = new float[ 64 ];
			for ( int gradIndex = 0; gradIndex < m_GradX.Length; ++gradIndex )
			{
				float m;

				//	Generate a random, non-zero length vector within the unit circle
				do
				{
					m_GradX[ gradIndex ] = ( ( float )rnd.NextDouble( ) * 2.0f ) - 1.0f;
					m_GradY[ gradIndex ] = ( ( float )rnd.NextDouble( ) * 2.0f ) - 1.0f;
					m = m_GradX[ gradIndex ] * m_GradX[ gradIndex ] + m_GradY[ gradIndex ] * m_GradY[ gradIndex ];
				} while ( ( m == 0.0f ) && ( m > 1.0f ) );

				//	Normalize the vector
				m = 1.0f / ( float )Math.Sqrt( m );
				m_GradX[ gradIndex ] *= m;
				m_GradY[ gradIndex ] *= m;
			}
		}

		#endregion
	}

}
