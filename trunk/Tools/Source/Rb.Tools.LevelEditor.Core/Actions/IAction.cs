
namespace Rb.Tools.LevelEditor.Core.Actions
{
	/// <summary>
	/// An action in the editor
	/// </summary>
	public interface IAction
	{
		/// <summary>
		/// Undoes the action
		/// </summary>
		void Undo( );

		/// <summary>
		/// Redoes the action
		/// </summary>
		void Redo( );
	}
}
