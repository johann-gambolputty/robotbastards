using System;
using Rb.Interaction.Classes;

namespace Rb.Interaction.Interfaces
{
	/// <summary>
	/// Command user interface
	/// </summary>
	public interface ICommandUser
	{
		/// <summary>
		/// User name
		/// </summary>
		string Name { get; }

		/// <summary>
		/// User unique identifier
		/// </summary>
		int Id { get; }

		/// <summary>
		/// Event raised when a command is triggered by this user
		/// </summary>
		event Action<CommandTriggerData> CommandTriggered;

		/// <summary>
		/// Raises the CommandTriggered event
		/// </summary>
		/// <param name="triggerData">Trigger data to pass to the event</param>
		void OnCommandTriggered( CommandTriggerData triggerData );
	}

}
