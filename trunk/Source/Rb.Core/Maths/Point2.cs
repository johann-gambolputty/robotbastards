using System;
using System.Diagnostics;

namespace Rb.Core.Maths
{
	/// <summary>
	/// A Point2 is a point in world space. Units are the same as Vector2
	/// </summary>
	[DebuggerDisplay("({X},{Y})")]
	[Serializable]
	public struct Point2
	{
		/// <summary>
		/// Origin
		/// </summary>
		public static readonly Point2 Origin = new Point2( 0, 0 );

		/// <summary>
		/// Copy constructor
		/// </summary>
		public Point2( Point2 src )
		{
			m_X = src.m_X;
			m_Y = src.m_Y;
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public Point2( float x, float y )
		{
			m_X = x;
			m_Y = y;
		}
		
		/// <summary>
		/// Converts this point to a string
		/// </summary>
		public override string ToString()
		{
			return string.Format( "({0},{1})", X, Y );
		}
		
		/// <summary>
		/// Returns true if obj is a Point3 -exactly equal- to this point (no floating point tolerance)
		/// </summary>
		public override bool Equals( object obj )
		{
			return ( obj is Point3 ) && ( ( Point2 )obj == this );
		}

		/// <summary>
		/// Returns the hash code of this point
		/// </summary>
		public unsafe override int GetHashCode( )
		{
			//	Is this a good hash? who knows? fast, though :)
			float res = m_X + m_Y;
			return *( int* )&res;
		}

		/// <summary>
		/// Sets the point's components
		/// </summary>
		public void Set( float x, float y )
		{
			m_X = x;
			m_Y = y;
		}

		/// <summary>
		/// Access to an indexed component of the vector (0==X,1==Y)
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
		/// Returns true if two points are -exactly- equal (no tolerance for floating point imprecision)
		/// </summary>
		public static bool operator == ( Point2 lhs, Point2 rhs )
		{
			return ( lhs.X == rhs.X ) && ( lhs.Y == rhs.Y );
		}

		/// <summary>
		/// Returns false if two points are in any way unequal (no tolerance for floating point imprecision)
		/// </summary>
		public static bool operator != ( Point2 lhs, Point2 rhs )
		{
			return ( lhs.X != rhs.X ) || ( lhs.Y != rhs.Y );
		}

		/// <summary>
		/// Subtracts two points to create a vector
		/// </summary>
		public static Vector2 operator - ( Point2 pt1, Point2 pt2 )
		{
			return new Vector2( ( pt1.X - pt2.X ), ( pt1.Y - pt2.Y ) );
		}

		/// <summary>
		/// Subtracts a vector from a point
		/// </summary>
		public static Point2 operator - ( Point2 pt, Vector2 vec )
		{
			return new Point2( ( pt.X - vec.X ), ( pt.Y - vec.Y ) );
		}

		/// <summary>
		/// Adds a vector to a point
		/// </summary>
		public static Point2 operator + ( Point2 pt, Vector2 vec )
		{
			return new Point2( pt.X + vec.X, pt.Y + vec.Y );
		}

		/// <summary>
		/// Scales the point by a scalar
		/// </summary>
		public static Point2 operator *( Point2 pt, float val )
		{
			return new Point2( pt.X * val, pt.Y * val );
		}

		/// <summary>
		/// Scales the point by a vector
		/// </summary>
		public static Point2 operator * ( Point2 pt, Vector2 vec )
		{
			return new Point2( pt.X * vec.X, pt.Y * vec.Y );
		}

		/// <summary>
		/// Gets the squared distance from one point to another
		/// </summary>
		public float SqrDistanceTo( Point2 pt )
		{
			float diffX = ( X - pt.X );
			float diffY = ( Y - pt.Y );

			return ( diffX * diffX ) + ( diffY * diffY );
		}

		/// <summary>
		/// Gets the distance from on point to another
		/// </summary>
		public float DistanceTo( Point2 pt )
		{
			return Functions.Sqrt( SqrDistanceTo( pt ) );
		}

		#region Private members

		private float	m_X;
		private float	m_Y;

		#endregion
	}
}
