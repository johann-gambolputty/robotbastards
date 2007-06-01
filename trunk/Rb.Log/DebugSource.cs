using System;
using System.Collections.Generic;
using System.Text;
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
            base.Write( msg, args );
        }

        #endregion
   
    }
}
