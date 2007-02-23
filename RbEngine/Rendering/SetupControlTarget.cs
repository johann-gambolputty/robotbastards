using System;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Sets up a control as a target
	/// </summary>
	public class SetupControlTarget : IApplicable
	{
		#region IApplicable Members

		/// <summary>
		/// Sets up the viewport to the control
		/// </summary>
		public void Apply( )
		{
			System.Windows.Forms.Control control = Renderer.Inst.CurrentControl;
			Renderer.Inst.SetViewport( 0, 0, control.Width, control.Height );
		}

		#endregion

	}
}
