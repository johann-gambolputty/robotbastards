using System;
using Rb.Core.Maths;

namespace Rb.World
{
	/// <summary>
	/// Adds layer information to a plain IRay3Intersector object
	/// </summary>
	[Serializable]
	public class Ray3LayerIntersector : IRayLayerIntersector, IRay3Intersector
	{
		#region Construction

		/// <summary>
		/// Initialises this ray intersector
		/// </summary>
		/// <param name="intersector">Base intersector</param>
		/// <param name="layers">Initial layers</param>
		public Ray3LayerIntersector( IRay3Intersector intersector, ulong layers )
		{
			if ( intersector == null )
			{
				throw new ArgumentNullException( "intersector", "Base intersector cannot be null" );
			}
			m_Intersector = intersector;
			m_Layers = layers;
		}

		#endregion

		/// <summary>
		/// Gets the base intersector
		/// </summary>
		public IRay3Intersector BaseIntersector
		{
			get { return m_Intersector; }
		}

		#region IRayLayerIntersector Members

		/// <summary>
		/// Event, raised when <see cref="Layers"/> is changed
		/// </summary>
		public event RayLayersChangedDelegate LayersChanged;

		/// <summary>
		/// Sets/gets the raycast layers
		/// </summary>
		public ulong Layers
		{
			get { return m_Layers; }
			set
			{
				ulong oldLayers = m_Layers;
				m_Layers = value;
				if ( LayersChanged != null )
				{
					LayersChanged( this, oldLayers, m_Layers );
				}
			}
		}

		#endregion

		#region IRay3Intersector Members

		/// <summary>
		/// Tests if the base intersector intersects the specified ray
		/// </summary>
		/// <param name="ray">Ray to test</param>
		/// <returns>True if ray is intersected</returns>
		public bool TestIntersection( Ray3 ray )
		{
			return m_Intersector.TestIntersection( ray );
		}

		/// <summary>
		/// Gets the intersection between the base intersector and a ray
		/// </summary>
		/// <param name="ray">Ray to test</param>
		/// <returns>Intersection details, or null if no intersection occurred</returns>
		public Line3Intersection GetIntersection( Ray3 ray )
		{
			return m_Intersector.GetIntersection( ray );
		}

		#endregion
		
		#region Private members

		private readonly IRay3Intersector m_Intersector;
		private ulong m_Layers;

		#endregion

	}
}
