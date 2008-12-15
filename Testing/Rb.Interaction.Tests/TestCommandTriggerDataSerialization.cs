using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Interaction.Classes;

namespace Rb.Interaction.Tests
{
	/// <summary>
	/// Tests serialization of <see cref="CommandTriggerData"/>
	/// </summary>
	[TestFixture]
	public class TestCommandTriggerDataSerialization
	{
		/// <summary>
		/// Initializes test data
		/// </summary>
		public TestCommandTriggerDataSerialization( )
		{
			m_Formatter = new BinaryFormatter( null, CommandSerializationContext.ToStreamingContext( m_UserRegistry, m_CommandRegistry ) );

			m_Commands = new CommandList( "testCommands", "", m_CommandRegistry );
			m_Command0 = m_Commands.NewCommand( "cmd0", "", "" );

			m_UserRegistry.Register( m_User );
		}

		/// <summary>
		/// Tests serializing then deserializing a <see cref="CommandTriggerData"/> object
		/// </summary>
		[Test]
		public void TestSerializeDeserializeWithNoInputState( )
		{
			CommandTriggerData dataIn = new CommandTriggerData( m_User, m_Command0, null );
			CommandTriggerData dataOut = SerializeDeserialize( m_Formatter, dataIn );

			Assert.AreEqual( dataIn.User, dataOut.User );
			Assert.AreEqual( dataIn.Command, dataOut.Command );
			Assert.AreEqual( dataIn.InputState, dataOut.InputState );
		}

		/// <summary>
		/// Tests serializing then deserializing a <see cref="CommandTriggerData"/> object
		/// </summary>
		[Test]
		public void TestSerializeDeserializeWithScalarInputState( )
		{
			CommandTriggerData dataIn = new CommandTriggerData( m_User, m_Command0, new CommandScalarInputState( 1, 2 ) );
			CommandTriggerData dataOut = SerializeDeserialize( m_Formatter, dataIn );

			Assert.AreEqual( dataIn.User, dataOut.User );
			Assert.AreEqual( dataIn.Command, dataOut.Command );
			Assert.IsTrue( Loose.DeepEquality( dataIn.InputState, dataOut.InputState ) );
		}

		/// <summary>
		/// Tests serializing then deserializing a <see cref="CommandTriggerData"/> object
		/// </summary>
		[Test]
		public void TestSerializeDeserializeWithPointInputState( )
		{
			CommandTriggerData dataIn = new CommandTriggerData( m_User, m_Command0, new CommandPointInputState( new Point2( 1, 1 ), new Point2( 2, 2 ) ) );
			CommandTriggerData dataOut = SerializeDeserialize( m_Formatter, dataIn );

			Assert.AreEqual( dataIn.User, dataOut.User );
			Assert.AreEqual( dataIn.Command, dataOut.Command );
			Assert.IsTrue( Loose.DeepEquality( dataIn.InputState, dataOut.InputState ) );
		}

		/// <summary>
		/// Ensures that attempting to deserialize a CommandTriggerData object without a serialization context throws
		/// </summary>
		[Test]
		public void TestDeserializeTriggerDataWithoutContextThrows( )
		{
			CommandTriggerData dataIn = new CommandTriggerData( m_User, m_Command0, null );
			try
			{
				SerializeDeserialize( new BinaryFormatter( ), dataIn );
				Assert.Fail( "Expected deserialization to throw a TargetInvocationException with an inner IOException" );
			}
			catch ( Exception ex )
			{
				Assert.AreEqual( typeof( TargetInvocationException ), ex.GetType( ) );
				Assert.AreEqual( typeof( IOException ), ex.InnerException.GetType( ) );
			}
		}

		#region Private Members

		private readonly BinaryFormatter m_Formatter;
		private readonly CommandUserRegistry m_UserRegistry = new CommandUserRegistry( );
		private readonly CommandUser m_User = new CommandUser( "", 1 );
		private readonly CommandRegistry m_CommandRegistry = new CommandRegistry( );
		private readonly CommandList m_Commands;
		private readonly Command m_Command0;

		/// <summary>
		/// Serializes a CommandTriggerData object, then deserializes it
		/// </summary>
		private static CommandTriggerData SerializeDeserialize( BinaryFormatter formatter, CommandTriggerData dataIn )
		{
			MemoryStream stream = new MemoryStream( );

			formatter.Serialize( stream, dataIn );

			stream.Position = 0;
			return ( CommandTriggerData )formatter.Deserialize( stream );
		}

		#endregion
	}

}
