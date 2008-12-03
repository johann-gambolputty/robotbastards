using System;
using NUnit.Framework;
using Rb.Core.Utils;

namespace Rb.Core.Tests.Utils
{
	/// <summary>
	/// Tests methods in the <see cref="DelegateHelpers"/> class
	/// </summary>
	[TestFixture]
	public class TestDelegateHelpers
	{
		/// <summary>
		/// Calling <see cref="DelegateHelpers.ValidateDelegateArguments"/>
		/// with a null delegate should throw an ArgumentNullException
		/// </summary>
		[Test, ExpectedException( typeof( ArgumentNullException ) )]
		public void TestValidateNullDelegateShouldThrow( )
		{
			DelegateHelpers.ValidateDelegateArguments( null, null );
		}

		/// <summary>
		/// Calling <see cref="DelegateHelpers.ValidateDelegateArguments"/>
		/// with a null argument list should work if the delegate takes no parameters
		/// </summary>
		[Test]
		public void TestSimpleDelegateValidatesWithNullArguments( )
		{
			DelegateHelpers.ValidateDelegateArguments( new ActionDelegates.Action( SimpleDelegate ), null );
		}

		/// <summary>
		/// Calling <see cref="DelegateHelpers.ValidateDelegateArgumentTypes"/>
		/// with a null argument list should work if the delegate takes no parameters
		/// </summary>
		[Test]
		public void TestSimpleDelegateValidatesWithNullArgumentTypes( )
		{
			DelegateHelpers.ValidateDelegateArgumentTypes( new ActionDelegates.Action( SimpleDelegate ), null );
		}

		/// <summary>
		/// Tests calling <see cref="DelegateHelpers.ValidateDelegateArguments"/> for
		/// some sample delegates.
		/// </summary>
		[Test]
		public void TestValidateDelegateWithValidArguments( )
		{
			DelegateHelpers.ValidateDelegateArguments( new ActionDelegates.Action( SimpleDelegate ), new object[ 0 ] );
			DelegateHelpers.ValidateDelegateArguments( new ActionDelegates.Action<int>( SimpleDelegate ), new object[ ] { new int( ) } );
			DelegateHelpers.ValidateDelegateArguments( new ActionDelegates.Action<int, object>( SimpleDelegate ), new object[ ] { new int( ), new object( ) } );
		}

		/// <summary>
		/// Tests calling <see cref="DelegateHelpers.ValidateDelegateArguments"/> for
		/// some sample delegates.
		/// </summary>
		[Test]
		public void TestValidateDelegateWithValidCastArguments( )
		{
			DelegateHelpers.ValidateDelegateArguments( new ActionDelegates.Action<int, object>( SimpleDelegate ), new object[] { new int( ), GetType( ) } );
		}

		/// <summary>
		/// Tests calling <see cref="DelegateHelpers.ValidateDelegateArguments"/> for
		/// too many arguments
		/// </summary>
		[Test, ExpectedException( typeof( ArgumentException ) )]
		public void TestValidateDelegateWithTooManyArguments( )
		{
			try
			{
				DelegateHelpers.ValidateDelegateArguments( new ActionDelegates.Action( SimpleDelegate ), new object[ 1 ] { new int( ) } );
			}
			catch ( Exception ex )
			{
				Console.WriteLine( "Received expected exception. Details:\n{0}", ex );
				throw;
			}
		}

		/// <summary>
		/// Tests calling <see cref="DelegateHelpers.ValidateDelegateArguments"/> for
		/// too few arguments
		/// </summary>
		[Test, ExpectedException( typeof( ArgumentException ) )]
		public void TestValidateDelegateWithTooFewArguments( )
		{
			try
			{
				DelegateHelpers.ValidateDelegateArguments( new ActionDelegates.Action<int>( SimpleDelegate ), null );
			}
			catch ( Exception ex )
			{
				Console.WriteLine( "Received expected exception. Details:\n{0}", ex );
				throw;
			}
		}

		/// <summary>
		/// Tests calling <see cref="DelegateHelpers.ValidateDelegateArguments"/> for
		/// an invalid argument
		/// </summary>
		[Test, ExpectedException( typeof( ArgumentException ) )]
		public void TestValidateDelegateWithInvalidArguments( )
		{
			try
			{
				DelegateHelpers.ValidateDelegateArguments( new ActionDelegates.Action<int>( SimpleDelegate ), new object[ 1 ] { new float( ) } );
			}
			catch ( Exception ex )
			{
				Console.WriteLine( "Received expected exception. Details:\n{0}", ex );
				throw;
			}
		}

		private static void SimpleDelegate( ) { }
		private static void SimpleDelegate( int p0 ) { }
		private static void SimpleDelegate( int p0, object p1 ) { }
	}
}
