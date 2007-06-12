using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Log
{
    /// <summary>
    /// Contains handy generic log sources
    /// </summary>
    /// <example>
    /// Rb.Log.App.DebugVerbose( "useless information, removed in non-DEBUG builds" );
	/// Rb.Log.App.DebugInfo( "useful information, removed in non-DEBUG builds" );
    /// 
    /// Rb.Log.App.Verbose( "useless information" );
    /// Rb.Log.App.Info( "useful information" );
    /// Rb.Log.App.Warning( "warnings" );
    /// Rb.Log.App.Error( "errors" );
    /// </example>
    public class App : StaticTag< App >
    {
		public override string TagName
		{
			get { return "App"; }
		}
    }
}
