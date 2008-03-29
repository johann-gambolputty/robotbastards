using System.Drawing;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Rb.World.Rendering;
using System.Diagnostics;

namespace Poc0.Core
{
	/// <summary>
	/// Shows performance information, specified in the <see cref="DebugInfo"/>
	/// </summary>
	public class PerformanceDisplay : SceneRenderable
	{
		/// <summary>
		/// Gets/sets the text colour
		/// </summary>
		public Color TextColour
		{
			get { return m_TextColour; }
			set { m_TextColour = value; }
		}

		/// <summary>
		/// Renders performance information
		/// </summary>
		/// <param name="context">Rendering context</param>
		public override void Render( IRenderContext context )
		{
			m_Counter.Update( );

			IFont font = Rb.Rendering.Graphics.Fonts.DebugFont;
			int y = 0;
			if ( DebugInfo.ShowFps )
			{
				font.Write( 0, y, TextColour, "Fps: {0:F2}", m_Counter.AverageFps );
				y += font.MaximumHeight + 2;
			}
			if ( DebugInfo.ShowMemoryWorkingSet )
			{
				Process currentProcess = Process.GetCurrentProcess( );
				font.Write( 0, y, TextColour, "Mem: {0:N}kb", currentProcess.WorkingSet64 / 1024 );
				y += font.MaximumHeight + 2;
			}
			if ( DebugInfo.ShowMemoryPeakWorkingSet )
			{
				Process currentProcess = Process.GetCurrentProcess( );
				font.Write( 0, y, TextColour, "PMem: {0:N}kb", currentProcess.PeakWorkingSet64 / 1024 );
			}

			base.Render( context );
		}

		#region Private Members

		private readonly FpsCounter m_Counter = new FpsCounter( );
		private Color m_TextColour = Color.WhiteSmoke;

		#endregion
	}
}
