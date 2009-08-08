using System;
using Rb.Rendering;

namespace Goo.Test
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main( )
		{
			TestHost host = new TestHost( );
			try
			{
				Graphics.Initialize( GraphicsInitialization.FromAppConfig( ) );
			}
			catch ( Exception ex )
			{
			//	MessageBox.Show( "Error initializing graphics engine - exiting:\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}
			host.Run( new TestUnit( ) );
		}
	}
}