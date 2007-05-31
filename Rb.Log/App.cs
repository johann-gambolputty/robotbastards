using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Log
{
    /// <summary>
    /// Contains handy generic entries
    /// </summary>
    /// <example>
    /// Rb.Log.App.DbgVerbose.Write( "useless information, removed in non-DEBUG builds" );
    /// Rb.Log.App.DbgInfo.Write( "useful information, removed in non-DEBUG builds" );
    /// 
    /// Rb.Log.App.Verbose.Write( "useless information" );
    /// Rb.Log.App.Info.Write( "useful information" );
    /// Rb.Log.App.Warning.Write( "warnings" );
    /// Rb.Log.App.Error.Write( "errors" );
    /// </example>
    public static class App
    {
        public static readonly Source       Root        = new Source( "App", Severity.Verbose );
        public static readonly DebugSource  DbgVerbose  = new DebugSource( Root, Severity.Verbose );
        public static readonly DebugSource  DbgInfo     = new DebugSource( Root, Severity.Info );
        public static readonly Source       Verbose     = new Source( Root, Severity.Verbose );
        public static readonly Source       Info        = new Source( Root, Severity.Info );
        public static readonly Source       Warning     = new Source( Root, Severity.Warning );
        public static readonly Source       Error       = new Source( Root, Severity.Error );
    }
}
