using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Rb.Core.Maths
{
	/// <summary>
	/// A point in 3d space. Uses single precision floats.
	/// </summary>
	/// <remarks>
	/// Uses a really dubious hack in the index operator - avoid if possible
	/// </remarks>
	[DebuggerDisplay("({X},{Y},{Z})")]
	[TypeConverter(typeof(Point3TypeConverter))]
	[Serializable]
	[StructLayout( LayoutKind.Sequential )]
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
		/// Creates a vector from this point
		/// </summary>
		public Vector3 ToVector3( )
		{
			return new Vector3( X, Y, Z );
		}

		/// <summary>
		/// Converts this point to a string
		/// </summary>
		public override string ToString()
		{
			return string.Format( "({0},{1},{2})", X, Y, Z );
		}
		
		/// <summary>
		/// Returns true if obj is a Point3 -exactly equal- to this point (no floating point tolerance)
		/// </summary>
		public override bool Equals( object obj )
		{
			return ( obj is Point3 ) && ( ( Point3 )obj == this );
		}

		/// <summary>
		/// Returns the hash code of this point
		/// </summary>
		public unsafe override int GetHashCode()
		{
			//	Not a good hash. fast, though :)
			float res = m_X + m_Y + m_Z;
			return *( int* )&res;
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
		/// Returns true if two points are -exactly- equal (no tolerance for floating point imprecision)
		/// </summary>
		public static bool operator == ( Point3 lhs, Point3 rhs )
		{
			return ( lhs.X == rhs.X ) && ( lhs.Y == rhs.Y ) && ( lhs.Z == rhs.Z );
		}

		/// <summary>
		/// Returns false if two points are in any way unequal (no tolerance for floating point imprecision)
		/// </summary>
		public static bool operator != ( Point3 lhs, Point3 rhs )
		{
			return ( lhs.X != rhs.X ) || ( lhs.Y != rhs.Y ) || ( lhs.Z != rhs.Z );
		}

		/// <summary>
		/// Returns the negative of this point
		/// </summary>
		public static Point3 operator - ( Point3 pt )
		{
			return new Point3( -pt.X, -pt.Y, -pt.Z );
		}

		/// <summary>
		/// Subtracts two points to create a vector
		/// </summary>
		public static Vector3	operator - ( Point3 pt1, Point3 pt2 )
		{
			return new Vector3( ( pt1.X - pt2.X ), ( pt1.Y - pt2.Y ), ( pt1.Z - pt2.Z ) );
		}

		/// <summary>
		/// Subtracts a vector from a point
		/// </summary>
		public static Point3 operator - ( Point3 pt, Vector3 vec )
		{
			return new Point3( ( pt.X - vec.X ), ( pt.Y - vec.Y ), ( pt.Z - vec.Z ) );
		}

		/// <summary>
		/// Adds a vector to a point
		/// </summary>
		public static Point3 operator + ( Point3 pt, Vector3 vec )
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
		public static Point3 operator * ( Point3 pt, Vector3 vec )
		{
			return new Point3( pt.X * vec.X, pt.Y * vec.Y, pt.Z * vec.Z );
		}

		/// <summary>
		/// Gets the squared distance from one point to another
		/// </summary>
		public float SqrDistanceTo( Point3 pt )
		{
			float diffX = ( X - pt.X );
			float diffY = ( Y - pt.Y );
			float diffZ = ( Z - pt.Z );

			return ( diffX * diffX ) + ( diffY * diffY ) + ( diffZ * diffZ );
		}

		/// <summary>
		/// Gets the distance from on point to another
		/// </summary>
		public float DistanceTo( Point3 pt )
		{
			return Functions.Sqrt( SqrDistanceTo( pt ) );
		}

		#region Point3TypeConverter

		public class Point3TypeConverter : ExpandableObjectConverter
		{
			public override bool GetCreateInstanceSupported( ITypeDescriptorContext context )
			{
				return true;
			}

			public override object CreateInstance( ITypeDescriptorContext context, IDictionary propertyValues)
			{
				return new Vector3( ( float )propertyValues[ "X" ], ( float )propertyValues[ "Y" ], ( float )propertyValues[ "Z" ] );
			}
		}

		#endregion

		private float	m_X;
		private float	m_Y;
		private float	m_Z;
	}
}
