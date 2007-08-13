using Rb.Rendering;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Interface for objects that can render a <see cref="TileGrid"/>
	/// </summary>
	public abstract class TileGridRenderer : IRenderable
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="grid">Grid to render</param>
		/// <param name="state">Grid edit state</param>
		public TileGridRenderer( TileGrid grid, TileGridEditState state )
		{
			Grid = grid;
			EditState = state;
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
		public virtual TileGridEditState EditState
		{
			get { return m_EditState;  }
			set { m_EditState = value; }
		}

		/// <summary>
		/// Renders the attached tile grid
		/// </summary>
		public abstract void Render( IRenderContext context );

		private TileGrid m_Grid;
		private TileGridEditState m_EditState;
	}
}
