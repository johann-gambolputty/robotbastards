using System;
using System.Diagnostics;

namespace RbEngine
{
	/// <summary>
	/// Debug switch. Output through this switch is disabled completely in Release builds
	/// </summary>
	/// <seealso cref="Output">Output</seealso>
	public class DebugSwitch : BooleanSwitch
	{
		/// <summary>
		/// Sets up the base BooleanSwitch name and description
		/// </summary>
		public DebugSwitch( string name, string description ) :
			base( name, description )
		{
		}
	}

	/// <summary>
	/// Trace switch. Output through this switch is always enabled (maybe not in Final builds?)
	/// </summary>
	/// <seealso cref="Output">Output</seealso>
	public class TraceSwitch : BooleanSwitch
	{
		/// <summary>
		/// Sets up the base BooleanSwitch name and description
		/// </summary>
		public TraceSwitch( string name, string description ) :
			base( name, description )
		{
		}
	}

	/// <summary>
	/// Helper class for diagnostic output
	/// </summary>
	/// <example>
	/// This code:
	/// <code>
	/// Output.WriteCall( Output.RenderingInfo, "Rendered stuff" );
	/// </code>	
	/// Writes this output (using System.Diagnostics.Debug):
	/// <code>
	/// MyClass.cs(20) : [renderInfo] Rendered stuff
	/// </code>
	/// </example>
	public class Output
	{
		#region	Common switches

		/// <summary>
		/// Switch for generic information. Disabled in non-DEBUG builds
		/// </summary>
		public static DebugSwitch		Info		= new DebugSwitch( "info", "Generic information diagnostic output" );

		/// <summary>
		/// Switch for generic warnings
		/// </summary>
		public static TraceSwitch		Warning 	= new TraceSwitch( "warn", "Generic warning diagnostic output" );

		/// <summary>
		/// Switch for generic errors
		/// </summary>
		public static TraceSwitch		Error 		= new TraceSwitch( "error", "Generic error diagnostic output" );

		#endregion

		#region	Common rendering switches

		/// <summary>
		/// Switch for rendering information. Disabled in non-DEBUG builds
		/// </summary>
		public static DebugSwitch		RenderingInfo		= new DebugSwitch( "renderInfo", "Rendering information diagnostic output" );

		/// <summary>
		/// Switch for rendering warnings
		/// </summary>
		public static TraceSwitch		RenderingWarning 	= new TraceSwitch( "renderWarn", "Rendering warning diagnostic output" );

		/// <summary>
		/// Switch for rendering errors
		/// </summary>
		public static TraceSwitch		RenderingError 		= new TraceSwitch( "renderError", "Rendering error diagnostic output" );

		#endregion

		#region	Common resource switches

		/// <summary>
		/// Switch for resource information. Disabled in non-DEBUG builds
		/// </summary>
		public static DebugSwitch		ResourceInfo		= new DebugSwitch( "resourceInfo", "Resource information diagnostic output" );

		/// <summary>
		/// Switch for resource warnings
		/// </summary>
		public static TraceSwitch		ResourceWarning 	= new TraceSwitch( "resourceWarn", "Resource warning diagnostic output" );

		/// <summary>
		/// Switch for resource errors
		/// </summary>
		public static TraceSwitch		ResourceError 		= new TraceSwitch( "resourceError", "Resource error diagnostic output" );

		#endregion

		#region	Common component switches

		/// <summary>
		/// Switch for component information. Disabled in non-DEBUG builds
		/// </summary>
		public static DebugSwitch		ComponentInfo		= new DebugSwitch( "componentInfo", "Component information diagnostic output" );

		/// <summary>
		/// Switch for component warnings
		/// </summary>
		public static TraceSwitch		ComponentWarning 	= new TraceSwitch( "componentWarn", "Component warning diagnostic output" );

		/// <summary>
		/// Switch for component errors
		/// </summary>
		public static TraceSwitch		ComponentError 		= new TraceSwitch( "componentError", "Component error diagnostic output" );

		#endregion

		#region	Common scene switches

		/// <summary>
		/// Switch for scene information. Disabled in non-DEBUG builds
		/// </summary>
		public static DebugSwitch		SceneInfo			= new DebugSwitch( "sceneInfo", "Scene information diagnostic output" );

		/// <summary>
		/// Switch for scene warnings
		/// </summary>
		public static TraceSwitch		SceneWarning		= new TraceSwitch( "sceneWarn", "Scene warning diagnostic output" );

		/// <summary>
		/// Switch for scene errors
		/// </summary>
		public static TraceSwitch		SceneError	 		= new TraceSwitch( "sceneError", "Scene error diagnostic output" );

		#endregion

		#region	Common networking switches

