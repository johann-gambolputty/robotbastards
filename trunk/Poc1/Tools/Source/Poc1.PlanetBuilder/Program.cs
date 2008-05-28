using System;
using System.Windows.Forms;
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
			Graphics.InitializeFromConfiguration( );
			Application.EnableVisualStyles( );
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new BuilderForm( ) );
		}
	}
}