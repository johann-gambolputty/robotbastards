using System;

namespace RbTestApp
{
	/// <summary>
	/// Stores contexts to pass to Output.WriteLine(), Output.Write(), etc. family of functions
	/// </summary>
	/// <example>
	/// <code lang="C#">
	/// RbEngine.Output.WriteLine( TestAppOutput.Info, "Started  application" );	// Writes "[testAppInfo] Started application"
	/// RbEngine.Output.WriteLineCall( TestAppOutput.Error, "Error occurred" );		// Writes "file.cs(1) : [testAppInfo] Error occurred"
	/// </code>
	/// </example>
	public class TestAppOutput
	{
		/// <summary>
		/// Information output context
		/// </summary>
		public static RbEngine.DebugSwitch	Info	= new RbEngine.DebugSwitch( "testAppInfo",	"Test application information diagnostic output" );

		/// <summary>
		/// Warning output context
		/// </summary>
		public static RbEngine.TraceSwitch	Warning	= new RbEngine.TraceSwitch( "testAppWarn",	"Test application warning diagnostic output" );

		/// <summary>
		/// Error output context
		/// </summary>
		public static RbEngine.TraceSwitch	Error	= new RbEngine.TraceSwitch( "testAppError",	"Test application error diagnostic output" );
	}
}