		/// <summary>
		/// Switch for networking information. Disabled in non-DEBUG builds
		/// </summary>
		public static DebugSwitch		NetworkInfo			= new DebugSwitch( "networkInfo", "Network information diagnostic output" );

		/// <summary>
		/// Switch for networking warnings
		/// </summary>
		public static TraceSwitch		NetworkWarning 		= new TraceSwitch( "networkWarn", "Network warning diagnostic output" );

		/// <summary>
		/// Switch for networking errors
		/// </summary>
		public static TraceSwitch		NetworkError 		= new TraceSwitch( "networkError", "Network error diagnostic output" );

		#endregion

		#region	Common input switches

		/// <summary>
		/// Switch for input information. Disabled in non-DEBUG builds
		/// </summary>
		public static DebugSwitch		InputInfo			= new DebugSwitch( "inputInfo", "Input information diagnostic output" );

		/// <summary>
		/// Switch for input warnings
		/// </summary>
		public static TraceSwitch		InputWarning		= new TraceSwitch( "inputWarn", "Input warning diagnostic output" );

		/// <summary>
		/// Switch for input errors
		/// </summary>
		public static TraceSwitch		InputError	 		= new TraceSwitch( "inputError", "Input error diagnostic output" );

		#endregion

		#region	Trace switch outputs

		/// <summary>
		/// Writes a message using System.Diagnostics.Trace.Write() (if this context is enabled), prefixed by the caller source location and the context name
		/// </summary>
		/// <param name="context"> Output context </param>
		/// <param name="str"> Message to write </param>
		/// <param name="formatArgs"> String format arguments, passed to String.Format() with str to create the output message </param>
		public static void		WriteCall( TraceSwitch context, string str, params object[] formatArgs )
		{
			if ( context.Enabled )
			{
				StackFrame caller = new StackFrame( 1, true );
				System.Diagnostics.Trace.Write( String.Format( "{0}({1},{2}) : [{3}]{4}", caller.GetFileName( ), caller.GetFileLineNumber( ), caller.GetFileColumnNumber( ), context.DisplayName, String.Format( str, formatArgs ) ) );
			}
		}

		/// <summary>
		/// Writes a message using System.Diagnostics.Trace.WriteLine() (if this context is enabled), prefixed by the caller source location and the context name
		/// </summary>
		/// <param name="context"> Output context </param>
		/// <param name="str"> Message to write </param>
		/// <param name="formatArgs"> String format arguments, passed to String.Format() with str to create the output message </param>
		public static void		WriteLineCall( TraceSwitch context, string str, params object[] formatArgs )
		{
			if ( context.Enabled )
			{
				StackFrame caller = new StackFrame( 1, true );
				System.Diagnostics.Trace.WriteLine( String.Format( "{0}({1},{2}) : [{3}]{4}", caller.GetFileName( ), caller.GetFileLineNumber( ), caller.GetFileColumnNumber( ), context.DisplayName, String.Format( str, formatArgs ) ) );
			}
		}

		/// <summary>
		/// Writes a message using System.Diagnostics.Trace.Write() (if this context is enabled), prefixed by the context name
		/// </summary>
		/// <param name="context"> Output context </param>
		/// <param name="str"> Message to write </param>
		/// <param name="formatArgs"> String format arguments, passed to String.Format() with str to create the output message </param>
		public static void		Write( TraceSwitch context, string str, params object[] formatArgs )
		{
			System.Diagnostics.Trace.WriteIf( context.Enabled, String.Format( str, formatArgs ), context.DisplayName );
		}

		/// <summary>
		/// Writes a message using System.Diagnostics.Trace.WriteLine() (if this context is enabled), prefixed by the context name
		/// </summary>
		/// <param name="context"> Output context </param>
		/// <param name="str"> Message to write </param>
		/// <param name="formatArgs"> String format arguments, passed to String.Format() with str to create the output message </param>
		public static void		WriteLine( TraceSwitch context, string str, params object[] formatArgs )
		{
			System.Diagnostics.Trace.WriteLineIf( context.Enabled, String.Format( str, formatArgs ), context.DisplayName );
		}


		/// <summary>
		/// Assert wrapper
		/// </summary>
		public static void		Assert( bool condition, TraceSwitch context, string str, params object[] formatArgs )
		{
			System.Diagnostics.Trace.Assert( condition, String.Format( "[{0}]", context.DisplayName ) + String.Format( str, formatArgs ) );
		}

		/// <summary>
		/// Fail wrapper
		/// </summary>
		public static void		Fail( TraceSwitch context, string str, params object[] formatArgs )
		{
			System.Diagnostics.Debug.Fail( String.Format( "[{0}]", context.DisplayName ) + String.Format( str, formatArgs ) );
		}

		#endregion

		#region	Debug switch outputs

