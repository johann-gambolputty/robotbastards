using System;
using System.Collections.Generic;
using Rb.Interaction.Interfaces;
using Rb.Core.Utils;

namespace Rb.Interaction.Classes
{
	/// <summary>
	/// Keeps track of all command users
	/// </summary>
	public class CommandUserRegistry
	{
		/// <summary>
		/// Registers a user
		/// </summary>
		/// <param name="user">User to register</param>
		/// <exception cref="ArgumentNullException">Thrown if user is null</exception>
		/// <exception cref="ArgumentException">Thrown if user already exists in the registry</exception>
		public void Register( ICommandUser user )
		{
			Arguments.CheckNotNull( user, "user" );
			InteractionLog.Info( "Registering user \"{0}\" ({1})", user.Name, user.Id );
			if ( m_Users.ContainsKey( user.Id ) )
			{
				throw new ArgumentException( string.Format( "User (name: {0}, id: {1}) is already registered", user.Name, user.Id ), "user" );
			}
			m_Users[ user.Id ] = user;
		}

		/// <summary>
		/// Finds a user by its identifier
		/// </summary>
		/// <param name="userId">User identifier</param>
		/// <returns>Returns the found user</returns>
		/// <exception cref="KeyNotFoundException">Thrown if userId is not present in the registry</exception>
		public ICommandUser FindById( int userId )
		{
			return m_Users[ userId ];
		}

		#region Private Members

		private readonly Dictionary<int, ICommandUser> m_Users = new Dictionary<int, ICommandUser>( );

		#endregion
	}

}
