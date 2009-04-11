using System.Windows.Forms;
using Bob.Core.Ui.Interfaces.Views;
using Bob.Core.Workspaces.Interfaces;
using Rb.Core.Utils;
using Rb.Interaction.Classes;

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
		public ControlViewInfo( string name, CreateViewDelegate createView )
		{
			Arguments.CheckNotNull( createView, "createView" );
			m_Name = name;
			m_CreateView = createView;
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public ControlViewInfo( string name, CreateViewDelegate createView, CommandGroup showCommandGroup )
		{
			Arguments.CheckNotNull( createView, "createView" );
			m_Name = name;
			m_CreateView = createView;

			if ( showCommandGroup != null )
			{
				m_ShowCommand = showCommandGroup.NewCommand( name, name, name );
			}
		}

		/// <summary>
		/// Setup constructor with explicit show command (show command can be null)
		/// </summary>
		public ControlViewInfo( string name, CreateViewDelegate createView, Command showCommand )
		{
			Arguments.CheckNotNull( createView, "createView" );
			m_Name = name;
			m_CreateView = createView;
			m_ShowCommand = showCommand;
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
		/// Gets the command used to show this view
		/// </summary>
		public Command ShowCommand
		{
			get { return m_ShowCommand; }
		}

		#endregion

		#region Private Members

		private readonly Command m_ShowCommand;
		private readonly string m_Name;
		private CreateViewDelegate m_CreateView;

		#endregion

	}
}
