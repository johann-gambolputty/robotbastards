using System;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Sets up a control as a target
	/// </summary>
	public class SetupControlTarget : IAppliance
	{
		#region IAppliance Members

		/// <summary>
		/// Sets up the viewport to the control
		/// </summary>
		public void Begin( )
		{
			System.Windows.Forms.Control control = Renderer.Inst.CurrentControl;
			Renderer.Inst.SetViewport( 0, 0, control.Width, control.Height );
		}

		/// <summary>
		/// Does nothing 
		/// </summary>
		public void End( )
		{
		}

		#endregion

	}
}
