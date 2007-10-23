using System;
using Rb.Core.Components;
using Rb.Rendering;

namespace Poc0.Core.Environment
{
	/// <summary>
	/// Scene environment
	/// </summary>
	[Serializable]
	public class Environment : Component
	{
		#region Public members

		public IEnvironmentGraphics Graphics
		{
			get { return m_Graphics; }
			set
			{
				if ( m_Graphics != null )
				{
					RemoveChild( m_Graphics );
				}
				m_Graphics = value;
				if ( m_Graphics != null )
				{
					AddChild( m_Graphics );
				}
			}
		}

		#endregion

		#region Private stuff

		private IEnvironmentGraphics m_Graphics;

		#endregion
	}
}
