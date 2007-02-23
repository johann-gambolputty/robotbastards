using System;
using System.Collections;

namespace RbEngine.Interaction
{
	/// <summary>
	/// User command
	/// </summary>
	public class Command
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="name">Command name</param>
		/// <param name="description">Command description</param>
		/// <param name="id">Identifier for the command</param>
		public Command( string name, string description, ushort id )
		{
			m_Name			= name;
			m_Description	= description;
			m_Id			= id;
		}

		/// <summary>
		/// Adds an input binding to this command
		/// </summary>
		/// <param name="binding"></param>
		public virtual void			AddBinding( CommandInputBinding binding )
		{
			m_BindingTemplates.Add( binding );
		}

		/// <summary>
		/// Command identifier
		/// </summary>
		public ushort				Id
		{
			get
			{
				return m_Id;
			}
		}

		/// <summary>
		/// The list of CommandInputBinding objects
		/// </summary>
		public ArrayList			Bindings
		{
			get
			{
				return m_BindingTemplates;
			}
		}

		/// <summary>
		/// Binds this command to a given client
		/// </summary>
		/// <param name="client">Client to bind to</param>
		/// <seealso cref="CommandInputBinding">CommandInputBinding</seealso>
		public void					BindToClient( Network.Client client )
		{
			foreach ( CommandInputBinding binding in m_BindingTemplates )
			{
				m_ClientBindings.Add( binding.BindToClient( client ) );
			}
		}

		/// <summary>
		/// Updates the command
		/// </summary>
		public void					Update( Components.IMessageHandler commandTarget )
		{
			foreach ( CommandInputBinding.ClientBinding binding in m_ClientBindings )
			{
				if ( binding.Active )
				{
					CommandMessage msg = GenerateMessageFromActiveBinding( binding );
					if ( msg != null )
					{
						commandTarget.HandleMessage( msg );
						return;
					}
				}
			}
		}

		/// <summary>
		/// Generates a CommandMessage form an active input binding
		/// </summary>
		protected virtual CommandMessage	GenerateMessageFromActiveBinding( CommandInputBinding.ClientBinding binding )
		{
			return new CommandMessage( this, binding.Client );
		}

		/// <summary>
		/// Active event delegate type
		/// </summary>
	//	public delegate void		ActiveDelegate( );

		/// <summary>
		/// Event, invoked by this command when it is active
		/// </summary>
	//	public event ActiveDelegate	Active;

		#region	Private stuff

		private string				m_Name;
		private string				m_Description;
		private ArrayList			m_BindingTemplates	= new ArrayList( );
		private ArrayList			m_ClientBindings	= new ArrayList( );
		private ushort				m_Id;

		#endregion
	}
}
