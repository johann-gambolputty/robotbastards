using System;

namespace RbEngine.Cameras
{

	/// <summary>
	/// Base class for cameras. Provides projection setup, and frustum support
	/// </summary>
	public abstract class CameraBase
	{

		/// <summary>
		/// Creates a default controller for this camera, for the specified windows control
		/// </summary>
		public abstract Object	CreateDefaultController( System.Windows.Forms.Control control, Scene.SceneDb scene );

		/// <summary>
		/// Applies camera transforms to the current renderer
		/// </summary>
		public abstract void	Apply( int width, int height );

	}
}
