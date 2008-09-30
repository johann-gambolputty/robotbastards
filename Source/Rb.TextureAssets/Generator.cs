using System;
using System.IO;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.TextureAssets
{
	/// <summary>
	/// Texture file generator
	/// </summary>
	public class Generator
	{
		/// <summary>
		/// Writes a texture to a stream
		/// </summary>
		/// <param name="texture">Texture to write</param>
		/// <param name="stream">Stream to write to</param>
		/// <param name="writeMipMaps">
		/// If true, all the texture's mip-map levels are written to the stream. Otherwise, only the level 0
		/// texture is written.
		/// </param>
		/// <exception cref="ArgumentNullException">Thrown if texture or stream arguments are null</exception>
		public static void WriteTextureToStream( ITexture2d texture, Stream stream, bool writeMipMaps )
		{
			if ( texture == null )
			{
				throw new ArgumentNullException( "texture" );
			}
			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );	
			}

			Texture2dData[] textureDataArray = texture.ToTextureData( writeMipMaps );

			using ( BinaryWriter writer = new BinaryWriter( stream ) )
			{
				writer.Write( TextureFileFormatVersion1.TextureFileIdentifier );

				TextureFileFormatVersion1.Group headerGroup = new TextureFileFormatVersion1.Group( );
				headerGroup.GroupId = GroupIdentifier.TextureHeaderGroup;
				long startOfHeaderGroup = BeginGroup( writer, headerGroup );

				//	Write the header
				TextureFileFormatVersion1.Header header = new TextureFileFormatVersion1.Header( );
				header.Dimensions = 2;
				header.Format = texture.Format;
				header.TextureDataEntries = textureDataArray.Length;
				header.Write( writer );

				//	Write texture data
				TextureFileFormatVersion1.Group textureDataGroup = new TextureFileFormatVersion1.Group( );
				textureDataGroup.GroupId = GroupIdentifier.Texture2dDataGroup;
				for ( int textureDataIndex = 0; textureDataIndex < textureDataArray.Length; ++textureDataIndex )
				{
					//	Write the texture group
					long startOfTextureGroup = BeginGroup( writer, textureDataGroup );
					Texture2dData textureData = textureDataArray[ textureDataIndex ];

					//	Write texture data
					writer.Write( textureData.Width );
					writer.Write( textureData.Height );
					writer.Write( textureData.Bytes );

					//	Update the texture group
					EndGroup( writer, startOfTextureGroup );
				}

				//	Update group with correct size
				EndGroup( writer, startOfHeaderGroup );
			}
		}

		/// <summary>
		/// Writes a group with a dummy length value to the binary writer
		/// </summary>
		private static long BeginGroup( BinaryWriter writer, TextureFileFormatVersion1.Group group )
		{
			long startOfGroup = writer.BaseStream.Length;
			group.Write( writer );
			return startOfGroup;
		}

		/// <summary>
		/// Completes a group written by <see cref="BeginGroup"/>
		/// </summary>
		private static void EndGroup( BinaryWriter writer, long startOfGroup )
		{
			long endOfGroup = writer.BaseStream.Length;

			writer.BaseStream.Seek( startOfGroup + 4, SeekOrigin.Begin );
			writer.Write( endOfGroup - startOfGroup );

			writer.BaseStream.Seek( endOfGroup, SeekOrigin.Begin );
		}
	}
}