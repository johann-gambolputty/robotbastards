using Rb.Core.Maths;

namespace Rb.World
{
    /// <summary>
    /// Interface for objects that can cast rays through sets of geometry
    /// </summary>
    public interface IRayCaster
	{
		#region Intersectors

		/// <summary>
        /// Adds an intersectable geometry object to the ray caster
        /// </summary>
        /// <param name="intersector">Intersector to add</param>
		/// <returns>Returns the layer intersector representation of intersector</returns>
        IRayLayerIntersector AddIntersector( IRay3Intersector intersector );

		/// <summary>
        /// Adds an intersectable geometry object to the ray caster
		/// </summary>
		/// <param name="layers">Initial layers that the intersector uses</param>
		/// <param name="intersector">Intersector to add</param>
		/// <returns>Returns the layer intersector representation of intersector</returns>
        IRayLayerIntersector AddIntersector( ulong layers, IRay3Intersector intersector );

		/// <summary>
		/// Changes the layers of an existing intersector
		/// </summary>
		/// <param name="intersector">Intersector</param>
		/// <param name="layers">New layers</param>
		void ChangeLayers( IRay3Intersector intersector, ulong layers );

        /// <summary>
        /// Removes an intersectable geometry object from the ray caster
        /// </summary>
        void RemoveIntersector( IRay3Intersector intersector );

		#endregion

		#region Intersection testing

		/// <summary>
        /// Casts a ray
        /// </summary>
        /// <param name="ray">Ray to cast</param>
        /// <param name="options">Options for determining which layers to check, objects to exclude, maximum ray length (!) etc.</param>
        /// <returns>Returns null if no intersection occurred, or details about the intersection</returns>
        Line3Intersection GetFirstIntersection( Ray3 ray, RayCastOptions options );

		/// <summary>
		/// Casts a ray
		/// </summary>
		/// <param name="ray">Ray to cast</param>
		/// <param name="options">Options for determining which layers to check, objects to exclude, maximum ray length (!) etc.</param>
		/// <returns>Returns an array of all intersections</returns>
		Line3Intersection[] GetIntersections( Ray3 ray, RayCastOptions options );

		#endregion
	}
}
