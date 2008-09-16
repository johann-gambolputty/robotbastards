using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Rb.Assets;
using Rb.Assets.Base;
using Rb.Assets.Interfaces;
using Rb.Core.Components;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.TextureAssets
{
	/// <summary>
	/// Texture asset loader
	/// </summary>
	public class Loader : AssetLoader
	{
		/// <summary>
		/// Texture loading parameters
		/// </summary>
		public class TextureLoadParameters : LoadParameters
		{
			/// <summary>
			/// Default constructor. Mipmap generation is off
			/// </summary>
			public TextureLoadParameters( )
			{
			}

			/// <summary>
			/// Setup constructor. Sets mipmap generation flag
			/// </summary>
			/// <param name="generateMipMaps">Mipmap generation flag</param>
			public TextureLoadParameters( bool generateMipMaps )
			{
				GenerateMipMaps = generateMipMaps;
			}

			/// <summary>
			/// Gets/sets the mipmap generation flag
			/// </summary>
			public bool GenerateMipMaps
			{
				get { return ( bool )Properties[ GenerateMipMapsPropertyName ]; }
				set { Properties[ GenerateMipMapsPropertyName ] = value; }
			}

			/// <summary>
			/// Gets/sets the flag that forces the loader to return texture data only.
			/// </summary>
			public bool ReturnTextureDataOnly
			{
				get { return ( bool )Properties[ ReturnTextureDataOnlyName ]; }
				set { Properties[ ReturnTextureDataOnlyName ] = value; }
			}
		}

		/// <summary>
		/// Name of the property in the load parameters, that enables mipmap generation when true
		/// </summary>
		public const string GenerateMipMapsPropertyName = "generateMipMaps";

		/// <summary>
		/// Name of the property in the load parameters that forces the loader to return texture data, not textures
		/// </summary>
		public const string ReturnTextureDataOnlyName = "returnTextureDataOnly";


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
			bool generateMipMaps = DynamicProperties.GetProperty( parameters.Properties, GenerateMipMapsPropertyName, false );
			bool returnTextureData = DynamicProperties.GetProperty( parameters.Properties, ReturnTextureDataOnlyName, false );
			using ( Stream stream = ( ( IStreamSource )source ).Open( ) )
			{
				using ( BinaryReader reader = new BinaryReader( stream ) )
				{
					int fileId = reader.ReadInt32( );
					if ( fileId != Header.TextureFileIdentifier )
					{
						throw new FileLoadException( string.Format( "Invalid texture file identifier ({0})", fileId ) );
					}
					int groupId = reader.ReadInt32( );
					if ( groupId != ( int )GroupIdentifier.HeaderGroup )
					{
						throw new FileLoadException( string.Format( "Texture file did not begin with a header group (started with group ID {0})", groupId ) );
					}
					Header header = Read<Header>( stream );
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
		private static object Load2dTexture( ISource source, BinaryReader reader, Header header, bool returnTextureData, bool generateMipMaps )
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
		/// Loads a 2d texture group
		/// </summary>
		private static void Load2dTextureGroup( BinaryReader reader, Header header, List<Texture2dData> textureDataList )
		{
			bool containsSubTexture = reader.ReadBoolean( );
			if ( containsSubTexture )
			{
				Group subTextureGroup = Read<Group>( reader.BaseStream );
				if ( subTextureGroup.GroupId != GroupIdentifier.Texture2dDataGroup )
				{
					throw new FileLoadException( "Expected texture group to satify sub-texture flag" );
				}
				Load2dTextureGroup( reader, header, textureDataList );
			}

			int width = reader.ReadInt32( );
			int height = reader.ReadInt32( );

			Texture2dData texData = new Texture2dData( );
			texData.Create( width, height, header.Format );

			reader.Read( texData.Bytes, 0, texData.Bytes.Length );

			textureDataList.Insert( 0, texData );
		}

		/// <summary>
		/// Loads 2d texture data from the specified stream
		/// </summary>
		private static Texture2dData[] Load2dTextureData( BinaryReader reader, Header header )
		{
			Group textureGroup = Read<Group>( reader.BaseStream );
			if ( textureGroup.GroupId != GroupIdentifier.Texture2dDataGroup )
			{
				throw new FileLoadException( "Expected texture group" );
			}
			List< Texture2dData > dataEntries = new List< Texture2dData >( );
			Load2dTextureGroup( reader, header, dataEntries );
			return dataEntries.ToArray( );
		}

		#endregion

		#region 3D texture loading

		/// <summary>
		/// Loads a 3d texture from the specified stream
		/// </summary>
		private static object Load3dTexture( ISource source, BinaryReader reader, Header header, bool returnTextureData, bool generateMipMaps )
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
