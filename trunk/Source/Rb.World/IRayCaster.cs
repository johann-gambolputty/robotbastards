using Rb.Core.Maths;

namespace Rb.World
{
    /// <summary>
    /// Interface for objects that can cast rays through sets of geometry
    /// </summary>
    public interface IRayCaster
    {
        /// <summary>
        /// Adds an intersectable geometry object to the ray caster
        /// </summary>
        void AddIntersector( IRay3Intersector intersector );

        /// <summary>
        /// Removes an intersectable geometry object from the ray caster
        /// </summary>
        void RemoveIntersector( IRay3Intersector intersector );

        /// <summary>
        /// Casts a ray
        /// </summary>
        /// <param name="ray">Ray to cast</param>
        /// <param name="options">Options for determining which layers to check, objects to exclude, maximum ray length (!) etc.</param>
        /// <returns>Returns null if no intersection occurred, or details about the intersection</returns>
        Line3Intersection GetFirstIntersection( Ray3 ray, RayCastOptions options );
    }
}
