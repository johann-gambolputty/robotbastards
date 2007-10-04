using System.Collections.Generic;
using Rb.Core.Maths;

namespace Rb.World
{
    /// <summary>
    /// Maintains a list of IRay3Intersector objects to intersect with
    /// </summary>
    public class RayCaster : List< IRay3Intersector >, IRayCaster
    {
        #region IRayCaster Members

		/// <summary>
		/// Adds an object that can be intersected to the raycaster
		/// </summary>
		/// <param name="intersector">Intersectable object</param>
        public void AddIntersector( IRay3Intersector intersector )
        {
            Add( intersector );
        }

		/// <summary>
		/// Removes an intersector
		/// </summary>
		/// <param name="intersector">Intersector to remove</param>
        public void RemoveIntersector( IRay3Intersector intersector )
        {
            Remove( intersector );
        }

		/// <summary>
		/// Gets the first intersection between a ray and the stored intersectable objects
		/// </summary>
		/// <param name="ray">Ray</param>
		/// <param name="options">Ray cast options</param>
		/// <returns>Intersection result (null if no intersection occurred)</returns>
        public Line3Intersection GetFirstIntersection( Ray3 ray, RayCastOptions options )
        {
            Line3Intersection closestIntersection = null;
            foreach ( IRay3Intersector intersector in this )
            {
                Line3Intersection intersection = intersector.GetIntersection( ray );
                if ( intersection != null )
                {
                    if ( ( closestIntersection == null ) || ( closestIntersection.Distance > intersection.Distance ) )
                    {
                        closestIntersection = intersection;
                    }
                }
            }

            return closestIntersection;
        }

        #endregion
    }
}
