using Rb.Rendering;
using Rb.World.Rendering;
using Rb.Core.Components;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Renders tile objects
	/// </summary>
	public class TileObjectRenderer : SceneRenderable
	{
		/// <summary>
		/// Sets the tile object to get renderer
		/// </summary>
		public TileObjectRenderer( TileObject tileObject )
		{
			m_TileObject = tileObject;
		}

		/// <summary>
		/// Renders the parent TileObject
		/// </summary>
		/// <param name="context"></param>
		public override void Render( IRenderContext context )
		{
			//	TODO: AP: Render object bounds
			int x = ( int )m_TileObject.X - 5;
			int y = ( int )m_TileObject.Y - 5;
			int width = 10;
			int height = 10;

			ShapeRenderer.Instance.DrawRectangle( x, y, width, height, System.Drawing.Color.White );

			//	Renders any objects attached to this renderer
			base.Render( context );
		}


		private readonly TileObject m_TileObject;
	}
}
