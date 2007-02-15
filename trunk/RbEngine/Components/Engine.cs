using System;

namespace RbEngine.Components
{
	/// <summary>
	/// Summary description for Engine.
	/// </summary>
	public class Engine
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public Engine( )
		{
		}

		public static Engine	Main
		{
			get
			{
				return ms_Main;
			}
		}

		private static Engine	ms_Main = new Engine( );
	}
}
