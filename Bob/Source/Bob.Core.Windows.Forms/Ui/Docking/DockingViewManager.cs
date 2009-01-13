using System.Windows.Forms;
using Bob.Core.Ui.Interfaces;
using Bob.Core.Workspaces.Interfaces;
using Rb.Core.Utils;
using WeifenLuo.WinFormsUI.Docking;

namespace Bob.Core.Windows.Forms.Ui.Docking
{
	/// <summary>
	/// Creates dockable views
	/// </summary>
	public class DockingViewManager : IViewManager
	{
		#region IViewManager Members

		/// <summary>
		/// Creates a view from a view object
		/// </summary>
		/// <param name="workspace">Workspace context</param>
		/// <param name="view">View information</param>
		public void Create( IWorkspace workspace, IViewInfo view )
		{
			Arguments.CheckNotNull( workspace, "workspace" );
			DockingViewInfo dockView = Arguments.CheckedNonNullCast<DockingViewInfo>( view );
			Control control = dockView.CreateControl( workspace );

			DockContent content = new WindowDockContent( control.GetType( ).Name );

			content.Text = view.Name;
			content.Controls.Add( control );
			content.AutoScroll = true;

			control.Dock = DockStyle.Fill;
		}

		#endregion

		#region Private Members


		/// <summary>
		/// Provides custom persistence strings to DockContent
		/// </summary>
		private class WindowDockContent : DockContent
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			/// <param name="persistString">String to write when content is persists</param>
			public WindowDockContent( string persistString )
			{
				m_PersistString = persistString;
			}

			/// <summary>
			/// Returns the persistence string passed to the constructor
			/// </summary>
			protected override string GetPersistString( )
			{
				return m_PersistString;
			}

			#region Private Members

			private readonly string m_PersistString;

			#endregion
		}


		#endregion
	}
}
