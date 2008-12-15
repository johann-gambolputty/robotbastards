
using Rb.Interaction.Interfaces;

namespace Rb.Interaction.Classes
{
	/// <summary>
	/// States for binary inputs
	/// </summary>
	public enum BinaryInputState
	{
		Down,
		Held,
		Up
	}

	/// <summary>
	/// Helper base class that monitors input that can be classified as a binary state change (e.g. key presses, mouse clicks)
	/// </summary>
	public abstract class CommandBinaryInputBindingMonitor : CommandInputBindingMonitor
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="binding">Input binding</param>
		/// <param name="user">Command user</param>
		public CommandBinaryInputBindingMonitor( CommandInputBinding binding, ICommandUser user ) :
			base( binding, user )
		{
		}

		/// <summary>
		/// Returns true if the crrent state of the key or button matches the monitored state
		/// </summary>
		public override bool IsActive
		{
			get { return m_Active; }
		}

		/// <summary>
		/// Updates the value returned by IsActive
		/// </summary>
		public override bool Update( )
		{
			m_Active = false;
			switch ( m_MonitorState )
			{
				case BinaryInputState.Up:
					m_Active = ( !m_IsDown ) && ( m_DownCount > 0 );
					break;
				case BinaryInputState.Held:
					m_Active = m_IsDown;
					break;
				case BinaryInputState.Down:
					m_Active = ( m_IsDown ) && ( m_DownCount == 1 );
					break;
			}
			if ( m_IsDown )
			{
				++m_DownCount;
			}
			else
			{
				m_DownCount = 0;
			}
			return m_Active;
		}

		#region Protected Members

		/// <summary>
		/// Gets/sets the state being monitored
		/// </summary>
		protected BinaryInputState MonitorState
		{
			get { return m_MonitorState; }
			set { m_MonitorState = value; }
		}

		/// <summary>
		/// The key or button is pressed
		/// </summary>
		protected void OnDown( )
		{
			if ( !m_IsDown )
			{
				m_IsDown = true;
				m_DownCount = 1;
			}
		}

		/// <summary>
		/// The key or button is released
		/// </summary>
		protected void OnUp( )
		{
			m_IsDown = false;
			m_DownCount = 1;
		}

		#endregion


		#region Private Members

		private BinaryInputState m_MonitorState;
		private bool m_IsDown;
		private int m_DownCount;
		private bool m_Active;

		#endregion
	}

}
