using System;
using RbEngine.Animation;

namespace RbOpenGlMd3Loader
{
	/// <summary>
	/// Summary description for AnimationLayer.
	/// </summary>
	public class AnimationLayer : IAnimationLayer
	{
		#region	Setup

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="set">The set of animations that this layer can use</param>
		public AnimationLayer( AnimationSet layerAnimations, ModelPart part )
		{
			m_Animations = layerAnimations;

			switch ( part )
			{
				case ModelPart.Head		:	m_IdleAnim = AnimationType.NumAnimations;	break;
				case ModelPart.Upper	:	m_IdleAnim = AnimationType.TorsoStand;		break;
				case ModelPart.Lower	:	m_IdleAnim = AnimationType.LegsIdle;		break;
			};
	
			if ( m_IdleAnim != AnimationType.NumAnimations )
			{
				PlayAnimation( m_Animations.GetAnimation( m_IdleAnim ) );
			}
		}

		#endregion

		#region	IAnimationLayer Members

		/// <summary>
		/// Gets the set of animations that this layer can play
		/// </summary>
		public IAnimationSet	LayerAnimations
		{
			get
			{
				return m_Animations;
			}
		}

		/// <summary>
		/// Gets the currently playing animation
		/// </summary>
		public IAnimation	PlayingAnimation
		{
			get
			{
				return m_CurrentAnimation;
			}
		}

		/// <summary>
		/// Gets the frame of the current animation
		/// </summary>
		public int			CurrentAnimationFrame
		{
			get
			{
				return m_CurrentAnimationFrame;
			}
		}

		/// <summary>
		/// Plays an animation
		/// </summary>
		public void			PlayAnimation( IAnimation anim )
		{
			m_CurrentAnimation		= ( Animation )anim;
			m_CurrentAnimationFrame	= m_CurrentAnimation.FirstFrame;
		}

		/// <summary>
		/// Stops playing the current animation
		/// </summary>
		public void			StopCurrentAnimation( )
		{
			if ( m_IdleAnim != AnimationType.NumAnimations )
			{
				m_CurrentAnimation		= m_Animations.GetAnimation( m_IdleAnim );
				m_CurrentAnimationFrame	= m_CurrentAnimation.FirstFrame;
			}
			else
			{
				m_CurrentAnimation		= null;
				m_CurrentAnimationFrame	= 0;
			}
		}

		/// <summary>
		/// Updates this layer
		/// </summary>
		public bool			Update( )
		{
			if ( m_CurrentAnimation != null )
			{
				if ( m_CurrentAnimation.LoopingFrames > 0 )
				{
					//	Looping animation - keep playing
					m_CurrentAnimationFrame = RbEngine.Maths.Utils.Wrap( m_CurrentAnimationFrame + 1, m_CurrentAnimation.FirstFrame, m_CurrentAnimation.LastFrame - 1 );
					return true;
				}
				++m_CurrentAnimationFrame;
				return ( m_CurrentAnimationFrame != m_CurrentAnimation.LastFrame );
			}

			return false;
		}

		#endregion

		#region	Private stuff

		private Animation		m_CurrentAnimation;
		private AnimationSet	m_Animations;
		private int				m_CurrentAnimationFrame;
		private AnimationType	m_IdleAnim;

		#endregion
	}
}
