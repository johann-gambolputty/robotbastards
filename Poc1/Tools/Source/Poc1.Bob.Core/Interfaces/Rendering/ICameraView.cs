
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Cameras;

namespace Poc1.Bob.Core.Interfaces.Rendering
{
	/// <summary>
	/// Represents the view from a single camera
	/// </summary>
	public interface ICameraView
	{
		/// <summary>
		/// Gets/sets the camera used by the view
		/// </summary>
		ICamera Camera
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the object rendered by this view
		/// </summary>
		IRenderable Renderable
		{
			get; set;
		}
	}
}
