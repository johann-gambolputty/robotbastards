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

		public IEnvironmentCollisions PointCollisions
		{
			get { return m_PointCollisions; }
			set
			{
				if ( m_PointCollisions != null )
				{
					RemoveChild( m_PointCollisions );
				}
				m_PointCollisions = value;
				if ( m_PointCollisions != null )
				{
					AddChild( m_PointCollisions );
				}
			}
		}

		public IEnvironmentCollisions EntityCollisions
		{
			get { return m_EntityCollisions; }
			set
			{
				if ( m_EntityCollisions != null )
				{
					RemoveChild( m_EntityCollisions );
				}
				m_EntityCollisions = value;
				if ( m_EntityCollisions != null )
				{
					AddChild( m_EntityCollisions );
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

		private IEnvironmentCollisions m_PointCollisions;
		private IEnvironmentCollisions m_EntityCollisions;
		private IEnvironmentGraphics m_Graphics;

		#endregion
	}
}
