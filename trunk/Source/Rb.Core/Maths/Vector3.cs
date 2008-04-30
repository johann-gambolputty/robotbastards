using System;
using System.Diagnostics;

namespace Rb.Core.Maths
{
	/// <summary>
	/// 3 element floating point vector
    /// </summary>
    [DebuggerDisplay("({X},{Y},{Z})")]
	[Serializable]
	public struct Vector3
	{
		#region	Vector constants

		/// <summary>
		/// X axis vector (1,0,0)
		/// </summary>
		public static readonly Vector3 XAxis = new Vector3( 1, 0, 0 );
		
		/// <summary>
		/// Y axis vector (0,1,0)
		/// </summary>
		public static readonly Vector3 YAxis = new Vector3( 0, 1, 0 );

		/// <summary>
		/// Z axis vector (0,0,1)
		/// </summary>
		public static readonly Vector3 ZAxis = new Vector3( 0, 0, 1 );

		/// <summary>
		/// Origin vector
		/// </summary>
		public static readonly Vector3 Origin = new Vector3( 0, 0, 0 );

		#endregion

		#region	Construction and setup

		/// <summary>
		/// Copies the source vector
		/// </summary>
		public Vector3( Vector3 src )
		{
			m_X = src.X;
			m_Y = src.Y;
			m_Z = src.Z;
		}

		/// <summary>
		/// Sets individual vector components
		/// </summary>
		public Vector3( float x, float y, float z )
		{
			m_X = x;
			m_Y = y;
			m_Z = z;
		}

		/// <summary>
		/// Sets individual vector components
		/// </summary>
		public void Set( float x, float y, float z )
		{
			X = x;
			Y = y;
			Z = z;
		}

		#endregion

		#region	Properties

		/// <summary>
		/// Access to the X component of the vector
		/// </summary>
		public float X
		{
			get { return m_X; }
			set { m_X = value; }
		}

		/// <summary>
		/// Access to the Y component of the vector
		/// </summary>
		public float Y
		{
			get { return m_Y; }
			set { m_Y = value; }
		}

		/// <summary>
		/// Access to the Z component of the vector
		/// </summary>
		public float Z
		{
			get { return m_Z; }
			set { m_Z = value; }
		}

		/// <summary>
		/// Access to an indexed component of the vector (0==X,1==Y,2==Z)
		/// </summary>
		public unsafe float this[ int index ]
		{
			get
			{
				//	oh dear...
				fixed ( float* vec = &m_X )
				{
					return vec[ index ];
				}
			}
			set
			{
				fixed ( float* vec = &m_X )
				{
					vec[ index ] = value;
				}
			}
		}

		/// <summary>
		/// Access to the squared length of this vector
		/// </summary>
		public float SqrLength
		{
			get
			{
				return ( X * X + Y * Y + Z * Z );
			}
			set
			{
				Length = Functions.Sqrt( value );
			}
		}

		/// <summary>
		/// Access to the length of this vector
		/// </summary>
		public float Length
		{
			get
			{
				return Functions.Sqrt( SqrLength );
			}
			set
			{
				IpMultiplyByValue( value / Functions.Sqrt( SqrLength ) );
			}
		}

		#endregion

		#region	In place overloaded operator secondary functions

		/// <summary>
		/// In-place add.
		/// </summary>
		public void IpAdd( Vector3 rhs )
		{
			X += rhs.X;
			Y += rhs.Y;
			Z += rhs.Z;
		}

		/// <summary>
		/// In-place subtraction
		/// </summary>
		public void IpSubtract( Vector3 rhs )
		{
			X -= rhs.X;
			Y -= rhs.Y;
			Z -= rhs.Z;
		}

		/// <summary>
		/// In-place multiplication by a scalar
		/// </summary>
		public void IpMultiplyByValue( float rhs )
		{
			X *= rhs;
			Y *= rhs;
			Z *= rhs;
		}

		/// <summary>
		/// In-place division by a scalar
		/// </summary>
		public void IpDivideByValue( float rhs )
		{
			X /= rhs;
			Y /= rhs;
			Z /= rhs;
		}

		#endregion

		#region	Overloaded operator secondary functions

		/// <summary>
		/// Adds lhs to rhs and returns the result
		/// </summary>
		public static Vector3 Add( Vector3 lhs, Vector3 rhs )
		{
			return new Vector3( lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z );
		}

		/// <summary>
		/// Subtracts rhs from lhs and returns the result
		/// </summary>
		public static Vector3 Subtract( Vector3 lhs, Vector3 rhs )
		{
			return new Vector3( lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z );
		}

		/// <summary>
		/// Multiplies the vector lhs by the vector rhs and returns the result
		/// </summary>
		public static Vector3 Multiply( Vector3 lhs, Vector3 rhs )
		{
			return new Vector3( lhs.X * rhs.X, lhs.Y * rhs.Y, lhs.Z * rhs.Z );
		}

		/// <summary>
		/// Multiplies the vector lhs by the scalar rhs and returns the result
		/// </summary>
		public static Vector3 MultiplyByValue( Vector3 lhs, float rhs )
		{
			return new Vector3( lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs );
		}

