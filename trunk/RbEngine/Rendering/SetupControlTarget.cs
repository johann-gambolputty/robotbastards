using System;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Sets up a control as a target
	/// </summary>
	public class SetupControlTarget : IApplicable
	{
		/// <summary>
		/// Sets the control as the target
		/// </summary>
		public SetupControlTarget( System.Windows.Forms.Control control )
		{
			m_Control = control;
		}

		#region IApplicable Members

		/// <summary>
		/// Sets up the viewport to the control
		/// </summary>
		public void Apply( )
		{
			Renderer.Inst.SetViewport( 0, 0, m_Control.Width, m_Control.Height );
		}

		#endregion


		private System.Windows.Forms.Control m_Control;
	}
}
