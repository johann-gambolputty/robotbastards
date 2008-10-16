using System;
using System.Windows.Forms;

namespace Poc1.PlanetBuilder
{
	public partial class ShowExceptionForm : Form
	{
		public ShowExceptionForm( )
		{
			InitializeComponent( );
		}

		#region Show helpers

		/// <summary>
		/// Shows an exception form
		/// </summary>
		public static void Display( Exception ex, string message )
		{
			Display( null, ex, message );
		}

		/// <summary>
		/// Shows an exception form
		/// </summary>
		public static void Display( Exception ex, string message, params object[] args )
		{
			Display( null, ex, message, args );
		}

		/// <summary>
		/// Shows an exception form
		/// </summary>
		public static void Display( IWin32Window parent, Exception ex, string message )
		{
			ShowExceptionForm form = new ShowExceptionForm( );
			form.Message = message;
			form.Exception = ex;
			form.ShowDialog( parent );
		}

		/// <summary>
		/// Shows an exception form
		/// </summary>
		public static void Display( IWin32Window parent, Exception ex, string message, params object[] args )
		{
			ShowExceptionForm form = new ShowExceptionForm( );
			form.Message = string.Format( message, args );
			form.Exception = ex;
			form.ShowDialog( parent );
		}

		#endregion

		/// <summary>
		/// Gets/sets the exception displayed by this form
		/// </summary>
		public Exception Exception
		{
			get { return m_Exception; }
			set
			{
				m_Exception = value;
				exceptionTextBox.Text = m_Exception.ToString( );
			}
		}

		/// <summary>
		/// Gets/sets the message text
		/// </summary>
		public string Message
		{
			get { return messageLabel.Text; }
			set { messageLabel.Text = value; }
		}

		private Exception m_Exception;
	}
}