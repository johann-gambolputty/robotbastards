using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Poc1.Tools.TerrainTextures.Core
{
	[Serializable]
	public class TerrainTypeSet
	{
		public TerrainTypeSet Clone( )
		{
			TerrainTypeSet clone = new TerrainTypeSet( );
			foreach ( TerrainType srcType in m_TerrainTypes )
			{
				clone.m_TerrainTypes.Add( srcType.Clone( ) );
			}
			return clone;
		}

		public List<TerrainType> TerrainTypes
		{
			get { return m_TerrainTypes; }
		}

		public void Save( string path )
		{
			using ( FileStream fs = new FileStream( path, FileMode.Create, FileAccess.Write ) )
			{
				Save( fs );
			}
		}

		public void Save( Stream dest )
		{
			IFormatter formatter = new BinaryFormatter( );
			formatter.Serialize( dest, this );
		}

		public static TerrainTypeSet Load( string path )
		{
			using ( FileStream fs = new FileStream( path, FileMode.Open, FileAccess.Read ) )
			{
				return Load( fs );
			}
		}

		public static TerrainTypeSet Load( Stream src )
		{
			IFormatter formatter = new BinaryFormatter( );
			return ( TerrainTypeSet )formatter.Deserialize( src );
		}

		public unsafe Bitmap CreateTerrainPackBitmap( )
		{
			return null;
		}

		public unsafe Bitmap CreateDistributionBitmap( )
		{
			Bitmap bmp = new Bitmap( 256, 256, PixelFormat.Format24bppRgb );

			if ( TerrainTypes.Count == 0 )
			{
				return bmp;
			}

			BitmapData bmpData = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.WriteOnly, bmp.PixelFormat );
			byte* rowFirstPixel = ( byte* )bmpData.Scan0;
			for ( int row = 0; row < bmp.Height; ++row, rowFirstPixel += bmpData.Stride )
			{
				float altitude = row / 255.0f;
				byte* curPixel = rowFirstPixel;
				for ( int col = 0; col < bmp.Width; ++col, curPixel += 3 )
				{
					float typeIndex = GetTypeIndex( col / 255.0f, altitude );
					byte scaledTypeIndex = ( byte )( typeIndex * ( 255.0f / TerrainTypes.Count ) );

					//	TODO: AP: Add noise to slope/altitude

					curPixel[ 0 ] = ( byte )typeIndex;
					curPixel[ 1 ] = scaledTypeIndex;
					curPixel[ 2 ] = unchecked( ( byte )( 255 - scaledTypeIndex ) );
				}
			}

			bmp.UnlockBits( bmpData );

			return bmp;
		}

		public TerrainType GetType( float altitude, float slope )
		{
			return ( TerrainTypes.Count == 0 ) ? null : TerrainTypes[ GetTypeIndex( altitude, slope ) ];
		}

		#region Private Members

		private readonly List<TerrainType> m_TerrainTypes = new List<TerrainType>(); 
		
		public int GetTypeIndex( float altitude, float slope )
		{
			int best = 0;
			float bestScore = TerrainTypes[ 0 ].GetScore( altitude, slope );

			for ( int index = 1; index < TerrainTypes.Count; ++index )
			{
				float score = TerrainTypes[ index ].GetScore( altitude, slope );
				if ( score > bestScore )
				{
					best = index;
					bestScore = score;
				}
			}

			return best;
		}

		#endregion
	}
}