		/// <summary>
		/// Divides the vector lhs by the scalar rhs and returns the result
		/// </summary>
		public static Vector3 DivideByValue( Vector3 lhs, float rhs )
		{
			return new Vector3( lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs );
		}

		#endregion

		#region	Overloaded operators

		
		/// <summary>
		/// Returns true if two vectors are -exactly- equal (no tolerance for floating point imprecision)
		/// </summary>
		public static bool operator == ( Vector3 lhs, Vector3 rhs )
		{
			return ( lhs.X == rhs.X ) && ( lhs.Y == rhs.Y ) && ( lhs.Z == rhs.Z );
		}

		/// <summary>
		/// Returns false if two vectors are in any way unequal (no tolerance for floating point imprecision)
		/// </summary>
		public static bool operator != ( Vector3 lhs, Vector3 rhs )
		{
			return ( lhs.X != rhs.X ) || ( lhs.Y != rhs.Y ) || ( lhs.Z != rhs.Z );
		}

		/// <summary>
		/// Unary minus operator
		/// </summary>
		/// <param name="src">Source vector</param>
		/// <returns>Reversed vector</returns>
		public static Vector3 operator - ( Vector3 src )
		{
			return new Vector3( -src.X, -src.Y, -src.Z );
		}

		/// <summary>
		/// Adds lhs to rhs and returns the result
		/// </summary>
		public static Vector3 operator + ( Vector3 lhs, Vector3 rhs )
		{
			return Add( lhs, rhs );
		}

		/// <summary>
		/// Subtracts rhs from lhs and returns the result
		/// </summary>
		public static Vector3 operator - ( Vector3 lhs, Vector3 rhs )
		{
			return Subtract( lhs, rhs );
		}

		/// <summary>
		/// Multiplies the vector lhs by the vector rhs and returns the result
		/// </summary>
		public static Vector3 operator * ( Vector3 lhs, Vector3 rhs )
		{
			return Multiply( lhs, rhs );
		}

		/// <summary>
		/// Multiplies the vector lhs by the scalar rhs and returns the result
		/// </summary>
		public static Vector3 operator * ( Vector3 lhs, float rhs )
		{
			return MultiplyByValue( lhs, rhs );
		}

		/// <summary>
		/// Divides the vector lhs by the scalar rhs and returns the result
		/// </summary>
		public static Vector3 operator / ( Vector3 lhs, float rhs )
		{
			return DivideByValue( lhs, rhs );
		}

		#endregion

		#region	Operations

		/// <summary>
		/// Creates a point from this vector
		/// </summary>
		public Point3 ToPoint3( )
		{
			return new Point3( X, Y, Z );
		}

		/// <summary>
		/// Converts this point to a string
		/// </summary>
		public override string ToString( )
		{
			return string.Format( "({0},{1},{2})", X, Y, Z );
		}
		
		/// <summary>
		/// Returns true if obj is a Point3 -exactly equal- to this point (no floating point tolerance)
		/// </summary>
		public override bool Equals( object obj )
		{
			return ( obj is Vector3 ) && ( ( Vector3 )obj == this );
		}

		/// <summary>
		/// Returns the hash code of this point
		/// </summary>
		public unsafe override int GetHashCode()
		{
			//	Is this a good hash? who knows? fast, though :)
			float res = m_X + m_Y + m_Z;
			return *( int* )&res;
		}

		/// <summary>
		/// Normalises this vector
		/// </summary>
		public void Normalise( )
		{
			Length = 1;
		}

		/// <summary>
		/// Creates a normalised copy of this vector
		/// </summary>
		public Vector3 MakeNormal( )
		{
			return this / Length;
		}

		/// <summary>
		/// Returns the square of the distance from this point to another
		/// </summary>
		public float SqrDistanceTo( Vector3 pt )
		{
			float diffX = X - pt.X;
			float diffY = Y - pt.Y;
			float diffZ = Z - pt.Z;

			return ( diffX * diffX + diffY * diffY + diffZ * diffZ );
		}

		/// <summary>
		/// Returns the dot product of this vector with another
		/// </summary>
		public float Dot( Vector3 vec )
		{
			return ( X * vec.X + Y * vec.Y + Z * vec.Z );
		}

		/// <summary>
		/// Returns the dot product of this vector with a point (used by plane intersection code...)
		/// </summary>
		public float Dot( Point3 pt )
		{
			return ( X * pt.X + Y * pt.Y + Z * pt.Z );
		}
		/// <summary>
		/// Returns the distance from this point to another
		/// </summary>
		public float DistanceTo( Vector3 pt )
		{
			return Functions.Sqrt( SqrDistanceTo( pt ) );
		}

		/// <summary>
		/// Returns the cross product of two vectors
		/// </summary>
		public static Vector3 Cross( Vector3 v1, Vector3 v2 )
		{
			return new Vector3( ( v1.Y * v2.Z ) - ( v1.Z * v2.Y ), ( v1.Z * v2.X ) - ( v1.X * v2.Z ), ( v1.X * v2.Y ) - ( v1.Y * v2.X ) );
		}

		#endregion

		#region	Fields

		private float m_X;
		private float m_Y;
		private float m_Z;

		#endregion

	}
}
