using Rb.Interaction.Interfaces;

namespace Rb.Interaction.Classes.InputBindings
{
	/// <summary>
	/// Asociates a command with a key input
	/// </summary>
	public class CommandKeyInputBinding : CommandInputBinding
	{
		/// <summary>
		/// Sets the command, and the key input bindings
		/// </summary>
		public CommandKeyInputBinding( Command command, string key, BinaryInputState keyState )
			: base( command )
		{
			m_Key = key;
			m_KeyState = keyState;
		}

		/// <summary>
		/// Gets the name key used to trigger the binding
		/// </summary>
		public string KeyName
		{
			get { return m_Key; }
		}

		/// <summary>
		/// Gets the state of the bound key that triggers the binding
		/// </summary>
		public BinaryInputState KeyState
		{
			get { return m_KeyState; }
		}

		/// <summary>
		/// Creates a monitor for this binding
		/// </summary>
		public override ICommandInputBindingMonitor CreateMonitor( ICommandInputBindingMonitorFactory factory, ICommandUser user )
		{
			return factory.CreateBindingMonitor( this, user );
		}

		#region Private Members

		private readonly string m_Key;
		private readonly BinaryInputState m_KeyState;

		#endregion
	}

}
