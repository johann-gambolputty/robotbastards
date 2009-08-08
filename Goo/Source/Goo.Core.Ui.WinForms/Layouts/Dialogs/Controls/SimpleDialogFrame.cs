using System;
using System.Windows.Forms;
using Goo.Common.Layouts.Dialogs;
using Goo.Core.Mvc;
using Goo.Core.Ui.WinForms.Layouts;
using Goo.Core.Workspaces;

namespace Goo.Common.WinForms.Layouts.Dialogs.Controls
{
	public partial class SimpleDialogFrame : Form, IDialogFrame
	{
		public SimpleDialogFrame( )
		{
			InitializeComponent( );
		}

		public SimpleDialogFrame( Form parentForm )
		{
			InitializeComponent( );
			m_ParentForm = parentForm;
		}

		/// <summary>
		/// Adds a button that causes the frame to close
		/// </summary>
		public void AddExitButton( string text, ResultOfDialog result )
		{
			Button button = new Button( );
			button.AutoSize = true;
			button.Text = text;
			button.Click +=
				delegate
					{
						m_Result = result;
						Close( );
					};
			button.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			buttonPanel.Controls.Add( button );

			if ( result == ResultOfDialog.Ok )
			{
				AcceptButton = button;
				button.DialogResult = DialogResult.OK;
			}
			else if ( result == ResultOfDialog.Yes )
			{
				AcceptButton = button;
				button.DialogResult = DialogResult.Yes;
			}
			else if ( result == ResultOfDialog.Cancel )
			{
				CancelButton = button;
				button.DialogResult = DialogResult.Cancel;
			}
			else if ( result == ResultOfDialog.No )
			{
				CancelButton = button;
				button.DialogResult = DialogResult.No;
			}
		}

		#region IDialogFrame Members

		/// <summary>
		/// Event raised when the frame is closed
		/// </summary>
		public event Action<ResultOfDialog> FrameClosed;

		/// <summary>
		/// The dialog result, valid if this frame has been close
		/// </summary>
		public ResultOfDialog FrameResult
		{
			get { return m_Result; }
		}

		/// <summary>
		/// Shows this frame as a modeless frame containing the specified view
		/// </summary>
		public void ShowModal( IWorkspace workspace, IView view )
		{
			AddView( view );
			ShowDialog( m_ParentForm );
		}

		/// <summary>
		/// Shows this frame as a modal frame containing the specified view
		/// </summary>
		public void ShowModeless( IWorkspace workspace, IView view )
		{
			AddView( view );
			Show( m_ParentForm );
		}

		#endregion

		#region Private Members

		private readonly Form m_ParentForm;
		private IView m_View;
		private ResultOfDialog m_Result;
		
		private void AddView( IView view )
		{
			if ( m_View != null )
			{
				m_View.OnFrameClosing( );
			}
			Control viewControl = WinFormsLayoutHelpers.GetViewAsControl( view );
			viewControl.Dock = DockStyle.Fill;
			hostPanel.Controls.Add( viewControl );
			m_View = view;
		}

		private void SimpleDialogFrame_FormClosing( object sender, FormClosingEventArgs e )
		{
			if ( m_View != null )
			{
				m_View.OnFrameClosing( );
			}
		}

		private void SimpleDialogFrame_FormClosed( object sender, FormClosedEventArgs e )
		{
			if ( FrameClosed != null )
			{
				FrameClosed( m_Result );
			}
		}
		
		#endregion
	}
}