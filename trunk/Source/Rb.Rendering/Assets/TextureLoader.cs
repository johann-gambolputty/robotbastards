using System.IO;
using Rb.Core.Assets;
using Rb.Core.Components;

namespace Rb.Rendering.Assets
{
	/// <summary>
	/// Texture object loader
	/// </summary>
	public class TextureLoader : AssetLoader
	{
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
			Texture2d texture = Graphics.Factory.NewTexture2d( );

			using ( Stream stream = source.Open( ) )
			{
				bool generateMipMaps = DynamicProperties.GetProperty( parameters.Properties, "generateMipMaps", false );
				texture.Load( stream, generateMipMaps );
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
