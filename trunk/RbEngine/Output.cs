using System;
using System.Diagnostics;

namespace RbEngine
{
	/// <summary>
	/// Debug switch. Output through this switch is disabled completely in Release builds
	/// </summary>
	/// <seealso>Output</seealso>
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
	/// <seealso>Output</seealso>
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

		#region	Trace switch outputs

		/// <summary>
		/// Writes a message using System.Diagnostics.Trace.Write() (if this context is enabled), prefixed by the caller source location and the context name
		/// </summary>
		/// <param name="str"> Message to write </param>
		public static void		WriteCall( TraceSwitch context, string str )
		{
			if ( context.Enabled )
			{
				StackFrame caller = new StackFrame( 1, true );
				System.Diagnostics.Trace.Write( String.Format( "{0}({1},{2}) : [{3}]{4}", caller.GetFileName( ), caller.GetFileLineNumber( ), caller.GetFileColumnNumber( ), context.DisplayName, str ) );
			}
		}

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
		/// <param name="str"> Message to write </param>
		public static void		WriteLineCall( TraceSwitch context, string str )
		{
			if ( context.Enabled )
			{
				StackFrame caller = new StackFrame( 1, true );
				System.Diagnostics.Trace.WriteLine( String.Format( "{0}({1},{2}) : [{3}]{4}", caller.GetFileName( ), caller.GetFileLineNumber( ), caller.GetFileColumnNumber( ), context.DisplayName, str ) );
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
		/// <param name="str"> Message to write </param>
		public static void		Write( TraceSwitch context, string str )
		{
			System.Diagnostics.Trace.WriteIf( context.Enabled, str, context.DisplayName );
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
		/// <param name="str"> Message to write </param>
		public static void		WriteLine( TraceSwitch context, string str )
		{
			System.Diagnostics.Trace.WriteIf( context.Enabled, str, context.DisplayName );
		}

		/// <summary>
		/// Writes a message using System.Diagnostics.Trace.WriteLine() (if this context is enabled), prefixed by the context name
		/// </summary>
		/// <param name="context"> Output context </param>
		/// <param name="str"> Message to write </param>
		/// <param name="formatArgs"> String format arguments, passed to String.Format() with str to create the output message </param>
		public static void		WriteLine( TraceSwitch context, string str, params object[] formatArgs )
		{
			System.Diagnostics.Trace.WriteIf( context.Enabled, String.Format( str, formatArgs ), context.DisplayName );
		}

		#endregion

		#region	Debug switch outputs

		/// <summary>
		/// Writes a message using System.Diagnostics.Debug.Write() (if this context is enabled), prefixed by the caller source location and the context name
		/// </summary>
		/// <param name="str"> Message to write </param>
		public static void		WriteCall( DebugSwitch context, string str )
		{
			if ( context.Enabled )
			{
				StackFrame caller = new StackFrame( 1, true );
				System.Diagnostics.Debug.Write( String.Format( "{0}({1},{2}) : [{3}]{4}", caller.GetFileName( ), caller.GetFileLineNumber( ), caller.GetFileColumnNumber( ), context.DisplayName, str ) );
			}
		}

		/// <summary>
		/// Writes a message using System.Diagnostics.Debug.Write() (if this context is enabled), prefixed by the caller source location and the context name
		/// </summary>
		/// <param name="context"> Output context </param>
		/// <param name="str"> Message to write </param>
		/// <param name="formatArgs"> String format arguments, passed to String.Format() with str to create the output message </param>
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
		/// <param name="str"> Message to write </param>
		public static void		WriteLineCall( DebugSwitch context, string str )
		{
			if ( context.Enabled )
			{
				StackFrame caller = new StackFrame( 1, true );
				System.Diagnostics.Debug.WriteLine( String.Format( "{0}({1},{2}) : [{3}]{4}", caller.GetFileName( ), caller.GetFileLineNumber( ), caller.GetFileColumnNumber( ), context.DisplayName, str ) );
			}
		}
		

		/// <summary>
		/// Writes a message using System.Diagnostics.Debug.WriteLine() (if this context is enabled), prefixed by the caller source location and the context name
		/// </summary>
		/// <param name="context"> Output context </param>
		/// <param name="str"> Message to write </param>
		/// <param name="formatArgs"> String format arguments, passed to String.Format() with str to create the output message </param>
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
		/// <param name="str"> Message to write </param>
		public static void		Write( DebugSwitch context, string str )
		{
			System.Diagnostics.Debug.WriteIf( context.Enabled, str, context.DisplayName );
		}
		
		/// <summary>
		/// Writes a message using System.Diagnostics.Debug.Write() (if this context is enabled), prefixed by the context name
		/// </summary>
		/// <param name="context"> Output context </param>
		/// <param name="str"> Message to write </param>
		/// <param name="formatArgs"> String format arguments, passed to String.Format() with str to create the output message </param>
		public static void		Write( DebugSwitch context, string str, params object[] formatArgs )
		{
			System.Diagnostics.Debug.WriteIf( context.Enabled, String.Format( str, formatArgs ), context.DisplayName );
		}

		/// <summary>
		/// Writes a message using System.Diagnostics.Debug.WriteLine() (if this context is enabled), prefixed by the context name
		/// </summary>
		/// <param name="str"> Message to write </param>
		public static void		WriteLine( DebugSwitch context, string str )
		{
			System.Diagnostics.Debug.WriteIf( context.Enabled, str, context.DisplayName );
		}

		/// <summary>
		/// Writes a message using System.Diagnostics.Debug.WriteLine() (if this context is enabled), prefixed by the context name
		/// </summary>
		/// <param name="context"> Output context </param>
		/// <param name="str"> Message to write </param>
		/// <param name="formatArgs"> String format arguments, passed to String.Format() with str to create the output message </param>
		public static void		WriteLine( DebugSwitch context, string str, params object[] formatArgs )
		{
			System.Diagnostics.Debug.WriteIf( context.Enabled, String.Format( str, formatArgs ), context.DisplayName );
		}

		#endregion
	}
}
