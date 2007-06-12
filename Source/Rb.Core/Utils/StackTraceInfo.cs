using System.Text.RegularExpressions;

namespace Rb.Core.Utils
{
    /// <summary>
    /// Breaks up a stack trace string (from Exception.StackTrace or Environment.StackTrace) into component parts
    /// </summary>
    public class StackTraceInfo
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="trace">Stack trace string</param>
        public StackTraceInfo( string trace )
        {
            m_Match = ms_StackTraceExp.Match( trace );
        }

        /// <summary>
        /// File at current match
        /// </summary>
        public string File
        {
            get { return m_Match.Groups[ 2 ].Value; }
        }

        /// <summary>
        /// Line at current match
        /// </summary>
        public string Line
        {
            get { return m_Match.Groups[ 3 ].Value; }
        }

        /// <summary>
        /// Method at current match
        /// </summary>
        public string Method
        {
            get { return m_Match.Groups[ 1 ].Value; }
        }

        /// <summary>
        /// true if the info has some stack information
        /// </summary>
        public bool HasMatch
        {
            get { return m_Match.Success; }
        }

        /// <summary>
        /// Moves to the next entry in the stack trace
        /// </summary>
        public void NextMatch( )
        {
            m_Match = m_Match.NextMatch( );
        }

        #region Private stuff

        private Match m_Match;

        /// <summary>
        /// Stack trace regular expression. Usually in the format " at X in Y: line Z", where X is the function name, Y is the filename, Z is the line number
        /// </summary>
        private static Regex ms_StackTraceExp = new Regex( @" at (.+) in (.+)\:line ([0-9]+)" );

        #endregion
    }
}
