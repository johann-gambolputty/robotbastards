using System;
using System.Collections.Generic;
using Rb.Interaction.Interfaces;
using Rb.Core.Utils;

namespace Rb.Interaction.Classes
{
	/// <summary>
	/// Manages command input bindings and their monitors
	/// </summary>
	public class CommandInputSource
	{
		/// <summary>
		/// Sets up this input source with a specified control
		/// </summary>
		/// <param name="monitorFactory">Creates binding monitors</param>
		/// <param name="context">Context that commands are triggered in</param>
		public CommandInputSource( ICommandInputBindingMonitorFactory monitorFactory, object context )
		{
			Arguments.CheckNotNull( monitorFactory, "monitorFactory" );
			m_MonitorFactory = monitorFactory;
			m_Context = context;
		}

		#region Bindings

		/// <summary>
		/// Event raised when any command bound to this source is triggered by a specified user
		/// </summary>
		public event Action<CommandTriggerData> CommandTriggered;

		/// <summary>
		/// Adds a list of input bindings to a control
		/// </summary>
		public void AddBindings( ICommandUser user, IEnumerable<CommandInputBinding> bindings )
		{
			foreach ( CommandInputBinding binding in bindings )
			{
				AddBinding( user, binding );
			}
		}

		/// <summary>
		/// Adds a command binding to a control for a specific user
		/// </summary>
		public virtual void AddBinding( ICommandUser user, CommandInputBinding binding )
		{
			ICommandInputBindingMonitor monitor = binding.CreateMonitor( m_MonitorFactory, user );
			GetSafeMonitorList( user ).Add( monitor );
			m_AllMonitors.Add( monitor );
			if ( m_Started )
			{
				monitor.Start( );
			}
		}

		/// <summary>
		/// Returns true if a specified command's monitor is active for any user
		/// </summary>
		public bool IsCommandTriggered( Command cmd )
		{
			return IsCommandActive( cmd, m_AllMonitors );
		}

		/// <summary>
		/// Returns true if a specified command's monitor is active for a specified user
		/// </summary>
		public bool IsCommandTriggered( Command cmd, ICommandUser user )
		{
			return IsCommandActive( cmd, GetSafeMonitorList( user ) );
		}

		#endregion

		#region Updates

		/// <summary>
		/// Calls Start() on all binding monitors
		/// </summary>
		public virtual void Start( )
		{
			if ( m_Started )
			{
				throw new InvalidOperationException( "Can't Start() an input source more than once without first calling Stop()" );
			}
			foreach ( ICommandInputBindingMonitor monitor in m_AllMonitors )
			{
				monitor.Start( );
			}
			m_Started = true;
		}

		/// <summary>
		/// Update is required to trigger commands that are not explicitly bound to UI events (e.g. key held)
		/// </summary>
		public virtual void Update( )
		{
			foreach ( ICommandInputBindingMonitor monitor in m_AllMonitors )
			{
				if ( monitor.Update( ) )
				{
					ICommandInputState inputState = monitor.CreateInputState( monitor.Binding.InputStateFactory, m_Context );
					Command cmd = monitor.Binding.Command;
					ICommandUser user = monitor.User;
					CommandTriggerData triggerData = new CommandTriggerData( user, cmd, inputState );
					cmd.Trigger( triggerData );
					user.OnCommandTriggered( triggerData );
					if ( CommandTriggered != null )
					{
						CommandTriggered( triggerData );
					}
				}
			}
		}

		/// <summary>
		/// Calls Stop() on all binding monitors
		/// </summary>
		public virtual void Stop( )
		{
			foreach ( ICommandInputBindingMonitor monitor in m_AllMonitors )
			{
				monitor.Stop( );
			}
			m_Started = false;
		}

		#endregion

		#region Private Members

		private bool m_Started;
		private readonly ICommandInputBindingMonitorFactory m_MonitorFactory;
		private readonly object m_Context;
		private readonly List<ICommandInputBindingMonitor> m_AllMonitors = new List<ICommandInputBindingMonitor>( );
		private readonly Dictionary<ICommandUser, List<ICommandInputBindingMonitor>> m_UserMonitors = new Dictionary<ICommandUser, List<ICommandInputBindingMonitor>>( );

		private static bool IsCommandActive( Command cmd, IEnumerable<ICommandInputBindingMonitor> monitors )
		{
			foreach ( ICommandInputBindingMonitor monitor in monitors )
			{
				if ( ( monitor.Binding.Command == cmd ) && ( monitor.IsActive ) )
				{
					return true;
				}
			}
			return false;
		}

		private List<ICommandInputBindingMonitor> GetSafeMonitorList( ICommandUser user )
		{
			List<ICommandInputBindingMonitor> monitors;
			if ( !m_UserMonitors.TryGetValue( user, out monitors ) )
			{
				monitors = new List<ICommandInputBindingMonitor>( );
				m_UserMonitors[ user ] = monitors;
			}
			return monitors;
		}

		#endregion
	}

}
