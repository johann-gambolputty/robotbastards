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

			DebugText.Write( "FPS: {0:F2}", m_Counter.AverageFps );
		}

		#region Private Members

		private Color m_Colour = Color.White;
		private readonly FpsCounter m_Counter = new FpsCounter( );

		#endregion
	}
}
