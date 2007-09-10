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
	public class PositionEditor : IRenderable, IHasWorldFrame
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="parent">Parent edit state</param>
		/// <param name="frame">World frame</param>
		/// <param name="x">Initial X position</param>
		/// <param name="y">Initial Y position</param>
		public PositionEditor( ObjectEditState parent, Matrix44 frame, float x, float y )
		{
			m_Parent = parent;
			m_Frame = frame;

			frame.Translation = new Point3( x, 0, y );
		}

		#region IRenderable Members

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			Matrix44 frame = m_Frame;

			//	TODO: AP: Render object bounds
			int x = ( int )frame.Translation.X - 5;
			int y = ( int )frame.Translation.Z - 5;
			int width = 10;
			int height = 10;

			Color colour = m_Parent.Selected ? Color.Red : Color.White;

			ShapeRenderer.Instance.DrawRectangle( x, y, width, height, colour );
		}

		#endregion

		private readonly ObjectEditState m_Parent;
		private readonly Matrix44 m_Frame;

		#region IHasWorldFrame Members

		public Matrix44 WorldFrame
		{
			get { return m_Frame; }
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
		public event ObjectChangedDelegate ObjectChanged;

		/// <summary>
		/// Invokes the <see cref="ObjectChanged"/> event
		/// </summary>
		public void OnObjectChanged( )
		{
			if ( ObjectChanged != null )
			{
				ObjectChanged( );
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

		private void BuildTemplateEditors( ObjectTemplate template, float x, float y )
		{
			ObjectTemplate worldFrameTemplate = template.FindTemplateOfType< IHasWorldFrame >( );
			if ( worldFrameTemplate != null )
			{
				Matrix44 frame = new Matrix44( );
				worldFrameTemplate[ "WorldFrame" ] = frame;
				AddChild( new PositionEditor( this, frame, x, y ) );
			}
		}

		private void BuildObjectEditors( object obj, float x, float y )
		{
			if ( obj is ObjectTemplate )
			{
				BuildTemplateEditors( ( ObjectTemplate )obj, x, y );
			}
			else
			{
				IHasWorldFrame frame = Rb.Core.Components.Parent.GetType<IHasWorldFrame>( obj );
				if ( frame != null )
				{
					frame.WorldFrame.Translation = new Point3( x, 0, y );
					AddChild( new PositionEditor( this, frame.WorldFrame, x, y ) );
				}
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
