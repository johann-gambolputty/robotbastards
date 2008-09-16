using System.IO;
using Rb.Assets;
using Rb.Assets.Base;
using Rb.Assets.Interfaces;
using Rb.Core.Components;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.Textures
{
	/// <summary>
	/// Texture object loader
	/// </summary>
	public class TextureImageLoader : AssetLoader
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
		}

		/// <summary>
		/// Name of the property in the load parameters, that enables mipmap generation when true
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
			get { return s_Extensions; }
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
			ITexture2d texture = Graphics.Factory.CreateTexture2d( );

			using ( Stream stream = ( ( IStreamSource )source ).Open( ) )
			{
				bool generateMipMaps = DynamicProperties.GetProperty( parameters.Properties, GenerateMipMapsPropertyName, false );
				Texture2dUtils.Load( texture, stream, generateMipMaps );
			}

			return texture;
		}

		#region Private Members

		/// <summary>
		/// Extensions that this loader supports
		/// </summary>
		private static readonly string[] s_Extensions = new string[]
			{
				"jpg",
				"jpeg",
				"bmp",
				"tga",
				"png"
			}; 

		#endregion
	}
}
