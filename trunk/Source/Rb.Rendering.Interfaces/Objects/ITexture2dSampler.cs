
namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Binds/unbinds an attached texture object
	/// </summary>
	/// <remarks>
	/// Beginning the sampler pass applies the texture, ending the pass unbinds the texture
	/// </remarks>
	public interface ITexture2dSampler : IPass
	{
		/// <summary>
		/// Access to the bound texture
		/// </summary>
		ITexture2d Texture
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

		/// <summary>
		/// Access to the way the texture sampling changes when the texture S coordinate reaches the edge of the texture
		/// </summary>
		TextureWrap WrapS
		{
			get; set;
		}

		/// <summary>
		/// Access to the way the texture sampling changes when the texture T coordinate reaches the edge of the texture
		/// </summary>
		TextureWrap WrapT
		{
			get; set;
		}

		/// <summary>
		/// Sets the way that the texture is interpreted when texturing a fragment
		/// </summary>
		TextureMode Mode
		{
			get; set;
		}
	}
}
