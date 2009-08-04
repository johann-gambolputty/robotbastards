using Goo.Core.Workspaces;
using Rb.Core.Utils;

namespace Goo.Core.Services.Workspaces
{
	/// <summary>
	/// Event class for environment changes
	/// </summary>
	public class ActiveWorkspaceChangedEvent
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="oldEnvironment">Old environment</param>
		/// <param name="newEnvironment">New environment</param>
		public ActiveWorkspaceChangedEvent( IWorkspace oldEnvironment, IWorkspace newEnvironment )
		{
			Arguments.CheckNotNull( oldEnvironment, "oldEnvironment" );
			Arguments.CheckNotNull( newEnvironment, "newEnvironment" );
			m_OldWorkspace = oldEnvironment;
			m_NewWorkspace = newEnvironment;
		}

		/// <summary>
		/// Gets the old environment
		/// </summary>
		public IWorkspace OldWorkspace
		{
			get { return m_OldWorkspace; }
		}

		/// <summary>
		/// Gets the new environment
		/// </summary>
		public IWorkspace NewWorkspace
		{
			get { return m_NewWorkspace; }
		}

		#region Private Members

		private readonly IWorkspace m_OldWorkspace;
		private readonly IWorkspace m_NewWorkspace;

		#endregion
	}
}
