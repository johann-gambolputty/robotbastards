using System;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Clears target depth
	/// </summary>
	public class ClearTargetDepth : IApplicable
	{
		/// <summary>
		/// Clear depth
		/// </summary>
		public float	Depth
		{
			get { return m_Depth;	}
			set { m_Depth = value;	}
		}

		/// <summary>
		/// Default clear depth to 1
		/// </summary>
		public ClearTargetDepth( )
		{
		}

		/// <summary>
		/// Sets the clear depth
		/// </summary>
		/// <param name="depth">Value to clear the target depth to</param>
		public ClearTargetDepth( float depth )
		{
			m_Depth = depth;
		}

		#region IApplicable Members

		/// <summary>
		/// Clears the target depth
		/// </summary>
		public void Apply( )
		{
			Renderer.Inst.ClearDepth( m_Depth );
		}

		#endregion

		private float	m_Depth = 1.0f;
	}
}
