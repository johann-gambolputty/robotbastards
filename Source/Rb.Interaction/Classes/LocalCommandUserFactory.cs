using System;
using System.Collections.Generic;
using Rb.Interaction.Interfaces;

namespace Rb.Interaction.Classes
{
	/// <summary>
	/// Creates command users
	/// </summary>
	public class LocalCommandUserFactory : ICommandUserFactory
	{
		#region ICommandUserFactory Members

		/// <summary>
		/// Creates a new command user
		/// </summary>
		/// <param name="name">Command user name</param>
		/// <returns>Returns the new command user</returns>
		public ICommandUser Create( string name )
		{
			if ( name == null )
			{
				throw new ArgumentNullException( "name" );
			}
			CommandUser user = new CommandUser( name, m_Users.Count );
			m_Users.Add( user );
			return user;
		}

		#endregion

		#region Private Members

		private List<CommandUser> m_Users = new List<CommandUser>( );

		#endregion
	}

}
