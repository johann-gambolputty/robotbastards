using WeifenLuo.WinFormsUI.Docking;

namespace Bob.Core.Windows.Forms.Ui.Docking
{
	/// <summary>
	/// Information about a dockable view
	/// </summary>
	public class DockingViewInfo : ControlViewInfo
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public DockingViewInfo( string name, CreateViewDelegate createView, bool availableAsCommand ) :
			this( name, createView, availableAsCommand, DockState.Float )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public DockingViewInfo( string name, CreateViewDelegate createView, bool availableAsCommand, DockState defaultDockState ) :
			base( name, createView, availableAsCommand )
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
