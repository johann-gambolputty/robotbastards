using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rb.Interaction.Classes;
using Rb.Interaction.Interfaces;

namespace Rb.Interaction.Tests
{
	/// <summary>
	/// Tests the <see cref="CommandUserRegistry"/> class.
	/// </summary>
	[TestFixture]
	public class TestCommandUserRegistry
	{
		/// <summary>
		/// Passing null to <see cref="CommandUserRegistry.Register"/> should throw
		/// </summary>
		[Test, ExpectedException( typeof( ArgumentNullException ) )]
		public void TestRegisterNullUserThrows( )
		{
			new CommandUserRegistry( ).Register( null );
		}

		/// <summary>
		/// Passing an ID of a non-existant user to <see cref="CommandUserRegistry.FindById"/> should throw
		/// </summary>
		[Test, ExpectedException( typeof( KeyNotFoundException ) )]
		public void TestFindNonExistantUserThrows( )
		{
			new CommandUserRegistry( ).FindById( 0 );
		}

		/// <summary>
		/// Tests registering a new user and finding it by ID
		/// </summary>
		[Test]
		public void TestFindUser( )
		{
			ICommandUser user = new CommandUser( "user", 1 );
			CommandUserRegistry registry = new CommandUserRegistry( );
			registry.Register( user );
			Assert.AreEqual( user, registry.FindById( 1 ) );
		}
	}
}
