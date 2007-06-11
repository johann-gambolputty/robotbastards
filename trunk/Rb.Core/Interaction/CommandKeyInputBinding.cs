using System;
using System.Windows.Forms;

namespace Rb.Core.Interaction
{
	/// <summary>
	/// Binds a command to a keypress
	/// </summary>
	public class CommandKeyInputBinding : CommandInputBinding
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="view">The view that this binding is attached to</param>
		/// <param name="key">Key to check for</param>
		public CommandKeyInputBinding( Command cmd, Scene.SceneView view, Keys key ) :
			base( cmd, view )
		{
			m_Key = key;

			view.Control.KeyDown	+= new KeyEventHandler( OnKeyDown );
			view.Control.KeyUp		+= new KeyEventHandler( OnKeyUp );
		}

		/// <summary>
		/// Handles a key down even in the scene view's control
		/// </summary>
		private void OnKeyDown( object sender, KeyEventArgs args )
		{
			if ( args.KeyCode == m_Key )
			{
				Active = true;
			}
		}

		/// <summary>
		/// Handles a key up even in the scene view's control
		/// </summary>
		private void OnKeyUp( object sender, KeyEventArgs args )
		{
			if ( args.KeyCode == m_Key )
			{
				Active = false;
			}
		}

		private Keys m_Key;
	}
}