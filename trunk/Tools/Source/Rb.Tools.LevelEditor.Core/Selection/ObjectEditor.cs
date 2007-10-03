using System;
using System.Collections.Generic;
using Rb.Rendering;
using Rb.Tools.LevelEditor.Core.Actions;

namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// A selectable object
	/// </summary>
	[Serializable]
	public class ObjectEditor : IObjectEditor, IRenderable
	{
		/// <summary>
		/// Sets the tile object to get renderer
		/// </summary>
		public ObjectEditor( PickInfoCursor pick, object obj )
		{
			m_Object = obj;
			EditorState.Instance.CurrentScene.Renderables.Add( this );

			BuildObjectEditors( obj, pick );
		}

		/// <summary>
		/// Gets the object being edited
		/// </summary>
		public object Instance
		{
			get { return m_Object; }
		}

		/// <summary>
		/// Event, invoked when <see cref="Instance"/> (or one of its children) is modified
		/// </summary>
		public event EventHandler ObjectChanged
		{
			add { m_ObjectChanged += value; }
			remove { m_ObjectChanged -= value; }
		}
		
		/// <summary>
		/// Invokes the <see cref="ObjectChanged"/> event
		/// </summary>
		public void OnObjectChanged( )
		{
			if ( m_ObjectChanged != null )
			{
				m_ObjectChanged( this, null );
			}
		}


		#region ISelectable Members

		/// <summary>
		/// Gets/sets the selected flag
		/// </summary>
		public bool Selected
		{
			get { return m_Selected; }
			set { m_Selected = value; }
		}

		/// <summary>
		/// Gets/sets the highlighted flag
		/// </summary>
		public bool Highlighted
		{
			get { return m_Highlighted; }
			set { m_Highlighted = value; }
		}

		/// <summary>
		/// Checks to see if this object is picked
		/// </summary>
		/// <param name="pick">Pick information</param>
		/// <returns>Returns true if picked</returns>
		public IPickable TestPick(IPickInfo pick)
		{
			foreach ( IPickable selectable in m_Selectables )
			{
				IPickable testResult = selectable.TestPick( pick );
				if ( testResult != null )
				{
					return testResult;
				}
			}
			return null;
		}

		/// <summary>
		/// Throws an exception - objects of this type should never be queried directly for a pick action
		/// </summary>
		public IPickAction CreatePickAction( IPickInfo pick )
		{
			throw new NotImplementedException( );
		}

		/// <summary>
		/// Always returns false
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public bool SupportsPickAction( IPickAction action )
		{
			return false;
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders all child selectable objects
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			foreach ( ISelectable childObj in m_Selectables )
			{
				IRenderable childRenderable = childObj as IRenderable;
				if ( childRenderable != null )
				{
					childRenderable.Render( context );
				}
			}
		}

		#endregion
		
		#region Private members

		[NonSerialized]
		private bool m_Selected;

		[NonSerialized]
		private bool m_Highlighted;

		[NonSerialized]
		private EventHandler m_ObjectChanged;

		private readonly List< ISelectable > m_Selectables = new List< ISelectable >( );

		private readonly object m_Object;
		
		private static void BuildObjectEditors( object obj, PickInfoCursor pick )
		{
			//IHasPosition hasPosition = Rb.Core.Components.Parent.GetType< IHasPosition >( obj );
			//if ( hasPosition != null )
			//{
			//    hasPosition.Position = new Point3( x, 0, z );
			//    AddChild( new PositionEditor( this, hasPosition ) );
			//}
		}

		#endregion
	}
}
