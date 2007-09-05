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
	/// State of an object in the editor
	/// </summary>
	[Serializable]
	public class ObjectEditState : ISelectable, IRenderable
	{
		/// <summary>
		/// Sets the tile object to get renderer
		/// </summary>
		public ObjectEditState( Scene scene, object parent )
		{
			scene.Renderables.Add( this );
			m_Frame = Parent.GetType< IHasWorldFrame >( parent );
		}

		/// <summary>
		/// Renders the parent TileObject
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			Matrix44 frame = m_Frame.WorldFrame;

			//	TODO: AP: Render object bounds
			int x = ( int )frame.Translation.X - 5;
			int y = ( int )frame.Translation.Z - 5;
			int width = 10;
			int height = 10;

			Color colour = Selected ? Color.Red : Color.White;

			ShapeRenderer.Instance.DrawRectangle( x, y, width, height, colour );
		}

		private readonly IHasWorldFrame m_Frame;

		[NonSerialized]
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
