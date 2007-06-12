using System;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Sphere geometry
	/// </summary>
	public class Sphere3
	{
		/// <summary>
		/// Gets or sets the sphere centre
		/// </summary>
		public Point3	Centre
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
			m_Centre	= new Point3( );
			m_Radius	= 1.0f;
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="srcCentre"> Sphere centre </param>
		/// <param name="srcRadius"> Sphere radius </param>
		public Sphere3( Point3 srcCentre, float srcRadius )
		{
			m_Centre = srcCentre;
			m_Radius = srcRadius;
		}

		private Point3	m_Centre;
		private float	m_Radius;
	}
}
