using System;
using Rb.Core.Components;

namespace Poc0.Core.Environment
{
	/// <summary>
	/// Scene environment
	/// </summary>
	[Serializable]
	public class Environment : Component
	{
		#region Public members

		public IEnvironmentCollisions Collisions
		{
			get { return m_Collisions; }
			set
			{
				if ( m_Collisions != null )
				{
					RemoveChild( m_Collisions );
				}
				m_Collisions = value;
				if ( m_Collisions != null )
				{
					AddChild( m_Collisions );
				}
			}
		}

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

		private IEnvironmentCollisions m_Collisions;
		private IEnvironmentGraphics m_Graphics;

		#endregion
	}
}
