using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Rb.Log
{
    /// <summary>
    /// Log entry
    /// </summary>
    public class Entry
    {
        #region Entry creation from log files

        /// <summary>
        /// Creates an array of Entry objects by parsing the contents of a log file
        /// </summary>
        /// <param name="path">Log file path</param>
        /// <returns>Returns entry array</returns>
        public static Entry[] CreateEntriesFromLogFile( string path )
        {
            StreamReader reader = System.IO.File.OpenText( path );
            return CreateEntriesFromLogText( reader );
        }

        /// <summary>
        /// Creates an array of Entry objects by parsing the contents of a log stream
        /// </summary>
        /// <param name="reader">Log text reader</param>
        /// <returns>Returns entry array</returns>
        public static Entry[] CreateEntriesFromLogText( TextReader reader )
        {
            List< Entry > entries = new List< Entry >( );

            foreach ( Match curMatch in LogEntryRegex.Matches( reader.ReadToEnd( ) ) )
            {
                string file     = curMatch.Groups[ "File" ].Value;
                string line     = curMatch.Groups[ "Line" ].Value;
                string column   = curMatch.Groups[ "Column" ].Value;
                string source   = curMatch.Groups[ "Source" ].Value;
                string message  = curMatch.Groups[ "Message" ].Value;
                string thread   = curMatch.Groups[ "Thread" ].Value;
                string method   = curMatch.Groups[ "Method" ].Value;

                //  TODO: AP: Time not written/read
                Entry newEntry = new Entry( Log.Source.BuildFromString( source ), message, thread, System.DateTime.Now.TimeOfDay );
                newEntry.Locate( file, int.Parse( line ), int.Parse( column ), method );
                entries.Add( newEntry );
            }

            return entries.ToArray( );
        }

        private static Regex LogEntryRegex = new Regex
            (
                @"(?<File>.*)\((?<Line>\d+),(?<Column>\d+)\)\:\<(?<Source>(?:\w+\.?)+)\>(?<Message>.*)\[(?<Thread>\d+)->(?<Method>.*)\]"
            );

        #endregion

        #region Public construction

        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="source">Message source</param>
        /// <param name="message">Message string</param>
        public Entry( Source source, string message )
        {
            m_Source        = source;
            m_Message       = message;
            m_ThreadName    = System.Threading.Thread.CurrentThread.Name;
            m_Time          = System.DateTime.Now.TimeOfDay;
            m_Id            = m_MessageId++;

            if ( string.IsNullOrEmpty( m_ThreadName ) )
            {
                m_ThreadName = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString( );
            }
        }
        
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="source">Message source</param>
        /// <param name="message">Message string</param>
        /// <param name="threadName">Thread source</param>
        /// <param name="time">Time that entry was generated</param>
        public Entry( Source source, string message, string threadName, System.TimeSpan time )
        {
            m_Source        = source;
            m_Message       = message;
            m_ThreadName    = threadName;
            m_Time          = time;
            m_Id            = m_MessageId++;
        }

        #endregion

        #region Location

        /// <summary>
        /// Locates this log entry by checking the stack
        /// </summary>
        /// <param name="skip">Stack frames to skip</param>
        /// <returns>Returns this</returns>
        public Entry Locate( int skip )
        {
            System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame( skip, true );
            return Locate( frame );
        }

        /// <summary>
        /// Locates this log entry from a stack frame
        /// </summary>
        /// <param name="frame">Location</param>
        /// <returns>Returns this</returns>
        public Entry Locate( System.Diagnostics.StackFrame frame )
        {
            return Locate( frame.GetFileName( ), frame.GetFileLineNumber( ), frame.GetFileColumnNumber( ), frame.GetMethod( ).Name );
        }

        /// <summary>
        /// Locates this log entry
        /// </summary>
        /// <param name="file">File location</param>
        /// <param name="line">Line location</param>
        /// <param name="column">Column location</param>
        /// <param name="method">Method name</param>
        /// <returns>Returns this</returns>
        public Entry Locate( string file, int line, int column, string method )
        {
            m_File = file;
            m_Line = line;
            m_Column = column;
            m_Method = method;
            m_CachedString = null; // Forces cached string to be regenerated by the next ToString() call
            return this;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Converts this entry to a string. Does not include time or ID. String is cached, so subsequent calls are free.
        /// </summary>
        /// <returns>String representation of this entry</returns>
        public override string ToString( )
        {
            if ( m_CachedString == null )
            {
                m_CachedString = string.Format( "{0}({1},{2}):<{5}>{6}[{3}->{4}]", File, Line, Column, Thread, Method, Source, Message );
            }
            return m_CachedString;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the severity of this entry (shortcut to get the source's severity)
        /// </summary>
        public Severity Severity
        {
            get { return m_Source.Severity; }
        }

        /// <summary>
        /// Time that the entry was generated
        /// </summary>
        public TimeSpan Time
        {
            get { return m_Time; }
        }

        /// <summary>
        /// Name of the thread that generated the entry
        /// </summary>
        public string Thread
        {
            get { return m_ThreadName; }
        }

        /// <summary>
        /// The source of the entry
        /// </summary>
        public Source Source
        {
            get { return m_Source; }
        }

        /// <summary>
        /// Unique entry identifier
        /// </summary>
        public uint Id
        {
            get { return m_Id; }
        }

        /// <summary>
        /// Entry location filename
        /// </summary>
        public string File
        {
            get { return m_File;  }
        }

        /// <summary>
        /// Entry location line number
        /// </summary>
        public int Line
        {
            get { return m_Line;  }
        }

        /// <summary>
        /// Entry location column
        /// </summary>
        public int Column
        {
            get { return m_Column; }
        }

        /// <summary>
        /// Entry location method
        /// </summary>
        public string Method
        {
            get { return m_Method; }
        }

        /// <summary>
        /// Entry message
        /// </summary>
        public string Message
        {
            get { return m_Message; }
        }

        #endregion
        
        #region Private stuff

        private Source                  m_Source;
        private string                  m_Message;
        private string                  m_ThreadName;
        private TimeSpan                m_Time;
        private uint                    m_Id;
        private string                  m_File;
        private int                     m_Line;
        private int                     m_Column;
        private string                  m_Method;
        private string                  m_CachedString;
        private static volatile uint    m_MessageId;

        #endregion
    }
}
