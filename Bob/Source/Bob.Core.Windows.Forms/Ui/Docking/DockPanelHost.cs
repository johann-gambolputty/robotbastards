
using System.Collections.Generic;
using System.Windows.Forms;
using Rb.Core.Utils;
using WeifenLuo.WinFormsUI.Docking;

namespace Bob.Core.Windows.Forms.Ui.Docking
{
	/// <summary>
	/// Helper class for hosting controls in a DockPanel
	/// </summary>
	public class DockPanelHost
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="mainDockPanel">Main dock panel</param>
		/// <exception cref="System.ArgumentNullException">Thrown if mainDockPanel is null</exception>
		public DockPanelHost( DockPanel mainDockPanel )
		{
			Arguments.CheckNotNull( mainDockPanel, "mainDockPanel" );
			m_MainDockPanel = mainDockPanel;
		}

		/// <summary>
		/// Returns true if a control is visible
		/// </summary>
		public bool MakeExistingControlVisible( string title )
		{
			DockContent content;
			if ( !m_ContentByTitle.TryGetValue( title, out content ) )
			{
				return false;
			}
			if ( content.DockPanel == null )
			{
				//	Content has been detached from the docking panel - user closed the content
				m_ContentByTitle.Remove( title );
				return false;
			}
			content.Show( );
			return true;
		}


		/// <summary>
		/// Hosts a control
		/// </summary>
		/// <param name="title">Title of the content window used to host the control</param>
		/// <param name="control">Control to host</param>
		public void HostControl( string title, Control control )
		{
		}

		#region Private Members

		private readonly DockPanel m_MainDockPanel;
		private readonly Dictionary<string, DockContent> m_ContentByTitle = new Dictionary<string, DockContent>( );


		#endregion
	}
}
