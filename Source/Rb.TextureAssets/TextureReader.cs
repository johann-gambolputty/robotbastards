using System.IO;
using Rb.Assets;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.TextureAssets
{
	public class TextureReader
	{
		/// <summary>
		/// Reads a texture from a stream
		/// </summary>
		/// <param name="streamUrl">Location that the stream was opened at</param>
		/// <param name="stream">Stream to read from</param>
		/// <param name="returnTextureData">If true, the raw texture data objects are returned</param>
		/// <param name="generateMipMaps">If true, mipmaps are generated for the texture from the top level image</param>
		/// <returns>Returns a new texture, or texture data</returns>
		public static object ReadTextureFromStream( string streamUrl, Stream stream, bool returnTextureData, bool generateMipMaps )
		{
			using ( BinaryReader reader = new BinaryReader( stream ) )
			{
				int fileId = reader.ReadInt32( );
				if ( fileId != TextureFileFormatVersion1.TextureFileIdentifier )
				{
					throw new FileLoadException( "Texture file did not begin with texture file identifer" );
				}
				TextureFileFormatVersion1.Group headerGroup = TextureFileFormatVersion1.Group.Read( reader );
				if ( headerGroup.GroupId != GroupIdentifier.TextureHeaderGroup )
				{
					throw new FileLoadException( string.Format( "Texture file did not begin with a header group (started with group ID {0})", headerGroup.GroupId ) );
				}
				TextureFileFormatVersion1.Header header = TextureFileFormatVersion1.Header.Read( reader );
				if ( header.Dimensions == 2 )
				{
					return Load2dTexture( streamUrl, reader, header, returnTextureData, generateMipMaps );
				}
				if ( header.Dimensions == 3 )
				{
					return Load3dTexture( streamUrl, reader, header, returnTextureData, generateMipMaps );
				}
				throw new FileLoadException( string.Format( "Invalid texture dimensions specified ({0})", header.Dimensions ) );
			}
		}

		#region 2D texture loading

		/// <summary>
		/// Loads 2d texture data
		/// </summary>
		private static object Load2dTexture( string streamUrl, BinaryReader reader, TextureFileFormatVersion1.Header header, bool returnTextureData, bool generateMipMaps )
		{
			Texture2dData[] data = Load2dTextureData( reader, header );
			if ( returnTextureData )
			{
				return data;
			}
			ITexture2d texture = Graphics.Factory.CreateTexture2d( );
			if ( generateMipMaps )
			{
				if ( data.Length > 1 )
				{
					AssetsLog.Warning( "Source \"{0}\" contained mip-maps that are being discarded in favour of generated ones", streamUrl );
				}
				texture.Create( data[ 0 ], true );
				return texture;
			}

			texture.Create( data );
			return texture;
		}

		/// <summary>
		/// Loads 2d texture data from the specified stream
		/// </summary>
		private static Texture2dData[] Load2dTextureData( BinaryReader reader, TextureFileFormatVersion1.Header header )
		{
			Texture2dData[] textureDataArray = new Texture2dData[ header.TextureDataEntries ];
			for ( int textureDataCount = 0; textureDataCount < header.TextureDataEntries; ++textureDataCount )
			{
				TextureFileFormatVersion1.Group textureGroup = TextureFileFormatVersion1.Group.Read( reader );
				if ( textureGroup.GroupId != GroupIdentifier.Texture2dDataGroup )
				{
					throw new FileLoadException( "Expected texture group" );
				}

				int width = reader.ReadInt32( );
				int height = reader.ReadInt32( );

				Texture2dData texData = new Texture2dData( );
				texData.Create( width, height, header.Format );

				reader.Read( texData.Bytes, 0, texData.Bytes.Length );

				textureDataArray[ textureDataCount ] = texData;
			}

			return textureDataArray;
		}

		#endregion

		#region CubeMap texture loading



		#endregion

		#region 3D texture loading

		/// <summary>
		/// Loads a 3d texture from the specified stream
		/// </summary>
		private static object Load3dTexture( string streamUrl, BinaryReader reader, TextureFileFormatVersion1.Header header, bool returnTextureData, bool generateMipMaps )
		{
			Texture3dData[] data = Load3dTextureData( reader, header );
			if ( returnTextureData )
			{
				return data;
			}
			ITexture3d texture = Graphics.Factory.CreateTexture3d( );
			texture.Create( data[ 0 ] );	//	TODO: AP: Add mipmap generation
			return texture;
		}

		/// <summary>
		/// Loads 3d texture data from the specified stream
		/// </summary>
		private static Texture3dData[] Load3dTextureData( BinaryReader reader, TextureFileFormatVersion1.Header header )
		{
			Texture3dData[] textureDataArray = new Texture3dData[ header.TextureDataEntries ];
			for ( int textureDataCount = 0; textureDataCount < header.TextureDataEntries; ++textureDataCount )
			{
				TextureFileFormatVersion1.Group textureGroup = TextureFileFormatVersion1.Group.Read( reader );
				if ( textureGroup.GroupId != GroupIdentifier.Texture3dDataGroup )
				{
					throw new FileLoadException( "Expected texture group" );
				}

				int width = reader.ReadInt32( );
				int height = reader.ReadInt32( );
				int depth = reader.ReadInt32( );

				Texture3dData texData = new Texture3dData( );
				texData.Create( width, height, depth, header.Format );

				reader.Read( texData.Bytes, 0, texData.Bytes.Length );

				textureDataArray[ textureDataCount ] = texData;
			}

			return textureDataArray;
		}

		#endregion
	}
}
