
using Rb.Core.Components;

namespace Rb.Rendering.Cameras
{

	/// <summary>
	/// Base class for cameras
	/// </summary>
	public abstract class CameraBase : Component, IPass
	{
		/// <summary>
		/// Applies camera transforms to the current renderer
		/// </summary>
		public virtual void Begin( )
		{
			Renderer.Inst.Camera = this;
		}

		/// <summary>
		/// Should remove camera transforms from the current renderer
		/// </summary>
		public virtual void End( )
		{
			Renderer.Inst.Camera = null;
		}
	}
}
