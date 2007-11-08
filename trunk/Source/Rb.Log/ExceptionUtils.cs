using System;
using System.Text;
using System.Collections.Generic;
using Rb.Log;

namespace Rb.Log
{
	/// <summary>
	/// Exception utilities.
	/// </summary>
	public class ExceptionUtils
	{
		/// <summary>
		/// Converts an exception to a string
		/// </summary>
		/// <remarks>
		/// This is analogous to e.ToString(), except that the stack trace is written as output-window clickable items (i.e. in the format
		/// file.cs(line) : message), and the inner exceptions are tabbed
		/// </remarks>
		public static string ToString( Exception e )
		{
			return ToString( e, 0 );
		}

		private const string kNewLine = "\r\n"; // <== GRRR

        /// <summary>
        /// Writes an exception into one or more log entries, storing them in entries
        /// </summary>
        /// <param name="source">Log source</param>
        /// <param name="ex">Exception</param>
        /// <param name="entries">Log entry list</param>
        public static void ToLogEntries( Source source, Exception ex, IList< Entry > entries )
        {
            ToLogEntries( source, ex, entries, "" );
        }

        /// <summary>
        /// Writes an exception into one or more log entries, storing them in entries with a given prefix
        /// </summary>
        /// <param name="source">Log source</param>
        /// <param name="ex">Exception</param>
        /// <param name="entries">Log entry list</param>
        /// <param name="prefix">Entry string prefix</param>
        public static void ToLogEntries( Source source, Exception ex, IList< Entry > entries, string prefix )
        {
            if ( ex == null )
            {
                return;
            }
            StackTraceInfo info = new StackTraceInfo( ex.StackTrace );
            
            Entry baseEntry = new Entry( source, prefix + ex.Message );
            if ( info.HasMatch )
            {
                baseEntry.Locate( info.File, int.Parse( info.Line ), 1, info.Method );
                info.NextMatch( );
            }

            entries.Add( baseEntry );

            while ( info.HasMatch )
            {
                entries.Add( new Entry( source, prefix + "(call stack)" ).Locate( info.File, int.Parse( info.Line ), 1, info.Method ) );
                info.NextMatch( );
            }

			ToLogEntries( source, ex.InnerException, entries, prefix + "\t" );
        }

        /// <summary>
        /// Writes an exception into one or more log entries
        /// </summary>
        /// <param name="source">Log source</param>
        /// <param name="ex">Exception</param>
        public static void ToLog( Source source, Exception ex )
        {
            List< Entry > entries = new List< Entry >( );
            ToLogEntries( source, ex, entries );
            foreach ( Entry entry in entries )
            {
                Source.HandleEntry( entry );
            }
        }

	    /// <summary>
		/// Converts an exception to a string, with a given number of tabs preceeding the output
		/// </summary>
		public static string ToString( Exception e, int tabs )
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
            for ( StackTraceInfo info = new StackTraceInfo( e.StackTrace ); info.HasMatch; info.NextMatch( ) )
			{
				builder.AppendFormat( tabString );
				builder.AppendFormat( "{0}({1}): {2}", info.File, info.Line, info.Method );
				builder.Append( kNewLine );
			}

			return builder + ToString( e.InnerException, tabs + 1 );
		}

	}
}
