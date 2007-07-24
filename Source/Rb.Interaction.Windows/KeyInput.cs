using System.Windows.Forms;

namespace Rb.Interaction.Windows
{
	/// <summary>
	/// Possible key states
	/// </summary>
	public enum KeyState
	{
		Up,
		Down,
		Held
	}

    /// <summary>
    /// Keyboard input
    /// </summary>
    public class KeyInput : Input
    {
		/// <summary>
		/// Setup constructor
		/// </summary>
        /// <param name="context">The input context</param>
		/// <param name="key">Key to check for</param>
		/// <param name="state">Key state to detect</param>
		public KeyInput( InputContext context, Keys key, KeyState state ) :
			base( context )
		{
			m_State = state;
			m_Key = key;

			( ( Control )context.Control ).KeyDown += new KeyEventHandler( OnKeyDown );
            ( ( Control )context.Control ).KeyUp += new KeyEventHandler( OnKeyUp );
		}

		/// <summary>
		/// Handles a key down even in the scene view's control
		/// </summary>
		private void OnKeyDown( object sender, KeyEventArgs args )
		{
			if ( args.KeyCode == m_Key )
			{
				IsActive = ( m_State == KeyState.Down ) || ( m_State == KeyState.Held );
			}
		}

		/// <summary>
		/// Handles a key up even in the scene view's control
		/// </summary>
		private void OnKeyUp( object sender, KeyEventArgs args )
		{
			if ( args.KeyCode == m_Key )
			{
                IsActive = ( m_State == KeyState.Up );
			}
		}

		/// <summary>
		/// Returns true if the key state being detected an edge state
		/// </summary>
		public override bool DeactivateOnUpdate
		{
			get
			{
				return m_State == KeyState.Down || m_State == KeyState.Up;
			}
		}

		private Keys m_Key;
		private KeyState m_State;

    }
}
