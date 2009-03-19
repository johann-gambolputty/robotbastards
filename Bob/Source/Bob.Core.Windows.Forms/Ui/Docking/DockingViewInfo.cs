using System.Windows.Forms;
using Bob.Core.Ui.Interfaces.Views;
using Bob.Core.Workspaces.Interfaces;
using Rb.Core.Utils;
using WeifenLuo.WinFormsUI.Docking;

namespace Bob.Core.Windows.Forms.Ui.Docking
{
	/// <summary>
	/// Information about a dockable view
	/// </summary>
	public class DockingViewInfo : IViewInfo
	{
		/// <summary>
		/// Delegate used for methods that create views from this DockingViewInfo
		/// </summary>
		/// <param name="workspace">Workspace to create the view in</param>
		public delegate Control CreateViewDelegate( IWorkspace workspace );

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
		public DockingViewInfo( string name, CreateViewDelegate createView, bool availableAsCommand, DockState defaultDockState )
		{
			Arguments.CheckNotNull( createView, "createView" );
			m_Name = name;
			m_CreateView = createView;
			m_AvailableAsCommand = availableAsCommand;
			m_DefaultDockState = defaultDockState;
		}

		/// <summary>
		/// Creates the underlying control
		/// </summary>
		public Control CreateControl( IWorkspace workspace )
		{
			return m_CreateView( workspace );
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

		#region IViewInfo Members

		/// <summary>
		/// Gets the name of this view
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}


		/// <summary>
		/// Gets/sets the flag that determines if this view can be created from a command
		/// </summary>
		public bool AvailableAsCommand
		{
			get { return m_AvailableAsCommand; }
		}

		#endregion

		#region Private Members

		private string m_DockPersistenceInfo;
		private readonly string m_Name;
		private readonly bool m_AvailableAsCommand;
		private CreateViewDelegate m_CreateView;
		private DockState m_DefaultDockState;

		#endregion

	}
}
