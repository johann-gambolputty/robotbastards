using System;
using System.Diagnostics;
using System.Collections;
using System.Text;

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
        /// Sets up the source object. Source name is the severity
        /// </summary>
        /// <param name="parent">The parent of the source</param>
        /// <param name="severity">Source severity</param>
        public Source( Source parent, Severity severity )
        {
            Construct( parent, severity.ToString( ), severity );
        }

        /// <summary>
        /// Sets up the source object
        /// </summary>
        /// <param name="parent">The parent of the source</param>
        /// <param name="name">Source name</param>
        /// <param name="severity">Source severity</param>
        public Source( Source parent, string name, Severity severity )
        {
            Construct( parent, name, severity );
        }

        /// <summary>
        /// Sets up this source object. Source parent is the Root source
        /// </summary>
        /// <param name="name">Source name</param>
        /// <param name="severity">Source severity</param>
        public Source( string name, Severity severity )
        {
            Construct( Root, name, severity );
        }

        /// <summary>
        /// Sets up this source object. Source parent is the Root source, source name is the severity string
        /// </summary>
        /// <param name="severity">Source severity</param>
        public Source( Severity severity )
        {
            Construct( Root, severity.ToString( ), severity );
        }

        /// <summary>
        /// Construction helper
        /// </summary>
        private void Construct( Source parent, string name, Severity severity )
        {
            m_Name = name;
            m_Parent = parent;
            m_FullName = (parent.IsRoot) ? m_Name : parent.FullName + "." + m_Name;
            m_Severity = severity;
            m_Parent.Children.Add(this);
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
        /// Gets the collection of child sources
        /// </summary>
        public ArrayList Children
        {
            get { return m_Children; }
        }

        /// <summary>
        /// Gets the name of this source
        /// </summary>
        public string Name
        {
            get { return m_Name; }
        }

        /// <summary>
        /// Gets the full name of this source (name, prefixed by parent names, delimited by full stops)
        /// </summary>
        public string FullName
        {
            get { return m_FullName; }
        }

        /// <summary>
        /// Gets the parent source (can return null if this is the root source)
        /// </summary>
        public Source Parent
        {
            get { return m_Parent; }
        }

        /// <summary>
        /// Suppresses or enables log entries written from this and all child sources
        /// </summary>
        public bool Suppress
        {
            get { return m_Suppress; }
            set
            {
                m_Suppress = value;
                foreach ( Source childSource in m_Children )
                {
                    childSource.Suppress = value;
                }
            }
        }

        /// <summary>
        /// Gets the root source
        /// </summary>
        public static Source Root
        {
            get { return ms_Root; }
        }

        /// <summary>
        /// Returns true if this is the root source
        /// </summary>
        public bool IsRoot
        {
            get { return m_Parent == null; }
        }


        #endregion

        #region Public methods

        /// <summary>
        /// Finds a named child Source
        /// </summary>
        /// <param name="name">Child Source name</param>
        /// <returns>Returns null if no child with the specified name is found. Otherwise, returns named child</returns>
        public Source FindChild( string name )
        {
            foreach ( Source curChild in m_Children )
            {
                if ( curChild.Name == name )
                {
                    return curChild;
                }
            }
            return null;
        }

        /// <summary>
        /// Converts this source to a string
        /// </summary>
        /// <returns>String name</returns>
        public override string ToString( )
        {
            return FullName;
        }

        #endregion

        #region Output

        /// <summary>
        /// Generates a log entry
        /// </summary>
        /// <param name="msg">Message string</param>
        /// <param name="args">Format arguments</param>
        public void Write( string msg, params object[] args )
        {
            if ( !m_Suppress )
            {
                //  Create a new log entry
                Entry newEntry = new Entry( this, string.Format( msg, args ) ).Locate( 2 );

                System.Diagnostics.Trace.WriteLine( newEntry.ToString( ) );

                if ( OnNewLogEntry != null )
                {
                    OnNewLogEntry( newEntry );
                }
            }
        }

        /// <summary>
        /// Invoked when a new log entry is created
        /// </summary>
        public static event OnNewLogEntryDelegate OnNewLogEntry;

        #endregion
   
        #region Private stuff

        private ArrayList       m_Children = new ArrayList( );
        private Source          m_Parent;
        private string          m_Name;
        private string          m_FullName;
        private static Source   ms_Root = new Source( );
        private bool            m_Suppress;
        private Severity        m_Severity = Severity.Verbose;

        /// <summary>
        /// Root source constructor
        /// </summary>
        private Source( )
        {
            m_Name = "";
        }

        #endregion
    }
}
