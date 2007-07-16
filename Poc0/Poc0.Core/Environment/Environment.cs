using System;
using System.Collections.Generic;
using System.Text;
using Rb.World;

namespace Poc0.Core.Environment
{
	class Environment : ISceneObject
	{
		#region Private stuff

		private Collisions	m_Collisions;
		private Graphics	m_Graphics;

		#endregion

		#region ISceneObject Members

		public void SetSceneContext( Scene scene )
		{
			scene.Renderables.Add( m_Graphics );
		}

		#endregion
	}
}
