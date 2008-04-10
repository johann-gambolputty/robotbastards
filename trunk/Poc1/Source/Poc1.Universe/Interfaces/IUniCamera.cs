using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects.Cameras;

namespace Poc1.Universe.Interfaces
{
	/// <summary>
	/// Universe camera
	/// </summary>
	public interface IUniCamera : ICamera
	{
		/// <summary>
		/// Gets the position of the camera
		/// </summary>
		UniPoint3 Position
		{
			get;
		}

		/// <summary>
		/// Gets the view frame of the camera (orientation matrix without translation)
		/// </summary>
		Matrix44 Frame
		{
			get;
		}

		/// <summary>
		/// Creates a pick ray from a screen position
		/// </summary>
		/// <param name="x">Screen X position</param>
		/// <param name="y">Screen Y position</param>
		/// <returns>Returns a universe ray</returns>
		UniRay3 PickRay( int x, int y );
	}
}
