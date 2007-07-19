
using System.Drawing;
using Rb.Rendering;
using Rb.Rendering.Windows;

namespace Poc0.LevelEditor
{
	internal partial class TileGridDisplay : Display
	{
		public TileGridDisplay()
		{
			InitializeComponent();
		}

		protected override void Draw()
		{
			if ( m_Grid == null )
			{
				base.Draw( );
			}
			else
			{
				Renderer.Instance.ClearDepth( 1.0f );
				Renderer.Instance.ClearVerticalGradient( Color.DarkSeaGreen, Color.Black );

				m_Renderer.Render( m_Grid );
			}
		}

		private TileGrid m_Grid;
		private ITileGridRenderer m_Renderer = new TileGrid2dRenderer( );

		private void TileGridDisplay_Load(object sender, System.EventArgs e)
		{
			m_Grid = new TileGrid( );
		}
	}
}
