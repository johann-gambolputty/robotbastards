using System;

namespace Rb.Animation
{
	/// <summary>
	/// A set of animations
	/// </summary>
	public interface IAnimationSet
	{
		/// <summary>
		/// Finds an animation in the set by name
		/// </summary>
		IAnimation	Find( string animationName );
	}
}
