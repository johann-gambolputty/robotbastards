using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Poc1.Tools.TerrainTextures.Core
{
	/// <summary>
	/// A set of terrain types
	/// </summary>
	[Serializable]
	public class TerrainTypeSet
	{
		/// <summary>
		/// Gets the list of terrain types in this set
		/// </summary>
		public List<TerrainType> TerrainTypes
		{
			get { return m_TerrainTypes; }
		}

		/// <summary>
		/// Gets/sets the name of this type set
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		/// <summary>
		/// Clones this set
		/// </summary>
		public TerrainTypeSet Clone( )
		{
			TerrainTypeSet clone = new TerrainTypeSet( );
			foreach ( TerrainType srcType in m_TerrainTypes )
			{
				clone.m_TerrainTypes.Add( srcType.Clone( ) );
			}
			return clone;
		}

		/// <summary>
		/// Saves this set to a file
		/// </summary>
		public void Save( string path )
		{
			using ( FileStream fs = new FileStream( path, FileMode.Create, FileAccess.Write ) )
			{
				Save( fs );
			}
		}

		/// <summary>
		/// Saves this set to a destination stream
		/// </summary>
		public void Save( Stream dest )
		{
			IFormatter formatter = new BinaryFormatter( );
			formatter.Serialize( dest, this );
		}

		/// <summary>
		/// Loads a new terrain type set from a file
		/// </summary>
		public static TerrainTypeSet Load( string path )
		{
			using ( FileStream fs = new FileStream( path, FileMode.Open, FileAccess.Read ) )
			{
				return Load( fs );
			}
		}

		/// <summary>
		/// Loads a new terrain type set from a source stream
		/// </summary>
		public static TerrainTypeSet Load( Stream src )
		{
			IFormatter formatter = new BinaryFormatter( );
			return ( TerrainTypeSet )formatter.Deserialize( src );
		}

		public const int TerrainTileSize = 256;
		public const int MaxPackBitmapSize = 1024;
		public const int PackBitmapTileResolution = MaxPackBitmapSize / TerrainTileSize;
		public const int MaxTerrainTiles = PackBitmapTileResolution * PackBitmapTileResolution;

		public string BaseName
		{
			get { return string.IsNullOrEmpty( Name ) ? "Default" : Name; }
		}

		public string[] GetExportOutputs( string directory )
		{
			return GetExportOutputs( directory, BaseName );
		}

		public string[] GetExportOutputs( string directory, string baseName )
		{
			string packName = Path.Combine( directory, baseName + " Pack.jpg" );
			string distName = Path.Combine( directory, baseName + " Distribution.bmp" );

			return new string[] { packName, distName };
		}

		/// <summary>
		/// Exports this terrain type set
		/// </summary>
		public void Export( string directory )
		{
			Export( directory, BaseName );
		}

		/// <summary>
		/// Exports this terrain type set
		/// </summary>
		public void Export( string directory, string baseName )
		{
			Bitmap packBitmap = CreateTerrainPackBitmap( );
			Bitmap distBitmap = CreateDistributionBitmap( );

			string packName = Path.Combine( directory, baseName + " Pack.jpg" );
			string distName = Path.Combine( directory, baseName + " Distribution.bmp" );

			packBitmap.Save( packName, ImageFormat.Jpeg );
			distBitmap.Save( distName, ImageFormat.Bmp );	
		}

		/// <summary>
		/// Packs all terrain type textures into a single uber-texture. Textures are laid out to match expectations
		/// in the terrain fragment shader (x == index / N, y == index % N, N=pack texture size / individual texture size).
		/// </summary>
		public Bitmap CreateTerrainPackBitmap( )
		{
			long totalArea = TerrainTileSize * TerrainTileSize * TerrainTypes.Count;
			int packSize = TerrainTileSize;
			for (; ( packSize * packSize ) < totalArea; packSize *= 2 ) { }
			if ( packSize > MaxPackBitmapSize )
			{
				throw new InvalidOperationException( string.Format( "Exceeded maximum pack bitmap size (required {0})", packSize ) );
			}
			//	TODO: AP: Forced to max size so fragment shader works correctly (lazy)
			packSize = MaxPackBitmapSize;
			Bitmap packBitmap = new Bitmap( packSize, packSize, PixelFormat.Format24bppRgb );

			using ( Graphics packBitmapGraphics = Graphics.FromImage( packBitmap ) )
			{
				int x = 0;
				int y = 0;
				for ( int typeIndex = 0; typeIndex < TerrainTypes.Count; ++typeIndex )
				{
					TerrainType type = TerrainTypes[ typeIndex ];
					if ( type.Texture != null )
					{
						packBitmapGraphics.DrawImage( type.Texture, new Rectangle( x, y, TerrainTileSize, TerrainTileSize ) );
					}
					x += TerrainTileSize;
					if ( x >= packSize )
					{
						x = 0;
						y += TerrainTileSize;
					}
				}
			}

			return packBitmap;
		}

		/// <summary>
		/// Creates an r8g8b8 bitmap that encodes the distribution of all types in the set over all altitudes and slopes.
		/// The index of the prevalent type at a given (altitude,slope) pair is encoded in the red channel. Green
		/// and blue channels contain the type index, mapped to the range 0..255, for better visualiation.
		/// Bitmap is indexed by x==slope, y==altitude
		/// </summary>
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

					curPixel[ 2 ] = ( byte )typeIndex;
					curPixel[ 1 ] = scaledTypeIndex;
					curPixel[ 0 ] = unchecked( ( byte )( 255 - scaledTypeIndex ) );
				}
			}

			bmp.UnlockBits( bmpData );

			return bmp;
		}

		/// <summary>
		/// Gets the prevalent type in the set, at a given slope and altitude
		/// </summary>
		public TerrainType GetType( float altitude, float slope )
		{
			return ( TerrainTypes.Count == 0 ) ? null : TerrainTypes[ GetTypeIndex( altitude, slope ) ];
		}


		#region Private Members

		private string m_Name;
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
