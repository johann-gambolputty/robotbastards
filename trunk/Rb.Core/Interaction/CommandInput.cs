using System;

namespace Rb.Core.Interaction
{
	/// <summary>
	/// Base class for command input
	/// </summary>
	public abstract class CommandInput
	{
		/// <summary>
		/// Binds this input to a view, creating a CommandInputBinding
		/// </summary>
		public abstract CommandInputBinding	BindToView( Command cmd, Scene.SceneView view );
	}
}
