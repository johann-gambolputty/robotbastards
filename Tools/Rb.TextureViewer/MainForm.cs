using System;
using System.IO;
using System.Windows.Forms;
using Rb.Rendering.Interfaces.Objects;
using Rb.TextureAssets;

namespace Rb.TextureViewer
{
	public partial class MainForm : Form
	{
		public MainForm( )
		{
			InitializeComponent( );
		}

		#region Event Handlers

		private void exitToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Close( );
		} 

		#endregion

		private const string FilterString = "*.texture;*.bmp;*.png;*.jpg;*.jpeg";

		private void openToolStripMenuItem_Click( object sender, EventArgs e )
		{
			/*
			Texture2dData data = new Texture2dData( 128, 128, TextureFormat.B8G8R8 );
			for ( int i = 0; i < 128 * 128 * 3; ++i )
			{
				data.Bytes[ i ] = 0xff;
			}

			ITexture2d texture = Graphics.Factory.CreateTexture2d( );
			texture.Create( data, true );

			/*/
			//	Choose the file to open
			OpenFileDialog openFile = new OpenFileDialog( );
			openFile.Filter = string.Format( "Image Files ({0})|{0}|All Files (*.*)|*.*", FilterString );
			if ( openFile.ShowDialog( this ) != DialogResult.OK )
			{
				return;
			}

			//ITexture2d texture;
			//try
			//{
			//    texture = ( ITexture2d )AssetManager.Instance.Load( openFile.FileName );
			//    Texture2dUtils.SaveTextureToImageFile( texture, Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "output.png" ) );
			//}
			//catch ( Exception ex )
			//{
			//    string msg = string.Format( "Error occurred opening \"{0}\" - {1}", Path.GetFileName( openFile.FileName ), ex.Message );
			//    MessageBox.Show( this, msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
			//    return;
			//}

			//*/

			TextureForm form = new TextureForm( openFile.FileName );
			form.MdiParent = this;
			form.Show( );
		}

		private void exportToolStripMenuItem_Click( object sender, EventArgs e )
		{
			SaveFileDialog saveFile = new SaveFileDialog( );
			saveFile.DefaultExt = "texture";
			saveFile.Filter = "Texture Files (*.texture)|*.texture|All Files (*.*)|*.*";
			if ( saveFile.ShowDialog( this ) != DialogResult.OK )
			{
				return;
			}

			ITexture2d texture = ( ( TextureForm )ActiveMdiChild ).Texture;

			try
			{
				using ( MemoryStream memStream = new MemoryStream( ) )
				{
					Generator.WriteTextureToStream( texture, memStream, true );

					File.WriteAllBytes( saveFile.FileName, memStream.ToArray( ) );
					//using ( FileStream fileStream = new FileStream( saveFile.FileName, FileMode.Create, FileAccess.Write ) )
					//{
					//    memStream.WriteTo( fileStream );
					//}
				}
			}
			catch ( Exception ex )
			{
				string msg = string.Format( "Error occurred saving texture to \"{0}\" - {1}", Path.GetFileName( saveFile.FileName ), ex.Message );
				MessageBox.Show( this, msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );	
			}
		}
	}
}