using System;
using Rb.World;

namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// In-game object editor
	/// </summary>
	public interface IObjectEditor
	{
		/// <summary>
		/// Raised when the object's properties change
		/// </summary>
		event EventHandler ObjectChanged;

		/// <summary>
		/// Builds the object(s) associated with this editor, adding them to a new scene
		/// </summary>
		/// <param name="scene">New scene to add created objects to</param>
		void Build( Scene scene );
	}
}
