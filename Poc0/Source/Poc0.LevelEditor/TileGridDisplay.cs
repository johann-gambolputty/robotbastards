
using System.Drawing;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.Rendering.OpenGl;
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

		/*
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
				m_Display.Render( null );
			}
		}
		*/

	}
}
