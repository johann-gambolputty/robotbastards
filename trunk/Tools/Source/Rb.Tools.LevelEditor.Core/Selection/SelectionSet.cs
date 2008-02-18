using System;
using System.Collections;
using Rb.Core.Components;

namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// Delegate, used by the <see cref="SelectionSet.ObjectSelected"/> event
	/// </summary>
	public delegate void ObjectSelectedDelegate( object selectedObject );

	/// <summary>
	/// Delegate, used by the <see cref="SelectionSet.ObjectDeselected"/> event
	/// </summary>
	public delegate void ObjectDeselectedDelegate( object deselectedObject );

	/// <summary>
	/// Stores selected objects
	/// </summary>
	public class SelectionSet
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
		public event ObjectSelectedDelegate ObjectSelected
		{
			add { m_ObjectSelected += value; }
			remove { m_ObjectSelected -= value; }
		}

		/// <summary>
		/// Event, invoked when an object is deselected
		/// </summary>
		public event ObjectDeselectedDelegate ObjectDeselected
		{
			add { m_ObjectDeselected += value; }
			remove { m_ObjectDeselected -= value; }
		}

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
		//	obj = GetActualSelectedObject( obj );
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
		/// Applies selection operation to an array of objects
		/// </summary>
		/// <param name="objects">Objects to (de)select</param>
		/// <param name="addToSelection">If true, then objects are added without clearing the
		///  existing selection</param>
		public void ApplySelect( object[] objects, bool addToSelection )
		{
			if ( !addToSelection )
			{
				ClearSelection( );
			}
			foreach ( object obj in objects )
			{
				object actualObj = obj; // GetActualSelectedObject(obj);
				if ( addToSelection )
				{
					if ( m_Selection.Contains( actualObj ) )
					{
						Deselect( actualObj );
					}
					else
					{
						Select( actualObj );
					}
				}
				else if ( !m_Selection.Contains( actualObj ) )
				{
					Select( actualObj );
				}
			}
		}

		/// <summary>
		/// Adds an object to the selection
		/// </summary>
		public void Select( object obj )
		{
			if ( obj == null )
			{
				return;
			}
			if ( !m_Selection.Contains( obj ) )
			{
				m_Selection.Add( obj );
				SetSelectedFlag( obj, true );
				if ( m_ObjectSelected != null )
				{
					m_ObjectSelected( obj );
				}
			}
		}

		/// <summary>
		/// Removes an object from the selection
		/// </summary>
		public void Deselect( object obj )
		{
			if ( obj == null )
			{
				return;
			}
			SetSelectedFlag( obj, false );
			m_Selection.Remove( obj );
			if ( m_ObjectDeselected != null )
			{
				m_ObjectDeselected( obj );
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

				if ( m_ObjectDeselected != null )
				{
					m_ObjectDeselected( obj );
				}
			}
		}
		
		/// <summary>
		/// Sets the selected flag in an object
		/// </summary>
		private static void SetSelectedFlag( object obj, bool selected )
		{
			ISelectable selectable = ( obj as ISelectable );
			if ( selectable == null )
			{
				selectable = Parent.GetChildOfType< ISelectable >( obj as IParent );
			}

			if ( selectable != null )
			{
				selectable.Selected = selected;
			}
		}

		#endregion

		#region Private stuff

		[NonSerialized]
		private ObjectSelectedDelegate m_ObjectSelected;

		[NonSerialized]
		private ObjectDeselectedDelegate m_ObjectDeselected;

		private readonly ArrayList m_Selection = new ArrayList( );
		
		//private static object GetActualSelectedObject( object obj )
		//{
		//    ISelectionModifier modifier = obj as ISelectionModifier;
		//    return ( modifier != null ) ? modifier.SelectedObject : obj;
		//}

		#endregion
	}
}
