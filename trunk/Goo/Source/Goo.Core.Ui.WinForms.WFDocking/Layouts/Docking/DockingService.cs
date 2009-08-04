using System.IO;
using System.Text;
using System.Windows.Forms;
using Goo.Core.Mvc;
using Goo.Core.Ui.Layouts;
using Goo.Core.Ui.Layouts.Docking;
using Rb.Core.Utils;
using WeifenLuo.WinFormsUI.Docking;

namespace Goo.Core.Ui.WinForms.MagicDocking.Layouts.Docking
{
	/// <summary>
	/// Docking service implementation, using the magic docking library
	/// </summary>
	public class DockingService : IDockingService, ILayoutSerializerService
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="form">Form to attach a docking panel to, when the first docking frame is created</param>
		public DockingService( Form form )
		{
			Arguments.CheckNotNull( form, "form" );
			m_Form = form;
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="dockPanel">Docking panel used by this service</param>
		public DockingService( DockPanel dockPanel )
		{
			Arguments.CheckNotNull( dockPanel, "dockPanel" );
			m_DockPanel = dockPanel;
		}

		#region IDockingService Members

		/// <summary>
		/// Creates a docking frame with the specified title
		/// </summary>
		/// <param name="title">Frame title</param>
		/// <returns>Returns a new frame</returns>
		public IDockingFrame CreateFrame( string title )
		{
			IDockingFrame frame = new DockingFrame( DockPanel );
			frame.Title = title;
			return frame;
		}

		/// <summary>
		/// Shows a view in a new docking frame
		/// </summary>
		/// <param name="title">Frame title</param>
		/// <param name="view">View to show</param>
		/// <param name="initialDockLocation">Initial docking state of the frame</param>
		/// <returns>Returns the docking frame created to host the view</returns>
		public IDockingFrame Show( string title, IView view, DockLocation initialDockLocation )
		{
			Arguments.CheckNotNull( view, "view" );
			IDockingFrame frame = CreateFrame( title );
			frame.View = view;
			frame.Show( initialDockLocation );
			return frame;
		}

		/// <summary>
		/// Shows a view in an existing docking frame
		/// </summary>
		/// <param name="frame">Existing frame</param>
		/// <param name="view">View to show</param>
		public void Show( IDockingFrame frame, IView view )
		{
			Arguments.CheckNotNull( frame, "frame" );
			Arguments.CheckNotNull( view, "view" );
			frame.View = view;
		}

		#endregion

		#region Private Members

		private readonly Form m_Form;
		private DockPanel m_DockPanel;

		private DockPanel DockPanel
		{
			get
			{
				if ( m_DockPanel != null )
				{
					return m_DockPanel;
				}
				m_Form.IsMdiContainer = true;
				m_DockPanel = new DockPanel( );
				m_DockPanel.ActiveAutoHideContent = null;
				m_DockPanel.Dock = DockStyle.Fill;
				m_DockPanel.Name = "dockPanel";
				m_Form.Controls.Add( m_DockPanel );
				m_DockPanel.BringToFront( );
				return m_DockPanel;
			}
		}

		#endregion

		#region ILayoutSerializerService Members

		/// <summary>
		/// Loads the service layout
		/// </summary>
		/// <param name="stream">Stream containing serialized service layout</param>
		public void Load( Stream stream )
		{
			DeserializeDockContent deserialize =
				delegate( string ps )
				{
					return new DockContent( );
				};
			DockPanel.LoadFromXml( stream, deserialize );
		}

		/// <summary>
		/// Saves the service layout
		/// </summary>
		/// <param name="stream">Stream to write serialized layout to</param>
		/// <returns>Returns true if anything was saved to the stream</returns>
		public bool Save( Stream stream )
		{
			if ( m_DockPanel == null )
			{
				return false;
			}
			DockPanel.SaveAsXml( stream, Encoding.UTF8 );
			return true;
		}

		#endregion
	}
}
