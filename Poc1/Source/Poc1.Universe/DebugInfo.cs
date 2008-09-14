
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
			get { return s_ShowCameraPosition; }
			set
			{
				s_ShowCameraPosition = value;
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
			get { return s_ShowPatchInfo; }
			set
			{
				s_ShowPatchInfo = value;
				OnPropertyChanged( );
			}
		}

		/// <summary>
		/// Switches terrain patch skirt generation on or off
		/// </summary>
		[Category( "Terrain Rendering" )]
		public static bool DisableTerainSkirts
		{
			get { return s_DisableTerainSkirts; }
			set { s_DisableTerainSkirts = value; }
		}

		/// <summary>
		/// Switches patch wireframe rendering on or off
		/// </summary>
		[Category( "Terrain Rendering" )]
		public static bool ShowPatchWireframe
		{
			get { return s_ShowPatchWireframe; }
			set
			{
				s_ShowPatchWireframe = value;
				OnPropertyChanged( );
			}
		}

		/// <summary>
		/// Switches patch slope rendering on or off
		/// </summary>
		[Category( "Terrain Rendering" )]
		public static bool ShowTerrainSlopes
		{
			get { return s_ShowTerrainSlopes; }
			set
			{
				s_ShowTerrainSlopes = value;
				OnPropertyChanged( );
			}
		}
		

		/// <summary>
		/// Shows the number of leaf nodes in the terrain quad-tree
		/// </summary>
		[Category( "Terrain Rendering" )]
		public static bool ShowTerrainLeafNodeCount
		{
			get { return s_ShowTerrainLeafNodeCount; }
			set
			{
				s_ShowTerrainLeafNodeCount = value;
				OnPropertyChanged( );
			}
		}

		/// <summary>
		/// Shows the number of terrain build items currently pending
		/// </summary>
		[Category( "Terrain Rendering" )]
		public static bool ShowPendingTerrainBuildItemCount
		{
			get { return s_ShowPendingTerrainBuildItemCount; }
			set
			{
				s_ShowPendingTerrainBuildItemCount = value;
				OnPropertyChanged( );
			}
		}

		#endregion

		#region Private members

		private static bool s_ShowCameraPosition;
		private static bool s_DisableTerainSkirts;


		private static bool s_ShowPendingTerrainBuildItemCount = true;
		private static bool s_ShowPatchInfo;
		private static bool s_ShowPatchWireframe;
		private static bool s_ShowTerrainSlopes;
		private static bool s_ShowTerrainLeafNodeCount;

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
