
namespace Rb.Rendering.Interfaces.Objects.Cameras
{
	/// <summary>
	/// Projection camera interface
	/// </summary>
	public interface IProjectionCamera : ICamera3
	{
		/// <summary>
		/// Gets or sets the field of view
		/// </summary>
		float PerspectiveFovDegrees { get; set; }

		/// <summary>
		/// The z value of the near clipping plane. The greater the better, really (means more z buffer precision)
		/// </summary>
		float PerspectiveZNear { get; set; }

		/// <summary>
		/// The z value of the far clipping plane
		/// </summary>
		float PerspectiveZFar { get; set; }
	}
}
