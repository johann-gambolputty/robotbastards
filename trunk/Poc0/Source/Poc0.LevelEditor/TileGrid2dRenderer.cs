using System;
using System.Collections.Generic;
using System.Text;
using Rb.Rendering;

namespace Poc0.LevelEditor
{
	class TileGrid2dRenderer : ITileGridRenderer
	{
		#region ITileGridRenderer Members

		/// <summary>
		/// Renders a tile grid
		/// </summary>
		/// <param name="grid"></param>
		public void Render( TileGrid grid )
		{
			int		tileStartX			= 0;
			int		tileStartY			= 0;
			int		tileEndX			= grid.Width;
			int		tileEndY			= grid.Height;
			int		screenStartX		= 0;
			int		screenY				= 0;
			int		screenXIncrement	= 32;
			int		screenYIncrement	= 32;

			for ( int tileY = tileStartY; tileY < tileEndY; ++tileY, screenY += screenYIncrement )
			{
				int screenX = screenStartX;
				for ( int tileX = tileStartX; tileX < tileEndX; ++tileX, screenX += screenXIncrement )
				{
					RenderTile( screenX, screenY, screenXIncrement, screenYIncrement, grid[ tileX, tileY ] );
				}
			}
		}

		#endregion

		/// <summary>
		/// Renders a specified tile
		/// </summary>
		private static void RenderTile( int screenX, int screenY, int screenWidth, int screenHeight, Tile tile )
		{
			ShapeRenderer.Instance.DrawImage( screenX, screenY, screenWidth, screenHeight, tile.TileType.DisplayTexture );
		}
	}
}
