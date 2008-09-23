
using Rb.Assets;

namespace Rb.Rendering.Textures
{
	/// <summary>
	/// Texture loading parameters
	/// </summary>
	public class TextureLoadParameters : LoadParameters
	{
		/// <summary>
		/// Name of the property in the load parameters, that enables mipmap generation when true
		/// </summary>
		public const string GenerateMipMapsPropertyName = "generateMipMaps";

		/// <summary>
		/// Name of the property in the load parameters that forces the loader to return texture data, not textures
		/// </summary>
		public const string ReturnTextureDataOnlyName = "returnTextureDataOnly";

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
}
