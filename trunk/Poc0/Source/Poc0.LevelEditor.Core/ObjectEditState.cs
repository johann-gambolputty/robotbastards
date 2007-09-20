using System;
using System.Drawing;
using Poc0.Core;
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.World;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Edits the position of a world frame object
	/// </summary>
	[Serializable]
	public class PositionEditor : IRenderable, IHasPosition
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="parent">Parent edit state</param>
		/// <param name="hasPosition">Positionable object</param>
		public PositionEditor( ObjectEditState parent, IHasPosition hasPosition )
		{
			m_Parent = parent;
			m_HasPosition = hasPosition;
		}

		#region IRenderable Members

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			//	TODO: AP: Render object bounds
			int x = ( int )m_HasPosition.Position.X - 5;
			int y = ( int )m_HasPosition.Position.Z - 5;
			int width = 10;
			int height = 10;

			Color colour = m_Parent.Selected ? Color.Red : Color.White;

			ShapeRenderer.Instance.DrawRectangle( x, y, width, height, colour );
		}

		#endregion

		private readonly ObjectEditState m_Parent;
		private readonly IHasPosition m_HasPosition;

		#region IHasWorldFrame Members
		
		/// <summary>
		/// Event, raised when Position is changed
		/// </summary>
		public event PositionChangedDelegate PositionChanged
		{
			add { throw new NotSupportedException( ); }
			remove { throw new NotSupportedException( ); }
		}

		/// <summary>
		/// Object's position
		/// </summary>
		public Point3 Position
		{
			get { return m_HasPosition.Position; }
			set { m_HasPosition.Position = value; }
		}

		#endregion
	}

	/// <summary>
	/// State of an object in the editor
	/// </summary>
	[Serializable]
	public class ObjectEditState : Component, ISelectable, IRenderable
	{
		/// <summary>
		/// Gets the object being edited
		/// </summary>
		public object Instance
		{
			get { return m_Object; }
		}

		/// <summary>
		/// Delegate used by the ObjectChanged event
		/// </summary>
		public delegate void ObjectChangedDelegate( );

		/// <summary>
		/// Event, invoked when <see cref="Instance"/> (or one of its children) is modified
		/// </summary>
		public event ObjectChangedDelegate ObjectChanged
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
				m_ObjectChanged( );
			}
		}

		/// <summary>
		/// Sets the tile object to get renderer
		/// </summary>
		public ObjectEditState( Scene scene, float x, float y, object obj )
		{
			m_Object = obj;
			scene.Renderables.Add( this );

			BuildObjectEditors( obj, x, y );
		}

		private void BuildObjectEditors( object obj, float x, float y )
		{
			IHasPosition hasPosition = Rb.Core.Components.Parent.GetType< IHasPosition >( obj );
			if ( hasPosition != null )
			{
				hasPosition.Position = new Point3( x, 0, y );
				AddChild( new PositionEditor( this, hasPosition ) );
			}
		}

		/// <summary>
		/// Renders the parent TileObject
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			foreach ( object childObj in Children )
			{
				IRenderable childRenderable = childObj as IRenderable;
				if ( childRenderable != null )
				{
					childRenderable.Render( context );
				}
			}
		}

		[NonSerialized]
		private bool m_Selected;

		[NonSerialized]
		private ObjectChangedDelegate m_ObjectChanged;

		private readonly object m_Object;

		#region ISelectable Members

		/// <summary>
		/// Gets the selected state of this object
		/// </summary>
		public bool Selected
		{
			get { return m_Selected; }
			set { m_Selected = value; }
		}

		#endregion
	}
}
