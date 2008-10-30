
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.Textures
{
	/// <summary>
	/// Animates a set of textures
	/// </summary>
	/// <remarks>
	/// AnimatedTexture2d is not responsible for managing the lifetime of the textures
	/// </remarks>
	public class AnimatedTexture2d
	{
		/// <summary>
		/// Sets up this animated texture
		/// </summary>
		/// <param name="frames">Animation frames</param>
		/// <param name="secondsForAnimation">Time taken for the animation to cycle through all the frames</param>
		public AnimatedTexture2d( ITexture[] frames, float secondsForAnimation )
		{
			m_Frames = frames;
			m_SecondsForFullAnimation = secondsForAnimation;
		}

		/// <summary>
		/// This gets the source texture
		/// </summary>
		public ITexture SourceTexture
		{
			get
			{
				return m_Frames[ m_TextureIndex ];
			}
		}

		/// <summary>
		/// This gets the destination texture (only valid when doing smooth transitions between source and destination)
		/// </summary>
		public ITexture DestinationTexture
		{
			get
			{
				return m_Frames[ ( m_TextureIndex + 1 ) % m_Frames.Length ];
			}
		}

		/// <summary>
		/// Gets the transition value between the source and destination textures
		/// </summary>
		public float LocalT
		{
			get { return m_LocalT; }
		}


		/// <summary>
		/// Updates the animation
		/// </summary>
		public void UpdateAnimation( long renderTick )
		{
			m_FullT += ( float )TinyTime.ToSeconds( renderTick - m_LastRenderTime );
			m_FullT = Utils.Wrap( m_FullT, 0, m_SecondsForFullAnimation );
			float tPerFrame = m_SecondsForFullAnimation / m_Frames.Length;
			float textureT = ( m_FullT / m_SecondsForFullAnimation ) * m_Frames.Length;
			m_TextureIndex = ( int )textureT;
			m_LocalT = textureT - ( int )textureT;
			m_LastRenderTime = renderTick;
		}

		#region Private Members

		private readonly ITexture[] m_Frames;
		private readonly float m_SecondsForFullAnimation = 5.0f;

		private long m_LastRenderTime;
		private int m_TextureIndex;
		private float m_FullT;
		private float m_LocalT;

		#endregion
	}
}
