using System;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Clears target depth
	/// </summary>
	public class ClearTargetDepth : IAppliance
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

		#region IAppliance Members

		/// <summary>
		/// Clears the target depth
		/// </summary>
		public void Begin( )
		{
			Renderer.Inst.ClearDepth( m_Depth );
		}

		/// <summary>
		/// Does nothing (clearing doesn't really have an 'end')
		/// </summary>
		public void End( )
		{
		}

		#endregion

		private float	m_Depth = 1.0f;
	}
}
