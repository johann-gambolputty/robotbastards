using System.Collections.Generic;

namespace Rb.Tools.LevelEditor.Core.Actions
{
	/// <summary>
	/// Stores a list of actions. Undo and Redo apply to the entire list
	/// </summary>
	class ActionList : List< IAction >, IAction
	{
		#region IAction Members

		/// <summary>
		/// Undoes all stored actions
		/// </summary>
		public void Undo( )
		{
			foreach ( IAction action in this )
			{
				action.Undo( );
			}
		}

		/// <summary>
		/// Redoes all stored actions
		/// </summary>
		public void Redo( )
		{
			foreach ( IAction action in this )
			{
				action.Redo( );
			}
		}

		#endregion
	}
}
