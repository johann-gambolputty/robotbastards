using System;

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
		/// Gets the in-game object
		/// </summary>
		object Instance
		{
			get;
		}
	}
}
