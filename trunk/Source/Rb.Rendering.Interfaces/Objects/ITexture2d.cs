
namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// 2 dimensional texture interface
	/// </summary>
	public interface ITexture2d : ITexture
	{
		/// <summary>
		/// Gets the width of the texture
		/// </summary>
		int Width
		{
			get;
		}

		/// <summary>
		/// Gets the height of the texture
		/// </summary>
		int Height
		{
			get;
		}

		/// <summary>
		/// Creates the texture from a single texture data object
		/// </summary>
		/// <param name="data">Texture data used to create the texture</param>
		/// <param name="generateMipMaps">Mipmap generation flag</param>
		void Create( Texture2dData data, bool generateMipMaps );

		/// <summary>
		/// Creates the texture from an array of texture data objects, that specify decreasing mipmap levels
		/// </summary>
		/// <param name="data">Texture data used to create the texture and its mipmaps</param>
		void Create( Texture2dData[] data );

		/// <summary>
		/// Gets texture data from this texture
		/// </summary>
		/// <param name="getMipMaps">If true, texture data for all mipmap levels are retrieved</param>
		/// <returns>
		/// Returns texture data extracted from this texture. If getMipMaps is false, only one <see cref="Texture2dData"/>
		/// object is returned. Otherwise, the array contains a <see cref="Texture2dData"/> object for each mipmap
		/// level.</returns>
		Texture2dData[] ToTextureData( bool getMipMaps );
	}
}
