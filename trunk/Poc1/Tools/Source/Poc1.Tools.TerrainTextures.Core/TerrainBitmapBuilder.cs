using System;
using System.Drawing;
using System.Drawing.Imaging;
using Rb.Core.Maths;

namespace Poc1.Tools.TerrainTextures.Core
{
	/// <summary>
	/// Builds bitmaps to display terrain type sets
	/// </summary>
	public class TerrainBitmapBuilder
	{
		public TerrainBitmapBuilder( )
		{
			m_InvWH = 1.0f / ( m_Width * m_Height );
		}
		
		/// <summary>
		/// Sets/gets apply lighting flag. If true, then the next Build() call will bake in lighting
		/// </summary>
		public bool ApplyLighting
		{
			get { return m_Lighting; }
			set { m_Lighting = value; }
		}

		/// <summary>
		/// Forces the next Build() call to create a new height map
		/// </summary>
		public void UpdateHeights( )
		{
			m_Noise = new Noise( TimeSeed );
			m_HeightMap = null;
			m_LightMap = null;
		}

		/// <summary>
		/// Builds a bitmap of the specified dimensions, using a terrain type set to colour the underlying heightmap
		/// </summary>
		public unsafe Bitmap Build( int width, int height, TerrainTypeSet terrainTypes )
		{
			if ( terrainTypes.TerrainTypeCount == 0 )
			{
				return Build( width, height );
			}

			Bitmap bmp = new Bitmap( width, height, PixelFormat.Format24bppRgb );

			BitmapData bmpData = bmp.LockBits( new System.Drawing.Rectangle( 0, 0, width, height ), ImageLockMode.WriteOnly, bmp.PixelFormat );
			byte* rowFirstPixel = ( byte* )bmpData.Scan0;
			
			float[,] heights = GetHeights( width, height );
			float[,] lighting = GetLighting( width, height );
			float[,] slopes = GetSlopes( width, height );

			for ( int row = 0; row < height; ++row )
			{
				byte* curPixel = rowFirstPixel;
				for ( int col = 0; col < width; ++col )
				{
					float h = heights[ col, row ];
					float s = slopes[ col, row ];
					float l = ApplyLighting ? lighting[ col, row ] : 1.0f;
					Color c = terrainTypes.GetType( h, s ).AverageColour;
					curPixel[ 0 ] = ( byte )( c.B * l );
					curPixel[ 1 ] = ( byte )( c.G * l );
					curPixel[ 2 ] = ( byte )( c.R * l );
					curPixel += 3;
				}

				rowFirstPixel += bmpData.Stride;
			}

			bmp.UnlockBits( bmpData );
			return bmp;
		}

		/// <summary>
		/// Builds a bitmap of the specified dimensions, using the height or light map
		/// </summary>
		public unsafe Bitmap Build( int width, int height )
		{
			Bitmap bmp = new Bitmap( width, height, PixelFormat.Format24bppRgb );

			BitmapData bmpData = bmp.LockBits( new System.Drawing.Rectangle( 0, 0, width, height ), ImageLockMode.WriteOnly, bmp.PixelFormat );
			byte* rowFirstPixel = ( byte* )bmpData.Scan0;

			float[,] data = ApplyLighting ? GetLighting( width, height ) : GetHeights( width, height );

			for ( int row = 0; row < height; ++row )
			{
				byte* curPixel = rowFirstPixel;
				for ( int col = 0; col < width; ++col )
				{
					byte b = ( byte )( data[ col, row ] * 255.0f );
					curPixel[ 0 ] = b;
					curPixel[ 1 ] = b;
					curPixel[ 2 ] = b;
					curPixel += 3;
				}

				rowFirstPixel += bmpData.Stride;
			}

			bmp.UnlockBits( bmpData );
			return bmp;
		}

		#region Private Members

		private readonly float m_StartX = 23.7f;
		private readonly float m_StartY = 23.7f;
		private readonly float m_Width = 16.2f;
		private readonly float m_Height = 16.2f;
		private readonly float m_InvWH;
		private Noise m_Noise = new Noise( );
		private bool m_Lighting;
		private float[,] m_HeightMap;
		private float[,] m_SlopeMap;
		private float[,] m_LightMap;

		/// <summary>
		/// Gets the height map, creating it if necessary
		/// </summary>
		private float[,] GetHeights( int width, int height )
		{
			if ( m_HeightMap != null )
			{
				if ( m_HeightMap.GetLength( 0 ) == width && m_HeightMap.GetLength( 1 ) == height )
				{
					return m_HeightMap;
				}
			}

			m_HeightMap = CreateHeightMap( width, height );
			return m_HeightMap;
		}

		/// <summary>
		/// Gets the light map, creating it if necessary
		/// </summary>
		private float[,] GetLighting( int width, int height )
		{
			if ( m_LightMap != null )
			{
				if ( m_LightMap.GetLength( 0 ) == width && m_LightMap.GetLength( 1 ) == height)
				{
					return m_LightMap;
				}
			}

			m_LightMap = new float[ width, height ];
			m_SlopeMap = new float[ width, height ];
			CreateLightAndSlopeMaps( m_LightMap, m_SlopeMap, Vector3.XAxis );

			return m_LightMap;
		}
		
