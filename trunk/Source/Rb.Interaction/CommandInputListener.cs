using System;
using Rb.Core.Components;

namespace Rb.Interaction
{
	/// <summary>
	/// Objects of this class subscribe to events generated by commands from a specified command list. These commands are turned into CommandMessage objects
	/// and sent to the parent object
	/// </summary>
	public class CommandInputListener : IChild
    {
        #region Construction

        /// <summary>
        /// Default constructor
        /// </summary>
        public CommandInputListener( )
        {
        }

        /// <summary>
        /// Setup constructor
        /// </summary>
        public CommandInputListener( object target, string commandListName )
        {
            m_Target = target;
            Commands = CommandListManager.Inst.Get( commandListName );
        }

        /// <summary>
        /// Setup constructor
        /// </summary>
        public CommandInputListener( object target, CommandList commands )
        {
            m_Target = target;
            Commands = commands;
        }

        #endregion

        #region	Command list

        /// <summary>
		/// Sets the command list that this object is associated with
		/// </summary>
		public CommandList Commands
		{
			set
			{
				if ( m_Commands != null )
				{
					m_Commands.CommandActivated -= new CommandEventDelegate( CommandActivated );
					m_Commands.CommandActive -= new CommandEventDelegate( CommandActive );
				}
				m_Commands = value;
				if ( m_Commands != null )
				{
					m_Commands.CommandActivated += new CommandEventDelegate( CommandActivated );
					m_Commands.CommandActive += new CommandEventDelegate( CommandActive );
				}
			}
			get
			{
				return m_Commands;
			}
		}

		/// <summary>
		/// Sets the command list that this object is associated with, from the name of the list
		/// </summary>
		public string CommandListName
		{
			set
			{
				Commands = CommandListManager.Inst.Get( value );
				if ( Commands == null )
				{
					throw new ApplicationException( string.Format( "Could not find the command list named \"{0}\"", value ) );
				}
			}
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Command messages are sent to this object. If it's not set, it defaults to the parent object
		/// </summary>
		public object Target
		{
			get { return m_Target; }
			set { m_Target = value; }
		}

		#endregion

		#region IChild Members

		/// <summary>
		/// Called when this object is added to a parent
		/// </summary>
		public void AddedToParent( Object parentObject )
		{
			if ( Target == null )
			{
				Target = parentObject;
			}
		}

		#endregion

		#region	Private stuff

		private Object		m_Target;
		private CommandList	m_Commands;

		/// <summary>
		/// Called when a command is activated
		/// </summary>
		private void CommandActivated( IInput input, CommandMessage message )
		{
			if ( Target != null )
			{
				( ( IMessageHandler )Target ).HandleMessage( message );
			}
		}

		/// <summary>
		/// Called when a command is active
		/// </summary>
		private void CommandActive( IInput input, CommandMessage message )
		{
			if ( Target != null )
			{
				( ( IMessageHandler )Target ).HandleMessage( message );
			}
		}

		#endregion

	}
}
