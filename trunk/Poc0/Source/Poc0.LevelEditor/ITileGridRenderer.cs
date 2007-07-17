using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Poc0.LevelEditor
{
	/// <summary>
	/// Rendering interface for a TileGrid object
	/// </summary>
	interface ITileGridRenderer
	{
		/// <summary>
		/// Renders the specified tile grid
		/// </summary>
		void Render( TileGrid grid );
	}
}
