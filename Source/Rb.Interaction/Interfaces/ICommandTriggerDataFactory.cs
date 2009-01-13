using Rb.Interaction.Classes;

namespace Rb.Interaction.Interfaces
{
	/// <summary>
	/// Command trigger data factory
	/// </summary>
	public interface ICommandTriggerDataFactory
	{
		/// <summary>
		/// Creates a new CommandTriggerData object
		/// </summary>
		CommandTriggerData Create( ICommandUser user, Command command, ICommandInputState inputState );
	}
}
