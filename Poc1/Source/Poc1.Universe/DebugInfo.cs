
using System.ComponentModel;

namespace Poc1.Universe
{
	public static class DebugInfo
	{
		#region Public Events

		/// <summary>
		/// PropertyChanged event delegate
		/// </summary>
		public delegate void PropertyChangedDelegate( );

		/// <summary>
		/// Event, invoked when a debug info property is changed
		/// </summary>
		public static event PropertyChangedDelegate PropertyChanged;

		#endregion

		#region Rendering

		/// <summary>
		/// Shows/hides debug information about patch errors
		/// </summary>
		[Category( "Rendering" )]
		public static bool ShowPatchInfo
		{
			get { return ms_ShowPatchInfo; }
			set
			{
				ms_ShowPatchInfo = value;
				OnPropertyChanged( );
			}
		}

		/// <summary>
		/// Switches patch wireframe rendering on or off
		/// </summary>
		[Category( "Rendering" )]
		public static bool ShowPatchWireframe
		{
			get { return ms_ShowPatchWireframe; }
			set
			{
				ms_ShowPatchWireframe = value;
				OnPropertyChanged( );
			}
		}

		#endregion

		#region Private members

		private static bool ms_ShowPatchInfo;
		private static bool ms_ShowPatchWireframe;

		/// <summary>
		/// Raises the DebugInfoChanged event
		/// </summary>
		private static void OnPropertyChanged( )
		{
			if ( PropertyChanged != null )
			{
				PropertyChanged( );
			}
		}

		#endregion
	}
}
