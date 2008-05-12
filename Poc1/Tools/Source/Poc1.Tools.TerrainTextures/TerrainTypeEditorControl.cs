using System;
using System.Drawing;
using System.Windows.Forms;
using Poc1.Tools.TerrainTextures.Core;

namespace Poc1.Tools.TerrainTextures
{
	public partial class TerrainTypeEditorControl : UserControl
	{
		public event Action<TerrainType> TerrainTypeChanged;
		public event Action<TerrainTypeEditorControl> RemoveControl;

		public TerrainTypeEditorControl( )
		{
			InitializeComponent( );

			TerrainType = new TerrainType( );

			elevationControl.DistributionChanged += delegate { OnTerrainTypeChanged( ); };
			slopeControl.DistributionChanged += delegate { OnTerrainTypeChanged( ); };
		}

		public TerrainType TerrainType
		{
			get { return m_TerrainType; }
			set
			{
				m_TerrainType = value;

				typeNameTextBox.Text = m_TerrainType.Name;
				texturePanel.BackgroundImage = m_TerrainType.Texture;

				elevationControl.Distribution = m_TerrainType.AltitudeDistribution;
				slopeControl.Distribution = m_TerrainType.SlopeDistribution;
			}
		}

		private TerrainType m_TerrainType = new TerrainType( );

		private void OnTerrainTypeChanged( )
		{
			if ( TerrainTypeChanged != null )
			{
				TerrainTypeChanged( m_TerrainType );
			}
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

			m_TerrainType.LoadBitmap( openDlg.FileName );
			texturePanel.BackgroundImage = m_TerrainType.Texture;

			OnTerrainTypeChanged( );
		}

		private void deleteButton_Click( object sender, EventArgs e )
		{
			if ( RemoveControl != null )
			{
				RemoveControl( this );
			}
		}

		private void typeNameTextBox_TextChanged( object sender, EventArgs e )
		{
			m_TerrainType.Name = typeNameTextBox.Text;
		}
	}
}
