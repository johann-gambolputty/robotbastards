using System;

namespace RbEngine.Interaction
{
	/// <summary>
	/// Binds input events from a control to a Command object
	/// </summary>
	/// <remarks>
	/// An input binding can be bound to 1 or more controls - it's up to the CommandInputBinding derived class
	/// to subscribe to input events from those controls, using the BindToControl() method. It's also up to the
	/// derived class to detect control focus changes and react accordingly.
	/// </remarks>
	public abstract class CommandInputBinding
	{
		/// <summary>
		/// Binds to the specified control
		/// </summary>
		/// <param name="control"></param>
		public abstract void	BindToControl( System.Windows.Forms.Control control );
	}
}
