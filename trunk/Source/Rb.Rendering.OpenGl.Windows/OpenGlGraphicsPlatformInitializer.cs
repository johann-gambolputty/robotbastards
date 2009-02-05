
using System.Drawing;
using System.Windows.Forms;
using Rb.Rendering.Windows;

namespace Rb.Rendering.OpenGl.Windows
{
	/// <summary>
	/// Handles initialization of OpenGL for windows
	/// </summary>
	public class OpenGlGraphicsPlatformInitializer : IGraphicsPlatformInitializer
	{
		#region IGraphicsPlatformInitializer Members

		/// <summary>
		/// Runs platform-specific graphics initialization
		/// </summary>
		public void Init( )
		{
			//	Create a 1x1 display to initialize OpenGL
			//	TODO: AP: There must be an easier way to do this...

			Form displayHost = new Form( );
			displayHost.Size = new Size( 1, 1 );
			displayHost.Controls.Add( new Display( ) );
			displayHost.Show( );
			displayHost.Hide( );

			s_DisplayHost = displayHost;
		}

		private static Form s_DisplayHost;

		#endregion
	}
}
