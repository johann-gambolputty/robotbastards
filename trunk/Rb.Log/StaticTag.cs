using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Rb.Log
{
	/// <summary>
	/// Static tag interface
	/// </summary>
	public interface IStaticTag
	{
		Tag ParentTag { get; }
		string TagName { get; }
	}

	/// <summary>
	/// Helper class to build a globally available tag and sources
	/// </summary>
	/// <typeparam name="DerivedType">The derived type</typeparam>
	/// <example>
	/// class MyTag : StaticTag< MyTag >
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
			ms_Root.GetDebugSource( Severity.Verbose ).Write( msg, args );
		}

		/// <summary>
		/// Writes to the DebugSource for the Severity.DebugInfo level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		[Conditional( "DEBUG" )]
		public static void DebugInfo( string msg, params object[ ] args )
		{
			ms_Root.GetDebugSource( Severity.Info ).Write( msg, args );
		}

		/// <summary>
		/// Writes to the DebugSource for the Severity.DebugWarning level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		[Conditional( "DEBUG" )]
		public static void DebugWarning( string msg, params object[ ] args )
		{
			ms_Root.GetDebugSource( Severity.Warning ).Write( msg, args );
		}

		/// <summary>
		/// Writes to the DebugSource for the Severity.DebugError level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		[Conditional( "DEBUG" )]
		public static void DebugError( string msg, params object[ ] args )
		{
			ms_Root.GetDebugSource( Severity.Error ).Write( msg, args );
		}

		#endregion

		#region Source output helpers

		/// <summary>
		/// Writes to the Source for the Severity.Verbose level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		public static void Verbose( string msg, params object[ ] args )
		{
			ms_Root.GetSource( Severity.Verbose ).Write( msg, args );
		}

		/// <summary>
		/// Writes to the Source for the Severity.Info level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		public static void Info( string msg, params object[ ] args )
		{
			ms_Root.GetSource( Severity.Info ).Write( msg, args );
		}

		/// <summary>
		/// Writes to the Source for the Severity.Warning level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		public static void Warning( string msg, params object[ ] args )
		{
			ms_Root.GetSource( Severity.Warning ).Write( msg, args );
		}

		/// <summary>
		/// Writes to the Source for the Severity.Error level
		/// </summary>
		/// <param name="msg">Message string</param>
		/// <param name="args">Format arguments</param>
		public static void Error( string msg, params object[ ] args )
		{
			ms_Root.GetSource( Severity.Error ).Write( msg, args );
		}

		#endregion

		#region IStaticTag Methods

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
			ms_Root = new Tag( tmp.ParentTag, tmp.TagName );
		}

		#endregion

		#region Tag access

		/// <summary>
		/// Returns the actual tag
		/// </summary>
		public static Tag Tag
		{
			get { return ms_Root; }
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

		private static Tag ms_Root;

		#endregion
	}
}
