
namespace Rb.Core.Maths
{
	/// <summary>
	/// 3d plane
	/// </summary>
    public class Plane3 : IRay3Intersector
	{
		/// <summary>
		/// Default constructor. Plane equation is (A=0,B=0,C=1,D=0)
		/// </summary>
		public Plane3( )
		{
		}
		
		/// <summary>
		/// Setup constructor
		/// </summary>
		public Plane3( float a, float b, float c, float d )
		{
			m_Normal	= new Vector3( a, b, c );
			m_Distance	= d;
		}

		/// <summary>
		/// Setup constructor. Plane equation is (A=normal.X,B=normal.Y,C=normal.Z,D=dist)
		/// </summary>
		public Plane3( Vector3 normal, float dist )
		{
			m_Normal	= normal;
			m_Distance	= dist;
		}

		/// <summary>
		/// Setup constructor. Builds this plane from a point on a plane, and the plane normal
		/// </summary>
		/// <param name="pointOnPlane">Point on the plane</param>
		/// <param name="normal">Plane normal</param>
		public Plane3( Point3 pointOnPlane, Vector3 normal )
		{
			m_Normal = normal;
			m_Distance = normal.Dot( pointOnPlane );

			//	TODO: AP: Remove checks
			System.Diagnostics.Debug.Assert( ClassifyPoint( pointOnPlane, 0.001f ) == PlaneClassification.On );
			System.Diagnostics.Debug.Assert( ClassifyPoint( pointOnPlane + normal, 0.001f ) == PlaneClassification.InFront );
			System.Diagnostics.Debug.Assert( ClassifyPoint( pointOnPlane - normal, 0.001f ) == PlaneClassification.Behind );
		}
		
		/// <summary>
		/// Inverts this plane
		/// </summary>
		public void Invert( )
		{
			m_Normal.X = -m_Normal.X;
			m_Normal.Y = -m_Normal.Y;
			m_Normal.Z = -m_Normal.Z;
			m_Distance = -m_Distance;
		}

		/// <summary>
		/// Creates a plane that is coplanar with this plane, but pointing in the opposite direction
		/// </summary>
		/// <returns>Inverse plane</returns>
		public Plane3 MakeInversePlane( )
		{
			return new Plane3( -m_Normal.X, -m_Normal.Y, -m_Normal.Z, -m_Distance );
		}

		/// <summary>
		/// Plane normal
		/// </summary>
		public Vector3 Normal
		{
			get { return m_Normal; }
			set { m_Normal = value; }
		}

		/// <summary>
		/// Plane distance
		/// </summary>
		public float Distance
		{
			get { return m_Distance; }
			set { m_Distance = value; }
        }

		/// <summary>
		/// Gets the signed distance to a point
		/// </summary>
		/// <param name="pt">Calculates distance to this point</param>
		/// <returns>Returns +ve distance to pt if pt is in front of this plane, -ve distance if pt is behind this plane</returns>
		public float GetSignedDistanceTo( Point3 pt )
		{
			return ( pt.X * m_Normal.X ) + ( pt.Y * m_Normal.Y ) + ( pt.Z * m_Normal.Z ) + m_Distance;
		}

		/// <summary>
		/// Classifies a point
		/// </summary>
		/// <param name="pt">Point to classify</param>
		/// <param name="tolerance">"On" plane tolerance</param>
		/// <returns>Returns the classification of the specified point with respect to this plane</returns>
		public PlaneClassification ClassifyPoint( Point3 pt, float tolerance )
		{
			float signedDist = GetSignedDistanceTo( pt );
			return signedDist < -tolerance ? PlaneClassification.Behind : ( signedDist > tolerance ? PlaneClassification.InFront : PlaneClassification.On );
		}

        #region IRay3Intersector Members

        /// <summary>
        /// Returns true if there is an intersection between the specified ray and this plane
        /// </summary>
        public bool TestIntersection( Ray3 ray )
        {
            return Intersections3.TestRayIntersection( ray, this );
        }

        /// <summary>
        /// Returns details about the intersection between a specified ray and this plane (returns null if no intersection exists)
        /// </summary>
        public Line3Intersection GetIntersection( Ray3 ray )
        {
            return Intersections3.GetRayIntersection( ray, this );
        }

        #endregion

        #region Private stuff

        private Vector3 m_Normal    = new Vector3( 0, 0, 1 );
        private float   m_Distance  = 0;

        #endregion
    }
}
