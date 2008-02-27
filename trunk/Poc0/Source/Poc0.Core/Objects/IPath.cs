using Rb.Core.Maths;

namespace Poc0.Core.Objects
{
	/// <summary>
	/// Walkable path interface
	/// </summary>
	public interface IPath
	{
		/// <summary>
		/// Returns true if a time t is at the beginning of the path
		/// </summary>
		/// <param name="t">Time value</param>
		/// <param name="tolerance">Distance tolerance</param>
		bool AtStart( float t, float tolerance );
		
		/// <summary>
		/// Returns true if a time t is at the end of the path
		/// </summary>
		/// <param name="t">Time value</param>
		/// <param name="tolerance">Distance tolerance</param>
		bool AtEnd( float t, float tolerance );

		/// <summary>
		/// Gets the position and tangent on the path at time t
		/// </summary>
		void GetFrame( float t, out Point3 pt, out Vector3 dir );
		
		/// <summary>
		/// Gets the closest point to pt on the path
		/// </summary>
		float GetClosestPoint( Point3 pt );
		
		/// <summary>
		/// Moves along the path a given distance
		/// </summary>
		/// <param name="t">Path time</param>
		/// <param name="distance">Distance along the path to move</param>
		/// <param name="reverse">Move backwards along the path</param>
		/// <returns>Returns the new path time</returns>
		float Move( float t, float distance, bool reverse );
	}
}
