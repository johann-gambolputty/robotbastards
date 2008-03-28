using System;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.OpenGl.Cg
{
	/// <summary>
	/// Implements a RenderPass object using a CGpass
	/// </summary>
	public class CgPass : IPass
	{
		/// <summary>
		/// Sets the CGpass handle
		/// </summary>
		public CgPass( IntPtr passHandle )
		{
			m_Pass = passHandle;
		}

		/// <summary>
		/// Sets the CG pass state
		/// </summary>
		public void Begin( )
		{
			Tao.Cg.Cg.cgSetPassState( m_Pass );
		}

		/// <summary>
		/// Resets the CG pass state
		/// </summary>
		public void End( )
		{
			Tao.Cg.Cg.cgResetPassState( m_Pass );
		}

		private readonly IntPtr m_Pass;
	}
}
