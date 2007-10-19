
using Rb.Core.Utils;

namespace Rb.Rendering
{
	/// <summary>
	/// Simple class for keep track of frames-per-second values
	/// </summary>
	public class FpsCounter
	{
		/// <summary>
		/// The last FPS value
		/// </summary>
		public float InstantFps
		{
			get { return m_LastFps; }
		}

		/// <summary>
		/// The FPS values averaged over the last 32 frames
		/// </summary>
		public float AverageFps
		{
			get { return m_AvgFps;  }
		}

		/// <summary>
		/// Updates the counters
		/// </summary>
		public void Update( )
		{
			long curTime = TinyTime.CurrentTime;
			float fps = 1.0f / ( float )TinyTime.ToSeconds( m_LastRenderTime, curTime );
			m_LastFps = fps;
			m_Fps[ m_FpsIndex++ ] = fps;
			m_FpsIndex = m_FpsIndex % m_Fps.Length;

			//	Calculate average (or avg+cur/N-last/N)
			float avgFps = 0;
			for ( int fpsIndex = 0; fpsIndex < m_Fps.Length; ++fpsIndex )
			{
				avgFps += m_Fps[ fpsIndex ];
			}
			avgFps /= m_Fps.Length;

			m_AvgFps = avgFps;
			m_LastRenderTime = curTime;
		}

		private float				m_LastFps;
		private float				m_AvgFps;
		private readonly float[]	m_Fps = new float[ 32 ];
		private int					m_FpsIndex = 0;
		private long				m_LastRenderTime = TinyTime.CurrentTime;
	}
}
