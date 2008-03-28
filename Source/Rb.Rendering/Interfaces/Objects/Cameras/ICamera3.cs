using Rb.Core.Maths;

namespace Rb.Rendering.Interfaces.Objects.Cameras
{
	/// <summary>
	/// 3d camera interface
	/// </summary>
	public interface ICamera3 : ICamera
	{
		#region	Unprojection

		/// <summary>
		/// Unprojects a screen space coordinate into world space
		/// </summary>
		/// <param name="x">Screen X position</param>
		/// <param name="y">Screen Y position</param>
		/// <param name="depth">View depth</param>
		/// <returns>Returns the unprojected world position</returns>
		Point3 Unproject( int x, int y, float depth );

		/// <summary>
		/// Creates a pick ray from a screen position
		/// </summary>
		/// <param name="x">Screen X position</param>
		/// <param name="y">Screen Y position</param>
		/// <returns>Returns world ray</returns>
		Ray3 PickRay( int x, int y );

		#endregion

		#region	Camera frame

		/// <summary>
		/// Camera frame
		/// </summary>
		Matrix44 Frame
		{
			get;
		}

		#endregion
	}
}
