using System;

namespace Rb.Animation
{
	/// <summary>
	/// Support for creating animation controls (<see cref="IAnimationControl"/> objects)
	/// </summary>
	public interface IAnimationControlBuilder
	{
		/// <summary>
		/// Returns a new animation control
		/// </summary>
		IAnimationControl	CreateControl( );
	}
}
