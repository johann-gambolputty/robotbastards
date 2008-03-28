using System.Configuration;
using System.IO;
using Rb.Assets;
using Rb.Rendering;

namespace Rb.Tools.LevelEditor.Core.Controls.Forms
{
	public static class EditorApp
	{
		/// <summary>
		/// Initializes the asset manager from the asset setup file specified in the application configuration file
		/// </summary>
		public static void InitializeAssets( )
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
		public static void InitializeRendering( )
		{
			//	Load the rendering assembly
			string renderAssemblyName = ConfigurationManager.AppSettings[ "renderAssembly" ];
			if ( renderAssemblyName == null )
			{
				renderAssemblyName = "Rb.Rendering.OpenGl.Windows";
			}
			Graphics.Initialize( renderAssemblyName );
			Graphics.LoadCustomTypeAssemblies( Directory.GetCurrentDirectory( ), false );
		}

		/// <summary>
		/// Initializes rendering and assets
		/// </summary>
		public static void InitializeAll( )
		{
			InitializeAssets( );
			InitializeRendering( );
		}
	}
}
