using System.Windows.Forms;
using Bob.Core.Ui.Interfaces.Views;
using Bob.Core.Workspaces.Interfaces;
using Rb.Core.Utils;

namespace Bob.Core.Windows.Forms.Ui.Docking
{
	/// <summary>
	/// Control based view info
	/// </summary>
	public class ControlViewInfo : IViewInfo
	{
		/// <summary>
		/// Delegate used for methods that create views from this DockingViewInfo
		/// </summary>
		/// <param name="workspace">Workspace to create the view in</param>
		public delegate Control CreateViewDelegate( IWorkspace workspace );
		
		/// <summary>
		/// Setup constructor
		/// </summary>
		public ControlViewInfo( string name, CreateViewDelegate createView, bool availableAsCommand )
		{
			Arguments.CheckNotNull( createView, "createView" );
			m_Name = name;
			m_CreateView = createView;
			m_AvailableAsCommand = availableAsCommand;
		}

		/// <summary>
		/// Creates the underlying control
		/// </summary>
		public Control CreateControl( IWorkspace workspace )
		{
			return m_CreateView( workspace );
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

		private readonly string m_Name;
		private readonly bool m_AvailableAsCommand;
		private CreateViewDelegate m_CreateView;

		#endregion

	}
}
