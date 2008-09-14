using System;
using System.Collections.Generic;
using System.Text;
using Rb.Core.Utils;

namespace Rb.Interaction
{
	/// <summary>
	/// Manages <see cref="CommandUser"/> objects, updating them according to an internal clock
	/// </summary>
	public class CommandUserManager
	{
		/// <summary>
		/// Gets the class singleton
		/// </summary>
		public static CommandUserManager Instance
		{
			get { return s_Singleton; }
		}

		/// <summary>
		/// Initialises the manger
		/// </summary>
		public CommandUserManager( )
		{
			m_Clock = new Clock( "UpdateClock", 20, false );
			m_Clock.Subscribe( OnUpdate );
		}

		/// <summary>
		/// Adds a user to the manager
		/// </summary>
		public void AddUser( CommandUser user )
		{
			if ( m_Users.Contains( user ) )
			{
				throw new ArgumentException( "User is already registered in command manager (registration is part of user constructor)" );
			}
			m_Users.Add( user );
		}

		/// <summary>
		/// Removes a user from the manager
		/// </summary>
		public void RemoveUser( CommandUser user )
		{
			m_Users.Remove( user );
		}

		/// <summary>
		/// Gets the clock, that's used to update users
		/// </summary>
		public Clock UpdateClock
		{
			get { return m_Clock; }
		}

		#region Private membmers

		private readonly List<CommandUser> m_Users = new List<CommandUser>( );
		private readonly Clock m_Clock;
		private static readonly CommandUserManager s_Singleton = new CommandUserManager( );

		/// <summary>
		/// Called by the update clock. Updates all users
		/// </summary>
		private void OnUpdate( Clock clock )
		{
			foreach ( CommandUser user in m_Users )
			{
				user.Update( );
			}
		}

		#endregion
	}
}
