using System.Drawing;

namespace Rb.Rendering
{
	/// <summary>
	/// Renderable object that displays the average frames per second
	/// </summary>
	public class FpsDisplay : IRenderable
	{
		/// <summary>
		/// Access to the text colour
		/// </summary>
		public Color TextColour
		{
			get { return m_Colour;  }
			set { m_Colour = value; }
		}

		/// <summary>
		/// Renders the display
		/// </summary>
		public void Render( IRenderContext context )
		{
			m_Counter.Update( );

			//	Display the FPS
			RenderFont font = RenderFonts.GetDefaultFont( DefaultFont.Debug );
			font.DrawText( 0, 0, m_Colour, Color.Black, "FPS: {0}", m_Counter.AverageFps.ToString( "G4" ) );
		}

		private Color m_Colour = Color.White;
		private FpsCounter m_Counter = new FpsCounter( );

	}
}
