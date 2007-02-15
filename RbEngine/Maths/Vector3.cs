using System;

namespace RbEngine.Maths
{
	/// <summary>
	/// 3 element floating point vector
	/// </summary>
	public class Vector3
	{
		#region	Vector constants

		public static Vector3 XAxis
		{
			get
			{
				return new Vector3( 1, 0, 0 );
			}
		}

		public static Vector3 YAxis
		{
			get
			{
				return new Vector3( 0, 1, 0 );
			}
		}

		public static Vector3 ZAxis
		{
			get
			{
				return new Vector3( 0, 0, 1 );
			}
		}

		#endregion

		#region	Construction and setup

		public Vector3( )
		{
		}

		public Vector3( Vector3 src )
		{
			X = src.X;
			Y = src.Y;
			Z = src.Z;
		}

		public Vector3( float x, float y, float z )
		{
			X = x;
			Y = y;
			Z = z;
		}

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
			get
			{
				return m_Vec[ 0 ];
			}
			set
			{
				m_Vec[ 0 ] = value;
			}
		}

		/// <summary>
		/// Access to the Y component of the vector
		/// </summary>
		public float Y
		{
			get
			{
				return m_Vec[ 1 ];
			}
			set
			{
				m_Vec[ 1 ] = value;
			}
		}

		/// <summary>
		/// Access to the Z component of the vector
		/// </summary>
		public float Z
		{
			get
			{
				return m_Vec[ 2 ];
			}
			set
			{
				m_Vec[ 2 ] = value;
			}
		}

		/// <summary>
		/// Access to an indexed component of the vector (0==X,1==Y,2==Z)
		/// </summary>
		public float this[ int index ]
		{
			get
			{
				return m_Vec[ index ];
			}
			set
			{
				m_Vec[ index ] = value;
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
				Length = value * value;
			}
		}

		/// <summary>
		/// Access to the length of this vector
		/// </summary>
		public float Length
		{
			get
			{
				return ( float )System.Math.Sqrt( SqrLength );
			}
			set
			{
				IpMultiplyByValue( value / ( float )System.Math.Sqrt( SqrLength ) );
			}
		}

		#endregion

		#region	In place overloaded operator secondary functions

		public void IpAdd( Vector3 rhs )
		{
			X += rhs.X;
			Y += rhs.Y;
			Z += rhs.Z;
		}

		public void IpSubtract( Vector3 rhs )
		{
			X -= rhs.X;
			Y -= rhs.Y;
			Z -= rhs.Z;
		}

		public void IpMultiplyByValue( float rhs )
		{
			X *= rhs;
			Y *= rhs;
			Z *= rhs;
		}

		public void IpDivideByValue( float rhs )
		{
			X /= rhs;
			Y /= rhs;
			Z /= rhs;
		}

		#endregion

		#region	Overloaded operator secondary functions

		public static Vector3 Add( Vector3 lhs, Vector3 rhs )
		{
			return new Vector3( lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z );
		}

		public static Vector3 Subtract( Vector3 lhs, Vector3 rhs )
		{
			return new Vector3( lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z );
		}

		public static Vector3 MultiplyByValue( Vector3 lhs, float rhs )
		{
			return new Vector3( lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs );
		}

		public static Vector3 DivideByValue( Vector3 lhs, float rhs )
		{
			return new Vector3( lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs );
		}

		#endregion

		#region	Overloaded arithmetic operators

		public static Vector3 operator + ( Vector3 lhs, Vector3 rhs )
		{
			return Add( lhs, rhs );
		}

		public static Vector3 operator - ( Vector3 lhs, Vector3 rhs )
		{
			return Subtract( lhs, rhs );
		}

		public static Vector3 operator * ( Vector3 lhs, float rhs )
		{
			return MultiplyByValue( lhs, rhs );
		}

		public static Vector3 operator / ( Vector3 lhs, float rhs )
		{
			return DivideByValue( lhs, rhs );
		}

		#endregion

		#region	Operations

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
		/// <param name="vec"></param>
		/// <returns></returns>
		public float Dot( Vector3 vec )
		{
			return ( X * vec.X + Y * vec.Y + Z * vec.Z );
		}

		/// <summary>
		/// Returns the distance from this point to another
		/// </summary>
		public float DistanceTo( Vector3 pt )
		{
			return ( float )System.Math.Sqrt( SqrDistanceTo( pt ) );
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

		private float[] m_Vec = new float[ 3 ];

		#endregion
	}
}
