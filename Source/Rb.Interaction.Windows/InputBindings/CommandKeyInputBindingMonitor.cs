using System;
using System.Windows.Forms;
using Rb.Interaction.Classes;
using Rb.Interaction.Classes.InputBindings;
using Rb.Interaction.Interfaces;

namespace Rb.Interaction.Windows.InputBindings
{
	/// <summary>
	/// Monitors key presses in a control. Initialized from a <see cref="CommandKeyInputBinding"/>
	/// </summary>
	public class CommandKeyInputBindingMonitor : CommandBinaryInputBindingMonitor
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="control">Control to bind to</param>
		/// <param name="binding">Binding definition</param>
		/// <param name="user">Originating user</param>
		public CommandKeyInputBindingMonitor( Control control, CommandKeyInputBinding binding, ICommandUser user ) :
			base( binding, user )
		{
			m_Control = control;
			m_Key = ( Keys )Enum.Parse( typeof( Keys ), binding.KeyName, true );
			MonitorState = binding.KeyState;
		}

		/// <summary>
		/// Returns the key binding definition that this monitor was created from
		/// </summary>
		public CommandKeyInputBinding KeyBinding
		{
			get { return ( CommandKeyInputBinding )Binding; }
		}

		/// <summary>
		/// Starts monitoring
		/// </summary>
		public override void Start( )
		{
			m_Control.KeyDown += OnKeyDown;
			m_Control.KeyUp += OnKeyUp;
		}

		/// <summary>
		/// Stops monitoring
		/// </summary>
		public override void Stop( )
		{
			m_Control.KeyDown -= OnKeyDown;
			m_Control.KeyUp -= OnKeyUp;
		}

		#region Private Members

		private readonly Control m_Control;
		private readonly Keys m_Key;

		/// <summary>
		/// Handles the control KeyDown event
		/// </summary>
		private void OnKeyDown( object sender, KeyEventArgs args )
		{
			if ( args.KeyCode == m_Key )
			{
				OnDown( );
			}
		}

		/// <summary>
		/// Handles the control KeyUp event
		/// </summary>
		private void OnKeyUp( object sender, KeyEventArgs args )
		{
			if ( args.KeyCode == m_Key )
			{
				OnUp( );
			}
		}

		#endregion
	}
}
