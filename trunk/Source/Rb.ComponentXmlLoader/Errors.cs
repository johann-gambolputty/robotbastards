using System;
using System.Collections.Generic;
using System.Xml;
using Rb.Core;
using Rb.Log;

namespace Rb.ComponentXmlLoader
{
    /// <summary>
    /// Maintains a collection of errors that were generated during loading
    /// </summary>
    internal class ErrorCollection : List< Entry >
    {
		/// <summary>
		/// Sets up this error collection
		/// </summary>
		/// <param name="inputSource">Path to the input (displayed in error logs)</param>
        public ErrorCollection( string inputSource )
        {
            m_InputSource = inputSource;
        }

		/// <summary>
		/// Adds an error message log entry
		/// </summary>
		/// <param name="builder">Builder that generated the error</param>
		/// <param name="format">Formatted message</param>
		/// <param name="args">Format arguments</param>
        public void Add( BaseBuilder builder, string format, params object[] args )
        {
            Add( builder.Line, builder.Column, format, args );
        }

		/// <summary>
		/// Adds an error message log entry
		/// </summary>
		/// <param name="reader">Reader in state when error ocurred</param>
		/// <param name="format">Formatted message</param>
		/// <param name="args">Format arguments</param>
        public void Add( XmlReader reader, string format, params object[] args )
        {
            IXmlLineInfo lineInfo = reader as IXmlLineInfo;
            if ( lineInfo != null )
            {
                Add( lineInfo.LineNumber, lineInfo.LinePosition, format, args );
            }
            else
            {
                Add( 0, 0, format, args );
            }
        }

		/// <summary>
		/// Adds an error message log entry
		/// </summary>
		/// <param name="line">Line number</param>
		/// <param name="column">Column number</param>
		/// <param name="format">Formatted message</param>
		/// <param name="args">Format arguments</param>
        public void Add( int line, int column, string format, params object[] args )
        {
            Entry entry = new Entry( AssetsLog.GetSource( Severity.Error ), string.Format( format, args ) );
            entry.Locate( m_InputSource, line, column, "" );
            Add( entry );
        }

		/// <summary>
		/// Adds an error message log entry, for a given exception
		/// </summary>
		/// <param name="builder">Builder that generated the error</param>
		/// <param name="ex">Exception detail</param>
		/// <param name="format">Formatted message</param>
		/// <param name="args">Format arguments</param>
        public void Add( BaseBuilder builder, Exception ex, string format, params object[] args )
        {
            Add( builder.Line, builder.Column, ex, format, args );
        }

		/// <summary>
		/// Adds an error message log entry
		/// </summary>
		/// <param name="reader">Reader in state when error ocurred</param>
		/// <param name="ex">Exception detail</param>
		/// <param name="format">Formatted message</param>
		/// <param name="args">Format arguments</param>
        public void Add( XmlReader reader, Exception ex, string format, params object[] args )
        {
            IXmlLineInfo lineInfo = reader as IXmlLineInfo;
            if ( lineInfo != null )
            {
                Add( lineInfo.LineNumber, lineInfo.LinePosition, ex, format, args );
            }
            else
            {
                Add( 0, 0, ex, format, args );
            }
        }

		/// <summary>
		/// Adds an error message log entry
		/// </summary>
		/// <param name="line">Line number</param>
		/// <param name="column">Column number</param>
		/// <param name="ex">Exception detail</param>
		/// <param name="format">Formatted message</param>
		/// <param name="args">Format arguments</param>
        public void Add( int line, int column, Exception ex, string format, params object[] args )
        {
            Add( line, column, format, args );
            IndentedAdd( "    ", ex );
        }

		/// <summary>
		/// Adds log entries for a given exception, and its inner exception (indented entries)
		/// </summary>
		/// <param name="indent">Current indent</param>
		/// <param name="ex">Exception details</param>
        private void IndentedAdd( string indent, Exception ex )
        {
            //  Convert the exception into log entries
            List< Entry > entries = new List< Entry >( );
            ExceptionUtils.ToLogEntries( AssetsLog.GetSource( Severity.Error ), ex, entries, indent );

            //  Store the log entries in the error collection
            foreach ( Entry curEntry in entries )
            {
                Add( curEntry );
            }

            //  Inner exception
            if ( ex.InnerException != null )
            {
                IndentedAdd( indent + "    ", ex.InnerException );
            }
        }

        private readonly string m_InputSource;

    }
}
