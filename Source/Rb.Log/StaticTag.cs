using System;
using System.Diagnostics;

namespace Rb.Log
{
	/// <summary>
	/// Static tag interface
	/// </summary>
	public interface IStaticTag
	{
		/// <summary>
		/// Gets the parent log tag
		/// </summary>
		Tag ParentTag { get; }

		/// <summary>
		/// Gets the tag name
		/// </summary>
		string TagName { get; }
	}

	/// <summary>
	/// Helper class to build a globally available tag and sources
	/// </summary>
	/// <typeparam name="DerivedType">The derived type</typeparam>
	/// <example>
	/// class MyTag : StaticTag{ MyTag }
	/// {
	///		public Tag ParentTag { return Tag.Root; }
	///		public string TagName { return "MyTag"; }
	/// }
	/// 
	/// // ...
	/// MyTag.DebugVerbose( "hello" );
	/// </example>
	public abstract class StaticTag< DerivedType > : IStaticTag where DerivedType : IStaticTag, new( )
	{
		#region Debug source output helpers

		/// <summary>
		/// Writes to the DebugSource for the Severity.DebugVerbose level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		[Conditional( "DEBUG" )]
		public static void DebugVerbose( string msg, params object[ ] args )
		{
			s_Root.GetDebugSource( Severity.Verbose ).Write( 1, msg, args );
		}

		/// <summary>
		/// Writes to the DebugSource for the Severity.DebugInfo level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		[Conditional( "DEBUG" )]
		public static void DebugInfo( string msg, params object[ ] args )
		{
			s_Root.GetDebugSource( Severity.Info ).Write( 1, msg, args );
		}

		/// <summary>
		/// Writes to the DebugSource for the Severity.DebugWarning level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		[Conditional( "DEBUG" )]
		public static void DebugWarning( string msg, params object[ ] args )
		{
			s_Root.GetDebugSource( Severity.Warning ).Write( 1, msg, args );
		}

		/// <summary>
		/// Writes to the DebugSource for the Severity.DebugError level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		[Conditional( "DEBUG" )]
		public static void DebugError( string msg, params object[ ] args )
		{
			s_Root.GetDebugSource( Severity.Error ).Write( 1, msg, args );
		}

		#endregion

		#region Timed scopes

		/// <summary>
		/// Creates a <see cref="TimedScope"/>, using the calling method's name as entry and exit message. The
		/// scope information is written to the verbose source.
		/// </summary>
		public static TimedScope EnterTimedScope( )
		{
			Source source = GetSource( Severity.Verbose );
			StackFrame frame = new StackFrame( 1 );
			return new TimedScope( source, frame.GetMethod( ).Name );
		}

		/// <summary>
		/// Creates a <see cref="TimedScope"/>, using a specified name for the scope. The scope information is written
		/// to the verbose source.
		/// </summary>
		public static TimedScope EnterTimedScope( string scopeName )
		{
			Source source = GetSource( Severity.Verbose );
			return new TimedScope( source, scopeName );
		}

		#endregion

		#region Source output helpers

		/// <summary>
		/// Asserts that a condition is true
		/// </summary>
		/// <param name="condition">Condition to test</param>
		/// <param name="msg">Message on failure</param>
		/// <param name="args">Message format parameters</param>
		public static void Assert( bool condition, string msg, params object[] args )
		{
			GetSource( Severity.Error ).Assert( condition, msg, args );
		}

		/// <summary>
		/// Fails, with a given message
		/// </summary>
		/// <param name="msg">Message</param>
		/// <param name="args">Message format parameters</param>
		public static void Fail( string msg, params object[] args )
		{
			GetSource( Severity.Error ).Fail( msg, args );
		}
		
		/// <summary>
		/// Writes an exception to the error source
		/// </summary>
		/// <param name="ex">Exception to log</param>
		/// <param name="msg">Message format</param>
		/// <param name="args">Format arguments</param>
		public static void Exception( Exception ex, string msg, params object[] args )
		{
			Error( msg, args );
			GetSource( Severity.Error ).Write( ex );
		}

		/// <summary>
		/// Writes to the Source for the Severity.Verbose level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		public static void Verbose( string msg, params object[ ] args )
		{
			s_Root.GetSource( Severity.Verbose ).Write( 1, msg, args );
		}

		/// <summary>
		/// Writes to the Source for the Severity.Info level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		public static void Info( string msg, params object[ ] args )
		{
			s_Root.GetSource( Severity.Info ).Write( 1, msg, args );
		}

		/// <summary>
		/// Writes to the Source for the Severity.Warning level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		public static void Warning( string msg, params object[ ] args )
		{
			s_Root.GetSource( Severity.Warning ).Write( 1, msg, args );
		}

		/// <summary>
		/// Writes to the Source for the Severity.Error level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		public static void Error( string msg, params object[ ] args )
		{
			s_Root.GetSource( Severity.Error ).Write( 1, msg, args );
		}

		#endregion

		#region IStaticTag Members

		/// <summary>
		/// Gets the parent tag. Defaults to the Root tag
		/// </summary>
		public virtual Tag ParentTag
		{
			get { return Tag.Root; }
		}

		/// <summary>
		/// Gets the tag name. Defaults to the DerivedType's type name
		/// </summary>
		public virtual string TagName
		{
			get { return typeof( DerivedType ).ToString( ); }
		}

		#endregion

		#region Static initialisation

		/// <summary>
		/// Static initialisation
		/// </summary>
		static StaticTag ( )
		{
			DerivedType tmp = new DerivedType( );
			s_Root = new Tag( tmp.ParentTag, tmp.TagName );
		}

		#endregion

		#region Tag access

		/// <summary>
		/// Returns the actual tag
		/// </summary>
		public static Tag Tag
		{
			get { return s_Root; }
		}

		/// <summary>
		/// Returns a debug source from the actual tag
		/// </summary>
		public static DebugSource GetDebugSource( Severity severity )
		{
			return Tag.GetDebugSource( severity );
		}

		/// <summary>
		/// Returns a source from the actual tag
		/// </summary>
		public static Source GetSource( Severity severity )
		{
			return Tag.GetSource( severity );
		}

		#endregion

		#region Private stuff

		private static readonly Tag s_Root;

		#endregion
	}
}
