using System;

namespace RbEngine.Maths
{
	/// <summary>
	/// Summary description for Vector2.
	/// </summary>
	public struct Vector2
	{
		#region	Vector constants

		/// <summary>
		/// X axis
		/// </summary>
		public static readonly Vector2	XAxis = new Vector2( 1, 0 );

		/// <summary>
		/// Y axis
		/// </summary>
		public static readonly Vector2	YAxis = new Vector2( 0, 1 );

		/// <summary>
		/// Origin
		/// </summary>
		public static readonly Vector2	Origin = new Vector2( 0, 0 );

		#endregion

		#region	Construction and setup

		/// <summary>
		/// Copy constructor
		/// </summary>
		public Vector2( Vector2 src )
		{
			m_X = src.X;
			m_Y = src.Y;
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public Vector2( float x, float y )
		{
			m_X = x;
			m_Y = y;
		}

		/// <summary>
		/// Sets x and y elements of this vector
		/// </summary>
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
				return m_X;
			}
			set
			{
				m_X = value;
			}
		}

		/// <summary>
		/// Access to the Y component of the vector
		/// </summary>
		public float Y
		{
			get
			{
				return m_Y;
			}
			set
			{
				m_Y = value;
			}
		}

		/// <summary>
		/// Access to an indexed component of the vector (0==X,1==Y,2==Z)
		/// </summary>
		public unsafe float this[ int index ]
		{
			get
			{
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

		private float	m_X;
		private float	m_Y;

		#endregion
	}
}
