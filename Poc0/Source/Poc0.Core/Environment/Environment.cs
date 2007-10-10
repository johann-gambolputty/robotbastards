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
		/// <summary>
		/// Default constructor
		/// </summary>
		public Environment( )
		{
			InitialiseComponents( );
		}

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

		#region Private stuff

		private EnvironmentGraphics m_Graphics;

		private void InitialiseComponents( )
		{
			m_Graphics = Graphics.Factory.Create<EnvironmentGraphics>( );
			AddChild( m_Graphics );
		}

		#endregion
	}
}
