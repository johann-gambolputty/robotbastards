using System;

namespace RbEngine.Cameras
{

	/// <summary>
	/// Base class for cameras
	/// </summary>
	public abstract class CameraBase : Rendering.IApplicable
	{

		/// <summary>
		/// Creates a default controller for this camera, for the specified windows control
		/// </summary>
		public abstract Object	CreateDefaultController( System.Windows.Forms.Control control, Scene.SceneDb scene );

		/// <summary>
		/// Applies camera transforms to the current renderer
		/// </summary>
		public virtual void		Apply( )
		{
			Rendering.Renderer.Inst.Camera = this;
		}
	}
}
