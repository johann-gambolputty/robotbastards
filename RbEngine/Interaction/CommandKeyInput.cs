using System;
using System.Windows.Forms;

namespace RbEngine.Interaction
{
	/// <summary>
	/// Input from keystrokes
	/// </summary>
	public class CommandKeyInput : CommandInput
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="key">Key to check for</param>
		public CommandKeyInput( Keys key )
		{
			m_Key = key;
		}

		/// <summary>
		/// Creates a CommandKeyInputBinding associated with the specified view
		/// </summary>
		public override CommandInputBinding BindToView( Scene.SceneView view )
		{
			return new CommandKeyInputBinding( view, m_Key );
		}

		private Keys m_Key;
	}
}