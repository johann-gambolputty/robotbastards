
namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Texture filtering options
	/// </summary>
	public enum TextureFilter
	{
		/// <summary>
		/// Chooses the nearest texel from the source texture
		/// </summary>
		NearestTexel,

		/// <summary>
		/// Weighted average of the four nearest texels from the source texture
		/// </summary>
		LinearTexel,

		/// <summary>
		/// Chooses the nearest texel from the nearest mipmap
		/// </summary>
		NearestTexelNearestMipMap,

		/// <summary>
		/// Takes the weighted average of the four nearest texels from the nearest mipmap
		/// </summary>
		LinearTexelNearestMipMap,

		/// <summary>
		/// Takes the nearest texel from the two nearest mipmaps, and returns their weighted average
		/// </summary>
		NearestTexelLinearMipMap,

		/// <summary>
		/// Takes the weighted average of the 4 nearest texels from the two nearest mipmaps, and returns the weighted average of these two values
		/// </summary>
		LinearTexelLinearMipMap

	};
}
