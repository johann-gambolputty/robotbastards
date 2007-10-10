using System;
using System.Collections.Generic;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;

namespace Rb.Tools.LevelEditor.Core
{
	/// <summary>
	/// The editor scene encapsulates the runtime scene, and stores extra editor state
	/// </summary>
	[Serializable]
	public class EditorScene : Scene
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="runtimeScene">The encapsulated runtime scene</param>
		public EditorScene( Scene runtimeScene )
		{
			m_RuntimeScene = runtimeScene;
		}

		/// <summary>
		/// Runtime scene
		/// </summary>
		public Scene RuntimeScene
		{
			get { return m_RuntimeScene; }
		}

		private readonly Scene m_RuntimeScene;
	}
}
