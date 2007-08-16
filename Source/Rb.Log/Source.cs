using System;

namespace Rb.Log
{
    /// <summary>
    /// Delegate type, used by Source.OnNewLogEntry
    /// </summary>
    /// <param name="entry">Entry data</param>
    public delegate void OnNewLogEntryDelegate( Entry entry );

    /// <summary>
    /// Identifies a source of log output
    /// </summary>
    public class Source
    {
        #region Public construction

        /// <summary>
        /// Sets up the source object
        /// </summary>
        /// <param name="parent">The parent of the source</param>
        /// <param name="severity">Source severity</param>
        public Source( Tag parent, Severity severity )
        {
			m_Severity = severity;
			m_Name = severity.ToString( );
			m_FullName = ( parent.IsRootTag ? m_Name : parent.FullName + "." + m_Name );
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the severity of the source
        /// </summary>
        public Severity Severity
        {
            get { return m_Severity; }
        }

        /// <summary>
        /// Gets the name of this source
        /// </summary>
        public string Name
        {
            get { return m_Name; }
        }

        /// <summary>
        /// Gets the full name of this source (name, prefixed by parent tag names, delimited by full stops)
        /// </summary>
        public string FullName
        {
            get { return m_FullName; }
        }

        /// <summary>
        /// Suppresses or enables log entries written from this source
        /// </summary>
        public bool Suppress
        {
            get { return m_Suppress; }
            set { m_Suppress = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Builds or finds a source from a '.' seperated string
        /// </summary>
        /// <param name="str">Tags and source string</param>
        /// <returns>New/found source</returns>
        public static Source BuildFromString( string str )
        {
            string[] sepStr = str.Split( new char[] { '.' } );

            Tag curTag = Tag.Root;
            for ( int strIndex = 0; strIndex < sepStr.Length - 1; ++strIndex )
            {
                Tag childTag = curTag.FindChildTag( sepStr[ strIndex ] );
                if ( childTag == null )
                {
                    childTag = new Tag( curTag, sepStr[ strIndex ] );
                }
                curTag = childTag;
            }

            Severity severity = ( Severity )Enum.Parse( typeof( Severity ), sepStr[ sepStr.Length - 1 ] );
            return curTag.GetSource( severity );
        }

        /// <summary>
        /// Converts this source to a string
        /// </summary>
        /// <returns>String name</returns>
        public override string ToString()
        {
            return FullName;
        }

        #endregion

        #region Output

		/// <summary>
		/// Asserts that a condition is true
		/// </summary>
		/// <param name="condition">Condition to test</param>
		/// <param name="msg">Message on failure</param>
		/// <param name="args">Message format parameters</param>
		public void Assert( bool condition, string msg, params object[] args )
		{
			//	TODO: AP: ...
			System.Diagnostics.Trace.Assert( condition, string.Format( msg, args ) );
		}

		/// <summary>
		/// Fails, with a given message
		/// </summary>
		/// <param name="msg">Message</param>
		/// <param name="args">Message format parameters</param>
		public void Fail( string msg, params object[] args )
		{
			//	TODO: AP: ...
			System.Diagnostics.Trace.Fail( string.Format( msg, args ) );
		}


		/// <summary>
		/// Generates a log entry for an exception
		/// </summary>
		/// <param name="ex">Exception</param>
		public void Write( Exception ex )
		{
			//	TODO: AP: ...
		}

    	/// <summary>
        /// Generates a log entry
        /// </summary>
        /// <param name="msg">Message string</param>
        /// <param name="args">Format arguments</param>
        public void Write( string msg, params object[] args )
        {
			Write( 1, msg, args );
        }

		/// <summary>
		/// Generates a log entry, with a specified stack frame offset as location
		/// </summary>
		/// <param name="skip">Number of stack frames to skip to get the location</param>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		public void Write( int skip, string msg, params object[ ] args )
		{
			if ( !m_Suppress )
			{
				//  Create a new log entry
				//	(skip gets 2 extra to cope with the Locate() call, and this call)
				Entry newEntry = new Entry( this, string.Format( msg, args ) ).Locate( skip + 2 );

                HandleEntry( newEntry );
			}
		}

        /// <summary>
        /// Handles a new log entry
        /// </summary>
        /// <param name="entry">New log entry</param>
        public static void HandleEntry( Entry entry )
        {
			System.Diagnostics.Trace.WriteLine( entry.ToString( ) );

			if ( OnNewLogEntry != null )
			{
				OnNewLogEntry( entry );
			}
        }

        /// <summary>
        /// Invoked when a new log entry is created
        /// </summary>
        public static event OnNewLogEntryDelegate OnNewLogEntry;

        #endregion

		#region Output helpers

		/// <summary>
		/// Writes common environment variables to the log
		/// </summary>
		public void WriteEnvironment( )
		{
			Write( "Working Directory: {0}", Environment.CurrentDirectory );
			Write( "Machine name: {0}", Environment.MachineName );
			Write( "OS Version: {0}", Environment.OSVersion );
			Write( "Processors: {0}", Environment.ProcessorCount );
			Write( "Username: {0}", Environment.UserName );
			Write( "CLR Version: {0}", Environment.Version );
		}

		#endregion

		#region Private stuff

		private string          m_Name;
        private string          m_FullName;
        private bool            m_Suppress;
        private Severity        m_Severity = Severity.Verbose;

        #endregion
    }
}
