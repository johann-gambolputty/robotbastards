using Rb.Rendering;
using Rb.World;
using Rb.World.Rendering;
using Rb.Core.Components;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Renders tile objects
	/// </summary>
	public class TileObjectRenderer : IRenderable
	{
		/// <summary>
		/// Sets the tile object to get renderer
		/// </summary>
		public TileObjectRenderer( Scene scene, TileObject tileObject )
		{
			scene.Renderables.Add( this );
			m_TileObject = tileObject;
		}

		/// <summary>
		/// Renders the parent TileObject
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			//	TODO: AP: Render object bounds
			int x = ( int )m_TileObject.X - 5;
			int y = ( int )m_TileObject.Y - 5;
			int width = 10;
			int height = 10;

			ShapeRenderer.Instance.DrawRectangle( x * TileCamera2d.TileScreenWidth, y * TileCamera2d.TileScreenHeight, width, height, System.Drawing.Color.White );
		}

		private readonly TileObject m_TileObject;
	}
}
