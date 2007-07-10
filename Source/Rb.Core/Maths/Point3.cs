using System;
using System.Diagnostics;

namespace Rb.Core.Maths
{
	/// <summary>
	/// A Point3 is a point in world space. Units are the same as Vector3
	/// </summary>
	[DebuggerDisplay("({X},{Y},{Z})")]
	[Serializable]
	public struct Point3
	{
		/// <summary>
		/// Origin
		/// </summary>
		public static readonly Point3 Origin = new Point3( 0, 0, 0 );

		/// <summary>
		/// Copy constructor
		/// </summary>
		public Point3( Point3 src )
		{
			m_X = src.m_X;
			m_Y = src.m_Y;
			m_Z = src.m_Z;
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public Point3( float x, float y, float z )
		{
			m_X = x;
			m_Y = y;
			m_Z = z;
		}

		/// <summary>
		/// Sets the point's components
		/// </summary>
		public void Set( float x, float y, float z )
		{
			m_X = x;
			m_Y = y;
			m_Z = z;
		}

		/// <summary>
		/// Access to an indexed component of the vector (0==X,1==Y,2==Z)
		/// </summary>
		public unsafe float this[ int index ]
		{
			get
			{
				fixed ( float* pt = &m_X )
				{
					return pt[ index ];
				}
			}
			set
			{
				fixed ( float* pt = &m_X )
				{
					pt[ index ] = value;
				}
			}
		}

		/// <summary>
		/// Point x coordinate
		/// </summary>
		public float X
		{
			get { return m_X; }
			set { m_X = value; }
		}

		/// <summary>
		/// Point y coordinate
		/// </summary>
		public float Y
		{
			get { return m_Y; }
			set { m_Y = value; }
		}

		/// <summary>
		/// Point z coordinate
		/// </summary>
		public float Z
		{
			get { return m_Z; }
			set { m_Z = value; }
		}

		/// <summary>
		/// Subtracts two points to create a vector
		/// </summary>
		public static Vector3	operator - ( Point3 pt1, Point3 pt2 )
		{
			return new Vector3( ( float )( pt1.X - pt2.X ), ( float )( pt1.Y - pt2.Y ), ( float )( pt1.Z - pt2.Z ) );
		}

		/// <summary>
		/// Subtracts a vector from a point
		/// </summary>
		public static Point3	operator - ( Point3 pt, Vector3 vec )
		{
			return new Point3( ( float )( pt.X - vec.X ), ( float )( pt.Y - vec.Y ), ( float )( pt.Z - vec.Z ) );
		}

		/// <summary>
		/// Adds a vector to a point
		/// </summary>
		public static Point3	operator + ( Point3 pt, Vector3 vec )
		{
			return new Point3( pt.X + vec.X, pt.Y + vec.Y, pt.Z + vec.Z );
		}

		/// <summary>
		/// Scales the point by a scalar
		/// </summary>
		public static Point3 operator *( Point3 pt, float val )
		{
			return new Point3( pt.X * val, pt.Y * val, pt.Z * val );
		}

		/// <summary>
		/// Scales the point by a vector
		/// </summary>
		public static Point3	operator * ( Point3 pt, Vector3 vec )
		{
			return new Point3( pt.X * vec.X, pt.Y * vec.Y, pt.Z * vec.Z );
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


		private float	m_X;
		private float	m_Y;
		private float	m_Z;
	}
}
