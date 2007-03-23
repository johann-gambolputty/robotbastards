using System;

namespace RbEngine.Maths
{
	/// <summary>
	/// Summary description for Plane3.
	/// </summary>
	public class Plane3
	{
		/// <summary>
		/// Default constructor. Plane equation is (A=0,B=0,C=0,D=0)
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
		public Vector3	Normal
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
		/// Plane distance
		/// </summary>
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

		private Vector3	m_Normal	= new Vector3( 0, 0, 0 );
		private float	m_Distance	= 0;

	}
}
