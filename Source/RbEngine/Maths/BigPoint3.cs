using System;

namespace RbEngine.Maths
{
	/// <summary>
	/// A point in world space, using 64 bit integers to store coordinates (units are in millimetres)
	/// </summary>
	/// <remarks>
	/// Units are in millimetres, which gives an effective range of +-9223372036854775.8075 metres. This is a pretty huge range.
	/// I chose millimetres, instead of micrometres, because the point values are slightly more readable on the debugger (e.g. a
	/// metre is 1000, instead of 1000000), not because I want to be able to measure the 1000 solar systems. To change the units,
	/// set the UnitsPerMetre constant.
	/// <note>
	/// BigPoint3 has a far greater range than a single float can offer. Therefore, take care when using floats and Vector3s (e.g.
	/// when using operator -() to get the vector between two points, or DistanceTo() to get the distance between points).
	/// </note>
	/// </remarks>
	/// <seealso href="http://home.comcast.net/~tom_forsyth/blog.wiki.html#%5B%5BA%20matter%20of%20precision%5D%5D">Tom Forsyth's blog</seealso>
	public struct BigPoint3
	{
		/// <summary>
		/// The number of units per metre
		/// </summary>
		public const float	UnitsPerMetre	= 1000;

		/// <summary>
		/// Converts a big point unit into metres
		/// </summary>
		public const float	FromMetres		= UnitsPerMetre;

		/// <summary>
		/// Converts a metre into big point units
		/// </summary>
		public const float	ToMetres		= 1.0f / UnitsPerMetre;

		/// <summary>
		/// Copy constructor
		/// </summary>
		public BigPoint3( BigPoint3 src )
		{
			m_X = src.m_X;
			m_Y = src.m_Y;
			m_Z = src.m_Z;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public BigPoint3( long x, long y, long z )
		{
			m_X = x;
			m_Y = y;
			m_Z = z;
		}

		/// <summary>
		/// Access to an indexed component of the vector (0==X,1==Y,2==Z)
		/// </summary>
		public unsafe long this[ int index ]
		{
			get
			{
				fixed ( long* pt = &m_X )
				{
					return pt[ index ];
				}
			}
			set
			{
				fixed ( long* pt = &m_X )
				{
					pt[ index ] = value;
				}
			}
		}

		/// <summary>
		/// Point x coordinate
		/// </summary>
		public long X
		{
			get { return m_X; }
			set { m_X = value; }
		}

		/// <summary>
		/// Point y coordinate
		/// </summary>
		public long Y
		{
			get { return m_Y; }
			set { m_Y = value; }
		}

		/// <summary>
		/// Point z coordinate
		/// </summary>
		public long Z
		{
			get { return m_Z; }
			set { m_Z = value; }
		}

		/// <summary>
		/// Subtracts two points to create a vector
		/// </summary>
		/// <remarks>
		/// Because BigPoint3 has so much more precision than a float can offer, be careful about using this function
		/// </remarks>
		public static Vector3	operator - ( BigPoint3 pt1, BigPoint3 pt2 )
		{
			return new Vector3( ( float )( pt1.X - pt2.X ), ( float )( pt1.Y - pt2.Y ), ( float )( pt1.Z - pt2.Z ) );
		}

		/// <summary>
		/// Adds a vector to a point
		/// </summary>
		public static BigPoint3	operator + ( BigPoint3 pt, Vector3 vec )
		{
			return new BigPoint3( pt.X + ( long )( vec.X * FromMetres ), pt.Y + ( long )( vec.Y * FromMetres ), pt.Z + ( long )( vec.Z * FromMetres ) );
		}

		/// <summary>
		/// Subtracts a vector from a point
		/// </summary>
		public static BigPoint3	operator - ( BigPoint3 pt, Vector3 vec )
		{
			return new BigPoint3( pt.X - ( long )( vec.X * FromMetres ), pt.Y - ( long )( vec.Y * FromMetres ), pt.Z - ( long )( vec.Z * FromMetres ) );
		}

		/// <summary>
		/// Gets the squared distance from one point to another
		/// </summary>
		public long	SqrDistanceTo( BigPoint3 pt )
		{
			long diffX = ( X - pt.X );
			long diffY = ( Y - pt.Y );
			long diffZ = ( Z - pt.Z );

			return ( diffX * diffX ) + ( diffY * diffY ) + ( diffZ * diffZ );
		}

		/// <summary>
		/// Gets the distance from on point to another
		/// </summary>
		/// <remarks>
		/// Because BigPoint3 has so much more precision than a float can offer, be careful about using this function
		/// </remarks>
		public float	DistanceTo( BigPoint3 pt )
		{
			return ( float )System.Math.Sqrt( ( double )SqrDistanceTo( pt ) );
		}

		private long	m_X;
		private long	m_Y;
		private long	m_Z;
	}
}
