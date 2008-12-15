using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rb.Interaction.Classes;

namespace Rb.Interaction.Tests
{
	/// <summary>
	/// Tests the <see cref="CommandRegistry"/> class.
	/// </summary>
	[TestFixture]
	public class TestCommandRegistry
	{
		/// <summary>
		/// Passing null to <see cref="CommandRegistry.Register"/> should throw
		/// </summary>
		[Test, ExpectedException( typeof( ArgumentNullException ) )]
		public void TestRegisterNullUserThrows( )
		{
			new CommandRegistry( ).Register( null );
		}

		/// <summary>
		/// Passing an ID of a non-existant command to <see cref="CommandRegistry.FindById"/> should throw
		/// </summary>
		[Test, ExpectedException( typeof( KeyNotFoundException ) )]
		public void TestFindNonExistantCommandThrows( )
		{
			new CommandRegistry( ).FindById( 0 );
		}

		/// <summary>
		/// Tests registering a new command and finding it by ID
		/// </summary>
		[Test]
		public void TestFindCommand( )
		{
			Command cmd = new Command( "cmd0", "", "" );
			CommandRegistry registry = new CommandRegistry( );
			registry.Register( cmd );
			Assert.AreEqual( cmd, registry.FindById( cmd.Id ) );
		}

		/// <summary>
		/// Tests registering a command twice (should throw on second attempt)
		/// </summary>
		[Test, ExpectedException( typeof( ArgumentException ) )]
		public void TestAddCommandWithDuplicateIdThrows( )
		{
			Command cmd = new Command( "cmd0", "", "" );
			CommandRegistry registry = new CommandRegistry( );
			registry.Register( cmd );
			registry.Register( cmd );
		}
	}
}
