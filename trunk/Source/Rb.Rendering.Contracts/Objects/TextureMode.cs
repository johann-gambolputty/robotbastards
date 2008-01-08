
namespace Rb.Rendering.Contracts.Objects
{
	/// <summary>
	/// Texture mode types
	/// </summary>
	public enum TextureMode
	{
		/// <summary>
		/// Replaces fragment with texel
		/// </summary>
		Replace,

		/// <summary>
		/// Combines fragment and texel
		/// </summary>
		Modulate,

		/// <summary>
		/// Blends fragment and texel
		/// </summary>
		Decal,

		/// <summary>
		/// Blends fragment and colour
		/// </summary>
		Blend
	};
}
