using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Rb.Assets;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Textures;
using Rb.Rendering.Windows.Properties;

namespace Rb.Rendering.Windows
{
	/// <summary>
	/// UI editor for textures
	/// </summary>
	[CustomUITypeEditor( typeof( ITexture2d ) )]
	public class Texture2dAssetEditor : UITypeEditor
	{
        public override UITypeEditorEditStyle GetEditStyle( ITypeDescriptorContext context )
        {
            return UITypeEditorEditStyle.Modal;
        }

		public override bool IsDropDownResizable
		{
			get { return false; }
		}

        public override object EditValue( ITypeDescriptorContext context, IServiceProvider provider, object value )
        {
            IWindowsFormsEditorService edSvc = ( IWindowsFormsEditorService )provider.GetService( typeof( IWindowsFormsEditorService ) );
            if( edSvc == null )
            {
            	return value;
            }

			OpenFileDialog openDlg = new OpenFileDialog( );
			openDlg.Filter = "Image Files (*.jpg;*.bmp;*.png;*.tga)|*.jpg;*.bmp;*.png;*.tga|All Files (*.*)|*.*";
			//	NOTE: AP: Can't use edSvc.ShowDialog() because OpenFileDialog doesn't derive from Form
			if ( openDlg.ShowDialog( ) != DialogResult.OK )
			{
				return value;
			}
			if ( string.IsNullOrEmpty( openDlg.FileName ) )
			{
				return value;
			}

			try
			{
				ITexture2d texture = ( ITexture2d )AssetManager.Instance.Load( openDlg.FileName, new TextureImageLoader.TextureLoadParameters( true ) );
				return texture;
			}
			catch ( Exception ex )
			{
				AssetsLog.Exception( ex, string.Format( "Error occured while attempting to load texture asset \"{0}\"", openDlg.FileName ) );
				throw new InvalidOperationException( string.Format( Resources.TextureAssetLoadError, openDlg.FileName ), ex );
			}
        }

        public override void PaintValue( PaintValueEventArgs e )
        {
			if ( e.Value == null )
			{
				return;
			}
			e.Graphics.DrawImage( ( ( ITexture2d )e.Value ).ToBitmap( ), e.Bounds );
        }

        public override bool GetPaintValueSupported( ITypeDescriptorContext context )
        {
            return true;
        }

	}
}
