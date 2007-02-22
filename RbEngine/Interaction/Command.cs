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
		/// Adds an input binding to this control
		/// </summary>
		/// <param name="binding"></param>
		public void					AddBinding( CommandInputBinding binding )
		{
			m_Bindings.Add( binding );
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
				return m_Bindings;
			}
		}

		/// <summary>
		/// Binds this command to a given control
		/// </summary>
		/// <param name="control">Control to bind to</param>
		/// <seealso cref="CommandInputBinding">CommandInputBinding</seealso>
		public void					BindToControl( System.Windows.Forms.Control control )
		{
			foreach ( CommandInputBinding binding in m_Bindings )
			{
				binding.BindToControl( control );
			}
		}

		/// <summary>
		/// Updates the control
		/// </summary>
		public void					Update( Components.IMessageHandler commandTarget )
		{
			foreach ( CommandInputBinding binding in m_Bindings )
			{
				if ( binding.Active )
				{
					commandTarget.HandleMessage( new CommandMessage( this ) );
					return;
				}
			}
		}

		/// <summary>
		/// Active event delegate type
		/// </summary>
		public delegate void		ActiveDelegate( );

		/// <summary>
		/// Event, invoked by this command when it is active
		/// </summary>
		public event ActiveDelegate	Active;

		#region	Private stuff

		private string				m_Name;
		private string				m_Description;
		private ArrayList			m_Bindings = new ArrayList( );
		private ushort				m_Id;

		#endregion
	}
}
