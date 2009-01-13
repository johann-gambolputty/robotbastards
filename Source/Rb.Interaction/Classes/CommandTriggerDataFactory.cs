using Rb.Interaction.Interfaces;

namespace Rb.Interaction.Classes
{
	/// <summary>
	/// Command trigger data factory
	/// </summary>
	public class CommandTriggerDataFactory : ICommandTriggerDataFactory
	{
		#region ICommandTriggerDataFactory Members

		/// <summary>
		/// Creates a CommandTriggerData object
		/// </summary>
		public CommandTriggerData Create( ICommandUser user, Command command, ICommandInputState inputState )
		{
			return new CommandTriggerData( user, command, inputState );
		}

		#endregion

		/// <summary>
		/// Gets the default instance
		/// </summary>
		public static CommandTriggerDataFactory Instance
		{
			get { return s_Instance; }
		}

		#region Private Members

		private readonly static CommandTriggerDataFactory s_Instance = new CommandTriggerDataFactory( );

		#endregion
	}
}
