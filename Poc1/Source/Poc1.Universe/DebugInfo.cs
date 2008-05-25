
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
		/// Shows the number of leaf nodes in the terrain quad-tree
		/// </summary>
		[Category( "Rendering" )]
		public static bool ShowCameraPosition
		{
			get { return ms_ShowCameraPosition; }
			set
			{
				ms_ShowCameraPosition = value;
				OnPropertyChanged( );
			}
		}

		#endregion

		#region Terrain Rendering

		/// <summary>
		/// Shows/hides debug information about patch errors
		/// </summary>
		[Category( "Terrain Rendering" )]
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
		/// Switches terrain patch skirt generation on or off
		/// </summary>
		[Category( "Terrain Rendering" )]
		public static bool DisableTerainSkirts
		{
			get { return ms_DisableTerainSkirts; }
			set { ms_DisableTerainSkirts = value; }
		}

		/// <summary>
		/// Switches patch wireframe rendering on or off
		/// </summary>
		[Category( "Terrain Rendering" )]
		public static bool ShowPatchWireframe
		{
			get { return ms_ShowPatchWireframe; }
			set
			{
				ms_ShowPatchWireframe = value;
				OnPropertyChanged( );
			}
		}

		/// <summary>
		/// Switches patch slope rendering on or off
		/// </summary>
		[Category( "Terrain Rendering" )]
		public static bool ShowTerrainSlopes
		{
			get { return ms_ShowTerrainSlopes; }
			set
			{
				ms_ShowTerrainSlopes = value;
				OnPropertyChanged( );
			}
		}
		

		/// <summary>
		/// Shows the number of leaf nodes in the terrain quad-tree
		/// </summary>
		[Category( "Terrain Rendering" )]
		public static bool ShowTerrainLeafNodeCount
		{
			get { return ms_ShowTerrainLeafNodeCount; }
			set
			{
				ms_ShowTerrainLeafNodeCount = value;
				OnPropertyChanged( );
			}
		}

		#endregion

		#region Private members

		private static bool ms_ShowCameraPosition;
		private static bool ms_DisableTerainSkirts;

		private static bool ms_ShowPatchInfo;
		private static bool ms_ShowPatchWireframe;
		private static bool ms_ShowTerrainSlopes;
		private static bool ms_ShowTerrainLeafNodeCount;

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
