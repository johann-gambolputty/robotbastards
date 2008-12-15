using System;
using System.Runtime.Serialization;
using Rb.Interaction.Interfaces;

namespace Rb.Interaction.Classes
{
	/// <summary>
	/// Command trigger event arguments
	/// </summary>
	[Serializable]
	public class CommandTriggerData : EventArgs, ISerializable
	{
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		public CommandTriggerData( SerializationInfo info, StreamingContext context )
		{
			CommandSerializationContext commandContext = context.Context as CommandSerializationContext;
			if ( commandContext == null )
			{
				throw new System.IO.IOException( "CommandTriggerData must be deserialized with a CommandSerializationContext present" );
			}
			m_User = commandContext.Users.FindById( ( int )info.GetValue( "u", typeof( int ) ) );
			m_Command = commandContext.Commands.FindById( ( int )info.GetValue( "c", typeof( int ) ) );
			m_InputState = ( ICommandInputState )info.GetValue( "s", typeof( ICommandInputState ) );
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="user">User that triggered the command</param>
		/// <param name="command">Command that was triggered</param>
		/// <param name="inputState">Command input state</param>
		public CommandTriggerData( ICommandUser user, Command command, ICommandInputState inputState )
		{
			m_User = user;
			m_Command = command;
			m_InputState = inputState;
		}

		/// <summary>
		/// Gets the user that triggered the event
		/// </summary>
		public ICommandUser User
		{
			get { return m_User; }
		}

		/// <summary>
		/// Gets the command that was triggered
		/// </summary>
		public Command Command
		{
			get { return m_Command; }
		}

		/// <summary>
		/// Gets the command input state
		/// </summary>
		public ICommandInputState InputState
		{
			get { return m_InputState; }
		}

		#region ISerializable Members

		/// <summary>
		/// Serializes this object
		/// </summary>
		public void GetObjectData( SerializationInfo info, StreamingContext context )
		{
			info.AddValue( "u", User.Id );
			info.AddValue( "c", Command.Id );
			info.AddValue( "s", InputState );
		}

		#endregion

		#region Private Members

		private readonly ICommandUser m_User;
		private readonly Command m_Command;
		private readonly ICommandInputState m_InputState;

		#endregion
	}

}
