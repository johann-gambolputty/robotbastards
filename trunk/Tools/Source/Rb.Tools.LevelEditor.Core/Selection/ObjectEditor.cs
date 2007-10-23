using System;
using System.Collections.Generic;
using Rb.Rendering;

namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// An object used for editing an in-game object
	/// </summary>
	[Serializable]
	public class ObjectEditor : IObjectEditor, IRenderable, ISelectable
	{
		/// <summary>
		/// Sets the tile object to get renderer
		/// </summary>
		public ObjectEditor( object obj )
		{
			m_Object = obj;
			EditorState.Instance.CurrentScene.Renderables.Add( this );
		}

		/// <summary>
		/// Adds a child editor
		/// </summary>
		/// <param name="childEditor">Child editor</param>
		public void Add( IObjectEditor childEditor )
		{
			m_Children.Add( childEditor );
			childEditor.ObjectChanged += ChildObjectChanged;
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

		#region IObjectEditor members

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

		#endregion


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

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders all child selectable objects
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			foreach ( IObjectEditor editor in m_Children )
			{
				IRenderable childRenderable = editor as IRenderable;
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

		private readonly List< IObjectEditor > m_Children = new List< IObjectEditor >( );

		private readonly object m_Object;

		/// <summary>
		/// Called when a child editor changes an object
		/// </summary>
		private void ChildObjectChanged( object sender, EventArgs args )
		{
			OnObjectChanged( );
		}

		#endregion

	}
}