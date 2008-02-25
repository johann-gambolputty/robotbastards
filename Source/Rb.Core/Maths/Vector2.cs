using System;
using System.Diagnostics;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Summary description for Vector2.
	/// </summary>
	[DebuggerDisplay("({X},{Y})")]
	[Serializable]
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
		public void IpAdd( Vector2 rhs )
		{
			X += rhs.X;
			Y += rhs.Y;
		}

		/// <summary>
		/// In-place subtraction
		/// </summary>
		public void IpSubtract( Vector2 rhs )
		{
			X -= rhs.X;
			Y -= rhs.Y;
		}

		/// <summary>
		/// In-place multiplication by a scalar
		/// </summary>
		public void IpMultiplyByValue( float rhs )
		{
			X *= rhs;
			Y *= rhs;
		}

		/// <summary>
		/// In-place division by a scalar
		/// </summary>
		public void IpDivideByValue( float rhs )
		{
			X /= rhs;
			Y /= rhs;
		}

		#endregion

		#region	Overloaded operator secondary functions

		/// <summary>
		/// Adds lhs to rhs and returns the result
		/// </summary>
		public static Vector2 Add( Vector2 lhs, Vector2 rhs )
		{
			return new Vector2( lhs.X + rhs.X, lhs.Y + rhs.Y );
		}

		/// <summary>
		/// Subtracts rhs from lhs and returns the result
		/// </summary>
		public static Vector2 Subtract( Vector2 lhs, Vector2 rhs )
		{
			return new Vector2( lhs.X - rhs.X, lhs.Y - rhs.Y );
		}

		/// <summary>
		/// Multiplies the vector lhs by the scalar rhs and returns the result
		/// </summary>
		public static Vector2 MultiplyByValue( Vector2 lhs, float rhs )
		{
			return new Vector2( lhs.X * rhs, lhs.Y * rhs );
		}

		/// <summary>
		/// Divides the vector lhs by the scalar rhs and returns the result
		/// </summary>
		public static Vector2 DivideByValue( Vector2 lhs, float rhs )
		{
			return new Vector2( lhs.X / rhs, lhs.Y / rhs );
		}

		#endregion

		#region	Overloaded arithmetic operators

		/// <summary>
		/// Adds lhs to rhs and returns the result
		/// </summary>
		public static Vector2 operator + ( Vector2 lhs, Vector2 rhs )
		{
			return Add( lhs, rhs );
		}

		/// <summary>
		/// Subtracts rhs from lhs and returns the result
		/// </summary>
		public static Vector2 operator - ( Vector2 lhs, Vector2 rhs )
		{
			return Subtract( lhs, rhs );
		}

		/// <summary>
		/// Multiplies the vector lhs by the scalar rhs and returns the result
		/// </summary>
		public static Vector2 operator * ( Vector2 lhs, float rhs )
		{
			return MultiplyByValue( lhs, rhs );
		}

		/// <summary>
		/// Divides the vector lhs by the scalar rhs and returns the result
		/// </summary>
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
		/// Creates a normalised, perpendicular copy of this vector
		/// </summary>
		public Vector2 MakePerpNormal( )
		{
			float length = Length;
			return new Vector2( m_Y / length, m_X / -length );
		}

		/// <summary>
		/// Calculates the perp dot product of two vectors
		/// </summary>
		public float DotPerp( Vector2 vec )
		{
			return m_X * vec.m_Y - m_Y * vec.m_X;
		}

		/// <summary>
		/// Makes this vector perpendicular to itself
		/// </summary>
		public void Perp( )
		{
			float x = m_X;
			m_X = m_Y;
			m_Y = -x;
		}

		/// <summary>
		/// Makes a vector perpendicular to this one
		/// </summary>
		public Vector2 MakePerp( )
		{
			return new Vector2( m_Y, -m_X );
		}

		/// <summary>
		/// Returns the dot product of this vector with another
		/// </summary>
		public float Dot( Vector2 vec )
		{
			return ( X * vec.X + Y * vec.Y );
		}

		/// <summary>
		/// Returns the dot product of this vector with another
		/// </summary>
		public float Dot( Point2 pt )
		{
			return ( X * pt.X + Y * pt.Y );
		}


		#endregion

		#region	Fields

		private float	m_X;
		private float	m_Y;

		#endregion
	}
}
