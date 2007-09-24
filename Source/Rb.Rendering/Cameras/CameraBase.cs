
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
			Graphics.Renderer.Camera = this;
		}

		/// <summary>
		/// Should remove camera transforms from the current renderer
		/// </summary>
		public virtual void End( )
		{
			Graphics.Renderer.Camera = null;
		}
	}
}
