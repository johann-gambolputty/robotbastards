
using Rb.Interaction.Classes;
using Rb.Interaction.Classes.InputBindings;

namespace Rb.Interaction.Interfaces
{
	/// <summary>
	/// Creates input binding monitors (<see cref="ICommandInputBindingMonitor"/>)
	/// </summary>
	public interface ICommandInputBindingMonitorFactory
	{
		/// <summary>
		/// Creates a monitor for a key binding
		/// </summary>
		ICommandInputBindingMonitor CreateBindingMonitor( CommandKeyInputBinding binding, ICommandUser user );

		/// <summary>
		/// Creates a monitor for a mouse button binding
		/// </summary>
		ICommandInputBindingMonitor CreateBindingMonitor( CommandMouseButtonInputBinding binding, ICommandUser user );

		/// <summary>
		/// Creates a monitor for a mouse wheel binding
		/// </summary>
		ICommandInputBindingMonitor CreateBindingMonitor( CommandMouseWheelInputBinding binding, ICommandUser user );

		/// <summary>
		/// Creates a monitor for a binding that is not explicitly supported by this interface
		/// </summary>
		ICommandInputBindingMonitor CreateBindingMonitor( CommandInputBinding binding, ICommandUser user );
	}

}
