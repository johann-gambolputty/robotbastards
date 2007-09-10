using System;
using Rb.World;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// The EditorScene also stores a child scene that is serialised for the game only
	/// </summary>
	[Serializable]
	public class EditorScene : Scene
	{
		/// <summary>
		/// Runtime scene
		/// </summary>
		public Scene RuntimeScene
		{
			get { return m_RuntimeScene; }
		}

		private readonly Scene m_RuntimeScene = new Scene( );
	}
}
