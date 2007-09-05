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

		/// <summary>
		/// Converts the selected object list to an array
		/// </summary>
		public object[] ToArray( )
		{
			return m_Selection.ToArray( );
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

		/// <summary>
		/// Returns true if the specified object is selected
		/// </summary>
		public bool IsSelected( object obj )
		{
			return m_Selection.Contains( obj );
		}

		/// <summary>
		/// Applies selection operation to an object
		/// </summary>
		/// <param name="obj">Object to (de)select</param>
		/// <param name="addToSelection">true if the object should be added to the selection, false to replace the selection</param>
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

		public void ApplySelect( object[] objects, bool addToSelection )
		{
			if ( !addToSelection )
			{
				ClearSelection( );
			}
			foreach ( object obj in objects )
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
				else if ( !m_Selection.Contains( obj ) )
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
				m_Selection.Add( obj );
				SetSelectedFlag( obj, true );
				if ( ObjectSelected != null )
				{
					ObjectSelected( obj );
				}
			}
		}

		/// <summary>
		/// Removes an object from the selection
		/// </summary>
		public void Deselect( object obj )
		{
			SetSelectedFlag( obj, false );
			m_Selection.Remove( obj );
			if ( ObjectDeselected != null )
			{
				ObjectDeselected( obj );
			}
		}

		/// <summary>
		/// Clears the selection
		/// </summary>
		public void ClearSelection( )
		{
			while ( m_Selection.Count > 0 )
			{
				object obj = m_Selection[ 0 ];
				SetSelectedFlag( obj, false );
				m_Selection.RemoveAt( 0 );

				if ( ObjectDeselected != null )
				{
					ObjectDeselected( obj );
				}
			}
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
				selectable = Parent.GetChildOfType< ISelectable >( obj as IParent );
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
