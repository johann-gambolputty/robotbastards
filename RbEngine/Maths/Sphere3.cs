using System;

namespace RbEngine.Maths
{
	/// <summary>
	/// Sphere geometry
	/// </summary>
	public class Sphere3
	{
		/// <summary>
		/// Gets or sets the sphere centre
		/// </summary>
		public Vector3	Centre
		{
			get
			{
				return m_Centre;
			}
			set
			{
				m_Centre = value;
			}
		}

		/// <summary>
		/// Gets or sets the radius of the sphere
		/// </summary>
		public float	Radius
		{
			get
			{
				return m_Radius;
			}
			set
			{
				m_Radius = value;
			}
		}

		/// <summary>
		/// Gets or sets the squared radius of the sphere
		/// </summary>
		public float	SqrRadius
		{
			get
			{
				return m_Radius * m_Radius;
			}
			set
			{
				m_Radius = ( float )System.Math.Sqrt( value );
			}
		}

		/// <summary>
		/// Default constructor. Creates a unit sphere at the origin
		/// </summary>
		public Sphere3( )
		{
			m_Centre	= new Vector3( );
			m_Radius	= 1.0f;
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="srcCentre"> Sphere centre </param>
		/// <param name="srcRadius"> Sphere radius </param>
		public Sphere3( Vector3 srcCentre, float srcRadius )
		{
			m_Centre = srcCentre;
			m_Radius = srcRadius;
		}

		private Vector3	m_Centre;
		private float	m_Radius;
	}
}
