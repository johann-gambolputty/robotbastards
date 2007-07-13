using System;
using System.Collections.Generic;
using System.Text;
using Rb.Rendering.Cameras;

namespace Rb.TestApp
{
	/// <summary>
	/// Interface used by camera controllers. For the benefit of the <see cref="MultiCameraController"/>
	/// </summary>
	internal interface ICameraController
	{
		/// <summary>
		/// Access to the camera being controlled
		/// </summary>
		CameraBase Camera
		{
			get; set;
		}

		/// <summary>
		/// Enables or disables the controller
		/// </summary>
		bool Enabled
		{
			get; set;
		}
	}
}
