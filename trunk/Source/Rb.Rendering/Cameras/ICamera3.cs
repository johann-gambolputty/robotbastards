using Rb.Core.Maths;

namespace Rb.Rendering.Cameras
{
	/*
	 * Rendering interfaces:
	 * 
	 * - Core
	 *	- IRenderer (name? IPipeline?)
	 *	- IDraw
	 *	- IState
	 *	- 
	 * 
	 * 
	 * 
	 * 
	 * 
	 */


	/// <summary>
	/// 3d camera interface
	/// </summary>
	public interface ICamera3
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
		///	Gets the camera frame's x axis
		/// </summary>
		Vector3 XAxis
		{
			get;
		}

		/// <summary>
		///	Gets the camera frame's y axis
		/// </summary>
		Vector3 YAxis
		{
			get;
		}

		/// <summary>
		///	Gets the camera frame's z axis
		/// </summary>
		Vector3 ZAxis
		{
			get;
		}

		/// <summary>
		///	Gets the camera's position
		/// </summary>
		Point3 Position
		{
			get;
		}

		#endregion

	}
}
