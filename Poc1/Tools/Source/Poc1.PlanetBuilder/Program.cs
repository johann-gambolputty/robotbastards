using System;
using System.Reflection;
using System.Windows.Forms;
using Rb.Assets;
using Rb.Rendering;

namespace Poc1.PlanetBuilder
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			//	Bodge in these assemblies. Required for factories
			Assembly.Load( "Rb.NiceControls" );

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

			try
			{
				Graphics.InitializeFromConfiguration( );
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

			Application.EnableVisualStyles( );
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new BuilderForm( ) );
		}
	}
}