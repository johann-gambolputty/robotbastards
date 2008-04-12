
namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Interface for samplers that can handle cube map textures
	/// </summary>
	/// <seealso cref="ICubeMapTexture"/>
	public interface ICubeMapTextureSampler : IPass
	{
		/// <summary>
		/// Access to the bound texture
		/// </summary>
		ICubeMapTexture Texture
		{
			get; set;
		}

		/// <summary>
		/// The filter used when the area covered by a fragment is greater than the area of a texel
		/// </summary>
		TextureFilter MinFilter
		{
			get; set;
		}

		/// <summary>
		/// The filter used when the area covered by a fragment is less than the area of a texel. Can be either kNearest or kLinear
		/// </summary>
		TextureFilter MagFilter
		{
			get; set;
		}

	}
}