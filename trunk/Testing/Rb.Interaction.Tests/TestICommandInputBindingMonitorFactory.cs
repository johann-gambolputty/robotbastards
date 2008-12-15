using System;
using NUnit.Framework;
using Rb.Interaction.Classes;
using Rb.Interaction.Classes.InputBindings;
using Rb.Interaction.Interfaces;
using Rb.TestUtils;

namespace Rb.Interaction.Tests
{
	/// <summary>
	/// Base class for testing implementations of <see cref="ICommandInputBindingMonitorFactory"/>
	/// </summary>
	public abstract class TestICommandInputBindingMonitorFactory
	{
		/// <summary>
		/// Ensures that passing null as a bindings or users to the binding monitor creation methods like
		/// <see cref="ICommandInputBindingMonitorFactory.CreateBindingMonitor(CommandKeyInputBinding, ICommandUser)"/>
		/// throw "ArgumentNullException" objects
		/// </summary>
		[Test]
		public void TestPassingNullArgumentsToCreateBindingMonitorThrows( )
		{
			UnitTestUtils.ExpectException<ArgumentNullException>( new CreateMonitorDelegate<CommandKeyInputBinding>( CreateFactory( ).CreateBindingMonitor ), null, CommandUser.Default );
			UnitTestUtils.ExpectException<ArgumentNullException>( new CreateMonitorDelegate<CommandKeyInputBinding>( CreateFactory( ).CreateBindingMonitor ), new CommandKeyInputBinding( m_Cmd, "K", BinaryInputState.Down ), null );

			UnitTestUtils.ExpectException<ArgumentNullException>( new CreateMonitorDelegate<CommandMouseButtonInputBinding>( CreateFactory( ).CreateBindingMonitor ), null, CommandUser.Default );
			UnitTestUtils.ExpectException<ArgumentNullException>( new CreateMonitorDelegate<CommandMouseButtonInputBinding>( CreateFactory( ).CreateBindingMonitor ), new CommandMouseButtonInputBinding( m_Cmd, MouseButtons.Left, BinaryInputState.Down ), null );

			UnitTestUtils.ExpectException<ArgumentNullException>( new CreateMonitorDelegate<CommandMouseWheelInputBinding>( CreateFactory( ).CreateBindingMonitor ), null, CommandUser.Default );
			UnitTestUtils.ExpectException<ArgumentNullException>( new CreateMonitorDelegate<CommandMouseWheelInputBinding>( CreateFactory( ).CreateBindingMonitor ), new CommandMouseWheelInputBinding( m_Cmd ), null );
		}

		[Test]
		public void TestKeyBinding( )
		{
			bool fired = false;
			Action<CommandTriggerData> handler =
				delegate
				{
					fired = true;
				};
			m_Cmd.CommandTriggered += handler;
			ICommandInputBindingMonitor monitor = CreateFactory( ).CreateBindingMonitor( new CommandKeyInputBinding( m_Cmd, "A", BinaryInputState.Down ), CommandUser.Default );
			monitor.Start( );
			FireKeyDown( "A" );
			monitor.Stop( );
			m_Cmd.CommandTriggered -= handler;

			Assert.IsTrue( fired );
		}

		/// <summary>
		/// Ensures that passing null as a user to the key binding monitor creation method <see cref="ICommandInputBindingMonitorFactory.CreateBindingMonitor(CommandKeyInputBinding, ICommandUser)"/> throws an ArgumentNullException
		/// </summary>
		[Test, ExpectedException( typeof( ArgumentNullException ) )]
		public void TestPassingNullUserToKeyBindingThrows( )
		{
			CreateFactory( ).CreateBindingMonitor( new CommandKeyInputBinding( m_Cmd, "K", BinaryInputState.Down ), null );
		}

		/// <summary>
		/// Creates an object that implements ICommandInputBindingMonitorFactory
		/// </summary>
		protected abstract ICommandInputBindingMonitorFactory CreateFactory( );

		/// <summary>
		/// Sends a key press to the input source that the created monitors observe
		/// </summary>
		protected abstract void FireKeyDown( string key );

		private delegate ICommandInputBindingMonitor CreateMonitorDelegate<BindingType>( BindingType binding, ICommandUser user );

		private readonly Command m_Cmd = new Command( "dummyCommand", "", "" );
	}
}
