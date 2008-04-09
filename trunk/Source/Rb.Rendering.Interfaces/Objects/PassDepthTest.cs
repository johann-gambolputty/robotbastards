
namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Depth test pass values (pixel is rendered if the result of a depth test is the selected value)
	/// </summary>
	/// <seealso cref="IRenderState.PassDepthTest"/>
	public enum PassDepthTest
	{
		/// <summary>
		/// Depth test will never be passed
		/// </summary>
		Never,

		/// <summary>
		/// Depth test will be passed if the rendered depth is less than the fragment depth
		/// </summary>
		Less,

		/// <summary>
		/// Depth test will be passed if the rendered depth is less than or equal to the fragment depth
		/// </summary>
		LessOrEqual,

		/// <summary>
		/// Depth test will be passed if the rendered depth is equal to the fragment depth
		/// </summary>
		Equal,

		/// <summary>
		/// Depth test will be passed if the rendered depth is not equal to the fragment depth
		/// </summary>
		NotEqual,

		/// <summary>
		/// Depth test will be passed if the rendered depth is greater than or equal to the fragment depth
		/// </summary>
		GreaterOrEqual,

		/// <summary>
		/// Depth test will be passed if the rendered depth is greater than the fragment depth
		/// </summary>
		Greater,

		/// <summary>
		/// Depth test will always be passed
		/// </summary>
		Always
	}
}
