using System;
using System.Windows.Forms;
using Rb.Assets;
using Rb.Assets.Files;
using Rb.Rendering;
using Rb.Rendering.Textures;

namespace Rb.TextureViewer
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main( )
		{
			AssetManager.Instance.AddLoader( new TextureImageLoader( ) );
			AssetManager.Instance.AddLoader( new Rb.TextureAssets.Loader( ) );
			Locations.Instance.Systems.Add( new FileSystem( ) );

			Graphics.Initialize( GraphicsInitialization.OpenGlCgWindows( ) );

			Application.EnableVisualStyles( );
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new MainForm( ) );
		}
	}
}