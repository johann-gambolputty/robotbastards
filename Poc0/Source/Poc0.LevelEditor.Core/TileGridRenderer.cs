
using Rb.Rendering;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Interface for objects that can render a <see cref="TileGrid"/>
	/// </summary>
	public abstract class TileGridRenderer : IRenderable
	{
		/// <summary>
		/// Grid to render
		/// </summary>
		public TileGrid Grid
		{
			get { return m_Grid; }
			set { m_Grid = value; }
		}

		/// <summary>
		/// Renders the attached tile grid
		/// </summary>
		public abstract void Render( IRenderContext context );

		private TileGrid m_Grid;
	}
}
