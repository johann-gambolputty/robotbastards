using System;
using System.Diagnostics;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Quaternion value type. Where is SIMD support in C#? eh? gits
	/// </summary>
	/// <remarks>
	/// Adapted from http://www.geometrictools.com
	/// </remarks>
	[DebuggerDisplay("({X},{Y},{Z},{W})")]
	[Serializable]
	public struct Quaternion
	{
		#region Builders

		/// <summary>
		/// Creates a quaternion from an axis and an angle
		/// </summary>
		public static Quaternion FromAxisAngle( Vector3 axis, float angleInRadians )
		{
			float halfAngle = angleInRadians / 2.0f;
			float sinHalfAngle = Functions.Sin( halfAngle );
			float cosHalfAngle = Functions.Cos( halfAngle );

			// Calculate the x, y and z of the quaternion
			float x = axis.X * sinHalfAngle;
			float y = axis.Y * sinHalfAngle;
			float z = axis.Z * sinHalfAngle;
	
			return new Quaternion( x, y, z, cosHalfAngle );
		}

		/// <summary>
		/// Creates a quaternion from a matrix
		/// </summary>
		public static Quaternion FromMatrix( Matrix44 matrix )
		{
			return new Quaternion( matrix );
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Sets up the coefficients of this quaternion
		/// </summary>
		/// <remarks>
		/// The stored quaternion is not normalized
		/// </remarks>
		public Quaternion( float x, float y, float z, float w )
		{
			m_X = x;
			m_Y = y;
			m_Z = z;
			m_W = w;
		}
		
		/// <summary>
		/// Makes a copy of the source quaternion
		/// </summary>
		public Quaternion( Quaternion src )
		{
			m_X = src.m_X;
			m_Y = src.m_Y;
			m_Z = src.m_Z;
			m_W = src.m_W;
		}

		///// <summary>
		///// Builds this quaternion from an axis-angle pair
		///// </summary>
		///// <param name="vec">Rotation vector</param>
		///// <param name="angle">Rotation angle (in radians)</param>
		//public Quaternion( Vector3 vec, float angle )
		//{
		//	//	TODO: AP: ...
		//}

		/// <summary>
		/// Builds this quaternion from the rotation part of a transform matrix
		/// </summary>
		public Quaternion( Matrix44 matrix )
		{    
			// Algorithm in Ken Shoemake's article in 1987 SIGGRAPH course notes
			// article "Quaternion Calculus and Fast Animation".

			float trace = matrix[ 0, 0 ] + matrix[ 1, 1 ] + matrix[ 2, 2 ];
			float root;

			if ( trace > 0.0f )
			{
				// |w| > 1/2, may as well choose w > 1/2
				root = Functions.Sqrt( trace + 1.0f );  // 2w
				m_W = 0.5f * root;
				root = 0.5f / root;  // 1/(4w)
				m_X = ( matrix[ 2, 1 ] - matrix[ 1, 2 ] ) * root;
				m_Y = ( matrix[ 0, 2 ] - matrix[ 2, 0 ] ) * root;
				m_Z = ( matrix[ 1, 0 ] - matrix[ 0, 1 ] ) * root;
			}
			else
			{
				// |w| <= 1/2
				int i = 0;
				if ( matrix[ 1, 1 ] > matrix[ 0, 0 ] )
				{
					i = 1;
				}
				if ( matrix[ 2, 2 ] > matrix[ i, i ] )
				{
					i = 2;
				}
				int j = ( i + 1 ) % 3;
				int k = ( j + 1 ) % 3;

				root = Functions.Sqrt( matrix[ i, i ] - matrix[ j, j ] - matrix[ k, k ] + 1.0f );

				Point3 quat = new Point3( );
				quat[ i ] = 0.5f * root;
				root = 0.5f / root;
				m_W = ( matrix[ k, j ] - matrix[ j, k ] ) * root;
				quat[ j ] = ( matrix[ j, i ] + matrix[ i, j ] ) * root;
				quat[ k ] = ( matrix[ k, i ] + matrix[ i, k ] ) * root;

				m_X = quat.X;
				m_Y = quat.Y;
				m_Z = quat.Z;
			}
		}

		#endregion

		#region Operations

		/// <summary>
		/// Normalizes this quaternion
		/// </summary>
		public void Normalize( )
		{
			float invLength = 1.0f / Functions.Sqrt( m_X * m_X + m_Y * m_Y + m_Z * m_Z + m_W * m_W );
			m_X *= invLength;
			m_Y *= invLength;
			m_Z *= invLength;
			m_W *= invLength;
		}

		/// <summary>
		/// Makes a normalized copy of this quaternion
		/// </summary>
		public Quaternion MakeNormal( )
		{
			float invLength = 1.0f / Functions.Sqrt( m_X * m_X + m_Y * m_Y + m_Z * m_Z + m_W * m_W );
			return new Quaternion( m_X * invLength, m_Y * invLength, m_Z * invLength, m_W * invLength );
		}

		#endregion

		#region Operators


		/// <summary>
		/// Returns the multiple of 2 quaternions (note that quaternion multiplication is non-commutative)
		/// </summary>
		public static Quaternion operator * ( Quaternion lhs, Quaternion rhs )
		{
			float x =
				lhs.m_W * rhs.m_X +
				lhs.m_X * rhs.m_W +
				lhs.m_Y * rhs.m_Z -
				lhs.m_Z * rhs.m_Y;

			float y =
				lhs.m_W * rhs.m_Y +
				lhs.m_Y * rhs.m_W +
				lhs.m_Z * rhs.m_X -
				lhs.m_X * rhs.m_Z;

			float z =
				lhs.m_W * rhs.m_Z +
				lhs.m_Z * rhs.m_W +
				lhs.m_X * rhs.m_Y -
				lhs.m_Y * rhs.m_X;

			float w =
				lhs.m_W * rhs.m_W -
				lhs.m_X * rhs.m_X -
				lhs.m_Y * rhs.m_Y -
				lhs.m_Z * rhs.m_Z;

			return new Quaternion( x, y, z, w );
		}
		
		public Quaternion MakeInverse( )
		{
			float norm = X * X + Y * Y + Z * Z + W * W;
			if ( norm > 0 )
			{
				float invNorm = 1.0f / norm;
				return new Quaternion
				(
					X * -invNorm,
					Y * -invNorm,
					Z * -invNorm,
					W * invNorm
				);
			}
			return new Quaternion();
		}

		#endregion

		#region	Properties

		/// <summary>
		/// Returns the squared length of this quaternion
		/// </summary>
		public float SqrLength
		{
			get { return ( m_X * m_X + m_Y * m_Y + m_Z * m_Z + m_W * m_W ); }
		}

		/// <summary>
		/// Returns the length of this quaternion
		/// </summary>
		public float Length
		{
			get { return Functions.Sqrt( SqrLength ); }
		}

		/// <summary>
		/// Access to the X component of the quaternion
		/// </summary>
		public float X
		{
			get { return m_X; }
			set { m_X = value; }
		}

		/// <summary>
		/// Access to the Y component of the quaternion
		/// </summary>
		public float Y
		{
			get { return m_Y; }
			set { m_Y = value; }
		}

		/// <summary>
		/// Access to the Z component of the quaternion
		/// </summary>
		public float Z
		{
			get { return m_Z; }
			set { m_Z = value; }
		}

		/// <summary>
		/// Access to the W component of the quaternion
		/// </summary>
		public float W
		{
			get { return m_W; }
			set { m_W = value; }
		}

		/// <summary>
		/// Access to an indexed component of the quaternion (0==X,1==Y,2==Z,3==W)
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

		#endregion

		#region Private members

		private float m_X;
		private float m_Y;
		private float m_Z;
		private float m_W;

		#endregion
	}
}
