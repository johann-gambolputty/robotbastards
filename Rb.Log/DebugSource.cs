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
        /// Sets up the source object. Source name is the severity
        /// </summary>
        /// <param name="parent">The parent of the source</param>
        /// <param name="severity">Source severity</param>
        public DebugSource( Source parent, Severity severity ) :
            base( parent, severity )
        {
            
        }

        /// <summary>
        /// Sets up the source's name. Parent is the Root source
        /// </summary>
        /// <param name="name">Source name</param>
        /// <param name="severity">Source severity</param>
        public DebugSource( string name, Severity severity ) :
            base( name, severity )
        {
        }

        /// <summary>
        /// Sets up the source's parent and name
        /// </summary>
        /// <param name="parent">Source parent</param>
        /// <param name="name">Source name</param>
        /// <param name="severity">Source severity</param>
        public DebugSource( Source parent, string name, Severity severity ) :
            base( parent, name, severity )
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
