using System;
using System.Windows.Forms;

namespace RbEngine.Interaction
{
	/// <summary>
	/// Binds a command to a keypress
	/// </summary>
	public class CommandKeyInputBinding : CommandInputBinding
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="key">Key to check for</param>
		public CommandKeyInputBinding( Keys key )
		{
			m_Key = key;
		}

		/// <summary>
		/// Binds to the specified control
		/// </summary>
		/// <param name="control"></param>
		public override void		BindToControl( Control control )
		{
			control.KeyDown += new KeyEventHandler( OnKeyDown );
			control.KeyUp += new KeyEventHandler( OnKeyUp );
		}

		private Keys				m_Key;

		private void				OnKeyDown( object sender, KeyEventArgs args )
		{
			if ( args.KeyCode == m_Key )
			{
				m_Active = true;
			}
		}

		private void				OnKeyUp( object sender, KeyEventArgs args )
		{
			if ( args.KeyCode == m_Key )
			{
				m_Active = false;
			}
		}
	}
}