		/// <summary>
		/// Writes a message using System.Diagnostics.Debug.Write() (if this context is enabled), prefixed by the caller source location and the context name
		/// </summary>
		/// <param name="context"> Output context </param>
		/// <param name="str"> Message to write </param>
		/// <param name="formatArgs"> String format arguments, passed to String.Format() with str to create the output message </param>
		[ Conditional( "DEBUG" ) ]
		public static void		WriteCall( DebugSwitch context, string str, params object[] formatArgs )
		{
			if ( context.Enabled )
			{
				StackFrame caller = new StackFrame( 1, true );
				System.Diagnostics.Debug.Write( String.Format( "{0}({1},{2}) : [{3}]{4}", caller.GetFileName( ), caller.GetFileLineNumber( ), caller.GetFileColumnNumber( ), context.DisplayName, String.Format( str, formatArgs ) ) );
			}
		}

		/// <summary>
		/// Writes a message using System.Diagnostics.Debug.WriteLine() (if this context is enabled), prefixed by the caller source location and the context name
		/// </summary>
		/// <param name="context"> Output context </param>
		/// <param name="str"> Message to write </param>
		/// <param name="formatArgs"> String format arguments, passed to String.Format() with str to create the output message </param>
		[ Conditional( "DEBUG" ) ]
		public static void		WriteLineCall( DebugSwitch context, string str, params object[] formatArgs )
		{
			if ( context.Enabled )
			{
				StackFrame caller = new StackFrame( 1, true );
				System.Diagnostics.Debug.WriteLine( String.Format( "{0}({1},{2}) : [{3}]{4}", caller.GetFileName( ), caller.GetFileLineNumber( ), caller.GetFileColumnNumber( ), context.DisplayName, String.Format( str, formatArgs ) ) );
			}
		}

		/// <summary>
		/// Writes a message using System.Diagnostics.Debug.Write() (if this context is enabled), prefixed by the context name
		/// </summary>
		/// <param name="context"> Output context </param>
		/// <param name="str"> Message to write </param>
		/// <param name="formatArgs"> String format arguments, passed to String.Format() with str to create the output message </param>
		[ Conditional( "DEBUG" ) ]
		public static void		Write( DebugSwitch context, string str, params object[] formatArgs )
		{
			System.Diagnostics.Debug.WriteIf( context.Enabled, String.Format( str, formatArgs ), context.DisplayName );
		}

		/// <summary>
		/// Writes a message using System.Diagnostics.Debug.WriteLine() (if this context is enabled), prefixed by the context name
		/// </summary>
		/// <param name="context"> Output context </param>
		/// <param name="str"> Message to write </param>
		/// <param name="formatArgs"> String format arguments, passed to String.Format() with str to create the output message </param>
		[ Conditional( "DEBUG" ) ]
		public static void		WriteLine( DebugSwitch context, string str, params object[] formatArgs )
		{
			System.Diagnostics.Debug.WriteLineIf( context.Enabled, String.Format( str, formatArgs ), context.DisplayName );
		}

		/// <summary>
		/// Assert wrapper
		/// </summary>
		[ Conditional( "DEBUG" ) ]
		public static void		Assert( bool condition, DebugSwitch context, string str, params object[] formatArgs )
		{
			System.Diagnostics.Debug.Assert( condition, String.Format( "[{0}] : ", context.DisplayName ) + String.Format( str, formatArgs ) );
		}

		/// <summary>
		/// Fail wrapper
		/// </summary>
		[ Conditional( "DEBUG" ) ]
		public static void		Fail( DebugSwitch context, string str, params object[] formatArgs )
		{
			System.Diagnostics.Debug.Fail( String.Format( "[{0}] : ", context.DisplayName ) + String.Format( str, formatArgs ) );
		}

		#endregion

		#region	Debug asserts

		//	NOTE: This is to allow System.Diagnostics.Debug.Assert() to be called with TraceSwitch contexts
		//	e.g. Output.DebugAssert( condition, Output.Error, "Some error check" );

		/// <summary>
		/// Debug.Assert wrapper
		/// </summary>
		[ Conditional( "DEBUG" ) ]
		public static void	DebugAssert( bool condition, DebugSwitch context, string str, params object[] formatArgs )
		{
			System.Diagnostics.Debug.Assert( condition, String.Format( "[{0}] : ", context.DisplayName ) + String.Format( str, formatArgs ) );
		}

		/// <summary>
		/// Debug.Assert wrapper
		/// </summary>
		[ Conditional( "DEBUG" ) ]
		public static void	DebugAssert( bool condition, TraceSwitch context, string str, params object[] formatArgs )
		{
			System.Diagnostics.Debug.Assert( condition, String.Format( "[{0}] : ", context.DisplayName ) + String.Format( str, formatArgs ) );
		}

		#endregion
	}
}
