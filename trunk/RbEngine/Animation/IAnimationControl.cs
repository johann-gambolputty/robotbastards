using System;

namespace RbEngine.Animation
{
	/// <summary>
	/// Interface for controlling animation
	/// </summary>
	public interface IAnimationControl
	{
		/// <summary>
		/// Gets an animation layer
		/// </summary>
		IAnimationLayer	GetLayer( string name );
	}
}
