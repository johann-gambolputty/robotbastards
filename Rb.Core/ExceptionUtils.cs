using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Rb.Core
{
	/// <summary>
	/// Exception utilities.
	/// </summary>
	public class ExceptionUtils
	{
		/// <summary>
		/// Unhandled exception handler for threads (form apps)
		/// </summary>
		public static void UnhandledThreadExceptionHandler( object sender, System.Threading.ThreadExceptionEventArgs args )
		{
			try
			{
				string exceptionString = ToString( args.Exception );
				Rb.Log.App.Error( exceptionString );

				switch ( MessageBox.Show( exceptionString, "Unhandled Exception", MessageBoxButtons.AbortRetryIgnore ) )
				{
					case DialogResult.Abort		:	Application.Exit( );	break;
					case DialogResult.Retry		:
					case DialogResult.Ignore	:	break;
				}
			}
			catch
			{
				Application.Exit( );
			}
		}

		/// <summary>
		/// Converts an exception to a string
		/// </summary>
		/// <remarks>
		/// This is analogous to e.ToString(), except that the stack trace is written as output-window clickable items (i.e. in the format
		/// file.cs(line) : message), and the inner exceptions are tabbed
		/// </remarks>
		public static string ToString( System.Exception e )
		{
			return ToString( e, 0 );
		}

		private const string kNewLine = "\r\n"; // <== GRRR

		/// <summary>
		/// Converts an exception to a string, with a given number of tabs preceeding the output
		/// </summary>
		public static string ToString( System.Exception e, int tabs )
		{
			if ( e == null )
			{
				return "";
			}

			StringBuilder tabStringBuilder = new StringBuilder( tabs, tabs + 1 );
			//	Insert tabs
			for ( int tabCount = 0; tabCount < tabs; ++tabCount )
			{
				tabStringBuilder.Append( '\t' );
			}
			string tabString = tabStringBuilder.ToString( );

			StringBuilder builder = new StringBuilder( 256 );

			//	Keep the first part of the original exception string format: "type : message"
			builder.Append( tabString );
			builder.Append( e.GetType( ).Name );
			builder.Append( " : " );
			builder.Append( e.Message );
			builder.Append( kNewLine );

			//	GRRR there's no representation of the stack trace apart from the string :(
			//	Tear it apart using regular expressions and rebuild
			//	TODO: If no matches are found, then just write the StackTrace to the builder
			for ( Match callMatch = ms_CallDumpExp.Match( e.StackTrace ); callMatch.Success; callMatch = callMatch.NextMatch( ) )
			{
				builder.AppendFormat( tabString );
				builder.AppendFormat( "{0}({1}): {2}", callMatch.Groups[ 2 ].Value, callMatch.Groups[ 3 ].Value, callMatch.Groups[ 1 ].Value );
				builder.Append( kNewLine );
			}

			return builder.ToString( ) + ToString( e.InnerException, tabs + 1 );
		}

		/// <summary>
		/// Call dump regular expression. Usually in the format " at X in Y: line Z", where X is the function name, Y is the filename, Z is the line number
		/// </summary>
		private static Regex ms_CallDumpExp = new Regex( @" at (.+) in (.+)\:line ([0-9]+)" );

	}
}
