using System;

namespace RbEngine.Maths
{
	/// <summary>
	/// Summary description for Vector2.
	/// </summary>
	public class Vector2
	{
		#region	Vector constants

		public static Vector2 XAxis
		{
			get
			{
				return new Vector2( 1, 0 );
			}
		}

		public static Vector2 YAxis
		{
			get
			{
				return new Vector2( 0, 1 );
			}
		}

		#endregion

		#region	Construction and setup

		public Vector2( )
		{
		}

		public Vector2( Vector2 src )
		{
			X = src.X;
			Y = src.Y;
		}

		public Vector2( float x, float y )
		{
			X = x;
			Y = y;
		}

		public void Set( float x, float y )
		{
			X = x;
			Y = y;
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
				return ( X * X + Y * Y );
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

		public void IpAdd( Vector2 rhs )
		{
			X += rhs.X;
			Y += rhs.Y;
		}

		public void IpSubtract( Vector2 rhs )
		{
			X -= rhs.X;
			Y -= rhs.Y;
		}

		public void IpMultiplyByValue( float rhs )
		{
			X *= rhs;
			Y *= rhs;
		}

		public void IpDivideByValue( float rhs )
		{
			X /= rhs;
			Y /= rhs;
		}

		#endregion

		#region	Overloaded operator secondary functions

		public static Vector2 Add( Vector2 lhs, Vector2 rhs )
		{
			return new Vector2( lhs.X + rhs.X, lhs.Y + rhs.Y );
		}

		public static Vector2 Subtract( Vector2 lhs, Vector2 rhs )
		{
			return new Vector2( lhs.X - rhs.X, lhs.Y - rhs.Y );
		}

		public static Vector2 MultiplyByValue( Vector2 lhs, float rhs )
		{
			return new Vector2( lhs.X * rhs, lhs.Y * rhs );
		}

		public static Vector2 DivideByValue( Vector2 lhs, float rhs )
		{
			return new Vector2( lhs.X / rhs, lhs.Y / rhs );
		}

		#endregion

		#region	Overloaded arithmetic operators

		public static Vector2 operator + ( Vector2 lhs, Vector2 rhs )
		{
			return Add( lhs, rhs );
		}

		public static Vector2 operator - ( Vector2 lhs, Vector2 rhs )
		{
			return Subtract( lhs, rhs );
		}

		public static Vector2 operator * ( Vector2 lhs, float rhs )
		{
			return MultiplyByValue( lhs, rhs );
		}

		public static Vector2 operator / ( Vector2 lhs, float rhs )
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
		public Vector2 MakeNormal( )
		{
			return this / Length;
		}

		/// <summary>
		/// Returns the square of the distance from this point to another
		/// </summary>
		public float SqrDistanceTo( Vector2 pt )
		{
			float diffX = X - pt.X;
			float diffY = Y - pt.Y;

			return ( diffX * diffX + diffY * diffY );
		}

		/// <summary>
		/// Returns the dot product of this vector with another
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public float Dot( Vector2 vec )
		{
			return ( X * vec.X + Y * vec.Y );
		}

		/// <summary>
		/// Returns the distance from this point to another
		/// </summary>
		public float DistanceTo( Vector2 pt )
		{
			return ( float )System.Math.Sqrt( SqrDistanceTo( pt ) );
		}

		#endregion

		#region	Fields

		private float[] m_Vec = new float[ 2 ];

		#endregion
	}
}
