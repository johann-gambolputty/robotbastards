using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Poc0.Core;
using Rb.Rendering;
using Rb.World;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// State of an object in the editor
	/// </summary>
	public class ObjectEditState : ISelectable, IRenderable
	{
		/// <summary>
		/// Sets the tile object to get renderer
		/// </summary>
		public ObjectEditState(Scene scene, object parent)
		{
			scene.Renderables.Add( this );
			m_Parent = ( IHasWorldFrame )parent;
		}

		/// <summary>
		/// Renders the parent TileObject
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			Frame frame = m_Parent.WorldFrame;

			//	TODO: AP: Render object bounds
			int x = ( int )frame.Position.X - 5;
			int y = ( int )frame.Position.Z - 5;
			int width = 10;
			int height = 10;

			Color colour = Selected ? Color.Red : Color.White;

			ShapeRenderer.Instance.DrawRectangle( x, y, width, height, colour );
		}

		private readonly IHasWorldFrame m_Parent;
		private bool m_Selected;
			
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
