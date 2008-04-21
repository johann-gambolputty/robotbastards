using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using Rb.Assets;
using Graphics=Rb.Rendering.Graphics;
using Poc1.Universe;

namespace Poc1.GameClient
{
	static class Program
	{
		private static void FastTest( )
		{
			NoiseTest.TestSlowNoise();
			//NoiseTest.TestFastNoise( );
			//NoiseTest.TestFastNoiseSP( );
			NoiseTest.TestVeryFastNoise();
			NoiseTest.TestFastSphereCloudsGenerator();

			MessageBox.Show("Done");
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main( )
		{
			//FastTest( );

			InitializeAssets( );
			InitializeRendering( );

			Application.EnableVisualStyles( );
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new GameClientForm( ) );
		}


		/// <summary>
		/// Initializes the asset manager from the asset setup file specified in the application configuration file
		/// </summary>
		private static void InitializeAssets( )
		{
			//	Load asset setup
			string assetSetupPath = ConfigurationManager.AppSettings[ "assetSetupPath" ];
			if ( assetSetupPath == null )
			{
				assetSetupPath = "../assetSetup.xml";
			}
			AssetUtils.Setup( assetSetupPath );
		}

		/// <summary>
		/// Loads up the rendering assembly specified in the application configuration file
		/// </summary>
		private static void InitializeRendering( )
		{
			//	Load the rendering assembly
			string renderAssemblyName = ConfigurationManager.AppSettings[ "renderAssembly" ];
			if ( renderAssemblyName == null )
			{
				renderAssemblyName = "Rb.Rendering.OpenGl";
			}
			Graphics.Initialize( renderAssemblyName );
			Graphics.LoadCustomTypeAssemblies( Directory.GetCurrentDirectory( ), false );
		}

	}
}