using System;

namespace RbEngine.Interaction
{
	/// <summary>
	/// Base class for command input
	/// </summary>
	public abstract class CommandInput
	{
		/// <summary>
		/// Binds this input to a view, creating a CommandInputBinding
		/// </summary>
		public abstract CommandInputBinding	BindToView( Scene.SceneView view );
	}
}
