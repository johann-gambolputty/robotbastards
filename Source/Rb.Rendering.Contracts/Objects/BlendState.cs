
namespace Rb.Rendering.Contracts.Objects
{
	/// <summary>
	/// Blending factors
	/// </summary>
	public enum BlendFactor
	{
		/// <summary>
		/// Zero contribution from input
		/// </summary>
		Zero,

		/// <summary>
		/// Input blended by destination colour
		/// </summary>
		DstColour,

		/// <summary>
		/// Input blended by one minus the destination colour
		/// </summary>
		OneMinusDstColour,

		/// <summary>
		/// Input blended by destination alpha
		/// </summary>
		DstAlpha,

		/// <summary>
		/// Input blended by one minus the destination alpha
		/// </summary>
		OneMinusDstAlpha,

		/// <summary>
		/// Input blended by source colour
		/// </summary>
		SrcColour,

		/// <summary>
		/// Input blended by one minus the source colour
		/// </summary>
		OneMinusSrcColour,

		/// <summary>
		/// Input blended by source alpha
		/// </summary>
		SrcAlpha,

		/// <summary>
		/// Input blended by one minus the source alpha
		/// </summary>
		OneMinusSrcAlpha,


		/// <summary>
		/// Input blended by saturated source alpha
		/// </summary>
		SrcAlphaSaturate,

		/// <summary>
		/// Input only
		/// </summary>
		One
	}
}
