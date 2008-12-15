using Rb.Interaction.Interfaces;

namespace Rb.Interaction.Classes
{

	/// <summary>
	/// Associates a command with input
	/// </summary>
	public abstract class CommandInputBinding
	{
		/// <summary>
		/// Sets the command to bind
		/// </summary>
		/// <param name="command">Command to bind</param>
		public CommandInputBinding( Command command )
		{
			m_Command = command;
		}

		/// <summary>
		/// Command being bound
		/// </summary>
		public Command Command
		{
			get { return m_Command; }
			set { m_Command = value; }
		}

		/// <summary>
		/// Gets/sets the factory that builds <see cref="ICommandInputState"/> objects
		/// </summary>
		public ICommandInputStateFactory InputStateFactory
		{
			get { return m_InputStateFactory; }
			set { m_InputStateFactory = value; }
		}

		/// <summary>
		/// Creates a monitor for this binding.
		/// </summary>
		/// <remarks>
		/// A binding monitor is an object that monitors a particular input source (e.g. control) for input that
		/// matches this binding.
		/// </remarks>
		public virtual ICommandInputBindingMonitor CreateMonitor( ICommandInputBindingMonitorFactory factory, ICommandUser user )
		{
			return factory.CreateBindingMonitor( this, user );
		}

		#region Private Members

		private Command m_Command;
		private ICommandInputStateFactory m_InputStateFactory = CommandInputStateFactory.Default;

		#endregion
	}

}
