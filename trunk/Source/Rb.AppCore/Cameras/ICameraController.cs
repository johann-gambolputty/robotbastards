
using Rb.Rendering.Cameras;

namespace Rb.AppCore.Cameras
{
	/// <summary>
	/// Interface used by camera controllers
	/// </summary>
	public interface ICameraController
	{
		/// <summary>
		/// Access to the camera being controlled
		/// </summary>
		CameraBase Camera
		{
			get;
			set;
		}

		/// <summary>
		/// Enables or disables the controller
		/// </summary>
		bool Enabled
		{
			get;
			set;
		}
	}
}
