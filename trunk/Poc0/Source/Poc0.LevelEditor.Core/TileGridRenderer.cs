using System;
using Rb.Rendering;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Interface for objects that can render a <see cref="TileGrid"/>
	/// </summary>
	[Serializable]
	public abstract class TileGridRenderer : IRenderable
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="grid">Grid to render</param>
		/// <param name="editContext">Grid edit state</param>
		public TileGridRenderer( TileGrid grid, EditModes.EditModeContext editContext )
		{
			Grid = grid;
			EditContext = editContext;
		}

		/// <summary>
		/// Grid to render
		/// </summary>
		public virtual TileGrid Grid
		{
			get { return m_Grid; }
			set { m_Grid = value; }
		}

		/// <summary>
		/// Access to the tile selection set that is rendered with the grid
		/// </summary>
		public virtual EditModes.EditModeContext EditContext
		{
			get { return m_EditContext; }
			set { m_EditContext = value; }
		}

		/// <summary>
		/// Renders the attached tile grid
		/// </summary>
		public abstract void Render( IRenderContext context );

		private TileGrid m_Grid;
		private EditModes.EditModeContext m_EditContext;
	}
}
