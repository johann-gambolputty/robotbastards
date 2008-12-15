using Rb.Interaction.Interfaces;

namespace Rb.Interaction.Classes.InputBindings
{
	/// <summary>
	/// Mouse button enumeration
	/// </summary>
	public enum MouseButtons
	{
		Left,
		Middle,
		Right
	}

	/// <summary>
	/// Asociates a command with a mouse button input
	/// </summary>
	public class CommandMouseButtonInputBinding : CommandInputBinding
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public CommandMouseButtonInputBinding( Command command, MouseButtons button, BinaryInputState state )
			: base( command )
		{
			m_Button = button;
			m_State = state;
		}

		/// <summary>
		/// Gets the mouse button bound to the command trigger
		/// </summary>
		public MouseButtons Button
		{
			get { return m_Button; }
		}

		/// <summary>
		/// Gets the state of the bound button that causes the command to trigger
		/// </summary>
		public BinaryInputState ButtonState
		{
			get { return m_State; }
		}

		/// <summary>
		/// Creates a monitor for this binding
		/// </summary>
		public override ICommandInputBindingMonitor CreateMonitor( ICommandInputBindingMonitorFactory factory, ICommandUser user )
		{
			return factory.CreateBindingMonitor( this, user );
		}

		#region Private Members

		private readonly MouseButtons m_Button;
		private readonly BinaryInputState m_State;

		#endregion
	}

}
