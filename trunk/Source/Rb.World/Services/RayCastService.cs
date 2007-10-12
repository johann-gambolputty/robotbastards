using System;
using System.Collections.Generic;
using Rb.Core.Maths;

namespace Rb.World.Services
{
    /// <summary>
    /// Maintains a list of IRay3Intersector objects to intersect with
    /// </summary>
    [Serializable]
    public class RayCastService : IRayCastService
    {
        #region IRayCaster Members

		/// <summary>
		/// Adds an object that can be intersected to the raycaster
		/// </summary>
		/// <param name="intersector">Intersectable object</param>
        public IRayLayerIntersector AddIntersector( IRay3Intersector intersector )
        {
			if ( intersector is IRayLayerIntersector )
			{
				m_Intersectors.Add( intersector );
				return ( IRayLayerIntersector )intersector;
			}
			return AddIntersector( ulong.MaxValue, intersector );
        }

		/// <summary>
		/// Adds an intersectable geometry object to the ray caster
		/// </summary>
		public IRayLayerIntersector AddIntersector( ulong layers, IRay3Intersector intersector )
		{
			if ( intersector is IRayLayerIntersector )
			{
				throw new ArgumentException( "Intersector already supports layers", "intersector" );
			}
			Ray3LayerIntersector layerIntersector = new Ray3LayerIntersector( intersector, layers );
			m_Intersectors.Add( layerIntersector );
			return layerIntersector;
		}

		/// <summary>
		/// Changes layers of a given intersector
		/// </summary>
		/// <param name="intersector"></param>
		/// <param name="layers"></param>
		public void ChangeLayers( IRay3Intersector intersector, ulong layers )
		{
			int index = Find( intersector );
			if ( index != -1 )
			{
				( ( IRayLayerIntersector )m_Intersectors[ index ] ).Layers = layers;
			}
		}

		/// <summary>
		/// Removes an intersector
		/// </summary>
		/// <param name="intersector">Intersector to remove</param>
        public void RemoveIntersector( IRay3Intersector intersector )
        {
			int index = Find( intersector );
			if ( index != -1 )
			{
				m_Intersectors.RemoveAt( index );
			}
        }

		/// <summary>
		/// Gets the first intersection between a ray and the stored intersectable objects
		/// </summary>
		/// <param name="ray">Ray</param>
		/// <param name="options">Ray cast options</param>
		/// <returns>Intersection result (null if no intersection occurred)</returns>
        public Line3Intersection GetFirstIntersection( Ray3 ray, RayCastOptions options )
		{
			options = options ?? ms_DefaultOptions;
            Line3Intersection closestIntersection = null;
            foreach ( IRay3Intersector intersector in m_Intersectors )
			{
				if ( !options.TestObject( intersector ) )
				{
					continue;
				}
                Line3Intersection intersection = intersector.GetIntersection( ray );
                if ( intersection != null )
                {
                    if ( ( closestIntersection == null ) || ( closestIntersection.Distance >= intersection.Distance ) )
                    {
                        closestIntersection = intersection;
                    }
                }
            }

            return closestIntersection;
        }

		/// <summary>
		/// Casts a ray
		/// </summary>
		/// <param name="ray">Ray to cast</param>
		/// <param name="options">Options for determining which layers to check, objects to exclude, maximum ray length (!) etc.</param>
		/// <returns>Returns an array of all intersections</returns>
		public Line3Intersection[] GetIntersections( Ray3 ray, RayCastOptions options )
		{
			options = options ?? ms_DefaultOptions;
			List< Line3Intersection > intersections = new List< Line3Intersection >( 4 );
			
            foreach ( IRay3Intersector intersector in m_Intersectors )
            {
				if ( !options.TestObject( intersector ) )
				{
					continue;
				}
                Line3Intersection intersection = intersector.GetIntersection( ray );
                if ( intersection != null )
                {
					intersections.Add( intersection );
                }
            }

			return intersections.ToArray( );
		}

        #endregion

		#region Private members

		private readonly static RayCastOptions ms_DefaultOptions = new RayCastOptions( );
		private readonly List< IRay3Intersector > m_Intersectors = new List< IRay3Intersector >( );
		
		/// <summary>
		/// Finds the index of a given intersector object
		/// </summary>
		/// <param name="intersector">Intersector to find</param>
		/// <returns>Returns the index of the intersector, or -1 if it can't be found</returns>
		private int Find( IRay3Intersector intersector )
		{
			for ( int index = 0; index < m_Intersectors.Count; ++index )
			{
				if ( m_Intersectors[ index ] == intersector )
				{
					return index;
				}

				Ray3LayerIntersector layerIntersector = m_Intersectors[ index ] as Ray3LayerIntersector;
				if ( layerIntersector != null && layerIntersector.BaseIntersector == intersector )
				{
					return index;
				}
			}
			return -1;
		}

		#endregion

	}
}
