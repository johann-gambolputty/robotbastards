using System;
using System.Reflection;
using System.Windows.Forms;
using Poc1.Bob.Controls;
using Rb.Assets;
using Rb.Rendering;

namespace Poc1.Bob
{
	static class Program
	{
		public static object Test( )
		{
			object effect = new EffectAssetHandle( "Effects/Planets/cloudLayer.cgfx", false ).Asset;
			return effect;
		}

		public class TestGetMethod
		{
			public void DoStuff( object o ) { }
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main( )
		{
			TestGetMethod test = new TestGetMethod();
			MethodInfo method = test.GetType( ).GetMethod( "DoStuff", new Type[] { test.GetType( ) } );
			if ( !Environment.CommandLine.Contains( "/noDataBuild" ) )
			{
				try
				{
					AppUtils.BuildAssets( );
				}
				catch ( Exception ex )
				{
					MessageBox.Show( string.Format( "Error running data build step ({0})", ex.Message ) );
				}
			}

			//	Must come before graphics initialization
			Application.EnableVisualStyles( );
			Application.SetCompatibleTextRenderingDefault( false );

			try
			{
				Graphics.Initialize( GraphicsInitialization.FromAppConfig( ) );
			}
			catch ( Exception ex )
			{
				MessageBox.Show( "Error initializing graphics engine - exiting:\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}
			try
			{
				AssetManager.InitializeFromConfiguration( );
			}
			catch ( Exception ex )
			{
				if ( MessageBox.Show( "Error initializing asset manager - assets may not load\n" + ex, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error ) != DialogResult.OK )
				{
					return;
				}
			}
			Application.Run( new MainForm( ) );
		}
	}
}