using System;
using System.Collections.Generic;

namespace Rb.Interaction
{
    /// <summary>
	/// Delegate used by <see cref="CommandUser"/> events
    /// </summary>
    /// <param name="msg">Command details</param>
    public delegate void CommandEventDelegate( CommandMessage msg );

	/// <summary>
	/// Stores a set of input to command bindings
	/// </summary>
	public class CommandUser : IDisposable
	{
		/// <summary>
		/// Adds this user to the <see cref="CommandUserManager"/>
		/// </summary>
		public CommandUser( )
		{
			CommandUserManager.Instance.AddUser( this );
		}

		/// <summary>
		/// Removes this user from the <see cref="CommandUserManager"/>
		/// </summary>
		~CommandUser( )
		{
			Dispose( );
		}

		/// <summary>
		/// Removes this user from the <see cref="CommandUserManager"/>
		/// </summary>
		public void Dispose( )
		{
			CommandUserManager.Instance.RemoveUser( this );
		}

		#region Listeners

		public void AddActiveListener( Command cmd, CommandEventDelegate listener )
		{
			InitialiseCommandBinding( cmd ).CommandActive += listener;
		}

		public void AddActivatedListener( Command cmd, CommandEventDelegate listener )
		{
			InitialiseCommandBinding( cmd ).CommandActivated += listener;
		}
		
		public void AddActiveListener( CommandList commands, CommandEventDelegate listener )
		{
			foreach ( Command cmd in commands )
			{
				AddActiveListener( cmd, listener );;
			}
		}

		public void AddActivatedListener( CommandList commands, CommandEventDelegate listener )
		{
			foreach ( Command cmd in commands )
			{
				AddActivatedListener( cmd, listener );
			}
		}
		
		public void RemoveListener( CommandEventDelegate listener )
		{
			foreach ( Binding binding in m_Bindings )
			{
				binding.CommandActivated -= listener;
				binding.CommandActive -= listener;
			}
		}

		#endregion

		/// <summary>
		/// Creates a binding for a given command
		/// </summary>
		private Binding InitialiseCommandBinding( Command command )
		{
			Binding binding = FindBindingForCommand( command );
			if ( binding == null )
			{
				binding = new Binding( command );
				m_Bindings.Add( binding );
			}

			return binding;
		}

		/// <summary>
		/// Binds a command to an input source. When the input is active, the command is active
		/// </summary>
		public void Bind( Command cmd, IInput input )
		{
			Binding binding = FindBindingForCommand( cmd );
			if ( binding == null )
			{
				binding = new Binding( cmd );
				m_Bindings.Add( binding );
			}
			binding.AddInput( input );
		}

		/// <summary>
		/// Updates the stored command bindings
		/// </summary>
		public void Update( )
		{
			foreach ( Binding binding in m_Bindings )
			{
				binding.Update( );
			}
		}

		#region Private stuff

		/// <summary>
		/// Stores a binding between a command and input sources
		/// </summary>
		private class Binding
		{
			/// <summary>
			/// Event invoked by Update() when a command becomes active
			/// </summary>
			public event CommandEventDelegate CommandActivated;

			/// <summary>
			/// Event invoked by Update() when a command is active
			/// </summary>
			public event CommandEventDelegate CommandActive;

			/// <summary>
			/// Initializes this binding
			/// </summary>
			/// <param name="cmd">Command to bind inputs to</param>
			public Binding( Command cmd )
			{
				m_Command = cmd;
			}

			/// <summary>
			/// Adds an input to this binding
			/// </summary>
			public void AddInput( IInput input )
			{
				m_Inputs.Add( input );
			}

			/// <summary>
			/// Returns the command associated with this binding
			/// </summary>
			public Command Command
			{
				get { return m_Command; }
			}

			/// <summary>
			/// Updates this binding
			/// </summary>
			public void Update( )
			{
				bool wasActive = ( m_LastActiveUpdate == m_UpdateCount );
				++m_UpdateCount;

				foreach ( IInput input in m_Inputs )
				{
					if ( input.IsActive )
					{
						CommandMessage message = input.CreateCommandMessage( m_Command );

						//	Invoke the Activated event if the command has only just gone active
						if ( !wasActive )
						{
							if ( CommandActivated != null )
							{
								CommandActivated( message );
							}
						}
						else
						{
							if ( CommandActive != null )
							{
								CommandActive( message );
							}
						}
						m_LastActiveUpdate = m_UpdateCount;
						break;
					}
				}

				foreach ( IInput input in m_Inputs )
				{
					if ( input.DeactivateOnUpdate )
					{
						input.IsActive = false;
					}
				}
			}

			private readonly Command		m_Command;
			private uint					m_UpdateCount;
			private uint					m_LastActiveUpdate;
			private readonly List< IInput >	m_Inputs = new List< IInput >( );
		}

		private readonly List< Binding > m_Bindings = new List< Binding >( );

		/// <summary>
		/// Returns an existing Binding object for a given command, or null
		/// </summary>
		private Binding FindBindingForCommand( Command cmd )
		{
			foreach ( Binding binding in m_Bindings )
			{
				if ( binding.Command == cmd )
				{
					return binding;
				}
			}
			return null;
		}


		#endregion
	}
}
