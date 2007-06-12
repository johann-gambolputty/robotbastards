using System;

namespace RbEngine.Animation
{
	/// <summary>
	/// An animation layer is a set of objects that are affected by a set of animations
	/// </summary>
	public interface IAnimationLayer
	{
		/// <summary>
		/// Gets the animations that this layer can play
		/// </summary>
		IAnimationSet		LayerAnimations
		{
			get;
		}

		/// <summary>
		/// Gets the currently playing animation
		/// </summary>
		IAnimation			PlayingAnimation
		{
			get;
		}

		/// <summary>
		/// Plays an animation
		/// </summary>
		void				PlayAnimation( IAnimation anim );

		/// <summary>
		/// Stops playing the current animation
		/// </summary>
		void				StopCurrentAnimation( );
	}
}
