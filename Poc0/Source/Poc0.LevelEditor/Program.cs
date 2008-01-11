using System;
using System.Windows.Forms;
using Rb.Tools.LevelEditor.Core.Controls.Forms;

namespace Poc0.LevelEditor
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main( )
		{
			AppDomain.CurrentDomain.Load( "MagicLibrary" );

			EditorApp.InitializeAll( );

			Application.EnableVisualStyles( );
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new MainForm( ) );
		}
	}
}