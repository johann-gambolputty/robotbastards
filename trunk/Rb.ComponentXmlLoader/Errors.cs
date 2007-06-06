using System;
using System.Collections.Generic;
using System.Xml;

using Rb.Core.Utils;
using Rb.Core;
using Rb.Log;

namespace Rb.ComponentXmlLoader
{
    /// <summary>
    /// Maintains a collection of errors that were generated during loading
    /// </summary>
    internal class ErrorCollection : List< Entry >
    {
        public ErrorCollection( string inputSource )
        {
            m_InputSource = inputSource;
        }

        public void Add( BaseBuilder builder, string format, params object[] args )
        {
            Add( builder.Line, builder.Column, format, args );
        }

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

        public void Add( int line, int column, string format, params object[] args )
        {
            Entry entry = new Entry( ResourcesLog.GetSource( Severity.Error ), string.Format( format, args ) );
            entry.Locate( m_InputSource, line, column, "" );
            Add( entry );
        }

        public void Add( BaseBuilder builder, Exception ex, string format, params object[] args )
        {
            Add( builder.Line, builder.Column, ex, format, args );
        }

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

        public void Add( int line, int column, Exception ex, string format, params object[] args )
        {
            //  Convert the exception into log entries
            List< Entry > entries = new List< Entry >( );
            ExceptionUtils.ToLogEntries( ResourcesLog.GetSource( Severity.Error ), ex, entries );

            //  Store the log entries in the error collection
            Add( line, column, format, args );
            foreach ( Entry curEntry in entries )
            {
                Add( curEntry );
            }
        }

        private string m_InputSource;

    }
}
