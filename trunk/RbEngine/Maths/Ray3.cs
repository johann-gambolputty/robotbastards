using System;

namespace RbEngine.Maths
{
	/// <summary>
	/// Summary description for Ray3.
	/// </summary>
	public class Ray3
	{
		/// <summary>
		/// Default constructor (ray at origin pointing along Z axis)
		/// </summary>
		public Ray3( )
		{
			m_Origin	= new Vector3( );
			m_Direction	= new Vector3( 0, 0, 1 );
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="origin"> Ray origin </param>
		/// <param name="direction"> Ray direction </param>
		public Ray3( Vector3 origin, Vector3 direction )
		{
			m_Origin = origin;
			m_Direction = direction;
		}

		/// <summary>
		/// Returns a point on the ray
		/// </summary>
		/// <param name="t"> Distance from ray origin </param>
		/// <returns> Position at specified distance along ray direction, from ray origin</returns>
		public Vector3		GetPointOnRay( float t )
		{
			return m_Origin + m_Direction * t;
		}

		/// <summary>
		/// Ray origin
		/// </summary>
		public Vector3		Origin
		{
			get
			{
				return m_Origin;
			}
			set
			{
				m_Origin = value;
			}
		}

		/// <summary>
		/// Ray direction
		/// </summary>
		public Vector3		Direction
		{
			get
			{
				return m_Direction;
			}
			set
			{
				m_Direction = value;
			}
		}

		private Vector3		m_Origin;
		private Vector3		m_Direction;
	}
}
