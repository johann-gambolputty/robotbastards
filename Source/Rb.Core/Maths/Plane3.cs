
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
		/// Setup constructor. Plane equation is (A=normal.X,B=normal.Y,C=normal.Z,D=dist)
		/// </summary>
		public Plane3( Vector3 normal, float dist )
		{
			m_Normal	= normal;
			m_Distance	= dist;
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
