using System;
using System.Drawing;
using System.Windows.Forms;

namespace Poc1.Tools.TerrainTextures
{
	public partial class TerrainTypeEditorControl : UserControl
	{
		public event Action<TerrainTypeEditorControl> RemoveControl;

		public TerrainTypeEditorControl( )
		{
			InitializeComponent( );
		}

		private void TerrainTypeEditorControl_Load( object sender, EventArgs e )
		{
			Bitmap deleteBmp = ( Bitmap )deleteButton.BackgroundImage;
			deleteBmp.MakeTransparent( Color.Magenta );
			deleteButton.BackgroundImage = deleteBmp;
		}

		private void texturePanel_Click( object sender, EventArgs e )
		{
			OpenFileDialog openDlg = new OpenFileDialog( );
			openDlg.Filter = "Image files (*.jpg, *.bmp)|*.JPG;*.BMP|All Files (*.*)|*.*";
			if ( openDlg.ShowDialog( ) != DialogResult.OK )
			{
				return;
			}

			Bitmap bmp = new Bitmap( openDlg.FileName );
			texturePanel.BackgroundImage = bmp;
		}

		private void deleteButton_Click( object sender, EventArgs e )
		{
			if ( RemoveControl != null )
			{
				RemoveControl( this );
			}
		}
	}
}
