
using System;
using System.Collections.Generic;

namespace Poc0.LevelEditor.Core.Actions
{
	/// <summary>
	/// Maintains a stack of <see cref="IAction"/> objects
	/// </summary>
	public class UndoStack
	{
		/// <summary>
		/// Called by Push() when an action is added
		/// </summary>
		public event Action< IAction > ActionAdded;

		/// <summary>
		/// Called by Undo() when an action is undone
		/// </summary>
		public event Action< IAction > ActionUndone;
		
		/// <summary>
		/// Called by Redone() when an action is undone
		/// </summary>
		public event Action< IAction > ActionRedone;

		/// <summary>
		/// Returns true if there are actions that can be undone
		/// </summary>
		public bool CanUndo
		{
			get { return m_UndoPos > 0; }
		}

		/// <summary>
		/// Returns true if there are actions that can be redone
		/// </summary>
		public bool CanRedo
		{
			get { return m_UndoPos < m_Actions.Count; }
		}

		/// <summary>
		/// Pushes a new action onto the stack
		/// </summary>
		/// <param name="action">Action to add</param>
		public void Push( IAction action )
		{
			if ( m_UndoPos < m_Actions.Count )
			{
				m_Actions.RemoveRange( m_UndoPos, m_Actions.Count - m_UndoPos );
			}

			m_Actions.Add( action );
			m_UndoPos = m_Actions.Count;

			if ( ActionAdded != null )
			{
				ActionAdded( action );
			}
		}

		/// <summary>
		/// Undoes the current action
		/// </summary>
		public void Undo( )
		{
			if ( !CanUndo )
			{
				return;
			}
			IAction action = m_Actions[ --m_UndoPos ];
			action.Undo( );
			if ( ActionUndone != null )
			{
				ActionUndone( action );
			}
		}

		/// <summary>
		/// Redoes the currentaction
		/// </summary>
		public void Redo( )
		{
			if ( !CanRedo )
			{
				return;
			}
			IAction action = m_Actions[ m_UndoPos++ ];
			action.Redo( );
			if ( ActionRedone != null )
			{
				ActionRedone( action );
			}
		}

		private readonly List< IAction > m_Actions = new List< IAction >( );
		private int m_UndoPos = 0;
	}
}