		/// <summary>
		/// Gets the slope map, creating it if necessary
		/// </summary>
		private float[,] GetSlopes( int width, int height )
		{
			if ( m_SlopeMap != null )
			{
				if ( m_SlopeMap.GetLength( 0 ) == width && m_SlopeMap.GetLength( 1 ) == height)
				{
					return m_SlopeMap;
				}
			}

			m_LightMap = new float[ width, height ];
			m_SlopeMap = new float[ width, height ];
			CreateLightAndSlopeMaps( m_LightMap, m_SlopeMap, Vector3.XAxis );

			return m_SlopeMap;
		}

		/// <summary>
		/// Basis function for <see cref="Tiled2dFunction"/>
		/// </summary>
		private delegate float Basis2dFunction( float x, float y );

		/// <summary>
		/// Tiles a 2d function
		/// </summary>
		private float Tiled2dFunction( float x, float y, Basis2dFunction basis )
		{
			x = Utils.Wrap( x, m_StartX, m_StartX + m_Width );
			y = Utils.Wrap( y, m_StartY, m_StartY + m_Height );

			float wrapX = x - m_Width;
			float wrapY = y - m_Height;

			float fX = x - m_StartX;
			float fY = y - m_StartX;
			float invX = m_Width - fX;
			float invY = m_Height - fY;

			float v0 = basis( x, y ) * invX * invY;
			float v1 = basis( wrapX, y ) * fX * invY;
			float v2 = basis( wrapX, wrapY ) * fX * fY;
			float v3 = basis( x, wrapY ) * invX * fY;

			return ( v0 + v1 + v2 + v3 ) * m_InvWH;
		}

		/// <summary>
		/// Creates lighting and slope maps
		/// </summary>
		private void CreateLightAndSlopeMaps( float[,] lightMap, float[,] slopeMap, Vector3 lightDir )
		{
			int width = lightMap.GetLength( 0 );
			int height = lightMap.GetLength( 1 );
			float xInc = m_Width / ( width - 1 );
			float yInc = m_Height / ( height - 1 );
			int lastCol = width - 1;
			int lastRow = height - 1;

			float[,] heights = GetHeights( width, height );
			float maxSlope = float.MinValue;
			for ( int row = 0; row < height; ++row )
			{
				for (int col = 0; col < width; ++col )
				{
					float hO = heights[ col, row ];
					float hL = heights[ col == 0 ? 0 : col - 1, row ];
					float hR = heights[ col == lastCol ? lastCol : col + 1, row ];
					float hU = heights[ col, row == 0 ? 0 : row - 1 ];
					float hD = heights[ col, row == lastRow ? lastRow : row + 1 ];

					Vector3 left = new Vector3( +xInc, hL - hO, 0 );
					Vector3 right = new Vector3( -xInc, hR - hO, 0 );
					Vector3 up = new Vector3( 0, hU - hO, +yInc );
					Vector3 down = new Vector3( 0, hD - hO, -yInc );

					Vector3 acc = Vector3.Cross( up, left );
					acc.IpAdd( Vector3.Cross( right, up ) );
					acc.IpAdd( Vector3.Cross( down, right ) );
					acc.IpAdd( Vector3.Cross( left, down ) );
					acc.Normalise( );

					float nDp = ( acc.Dot( lightDir ) + 1.0f ) / 2.0f;
					lightMap[ col, row ] = nDp;
					float slope = 1.0f - acc.Dot( Vector3.YAxis );
					maxSlope = Utils.Max( maxSlope, slope );

					//	Let's say that slope never goes above 0.8...
					slope = Utils.Min( slope, 0.8f ) / 0.8f;
					slopeMap[ col, row ] = slope;
				}
			}
		}

		/// <summary>
		/// Creates a heightmap
		/// </summary>
		private float[,] CreateHeightMap( int width, int height )
		{
			//	TODO: AP: Can wrap better by caching results in separate map
			float y = m_StartY;
			float xInc = m_Width / ( width - 0 );
			float yInc = m_Height / ( height - 0 );
			float[,] heights = new float[ width, height ];
			Basis2dFunction basis =
				delegate( float fX, float fY )
				{
					return Fractals.RidgedFractal( fX, fY, 1.2f, 8, 1.2f, m_Noise.GetNoise );
				};
			for ( int row = 0; row < height; ++row, y += yInc )
			{
				float x = m_StartX;
				for (int col = 0; col < width; ++col, x += xInc)
				{
					heights[ col, row ] = Tiled2dFunction( x, y, basis );
				}
			}

			return heights;
		}

		/// <summary>
		/// Generates a seed value for noise setup constructor, based on the current time
		/// </summary>
		private static int TimeSeed
		{
			get { return ( int )DateTime.Now.Ticks; }
		}

		#endregion
	}
}
