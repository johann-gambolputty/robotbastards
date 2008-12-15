using Rb.Interaction.Classes;

namespace Rb.Interaction.Interfaces
{
	/// <summary>
	/// Monitors a particular input source for input that matches a binding definition.
	/// </summary>
	public interface ICommandInputBindingMonitor
	{
		/// <summary>
		/// Gets the associated command user
		/// </summary>
		ICommandUser User
		{
			get;
		}

		/// <summary>
		/// Gets the associated command binding
		/// </summary>
		CommandInputBinding Binding
		{
			get;
		}

		/// <summary>
		/// Starts listening for user input that will activate the binding
		/// </summary>
		void Start( );

		/// <summary>
		/// Updates this monitor
		/// </summary>
		/// <returns>Returns true if the associated command binding is active</returns>
		bool Update( );

		/// <summary>
		/// Returns true if the associated command binding is active
		/// </summary>
		bool IsActive
		{
			get;
		}

		/// <summary>
		/// Stops listening for user input that activates the binding
		/// </summary>
		void Stop( );

		/// <summary>
		/// Creates an input state from the state of this monitor
		/// </summary>
		ICommandInputState CreateInputState( ICommandInputStateFactory factory, object context );
	}

}
