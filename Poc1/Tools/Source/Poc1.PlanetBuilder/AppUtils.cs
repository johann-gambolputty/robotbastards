using System.Diagnostics;
using System.IO;

namespace Poc1.PlanetBuilder
{
	/// <summary>
	/// Class containing static application helper functions
	/// </summary>
	static class AppUtils
	{
		/// <summary>
		/// Runs the asset build batch file
		/// </summary>
		public static void BuildAssets( )
		{
			string buildPath = Path.Combine( s_BuildDir, "Build.bat" );
			ProcessStartInfo pStart = new ProcessStartInfo( buildPath );
			pStart.WorkingDirectory = s_BuildDir;
			Process p = Process.Start( pStart );
			p.WaitForExit( );	
		}

		#region Private Members

		private static string s_BuildDir;

		static AppUtils( )
		{
			s_BuildDir = Path.GetFullPath( @"..\..\..\Data\" );
		}

		#endregion
	}
}
