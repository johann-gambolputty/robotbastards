using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Rb.Core.Components;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Delegate, used by the <see cref="SelectedObjects.ObjectSelected"/> event
	/// </summary>
	public delegate void ObjectSelectedDelegate( object selectedObject );

	/// <summary>
	/// Delegate, used by the <see cref="SelectedObjects.ObjectDeselected"/> event
	/// </summary>
	public delegate void ObjectDeselectedDelegate( object deselectedObject );

	/// <summary>
	/// Stores selected objects
	/// </summary>
	public class SelectedObjects
	{
		#region Public properties

		/// <summary>
		/// Gets the selected objects
		/// </summary>
		public IEnumerable Selection
		{
			get { return m_Selection; }
		}

		#endregion

		#region Public events

		/// <summary>
		/// Event, invoked when an object is selected
		/// </summary>
		public event ObjectSelectedDelegate ObjectSelected;

		/// <summary>
		/// Event, invoked when an object is deselected
		/// </summary>
		public event ObjectDeselectedDelegate ObjectDeselected;

		#endregion

		#region Public methods

		public void ApplySelect( object obj, bool addToSelection )
		{
			if ( addToSelection )
			{
				if ( m_Selection.Contains( obj ) )
				{
					Deselect( obj );
				}
				else
				{
					Select( obj );
				}
			}
			else
			{
				bool select = !m_Selection.Contains( obj );
				ClearSelection( );

				if ( select )
				{
					Select( obj );
				}
			}
		}

		/// <summary>
		/// Adds an object to the selection
		/// </summary>
		public void Select( object obj )
		{
			if ( !m_Selection.Contains( obj ) )
			{
				if ( ObjectSelected != null )
				{
					ObjectSelected( obj );
				}
				m_Selection.Add( obj );
				SetSelectedFlag( obj, true );
			}
		}

		/// <summary>
		/// Removes an object from the selection
		/// </summary>
		private void Deselect( object obj )
		{
			SetSelectedFlag( obj, false );
			if ( ObjectDeselected != null )
			{
				ObjectDeselected( obj );
			}
			m_Selection.Remove( obj );
		}

		/// <summary>
		/// Clears the selection
		/// </summary>
		public void ClearSelection( )
		{
			foreach ( object obj in m_Selection )
			{
				SetSelectedFlag( obj, false );

				if ( ObjectDeselected != null )
				{
					ObjectDeselected( obj );
				}
			}
			m_Selection.Clear( );
		}
		
		/// <summary>
		/// Sets the selected flag in an object
		/// </summary>
		private static void SetSelectedFlag( object obj, bool selected )
		{
			ISelectable selectable = ( obj as ISelectable );
			if ( selectable != null )
			{
				selectable.Selected = selected;
			}
			else
			{
				selectable = ParentHelpers.GetChildOfType< ISelectable >( obj as IParent );
				if ( selectable != null )
				{
					selectable.Selected = selected;
				}
			}
		}

		#endregion

		#region Private stuff

		private readonly ArrayList m_Selection = new ArrayList( );

		#endregion
	}
}
