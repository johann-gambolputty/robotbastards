using System;

namespace RbEngine.Maths
{
	/// <summary>
	/// Summary description for RayIntersection.
	/// </summary>
	public class RayIntersection
	{
		/// <summary>
		/// The object that the ray intersected. Can be null even if intersection was successful
		/// </summary>
		public Object	IntersectedObject
		{
			get
			{
				return m_Object;
			}
			set
			{
				m_Object = value;
			}
		}

		/// <summary>
		/// The point that the ray intersected the object. Always valid
		/// </summary>
		public Vector3	IntersectionPosition
		{
			get
			{
				return m_Position;
			}
			set
			{
				m_Position = value;
			}
		}

		/// <summary>
		/// The normal on the object surface at the point that the ray intersected. Always valid
		/// </summary>
		public Vector3	IntersectionNormal
		{
			get
			{
				return m_Normal;
			}
			set
			{
				m_Normal = value;
			}
		}

		/// <summary>
		/// Distance along the ray at which the intersection occurred
		/// </summary>
		/// <remarks>
		/// The IntersectionPosition can also be determined by calling Ray3.GetPointOnRay( Distance )
		/// </remarks>
		public float	Distance
		{
			get
			{
				return m_Distance;
			}
			set
			{
				m_Distance = value;
			}
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public			RayIntersection( )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="pos"> Ray intersection position </param>
		/// <param name="normal"> Ray intersection normal </param>
		/// <param name="distance"> Ray intersection distance </param>
		public			RayIntersection( Vector3 pos, Vector3 normal, float distance )
		{
			m_Position	= pos;
			m_Normal	= normal;
			m_Distance	= distance;
		}


		private Object	m_Object;
		private Vector3	m_Position;
		private Vector3	m_Normal;
		private float	m_Distance;
	}
}
