using System;
using System.IO;
using System.Runtime.InteropServices;
using Rb.Assets;
using Rb.Assets.Base;
using Rb.Assets.Interfaces;
using Rb.Core.Components;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Textures;

namespace Rb.TextureAssets
{
	/// <summary>
	/// Texture asset loader
	/// </summary>
	public class Loader : AssetLoader
	{
		/// <summary>
		/// Gets the name of this loader
		/// </summary>
		public override string Name
		{
			get { return "Texture"; }
		}

		/// <summary>
		/// Gets the extensions that this loader supports
		/// </summary>
		public override string[] Extensions
		{
			get { return new string[] { "texture" }; }
		}

		/// <summary>
		/// Creates default texture load parameters
		/// </summary>
		/// <param name="addAllProperties">If true, adds all dynamic properties with default values to the parameters</param>
		/// <returns>Returns default load parameters</returns>
		public override LoadParameters CreateDefaultParameters( bool addAllProperties )
		{
			TextureLoadParameters parameters = new TextureLoadParameters( );
			if ( addAllProperties )
			{
				parameters.GenerateMipMaps = false;
				parameters.ReturnTextureDataOnly = false;
			}
			return parameters;
		}

		/// <summary>
		/// Loads an asset supported by this loader
		/// </summary>
		/// <param name="source">Asset source</param>
		/// <param name="parameters">Asset load parameters</param>
		/// <returns>
		/// If the parameters specify "returnTextureDataOnly" as true  (<see cref="TextureLoadParameters.ReturnTextureDataOnly"/>),
		/// the method returns an array of <see cref="Texture2dData"/> or <see cref="Texture3dData"/>. If the "returnTextureDataOnly" 
		/// is false, or does not exist, this method returns an <see cref="ITexture"/> object.
		/// </returns>
		public override unsafe object Load( ISource source, LoadParameters parameters )
		{
			bool generateMipMaps = DynamicProperties.GetProperty( parameters.Properties, TextureLoadParameters.GenerateMipMapsPropertyName, false );
			bool returnTextureData = DynamicProperties.GetProperty( parameters.Properties, TextureLoadParameters.ReturnTextureDataOnlyName, false );
			using ( Stream stream = ( ( IStreamSource )source ).Open( ) )
			{
				using ( BinaryReader reader = new BinaryReader( stream ) )
				{
					int fileId = reader.ReadInt32( );
					if ( fileId != TextureFileFormatVersion1.TextureFileIdentifier )
					{
						throw new FileLoadException( "Texture file did not begin with texture file identifer" );
					}
					TextureFileFormatVersion1.Group headerGroup = Read<TextureFileFormatVersion1.Group>( stream );
					if ( headerGroup.GroupId != GroupIdentifier.TextureHeaderGroup )
					{
						throw new FileLoadException( string.Format( "Texture file did not begin with a header group (started with group ID {0})", headerGroup.GroupId ) );
					}
					TextureFileFormatVersion1.Header header = Read<TextureFileFormatVersion1.Header>( stream );
					if ( header.Dimensions == 2 )
					{
						return Load2dTexture( source, reader, header, returnTextureData, generateMipMaps );
					}
					if ( header.Dimensions == 3 )
					{
						return Load3dTexture( source, reader, header, returnTextureData, generateMipMaps );
					}
					throw new FileLoadException( string.Format( "Invalid texture dimensions specified ({0})", header.Dimensions ) );
				}

			}
		}

		#region 2D texture loading

		/// <summary>
		/// Loads 2d texture data
		/// </summary>
		private static object Load2dTexture( ISource source, BinaryReader reader, TextureFileFormatVersion1.Header header, bool returnTextureData, bool generateMipMaps )
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
					AssetsLog.Warning( "Source \"{0}\" contained mip-maps that are being discarded in favour of generated ones", source );
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
				TextureFileFormatVersion1.Group textureGroup = Read<TextureFileFormatVersion1.Group>( reader.BaseStream );
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

		#region 3D texture loading

		/// <summary>
		/// Loads a 3d texture from the specified stream
		/// </summary>
		private static object Load3dTexture( ISource source, BinaryReader reader, TextureFileFormatVersion1.Header header, bool returnTextureData, bool generateMipMaps )
		{
			throw new NotImplementedException( );
		}

		#endregion

		/// <summary>
		/// Reads a type directly from a stream
		/// </summary>
		private unsafe static T Read<T>( Stream stream ) where T : struct
		{
			byte[] buffer = new byte[ Marshal.SizeOf( typeof( T ) ) ];
			stream.Read( buffer, 0, buffer.Length );
			fixed ( byte* bufferPtr = buffer )
			{
				return ( T )Marshal.PtrToStructure( new IntPtr( bufferPtr ), typeof( T ) );
			}
		}
	}
}
