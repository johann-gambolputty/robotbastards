using Rb.Interaction.Interfaces;

namespace Rb.Interaction.Classes.InputBindings
{
	/// <summary>
	/// Defines the binding between a command and mouse wheel input
	/// </summary>
	public class CommandMouseWheelInputBinding : CommandInputBinding
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public CommandMouseWheelInputBinding( Command command )
			: base( command )
		{
		}

		/// <summary>
		/// Creates a monitor for this binding
		/// </summary>
		public override ICommandInputBindingMonitor CreateMonitor( ICommandInputBindingMonitorFactory factory, ICommandUser user )
		{
			return factory.CreateBindingMonitor( this, user );
		}
	}

}
