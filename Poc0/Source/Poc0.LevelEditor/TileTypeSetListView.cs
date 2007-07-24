using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Poc0.LevelEditor.Core;

namespace Poc0.LevelEditor
{
	public partial class TileTypeSetListView : ListView
	{
		public TileTypeSetListView( )
		{
			InitializeComponent( );
		}

		public TileTypeSet TileTypes
		{
			get { return m_TileTypes; }
			set
			{
				m_TileTypes = value;

				if ( m_TileTypes == null )
				{
					return;
				}
				m_Images = new ImageList( );
				m_Images.ImageSize = new Size( 32, 32 );
				m_Images.ColorDepth = ColorDepth.Depth24Bit;

				Items.Clear( );

				foreach ( TileType tileType in m_TileTypes.TileTypes )
				{
					int imageIndex = m_Images.Images.Count;
					Bitmap typeBmp = tileType.CreateBitmap( PixelFormat.Format24bppRgb );

					typeBmp.Save( string.Format( "TileType{0}.png", imageIndex ) );
					m_Images.Images.Add( typeBmp );

					Items.Add( tileType.Name, imageIndex ).Tag = tileType;
				}

				LargeImageList = m_Images;
			}
		}

		private TileTypeSet m_TileTypes;
		private ImageList m_Images = new ImageList( );
	}
}
