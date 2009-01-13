using Bob.Core.Ui.Interfaces;
using Bob.Core.Workspaces.Interfaces;
using Rb.Core.Sets.Classes;
using Rb.Core.Sets.Interfaces;
using Rb.Core.Utils;

namespace Bob.Core.Workspaces.Classes
{
	/// <summary>
	/// Implementation of <see cref="IWorkspace"/>
	/// </summary>
	public class Workspace : IWorkspace
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="mainDisplay">Main application display</param>
		/// <exception cref="System.ArgumentNullException">Thrown if mainDisplay is null</exception>
		public Workspace( IMainApplicationDisplay mainDisplay )
		{
			Arguments.CheckNotNull( mainDisplay, "mainDisplay" );
			m_MainDisplay = mainDisplay;
		}

		#region IWorkspace Members

		/// <summary>
		/// Gets the main display
		/// </summary>
		public IMainApplicationDisplay MainDisplay
		{
			get { return m_MainDisplay; }
		}

		/// <summary>
		/// Gets workspace services
		/// </summary>
		public IObjectSetServiceMap Services
		{
			get { return m_Services; }
		}

		#endregion

		#region Private Members

		private readonly IMainApplicationDisplay m_MainDisplay;
		private readonly IObjectSetServiceMap m_Services = new ObjectSetServiceMap( );

		#endregion
	}
}
