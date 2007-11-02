using System.IO;
using Rb.Core.Assets;
using Rb.Core.Components;
using Rb.Rendering.Textures;

namespace Rb.Rendering.Assets
{
	/// <summary>
	/// Texture object loader
	/// </summary>
	public class TextureLoader : AssetLoader
	{
		/// <summary>
		/// Name of the property in the load parameters, that sets mipmap generation when true
		/// </summary>
		public const string GenerateMipMapsPropertyName = "generateMipMaps";

		/// <summary>
		/// Gets the asset name
		/// </summary>
		public override string Name
		{
			get { return "Texture Loader"; }
		}

		/// <summary>
		/// Gets the asset extension(s)
		/// </summary>
		public override string[] Extensions
		{
			get { return ms_Extensions; }
		}

		/// <summary>
		/// Creates default texture load parameters
		/// </summary>
		/// <param name="addAllProperties">If true, adds all dynamic properties with default values to the parameters</param>
		/// <returns>Returns default load parameters</returns>
		public override LoadParameters CreateDefaultParameters( bool addAllProperties )
		{
			LoadParameters parameters = new LoadParameters( );
			if ( addAllProperties )
			{
				parameters.Properties.Add( GenerateMipMapsPropertyName, false );
			}
			return parameters;
		}

		/// <summary>
		/// Loads an asset
		/// </summary>
		/// <param name="source">Source of the asset</param>
		/// <param name="parameters">Load parameters</param>
		/// <returns>Loaded asset</returns>
		/// <remarks>
		/// If parameters contains the bool typed parameter "generateMipMaps" set to true, the loaded texture
		/// has mipmaps generated.
		/// </remarks>
		public override object Load( ISource source, LoadParameters parameters )
		{
			parameters.CanCache = true;
			ITexture2d texture = Graphics.Factory.NewTexture2d( );

			using ( Stream stream = source.Open( ) )
			{
				bool generateMipMaps = DynamicProperties.GetProperty( parameters.Properties, GenerateMipMapsPropertyName, false );
				TextureUtils.Load( texture, stream, generateMipMaps );
			}

			return texture;
		}

		private static readonly string[] ms_Extensions = new string[]
			{
				"jpg",
				"jpeg",
				"bmp",
				"tga",
				"png"
			};
	}
}
