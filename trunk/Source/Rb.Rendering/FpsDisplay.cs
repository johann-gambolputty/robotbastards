using System;
using System.Drawing;
using Rb.Core.Utils;

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
			long curTime = TinyTime.CurrentTime;
			float fps = 1.0f / ( float )TinyTime.ToSeconds( m_LastRenderTime, curTime );
			m_Fps[ m_FpsIndex++ ] = fps;
			m_FpsIndex = m_FpsIndex % m_Fps.Length;

			//	Calculate average (or avg+cur/N-last/N)
			float avgFps = 0;
			for ( int fpsIndex = 0; fpsIndex < m_Fps.Length; ++fpsIndex )
			{
				avgFps += m_Fps[ fpsIndex ];
			}
			avgFps /= m_Fps.Length;

			//	Display the FPS
			RenderFont font = RenderFonts.GetDefaultFont( DefaultFont.Debug );
			font.DrawText( 0, 0, m_Colour, "FPS: {0}", avgFps.ToString( "G4" ) );

			m_LastRenderTime = curTime;
		}

		/// <summary>
		/// Frame times for the last N frames
		/// </summary>
		private float[]	m_Fps = new float[ 16 ];

		/// <summary>
		/// Current index in the m_Fps array
		/// </summary>
		private int		m_FpsIndex = 0;

		/// <summary>
		/// Colour that the FPS text is rendered as
		/// </summary>
		private Color	m_Colour = Color.White;

		/// <summary>
		/// The last time Render() was called
		/// </summary>
		private long	m_LastRenderTime = TinyTime.CurrentTime;

	}
}
