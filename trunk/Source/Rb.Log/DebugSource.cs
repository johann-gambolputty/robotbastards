using System.Diagnostics;

namespace Rb.Log
{
    /// <summary>
    /// Output from a debug source will be automatically disabled in non-DEBUG builds
    /// </summary>
    public class DebugSource : Source
    {
        #region Public construction

		/// <summary>
		/// Sets up the tag and severity of this source
		/// </summary>
		/// <param name="parent">Parent tag</param>
		/// <param name="severity">Source severity</param>
		public DebugSource( Tag parent, Severity severity ) :
			base( parent, severity )
		{
		}

        #endregion

        #region Output

        /// <summary>
        /// Generates a log entry
        /// </summary>
        /// <param name="msg">Message string</param>
        /// <param name="args">Format arguments</param>
        [ ConditionalAttribute( "DEBUG" )]
        public new void Write( string msg, params object[] args )
        {
            base.Write( 2, msg, args );
        }

		/// <summary>
		/// Generates a log entry. Skips a given number of stack frames to get the location
		/// </summary>
		/// <param name="skip">Number of frames to skip</param>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		[ConditionalAttribute( "DEBUG" )]
		public new void Write( int skip, string msg, params object[ ] args )
		{
			base.Write( skip + 1, msg, args );
		}

        #endregion
   
    }
}
