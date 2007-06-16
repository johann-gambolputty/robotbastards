using System.Windows.Forms;

namespace Rb.Rendering.Windows
{
	/// <summary>
	/// Extends the IDisplaySetup interface to include windows-specific setup options
	/// </summary>
	public interface IWindowsDisplaySetup : IDisplaySetup
	{
		/// <summary>
		/// Returns the class styles required by the owner window
		/// </summary>
		int ClassStyles
		{
			get;
		}

		/// <summary>
		/// Returns control styles that the control should apply
		/// </summary>
		ControlStyles AddStyles
		{
			get;
		}

		/// <summary>
		/// Returns control styles that the control should remove
		/// </summary>
		ControlStyles RemoveStyles
		{
			get;
		}

		/// <summary>
		/// Creates an image that is displayed in this control in design mode
		/// </summary>
		System.Drawing.Image CreateDesignImage( );

	}
}
