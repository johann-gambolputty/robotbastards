using System;


namespace Rb.Rendering.OpenGl.Md3Loader
{
	/// <summary>
	/// An MD3 animation
	/// </summary>
	public class Animation : Rb.Animation.IAnimation
	{
		/// <summary>
		/// Animation setup
		/// </summary>
		public Animation( AnimationType animType, int firstFrame, int numFrames, int loopingFrames, int framesPerSecond )
		{
			m_AnimType			= animType;
			m_FirstFrame		= firstFrame;
			m_NumFrames			= numFrames;
			m_LoopingFrames		= loopingFrames;
			m_FramesPerSecond	= framesPerSecond;
		}

		/// <summary>
		/// Gets the first frame of this animation
		/// </summary>
		public int			FirstFrame
		{
			get { return m_FirstFrame; }
			set { m_FirstFrame = value; }
		}

		/// <summary>
		/// Gets the last frame of this animation
		/// </summary>
		public int			LastFrame
		{
			get { return FirstFrame + NumFrames; }
		}

		/// <summary>
		/// Gets the number of frames this animation covers
		/// </summary>
		public int			NumFrames
		{
			get { return m_NumFrames; }
		}

		/// <summary>
		/// Gets the number of looping frames
		/// </summary>
		public int			LoopingFrames
		{
			get { return m_LoopingFrames; }
		}

		/// <summary>
		/// Gets the frames per second rate
		/// </summary>
		public int			FramesPerSecond
		{
			get { return m_FramesPerSecond; }

		}
		/// <summary>
		/// Gets the type of animation
		/// </summary>
		public AnimationType	AnimType
		{
			get { return m_AnimType; }
		}


		#region	INamed Members

		/// <summary>
		/// Gets the name of this animation
		/// </summary>
		public string			Name
		{
			get
			{
				return m_AnimType.ToString( );
			}
			set
			{
				throw new ApplicationException( "Can't set the name of an MD3 animation" );
			}
		}

		#endregion

		#region	Private stuff

		private AnimationType	m_AnimType;
		private int				m_FirstFrame;
		private int				m_NumFrames;
		private int				m_LoopingFrames;
		private int				m_FramesPerSecond;

		#endregion
	}
}
