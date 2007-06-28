using System;
using System.Collections.Generic;
using System.Text;
using Rb.Core.Maths;

namespace Rb.World
{
    /// <summary>
    /// Maintains a list of IRay3Intersector objects to intersect with
    /// </summary>
    public class RayCaster : List< IRay3Intersector >, IRayCaster
    {
        #region IRayCaster Members

        public void AddIntersector( IRay3Intersector intersector )
        {
            Add( intersector );
        }

        public void RemoveIntersector( IRay3Intersector intersector )
        {
            Remove( intersector );
        }

        public Ray3Intersection GetFirstIntersection( Ray3 ray, RayCastOptions options )
        {
            Ray3Intersection closestIntersection = null;
            foreach ( IRay3Intersector intersector in this )
            {
                Ray3Intersection intersection = intersector.GetIntersection( ray );
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
