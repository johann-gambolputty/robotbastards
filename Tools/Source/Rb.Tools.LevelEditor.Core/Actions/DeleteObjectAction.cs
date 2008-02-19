using System.Collections;
using System.Collections.Generic;
using Rb.Tools.LevelEditor.Core.Selection;

namespace Rb.Tools.LevelEditor.Core.Actions
{
	/// <summary>
	/// Deletes a set of objects from the scene
	/// </summary>
	class DeleteObjectAction : IAction
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="objects">Objects to delete from the scene</param>
		public DeleteObjectAction( IEnumerable objects )
		{
			m_Objects = new List< IDelete >( );
			foreach ( object obj in objects )
			{
				if ( obj is IDelete )
				{
					m_Objects.Add( ( IDelete )obj );
				}
				else if ( obj is ISelectionModifier )
				{
					//	Bit of a cheat - if the object is a selection modifier, then check if the modified result
					//	is deletable
					IDelete mod = ( ( ISelectionModifier )obj ).SelectedObject as IDelete;
					if ( mod != null )
					{
						m_Objects.Add( mod );
					}
				}
			}
			Redo( );
		}

		#region IAction Members

		/// <summary>
		/// Undoes the action
		/// </summary>
		public void Undo( )
		{
			foreach ( IDelete obj in m_Objects )
			{
				obj.UnDelete( );
			}
		}

		/// <summary>
		/// Redoes the action
		/// </summary>
		public void Redo( )
		{
			foreach ( IDelete obj in m_Objects )
			{
				obj.Delete( );
			}
		}

		#endregion

		#region Private members

		private readonly List< IDelete > m_Objects;

		#endregion
	}
}
