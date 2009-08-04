
namespace Goo.Common.Layouts.Dialogs
{
	/// <summary>
	/// Dialog result
	/// </summary>
	public class ResultOfDialog
	{
		/// <summary>
		/// Returns the default "OK" option
		/// </summary>
		public static ResultOfDialog Ok
		{
			get { return s_Ok; }
		}

		/// <summary>
		/// Returns the default "Cancel" option
		/// </summary>
		public static ResultOfDialog Cancel
		{
			get { return s_Cancel; }
		}

		/// <summary>
		/// Returns the default "Yes" option
		/// </summary>
		public static ResultOfDialog Yes
		{
			get { return s_Yes; }
		}

		/// <summary>
		/// Returns the default "No" option
		/// </summary>
		public static ResultOfDialog No
		{
			get { return s_No; }
		}

		/// <summary>
		/// Returns the default "Retry" option
		/// </summary>
		public static ResultOfDialog Retry
		{
			get { return s_Retry; }
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="text">Result text</param>
		public ResultOfDialog( string text )
		{
			m_Text = text;
		}

		/// <summary>
		/// Returns the result text
		/// </summary>
		public override string ToString( )
		{
			return m_Text;
		}

		#region Private Members

		private readonly string m_Text;

		private static readonly ResultOfDialog s_Ok = new ResultOfDialog( "Ok" );
		private static readonly ResultOfDialog s_Cancel = new ResultOfDialog( "Cancel" );

		private static readonly ResultOfDialog s_Yes = new ResultOfDialog( "Yes" );
		private static readonly ResultOfDialog s_No = new ResultOfDialog( "No" );

		private static readonly ResultOfDialog s_Retry = new ResultOfDialog( "Retry" );
		
		#endregion
	}
}
