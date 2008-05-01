using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Rb.Rendering;

namespace Poc1.TerrainPatchTest
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			string renderAssemblyName = "Rb.Rendering.OpenGl";
			Graphics.Initialize( renderAssemblyName );
			Graphics.LoadCustomTypeAssemblies( Directory.GetCurrentDirectory( ), false );

			Application.EnableVisualStyles( );
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new MainForm( ) );
		}
	}
}