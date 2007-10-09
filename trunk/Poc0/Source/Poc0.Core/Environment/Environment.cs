using System;
using Rb.Core.Components;
using Rb.Rendering;
using Rb.World;

namespace Poc0.Core.Environment
{
	/// <summary>
	/// Scene environment
	/// </summary>
	[Serializable]
	public class Environment : ISceneObject
	{
		#region Public members

		/// <summary>
		/// Sets the root wall node
		/// </summary>
		public WallNode Walls
		{
			set
			{
				m_Graphics.Walls = value;
			}
		}

		#endregion

		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to a scene
		/// </summary>
		/// <param name="scene">Scene</param>
		public void AddedToScene( Scene scene )
		{
			//	TODO: AP: Can this be done during construction?
			m_Graphics = Graphics.Factory.Create< EnvironmentGraphics >( );
		}

		/// <summary>
		/// Called when this object is removed from a scene
		/// </summary>
		/// <param name="scene">Scene</param>
		public void RemovedFromScene( Scene scene )
		{
		}

		#endregion

		#region Private stuff

		private EnvironmentGraphics m_Graphics;

		#endregion
	}
}
