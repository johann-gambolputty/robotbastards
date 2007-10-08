
namespace Rb.Log
{
    /// <summary>
    /// Contains handy generic log sources
    /// </summary>
    /// <example>
	/// AppLog.DebugVerbose( "useless information, removed in non-DEBUG builds" );
	/// AppLog.DebugInfo( "useful information, removed in non-DEBUG builds" );
	/// 
	/// AppLog.Exception( ex, "exception information, written to error source" );
	/// AppLog.Verbose( "useless information" );
	/// AppLog.Info( "useful information" );
	/// AppLog.Warning( "warnings" );
	/// AppLog.Error( "errors" );
    /// </example>
    public class AppLog : StaticTag< AppLog >
    {
		public override string TagName
		{
			get { return "App"; }
		}
    }
}
