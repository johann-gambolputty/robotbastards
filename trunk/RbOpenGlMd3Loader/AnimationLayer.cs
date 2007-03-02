using System;
using RbEngine.Animation;

namespace RbOpenGlMd3Loader
{
	/// <summary>
	/// Summary description for AnimationLayer.
	/// </summary>
	public class AnimationLayer : IAnimationLayer, RbEngine.Components.INamedObject
	{
		#region	Setup

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="set">The set of animations that this layer can use</param>
		public AnimationLayer( string name, AnimationSet layerAnimations )
		{
			m_Name			= name;
			m_Animations	= layerAnimations;
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
		/// Plays an animation
		/// </summary>
		public void			PlayAnimation( IAnimation anim )
		{
			m_CurrentAnimation = anim;
		}

		/// <summary>
		/// Stops playing the current animation
		/// </summary>
		public void			StopCurrentAnimation( )
		{
			m_CurrentAnimation = null;
		}

		#endregion

		#region INamedObject Members

		/// <summary>
		/// Event, invoked when the name of this layer changes
		/// </summary>
		public event RbEngine.Components.NameChangedDelegate NameChanged;

		/// <summary>
		/// The name of this layer
		/// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
				if ( NameChanged != null )
				{
					NameChanged( this );
				}
			}
		}

		#endregion

		#region	Private stuff

		private IAnimation		m_CurrentAnimation;
		private IAnimationSet	m_Animations;
		private string			m_Name;

		#endregion
	}
}
