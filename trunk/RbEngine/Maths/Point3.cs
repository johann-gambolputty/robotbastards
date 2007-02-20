using System;


namespace RbEngine.Maths
{
	/// <summary>
	/// A Point3 is a point in world space. Units are the same as Vector3
	/// </summary>
	public class Point3
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public Point3( )
		{
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		public Point3( Point3 src )
		{
			m_Pt[ 0 ] = src.m_Pt[ 0 ];
			m_Pt[ 1 ] = src.m_Pt[ 1 ];
			m_Pt[ 2 ] = src.m_Pt[ 2 ];
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public Point3( float x, float y, float z )
		{
			m_Pt[ 0 ] = x;
			m_Pt[ 1 ] = y;
			m_Pt[ 2 ] = z;
		}

		/// <summary>
		/// Sets the point's components
		/// </summary>
		public void Set( float x, float y, float z )
		{
			m_Pt[ 0 ] = x;
			m_Pt[ 1 ] = y;
			m_Pt[ 2 ] = z;
		}

		/// <summary>
		/// Access to an indexed component of the vector (0==X,1==Y,2==Z)
		/// </summary>
		public float this[ int index ]
		{
			get
			{
				return m_Pt[ index ];
			}
			set
			{
				m_Pt[ index ] = value;
			}
		}

		/// <summary>
		/// Point x coordinate
		/// </summary>
		public float X
		{
			get { return m_Pt[ 0 ]; }
			set { m_Pt[ 0 ] = value; }
		}

		/// <summary>
		/// Point y coordinate
		/// </summary>
		public float Y
		{
			get { return m_Pt[ 1 ]; }
			set { m_Pt[ 1 ] = value; }
		}

		/// <summary>
		/// Point z coordinate
		/// </summary>
		public float Z
		{
			get { return m_Pt[ 2 ]; }
			set { m_Pt[ 2 ] = value; }
		}

		/// <summary>
		/// Subtracts two points to create a vector
		/// </summary>
		public static Vector3	operator - ( Point3 pt1, Point3 pt2 )
		{
			return new Vector3( ( float )( pt1.X - pt2.X ), ( float )( pt1.Y - pt2.Y ), ( float )( pt1.Z - pt2.Z ) );
		}

		/// <summary>
		/// Adds a vector to a point
		/// </summary>
		public static Point3	operator + ( Point3 pt, Vector3 vec )
		{
			return new Point3( pt.X + vec.X, pt.Y + vec.Y, pt.Z + vec.Z );
		}

		/// <summary>
		/// Gets the squared distance from one point to another
		/// </summary>
		public float	SqrDistanceTo( Point3 pt )
		{
			float diffX = ( float )( X - pt.X );
			float diffY = ( float )( Y - pt.Y );
			float diffZ = ( float )( Z - pt.Z );

			return ( diffX * diffX ) + ( diffY * diffY ) + ( diffZ * diffZ );
		}

		/// <summary>
		/// Gets the distance from on point to another
		/// </summary>
		public float	DistanceTo( Point3 pt )
		{
			return ( float )System.Math.Sqrt( SqrDistanceTo( pt ) );
		}

		private float[]	m_Pt = new float[ 3 ] { 0, 0, 0 };
	}
}
