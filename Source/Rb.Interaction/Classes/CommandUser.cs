using System;
using Rb.Core.Utils;
using Rb.Interaction.Interfaces;

namespace Rb.Interaction.Classes
{
	/// <summary>
	/// Command user
	/// </summary>
	public class CommandUser : ICommandUser
	{
		/// <summary>
		/// Event raised when a command is triggered by this user
		/// </summary>
		public event Action<CommandTriggerData> CommandTriggered;

		/// <summary>
		/// User name
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// User unique identifier
		/// </summary>
		public int Id
		{
			get { return m_Id; }
		}

		/// <summary>
		/// Sets up this command user
		/// </summary>
		/// <param name="name">User name</param>
		/// <param name="id">User unique identifier</param>
		public CommandUser( string name, int id )
		{
			m_Name = name;
			m_Id = id;
		}

		/// <summary>
		/// Raises the CommandTriggered event
		/// </summary>
		/// <param name="triggerData">Trigger data to pass to the event</param>
		public void OnCommandTriggered( CommandTriggerData triggerData )
		{
			Arguments.CheckNotNull( triggerData, "triggerData" );
			if ( triggerData.User.Id != Id )
			{
				throw new ArgumentException( string.Format( "Trigger data did not contain the correct user (was user \"{0}\", expected user \"{1}\"", triggerData.User.Name, Name ) );
			}
			if ( CommandTriggered != null )
			{
				CommandTriggered( triggerData );
			}
		}

		/// <summary>
		/// Returns the default command user
		/// </summary>
		/// <remarks>
		/// The default command user is not present in any CommandUserRegistry.
		/// Should be used for single-user applications only.
		/// </remarks>
		public static CommandUser Default
		{
			get { return s_User; }
		}

		#region Private Members

		private readonly string m_Name;
		private readonly int m_Id;
		private readonly static CommandUser s_User = new CommandUser( "me", 0 );

		#endregion
	}
}
