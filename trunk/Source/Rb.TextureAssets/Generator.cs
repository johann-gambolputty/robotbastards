using System;
using System.IO;
using System.Runtime.InteropServices;
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
				headerGroup.GroupSizeInBytes = 0;
				long startOfHeaderGroup = WriteGroup( writer, headerGroup );

				//	Write the header
				TextureFileFormatVersion1.Header header = new TextureFileFormatVersion1.Header( );
				header.Created = DateTime.Now;
				header.Dimensions = 2;
				header.Format = texture.Format;
				header.TextureDataEntries = textureDataArray.Length;
				Write( writer, header );

				//	Write texture data
				TextureFileFormatVersion1.Group textureDataGroup = new TextureFileFormatVersion1.Group( );
				textureDataGroup.GroupId = GroupIdentifier.Texture2dDataGroup;
				for ( int textureDataIndex = 0; textureDataIndex < textureDataArray.Length; ++textureDataIndex )
				{
					//	Write the texture group
					long startOfTextureGroup = WriteGroup( writer, textureDataGroup );
					Texture2dData textureData = textureDataArray[ textureDataIndex ];

					//	Write texture data
					writer.Write( textureData.Width );
					writer.Write( textureData.Height );
					writer.Write( textureData.Bytes );

					//	Update the texture group
					CompleteGroup( writer, startOfTextureGroup );
				}

				//	Update group with correct size
				long endOfHeaderGroup = stream.Length;
				stream.Seek( startOfHeaderGroup, SeekOrigin.Begin );
				headerGroup.GroupSizeInBytes = endOfHeaderGroup - startOfHeaderGroup;
				Write( writer, headerGroup );
			}
		}

		/// <summary>
		/// Writes a group with a dummy length value to the binary writer
		/// </summary>
		private static long WriteGroup( BinaryWriter writer, TextureFileFormatVersion1.Group group )
		{
			long startOfGroup = writer.BaseStream.Length;
			writer.Write( ( int )group.GroupId );
			writer.Write( ( long )0 );
			return startOfGroup;
		}

		/// <summary>
		/// Completes a group written by <see cref="WriteGroup"/>
		/// </summary>
		private static void CompleteGroup( BinaryWriter writer, long startOfGroup )
		{
			long endOfGroup = writer.BaseStream.Length;

			writer.BaseStream.Seek( startOfGroup + 4, SeekOrigin.Begin );
			writer.Write( endOfGroup - startOfGroup );

			writer.BaseStream.Seek( endOfGroup, SeekOrigin.Begin );
		}

		/// <summary>
		/// Writes a simple object to a binary stream
		/// </summary>
		private static unsafe void Write<T>( BinaryWriter writer, T obj )
		{
			byte[] bytes = new byte[ Marshal.SizeOf( obj ) ];
			fixed ( byte* mem = bytes )
			{
				Marshal.StructureToPtr( obj, new IntPtr( mem ), false );
				writer.Write( bytes );
			}
		}
	}
}