using System;
using System.Collections;

namespace RbEngine.Scene
{
	/// <summary>
	/// Manages raycasts in a scene
	/// </summary>
	public class RayCastManager : IRayCaster
	{
		#region IRayCaster Members

		/// <summary>
		/// Adds an intersectable object to this manager
		/// </summary>
		public void Add( Maths.IRay3Intersector obj )
		{
			m_Intersectors.Add( obj );
		}

		/// <summary>
		/// Removes an intersectable object to this manager
		/// </summary>
		public void Remove( Maths.IRay3Intersector obj )
		{
			m_Intersectors.Remove( obj );
		}

		/// <summary>
		/// Casts a ray into all registered intersectors
		/// </summary>
		public Maths.Ray3Intersection GetFirstIntersection( Maths.Ray3 ray, RayCastOptions options )
		{
			if ( options == null )
			{
				options = RayCastOptions.Default;
			}

			Maths.Ray3Intersection closestIntersection = null;
			foreach ( Maths.IRay3Intersector intersector in m_Intersectors )
			{
				if ( options.IsExcluded( intersector ) || ( !options.IsLayerIncluded( intersector.Layer ) ) )
				{
					continue;
				}
				Maths.Ray3Intersection curIntersection = intersector.GetIntersection( ray );
				if ( ( closestIntersection == null ) || ( curIntersection.Distance < closestIntersection.Distance ) )
				{
					closestIntersection = curIntersection;
				}
			}
			return closestIntersection;
		}

		#endregion

		private ArrayList	m_Intersectors = new ArrayList( );
	}
}
