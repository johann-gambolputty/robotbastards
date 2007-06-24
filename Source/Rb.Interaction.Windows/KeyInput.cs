using System.Windows.Forms;

namespace Rb.Interaction.Windows
{
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
		public KeyInput( InputContext context, Keys key ) :
			base( context )
		{
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
                IsActive = true;
			}
		}

		/// <summary>
		/// Handles a key up even in the scene view's control
		/// </summary>
		private void OnKeyUp( object sender, KeyEventArgs args )
		{
			if ( args.KeyCode == m_Key )
			{
                IsActive = false;
			}
		}

		private Keys m_Key;

    }
}
