using System;
using System.Reflection;
using System.Text;
using NUnit.Framework;

namespace Rb.TestUtils
{
	/// <summary>
	/// Handy unit test utilities
	/// </summary>
	public static class UnitTestUtils
	{

		/// <summary>
		/// Expects a delegate to throw an exception of a given type (like nunit's ExpectedExceptionAttribute, but doesn't exit if correct exception is thrown)
		/// </summary>
		/// <typeparam name="ExceptionType">Expected exception type</typeparam>
		/// <param name="del">Delegate to call</param>
		/// <param name="delArgs">Delegate arguments</param>
		public static void ExpectException<ExceptionType>( Delegate del, params object[] delArgs ) where ExceptionType : Exception
		{
			StringBuilder callStr = new StringBuilder( );
			callStr.AppendFormat( "{0}(", del.Method.Name );
			for ( int argIndex = 0; argIndex < delArgs.Length; ++argIndex )
			{
				if ( argIndex > 0 )
				{
					callStr.Append( ", " );
				}
				callStr.Append( delArgs[ argIndex ] );
			}
			callStr.Append( ")" );
			try
			{
				del.DynamicInvoke( delArgs );
			}
			catch ( Exception ex )
			{
				if ( ex is TargetInvocationException )
				{
					//	DynamicInvoke() wraps any exception thrown by the called method in a TargetInvocationException
					ex = ex.InnerException;
				}
				Assert.IsInstanceOfType( typeof( ExceptionType ), ex, "{0} did not throw expected exception type {1}. Actual exception type {2} ({3})", callStr, typeof( ExceptionType ), ex.GetType( ), ex.Message );
				Console.WriteLine( "{0} threw expected exception \"{1}\" ({2})", callStr, ex.GetType( ), ex.Message );
				return;
			}
			Assert.Fail( "{0} did not throw. Expected exception type {1}.", callStr, typeof( ExceptionType ) );
		}
	}
}
