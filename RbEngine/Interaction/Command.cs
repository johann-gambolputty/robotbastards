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
		public Command( string name, string description )
		{
			m_Name = name;
			m_Description = description;
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

		#endregion
	}
}
