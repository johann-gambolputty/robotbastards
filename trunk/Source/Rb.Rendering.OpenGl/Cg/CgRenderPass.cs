using System;

namespace Rb.Rendering.OpenGl.Cg
{
	/// <summary>
	/// Implements a RenderPass object using a CGpass
	/// </summary>
	public class CgRenderPass : RenderPass
	{
		/// <summary>
		/// Sets the CGpass handle
		/// </summary>
		public CgRenderPass( IntPtr passHandle )
		{
			m_Pass = passHandle;
		}

		/// <summary>
		/// Sets the CG pass state
		/// </summary>
		public override void Begin( )
		{
			Tao.Cg.Cg.cgSetPassState( m_Pass );
			base.Begin( );
		}

		/// <summary>
		/// Resets the CG pass state
		/// </summary>
		public override void End( )
		{
			base.End( );
			Tao.Cg.Cg.cgResetPassState( m_Pass );
		}

		private IntPtr	m_Pass;
	}
}
