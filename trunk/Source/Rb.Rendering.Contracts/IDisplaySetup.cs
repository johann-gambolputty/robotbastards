namespace Rb.Rendering.Contracts
{
	/// <summary>
	/// Platform independent interface for setting up displays (e.g. Rb.Rendering.Windows.Display)
	/// </summary>
	/// <seealso cref="IGraphicsFactory.CreateDisplaySetup"/>
	public interface IDisplaySetup
	{
		/// <summary>
		/// Creates the display control with given buffer sizes
		/// </summary>
		/// <param name="display">Display control object</param>
		/// <param name="colourBits">Number of bits in the colour buffer</param>
		/// <param name="depthBits">Number of bits in the depth buffer</param>
		/// <param name="stencilBits">Number of bits in the stencil buffer</param>
		void Create( object display, int colourBits, int depthBits, int stencilBits );

		/// <summary>
		/// Starts rendering to the display control
		/// </summary>
		/// <param name="displayControl">Display control object</param>
		/// <returns>Returns true if preparing the control for painting succeeded</returns>
		bool BeginPaint( object displayControl );

		/// <summary>
		/// Starts rendering to the display control
		/// </summary>
		/// <param name="displayControl">Display control object</param>
		void EndPaint( object displayControl );
	}
}
