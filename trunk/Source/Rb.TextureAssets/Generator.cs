using System;
using System.IO;

namespace Rb.TextureAssets
{
	/// <summary>
	/// Texture file generator
	/// </summary>
	public class Generator
	{
		/// <summary>
		/// Sets up the 2D texture data to write
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="data"></param>
		/// <param name="generateMipMaps"></param>
		public static Generator From2dTexture( int width, int height, byte[] data, bool generateMipMaps )
		{
			throw new NotImplementedException( );
		}

		/// <summary>
		/// Sets up the 3D texture data to write
		/// </summary>
		/// <param name="width">Texture width</param>
		/// <param name="height">Texture height</param>
		/// <param name="depth">Texture depth</param>
		/// <param name="data">Texture data</param>
		/// <param name="generateMipMaps">If true, mip-maps are generated in the texture file</param>
		public static Generator From3dTexture( int width, int height, int depth, byte[] data, bool generateMipMaps )
		{
			throw new NotImplementedException( );
		}

		/// <summary>
		/// Writes the generated texture data to a stream
		/// </summary>
		/// <param name="stream">Stream to write to</param>
		public void WriteToStream( Stream stream )
		{
		}

		/// <summary>
		/// Writes the generated texture data to a file
		/// </summary>
		/// <param name="path">File path</param>
		public void WriteToFile( string path )
		{
		}
	}
}
