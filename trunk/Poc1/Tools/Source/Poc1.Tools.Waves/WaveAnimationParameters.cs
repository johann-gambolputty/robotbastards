namespace Poc1.Tools.Waves
{
	/// <summary>
	/// Wave map parameters
	/// </summary>
	public class WaveAnimationParameters
	{
		/// <summary>
		/// Clones these parameters
		/// </summary>
		public WaveAnimationParameters Clone( )
		{
			WaveAnimationParameters clone = new WaveAnimationParameters( );
			clone.Width = Width;
			clone.Height = Height;
			clone.Frames = Frames;
			clone.Time = Time;
			clone.WindDirectionDegrees = WindDirectionDegrees;
			clone.WindSpeed = WindSpeed;
			clone.WaveModifier = WaveModifier;
			return clone;
		}

		/// <summary>
		/// Gets/sets the width of the output bitmaps
		/// </summary>
		public int Width
		{
			get { return m_Width; }
			set { m_Width = value; }
		}

		/// <summary>
		/// Gets/sets the height of the output bitmaps
		/// </summary>
		public int Height
		{
			get { return m_Height; }
			set { m_Height = value; }
		}

		/// <summary>
		/// Gets/sets the number of frames generated
		/// </summary>
		public int Frames
		{
			get { return m_Frames; }
			set { m_Frames = value; }
		}

		/// <summary>
		/// Gets/sets the time span covered by the animation
		/// </summary>
		public float Time
		{
			get { return m_Time; }
			set { m_Time = value; }
		}

		/// <summary>
		/// Gets/sets the wind direction angle, in degrees
		/// </summary>
		public float WindDirectionDegrees
		{
			get { return m_WindDirection; }
			set { m_WindDirection = value; }
		}

		/// <summary>
		/// Gets/sets the wind speed
		/// </summary>
		public float WindSpeed
		{
			get { return m_WindSpeed; }
			set { m_WindSpeed = value; }
		}

		/// <summary>
		/// Gets/sets the wave modifier
		/// </summary>
		public float WaveModifier
		{
			get { return m_WaveModifier; }
			set { m_WaveModifier = value; }
		}

		#region Private Members

		private int		m_Width = 512;
		private int		m_Height = 512;
		private int		m_Frames = 32;
		private float	m_Time = 0.2f;
		private float	m_WindDirection;
		private float	m_WindSpeed = 14;
		private float	m_WaveModifier = 0.00028f;

		#endregion
	}
}
