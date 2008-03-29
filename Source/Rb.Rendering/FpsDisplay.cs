using System.Drawing;
using Rb.Rendering.Interfaces.Objects;

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
			IFont font = Graphics.Fonts.DebugFont;
			font.Write( 0, 0, m_Colour, "FPS: {0}", m_Counter.AverageFps.ToString( "G4" ) );
		}

		#region Private Members

		private Color m_Colour = Color.White;
		private readonly FpsCounter m_Counter = new FpsCounter( );

		#endregion
	}
}
