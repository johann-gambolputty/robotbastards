
using System.ComponentModel;
using Rb.Rendering;

namespace Poc0.Core
{
	/// <summary>
	/// Flags and settings for displaying debug information about various objects
	/// </summary>
	public static class DebugInfo
	{
		public delegate void DebugInfoChangedDelegate( );

		public static event DebugInfoChangedDelegate DebugInfoChanged;

		#region Performance information

		/// <summary>
		/// Shows/hides the current fps count (averaged over the last second)
		/// </summary>
		[Category( "Performance" )]
		public static bool ShowFps
		{
			get { return m_ShowFps; }
			set
			{
				m_ShowFps = value;
				OnDebugInfoChanged( );
			}
		}

		/// <summary>
		/// Shows/hides the current working memory set
		/// </summary>
		[Category( "Performance" )]
		public static bool ShowMemoryWorkingSet
		{
			get { return m_ShowWorkingMem; }
			set
			{
				m_ShowWorkingMem = value;
				OnDebugInfoChanged( );
			}
		}

		/// <summary>
		/// Shows/hides the peak working memory set
		/// </summary>
		[Category( "Performance" )]
		public static bool ShowMemoryPeakWorkingSet
		{
			get { return m_ShowPeakWorkingMem; }
			set
			{
				m_ShowPeakWorkingMem = value;
				OnDebugInfoChanged( );
			}
		}

		#endregion

		#region Object debug information

		/// <summary>
		/// Shows/hides debug information about signposts
		/// </summary>
		[Category( "Objects" )]
		public static bool ShowSignposts
		{
			get { return m_ShowSignposts; }
			set
			{
				m_ShowSignposts = value;
				OnDebugInfoChanged( );
			}
		}

		/// <summary>
		/// Shows/hides debug information about lights
		/// </summary>
		[Category( "Objects" )]
		public static bool ShowLights
		{
			get { return m_ShowLights; }
			set
			{
				m_ShowLights = value;
				OnDebugInfoChanged( );
			}
		}
		
		/// <summary>
		/// Shows/hides entity names
		/// </summary>
		[Category( "Objects" )]
		public static bool ShowEntityNames
		{
			get { return m_ShowEntityNames; }
			set
			{
				m_ShowEntityNames = value;
				OnDebugInfoChanged( );
			}
		}

		/// <summary>
		/// Shows/hides entity bounds
		/// </summary>
		[Category( "Objects" )]
		public static bool ShowEntityBounds
		{
			get { return m_ShowEntityBounds; }
			set
			{
				m_ShowEntityBounds = value;
				OnDebugInfoChanged( );
			}
		}

		#endregion

		#region Environment debug information

		#endregion

		#region Private members

		private static bool m_ShowFps;
		private static bool m_ShowWorkingMem;
		private static bool m_ShowPeakWorkingMem;

		private static bool m_ShowSignposts;
		private static bool m_ShowLights;
		private static bool m_ShowEntityBounds;
		private static bool m_ShowEntityNames;

		/// <summary>
		/// Raises the DebugInfoChanged event
		/// </summary>
		private static void OnDebugInfoChanged( )
		{
			if ( DebugInfoChanged != null )
			{
				DebugInfoChanged( );
			}
		}

		#endregion
	}
}
