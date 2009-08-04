using System;
using System.Windows.Forms;
using Goo.Common.Layouts.Dialogs;
using Goo.Common.WinForms.Layouts.Dialogs.Controls;
using Goo.Core.Mvc;
using Goo.Core.Services.Workspaces;
using log4net;
using Rb.Core.Utils;

namespace Goo.Common.WinForms.Layouts.Dialogs
{
	/// <summary>
	/// Dialog layout service implementation
	/// </summary>
	public class DialogService : IDialogService
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="mainForm">Main windows form, used as parent for all dialogs</param>
		/// <param name="activeWorkspaceService">Active workspace provider</param>
		public DialogService( Form mainForm, IActiveWorkspaceService activeWorkspaceService )
		{
			Arguments.CheckNotNull( mainForm, "mainForm" );
			Arguments.CheckNotNull( activeWorkspaceService, "activeWorkspaceService" );
			m_MainForm = mainForm;
			m_ActiveWorkspaceService = activeWorkspaceService;
			m_Log = LogManager.GetLogger( GetType( ) );
		}

		#region IDialogService Members

		/// <summary>
		/// Creates a default dialog frame
		/// </summary>
		/// <param name="frameType">Default frame type</param>
		/// <param name="title">Title text</param>
		/// <returns>Returns a new dialog frame</returns>
		public IDialogFrame CreateDefaultDialogFrame( DefaultDialogFrameType frameType, string title )
		{
			SimpleDialogFrame frame = new SimpleDialogFrame( m_MainForm );
			frame.Text = title;
			m_Log.Info( "Creating default dialog frame " + frame );
			switch ( frameType )
			{
				case DefaultDialogFrameType.Ok :
					frame.AddExitButton( "Ok", ResultOfDialog.Ok );
					return frame;
				case DefaultDialogFrameType.OkCancel :
					frame.AddExitButton( "Ok", ResultOfDialog.Ok );
					frame.AddExitButton( "Cancel", ResultOfDialog.Cancel );
					return frame;
				case DefaultDialogFrameType.YesNo:
					frame.AddExitButton( "Yes", ResultOfDialog.Yes );
					frame.AddExitButton( "No", ResultOfDialog.No );
					return frame;
				case DefaultDialogFrameType.YesNoCancel:
					frame.AddExitButton( "Yes", ResultOfDialog.Yes );
					frame.AddExitButton( "No", ResultOfDialog.No );
					frame.AddExitButton( "Cancel", ResultOfDialog.Cancel );
					return frame;
				case DefaultDialogFrameType.RetryCancel:
					frame.AddExitButton( "Retry", ResultOfDialog.Retry );
					frame.AddExitButton( "Cancel", ResultOfDialog.Cancel );
					return frame;
			}
			throw new NotSupportedException( "Unsupported default frame type " + frameType );
		}

		/// <summary>
		/// Shows a view in a default modal frame
		/// </summary>
		/// <param name="frameType">Default frame type to use</param>
		/// <param name="title">Frame title text</param>
		/// <param name="view">View to show in the frame</param>
		/// <returns>Returns the result of the dialog</returns>
		public ResultOfDialog ShowModal( DefaultDialogFrameType frameType, string title, IView view )
		{
			m_Log.InfoFormat( "Displaying view \"{0}\" in modal default frame " + frameType, view );
			IDialogFrame frame = CreateDefaultDialogFrame( frameType, title );
			frame.ShowModal( m_ActiveWorkspaceService.CurrentWorkspace, view );
			return frame.FrameResult;
		}

		/// <summary>
		/// Shows a view in a default modeless frame
		/// </summary>
		/// <param name="frameType">Default frame type to use</param>
		/// <param name="title">Frame title text</param>
		/// <param name="view">View to show in the frame</param>
		/// <returns>Returns the new default dialog frame used to show the view</returns>
		public IDialogFrame Show( DefaultDialogFrameType frameType, string title, IView view )
		{
			m_Log.InfoFormat( "Displaying view \"{0}\" in modeless default frame " + frameType, view );
			IDialogFrame frame = CreateDefaultDialogFrame( frameType, title );
			frame.ShowModeless( m_ActiveWorkspaceService.CurrentWorkspace, view );
			return frame;
		}

		#endregion

		#region Private Members

		private readonly Form m_MainForm;
		private readonly ILog m_Log;
		private readonly IActiveWorkspaceService m_ActiveWorkspaceService;

		#endregion

	}
}
