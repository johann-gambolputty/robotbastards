using System;
using NUnit.Framework;
using Rb.Interaction.Classes;
using Rb.TestUtils;

namespace Rb.Interaction.Tests
{
	[TestFixture]
	public class TestCommandList
	{
		/// <summary>
		/// Tests combinations of invalid constructor arguments when creating a root command list (command list with no parent)
		/// </summary>
		[Test]
		public void TestRootCommandListInvalidConstructorArguments( )
		{
			CreateDelegate createList = Create;
			UnitTestUtils.ExpectException<ArgumentNullException>( createList, "abc", "abc", null );
			UnitTestUtils.ExpectException<ArgumentNullException>( createList, null, "abc", CommandRegistry.Instance );
			UnitTestUtils.ExpectException<ArgumentException>( createList, string.Empty, "abc", CommandRegistry.Instance );
		}

		/// <summary>
		/// Tests combinations of invalid constructor arguments when creating a child command list
		/// </summary>
		[Test]
		public void TestChildCommandListInvalidConstructorArguments( )
		{
			CreateChildListDelegate createList = Create;
			CommandList parentList = Create( "parent", "parent", CommandRegistry.Instance );
			UnitTestUtils.ExpectException<ArgumentNullException>( createList, null, "abc", "abc" );
			UnitTestUtils.ExpectException<ArgumentNullException>( createList, parentList, null, "abc" );
			UnitTestUtils.ExpectException<ArgumentException>( createList, parentList, string.Empty, "abc" );
		}

		#region Private Members

		private delegate CommandList CreateChildListDelegate( CommandList parentList, string name, string locName );
		private delegate CommandList CreateDelegate( string name, string locName, CommandRegistry registry );

		private static CommandList Create( CommandList parentList, string name, string locName )
		{
			return new CommandList( parentList, name, locName );
		}

		private static CommandList Create( string name, string locName, CommandRegistry registry )
		{
			return new CommandList( name, locName, registry );
		}

		#endregion
	}
}
