using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects.Cameras;

namespace Poc1.Universe.Interfaces
{
	/// <summary>
	/// Universe camera
	/// </summary>
	public interface IUniCamera : ICamera3, IProjectionCamera
	{
		/// <summary>
		/// Gets the position of the camera
		/// </summary>
		UniPoint3 Position
		{
			get;
		}
	}
}
