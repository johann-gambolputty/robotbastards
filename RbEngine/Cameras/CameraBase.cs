using System;

namespace RbEngine.Cameras
{

	/// <summary>
	/// Base class for cameras
	/// </summary>
	public abstract class CameraBase : Rendering.IAppliance
	{

		/// <summary>
		/// Creates a default controller for this camera, for the specified windows control
		/// </summary>
		public abstract Object	CreateDefaultController( System.Windows.Forms.Control control, Scene.SceneDb scene );

		/// <summary>
		/// Applies camera transforms to the current renderer
		/// </summary>
		public virtual void		Begin( )
		{
			Rendering.Renderer.Inst.Camera = this;
		}

		/// <summary>
		/// Should remove camera transforms from the current renderer
		/// </summary>
		public virtual void		End( )
		{
			Rendering.Renderer.Inst.Camera = null;
		}
	}
}
