
namespace Rb.Rendering
{
	/// <summary>
	/// Platform independent interface for setting up displays (e.g. Rb.Rendering.Windows.Display)
	/// </summary>
	/// <seealso cref="RenderFactory.CreateDisplaySetup"/>
	public interface IDisplaySetup
	{
		/// <summary>
		/// Creates the display control with given buffer sizes
		/// </summary>
		/// <param name="display">Display control object</param>
		/// <param name="colourBits">Number of bits in the colour buffer</param>
		/// <param name="depthBits">Number of bits in the depth buffer</param>
		/// <param name="stencilBits">Number of bits in the stencil buffer</param>
		void Create( object display, byte colourBits, byte depthBits, byte stencilBits );

		/// <summary>
		/// Starts rendering to the display control
		/// </summary>
		/// <param name="display">Display control object</param>
		/// <returns>Returns true if preparing the control for painting succeeded</returns>
		bool BeginPaint( object display );

		/// <summary>
		/// Starts rendering to the display control
		/// </summary>
		void EndPaint( object display );
	}
}
