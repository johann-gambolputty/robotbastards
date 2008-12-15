using System;
using NUnit.Framework;
using Rb.Interaction.Classes;
using Rb.Interaction.Interfaces;

namespace Rb.Interaction.Tests
{
	/// <summary>
	/// Abstract test fixture for testing implementations of <see cref="ICommandUserFactory"/>
	/// </summary>
	public abstract class TestICommandUserFactory
	{
		/// <summary>
		/// Passing a null reference as a name to <see cref="ICommandUserFactory.Create"/> should throw
		/// </summary>
		[Test, ExpectedException( typeof( ArgumentNullException ) )]
		public void TestCreateWithNullNameThrows( )
		{
			CreateFactory( ).Create( null );
		}

		/// <summary>
		/// Tests creating a user
		/// </summary>
		[Test]
		public void TestCreate( )
		{
			ICommandUser user = CreateFactory( ).Create( "Pie" );
			Assert.IsNotNull( user );
			Assert.AreEqual( user.Name, "Pie" );
		}

		/// <summary>
		/// Ensures that calling OnCommandTriggered with null trigger data reference throws an ArgumentNullException
		/// </summary>
		[Test, ExpectedException( typeof( ArgumentNullException ) )]
		public void TestNullTriggerCommandThrows( )
		{
			CreateFactory( ).Create( "Pie" ).OnCommandTriggered( null );
		}

		/// <summary>
		/// Ensures that calling OnCommandTriggered with trigger data referencing another user throws an ArgumentException
		/// </summary>
		[Test, ExpectedException( typeof( ArgumentException ) )]
		public void TestTriggerCommandFromDifferentUserThrows( )
		{
			ICommandUserFactory factory = CreateFactory( );
			ICommandUser otherUser = factory.Create( "me" );
			factory.Create( "Pie" ).OnCommandTriggered( new CommandTriggerData( otherUser, new Command( "", "", "" ), null ) );
		}

		/// <summary>
		/// Checks that calling OnCommandTriggered when no listeners are subscribed to the event does not fail
		/// </summary>
		[Test]
		public void TestTriggerCommandWithNoListeners( )
		{
			ICommandUser user = CreateFactory( ).Create( "pie" );
			user.OnCommandTriggered( new CommandTriggerData( user, m_TestCommand, null ) );
		}


		/// <summary>
		/// Ensures that calling OnCommandTriggered works
		/// </summary>
		[Test]
		public void TestTriggerCommand( )
		{
			ICommandUser user = CreateFactory( ).Create( "pie" );
			ICommandInputState state = new CommandScalarInputState( 1, 2 );
			bool triggered = false;
			user.CommandTriggered +=
				delegate( CommandTriggerData triggerData )
				{
					Assert.AreEqual( user, triggerData.User );
					Assert.AreEqual( m_TestCommand, triggerData.Command );
					Assert.AreEqual( state, triggerData.InputState );
					triggered = true;
				};
			user.OnCommandTriggered( new CommandTriggerData( user, m_TestCommand, state ) );
			Assert.IsTrue( triggered );
		}

		/// <summary>
		/// Creates an ICommandUserFactory instance
		/// </summary>
		protected abstract ICommandUserFactory CreateFactory( );

		private readonly Command m_TestCommand = new Command( "testCommand", "", "" );
	}

}
