
using System.ComponentModel;

namespace Poc0.Core
{
	/// <summary>
	/// Flags and settings for displaying debug information about various objects
	/// </summary>
	public static class DebugInfo
	{
		#region Performance information

		/// <summary>
		/// Shows/hides the current fps count (averaged over the last second)
		/// </summary>
		[Category( "Performance" )]
		public static bool ShowFps
		{
			get { return m_ShowFps; }
			set { m_ShowFps = value; }
		}

		/// <summary>
		/// Shows/hides the current working memory set
		/// </summary>
		[Category( "Performance" )]
		public static bool ShowMemoryWorkingSet
		{
			get { return m_ShowWorkingMem; }
			set { m_ShowWorkingMem = value; }
		}

		/// <summary>
		/// Shows/hides the peak working memory set
		/// </summary>
		[Category( "Performance" )]
		public static bool ShowMemoryPeakWorkingSet
		{
			get { return m_ShowPeakWorkingMem; }
			set { m_ShowPeakWorkingMem = value; }
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
			set { m_ShowSignposts = value; }
		}
		
		/// <summary>
		/// Shows/hides entity names
		/// </summary>
		[Category( "Objects" )]
		public static bool ShowEntityNames
		{
			get { return m_ShowEntityNames; }
			set { m_ShowEntityNames = value; }
		}

		/// <summary>
		/// Shows/hides entity bounds
		/// </summary>
		[Category( "Objects" )]
		public static bool ShowEntityBounds
		{
			get { return m_ShowEntityBounds; }
			set { m_ShowEntityBounds = value; }
		}

		#endregion

		#region Environment debug information

		#endregion

		#region Private members

		private static bool m_ShowFps;
		private static bool m_ShowWorkingMem;
		private static bool m_ShowPeakWorkingMem;

		private static bool m_ShowSignposts;
		private static bool m_ShowEntityBounds;
		private static bool m_ShowEntityNames;

		#endregion
	}
}
