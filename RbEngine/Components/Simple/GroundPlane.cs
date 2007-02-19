using System;

namespace RbEngine.Components.Simple
{
	/// <summary>
	/// The ground plane (XZ plane)
	/// </summary>
	public class GroundPlane : Component, Maths.IRay3Intersector
	{

		/// <summary>
		/// Constructor. Adds an implementation of Rendering.Composites.GroundPlaneArea
		/// </summary>
		public GroundPlane( )
		{
			AddChild( Rendering.RenderFactory.Inst.NewComposite( typeof( Rendering.Composites.GroundPlaneArea ) ) );
		}

		#region IRay3Intersector Members

		/// <summary>
		/// Checks if a ray intersects this object
		/// </summary>
		/// <param name="ray">Ray to check</param>
		/// <returns>true if the ray intersects this object</returns>
		public bool TestIntersection( Maths.Ray3 ray )
		{
			return Maths.Intersection.TestIntersection( ray, m_Plane );
		}

		/// <summary>
		/// Checks if a ray intersects this object, returning information about the intersection if it does
		/// </summary>
		/// <param name="ray">Ray to check</param>
		/// <returns>Intersection information. If no intersection takes place, this method returns null</returns>
		public Maths.Ray3Intersection GetIntersection( Maths.Ray3 ray )
		{
			return Maths.Intersection.GetIntersection( ray, m_Plane );
		}

		#endregion

		private Maths.Plane3	m_Plane = new Maths.Plane3( new Maths.Vector3( 0, 1, 0 ), 0 );
	}
}
