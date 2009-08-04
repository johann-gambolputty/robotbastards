using System;
using System.Windows.Forms;
using Goo.Core.Mvc;
using Goo.Core.Ui.Layouts.Docking;
using Goo.Core.Ui.WinForms.Layouts;
using Rb.Core.Utils;
using WeifenLuo.WinFormsUI.Docking;

namespace Goo.Core.Ui.WinForms.MagicDocking.Layouts.Docking
{
	/// <summary>
	/// Docking frame implementation
	/// </summary>
	public class DockingFrame : IDockingFrame
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="panel">Docking panel used to host this frame</param>
		public DockingFrame( DockPanel panel )
		{
			Arguments.CheckNotNull( panel, "panel" );
			m_Panel = panel;
			m_Content = new DockContent( );
			m_Content.AutoScroll = true;
		}

		/// <summary>
		/// Shows the panel
		/// </summary>
		public void Show( )
		{
			m_Content.Show( m_Panel, DockState.DockLeft );
		}

		#region IDockingFrame Members

		/// <summary>
		/// Gets/sets the title of this frame
		/// </summary>
		public string Title
		{
			get { return m_Content.Text; }
			set { m_Content.Text = value; }
		}

		/// <summary>
		/// Gets/sets the view displayed in this frame
		/// </summary>
		public IView View
		{
			get { return m_View; }
			set
			{
				if ( m_View == value )
				{
					return;
				}
				m_Content.Controls.Clear( );
				if ( value == null )
				{
					return;
				}
				Control viewControl = WinFormsLayoutHelpers.GetViewAsControl( value );
				m_Content.Controls.Add( viewControl );
				viewControl.Dock = DockStyle.Fill;
				m_View = value;
			}
		}

		/// <summary>
		/// Gets/sets whether or not this frame is visible
		/// </summary>
		public bool Visible
		{
			get { return m_Content.Visible; }
			set { m_Content.Visible = value; }
		}

		/// <summary>
		/// Shows the frame in a given location
		/// </summary>
		/// <param name="location"></param>
		public void Show( DockLocation location )
		{
			switch ( location )
			{
				case DockLocation.Left	:
					m_Content.Show( m_Panel, DockState.DockLeft );
					return;
				case DockLocation.Right	:
					m_Content.Show( m_Panel, DockState.DockRight );
					return;
				case DockLocation.Top:
					m_Content.Show( m_Panel, DockState.DockTop );
					return;
				case DockLocation.Bottom:
					m_Content.Show( m_Panel, DockState.DockBottom );
					return;
				case DockLocation.Floating:
					m_Content.Show( m_Panel, DockState.Float );
					return;
				case DockLocation.Fill:
					m_Content.Show( m_Panel, DockState.Document );
					return;
			}
			throw new NotSupportedException( "Unsupported docking location " + location );
		}

		#endregion

		#region Private Members

		private IView m_View;
		private readonly DockPanel m_Panel;
		private DockContent m_Content;

		#endregion
	}
}
