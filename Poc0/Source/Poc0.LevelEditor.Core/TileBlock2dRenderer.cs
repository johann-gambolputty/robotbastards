using System;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Base class for 2D tile block renderers
	/// </summary>
	[Serializable]
	public abstract class TileBlock2dRenderer : TileBlockRenderer
	{
		#region Public constructor

		/// <summary>
		/// Sets up the renderer
		/// </summary>
		/// <param name="grid">Grid to render</param>
		/// <param name="editContext">Edit context to render on top of the grid</param>
		public TileBlock2dRenderer( TileGrid grid, EditModes.EditModeContext editContext ) :
			base( grid, editContext )
		{
		}

		#endregion
	}
}
