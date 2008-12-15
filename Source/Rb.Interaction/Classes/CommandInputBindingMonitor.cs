using Rb.Interaction.Interfaces;

namespace Rb.Interaction.Classes
{
	/// <summary>
	/// Base class for monitoring an input binding
	/// </summary>
	public abstract class CommandInputBindingMonitor : ICommandInputBindingMonitor
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="binding">Input binding</param>
		/// <param name="user">Command user</param>
		public CommandInputBindingMonitor( CommandInputBinding binding, ICommandUser user )
		{
			m_Binding = binding;
			m_User = user;
		}

		#region ICommandInputBindingMonitor Members

		/// <summary>
		/// Gets the user
		/// </summary>
		public ICommandUser User
		{
			get { return m_User; }
		}

		/// <summary>
		/// Gets the input binding
		/// </summary>
		public CommandInputBinding Binding
		{
			get { return m_Binding; }
		}

		/// <summary>
		/// Starts listening for user input that will activate the binding
		/// </summary>
		public abstract void Start( );

		/// <summary>
		/// Updates the monitor
		/// </summary>
		public abstract bool Update( );

		/// <summary>
		/// Returns true if the associated command binding is active
		/// </summary>
		public abstract bool IsActive
		{
			get;
		}

		/// <summary>
		/// Stops listening for user input that activates the binding
		/// </summary>
		public abstract void Stop( );

		/// <summary>
		/// Creates an input state from the state of this monitor
		/// </summary>
		public virtual ICommandInputState CreateInputState( ICommandInputStateFactory factory, object context )
		{
			return factory.NewInputState( context );
		}

		#endregion

		#region Private Members

		private readonly CommandInputBinding m_Binding;
		private readonly ICommandUser m_User;

		#endregion
	}

}
