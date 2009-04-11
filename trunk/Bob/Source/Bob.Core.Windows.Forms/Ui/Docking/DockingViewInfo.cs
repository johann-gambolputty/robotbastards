using Rb.Interaction.Classes;
using WeifenLuo.WinFormsUI.Docking;

namespace Bob.Core.Windows.Forms.Ui.Docking
{
	/// <summary>
	/// Information about a dockable view
	/// </summary>
	public class DockingViewInfo : ControlViewInfo
	{
		/// <summary>
		/// Setup constructor with explicit show command
		/// </summary>
		public DockingViewInfo( string name, CreateViewDelegate createView ) :
			this( name, createView, ( Command )null )
		{
		}
		
		/// <summary>
		/// Setup constructor with explicit show command
		/// </summary>
		public DockingViewInfo( string name, CreateViewDelegate createView, DockState defaultDockState ) :
			this( name, createView, ( Command )null, defaultDockState )
		{
		}
		
		/// <summary>
		/// Setup constructor with show command created in the specified group
		/// </summary>
		public DockingViewInfo( string name, CreateViewDelegate createView, CommandGroup showCommandGroup ) :
			this( name, createView, showCommandGroup, DockState.Float )
		{
		}

		/// <summary>
		/// Setup constructor with explicit show command
		/// </summary>
		public DockingViewInfo( string name, CreateViewDelegate createView, Command showCommand ) :
			this( name, createView, showCommand, DockState.Float )
		{
		}

		/// <summary>
		/// Setup constructor with explicit dock state and a show command created in the specified command group
		/// </summary>
		public DockingViewInfo( string name, CreateViewDelegate createView, CommandGroup showCommandGroup, DockState defaultDockState ) :
			base( name, createView, showCommandGroup )
		{
			m_DefaultDockState = defaultDockState;
		}

		/// <summary>
		/// Setup constructor with explicit dock state and show command
		/// </summary>
		public DockingViewInfo( string name, CreateViewDelegate createView, Command showCommand, DockState defaultDockState ) :
			base( name, createView, showCommand )
		{
			m_DefaultDockState = defaultDockState;
		}

		/// <summary>
		/// Gets the default dock state of a created view
		/// </summary>
		public DockState DefaultDockState
		{
			get { return m_DefaultDockState; }
		}

		/// <summary>
		/// Gets/sets the string used to customised persistence of this view
		/// </summary>
		public string DockPersistenceInfo
		{
			get { return m_DockPersistenceInfo; }
			set { m_DockPersistenceInfo = value; }
		}

		#region Private Members

		private string m_DockPersistenceInfo;
		private DockState m_DefaultDockState;

		#endregion

	}
}
