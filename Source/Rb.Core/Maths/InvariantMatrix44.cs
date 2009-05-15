
using System;
using Rb.Core.Utils;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Invariant matrix
	/// </summary>
	/// <remarks>
	/// Used as a base class for <see cref="Matrix44"/>. This matrix class only contains operations
	/// that do not change the underlying matrix.
	/// </remarks>
	public class InvariantMatrix44
	{
		#region	Predefined matrices

		/// <summary>
		/// Identity matrix
		/// </summary>
		public static readonly InvariantMatrix44 Identity = MakeIdentityMatrix( );

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor. Builds this as an identity matrix
		/// </summary>
		public InvariantMatrix44( )
		{
			m_Elements[ 0 ] = m_Elements[ 5 ] = m_Elements[ 10 ] = m_Elements[ 15 ] = 1;
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		public InvariantMatrix44( InvariantMatrix44 src )
		{
			Arguments.CheckNotNull( src, "src" );
			for ( int index = 0; index < 16; ++index )
			{
				m_Elements[ index ] = src.m_Elements[ index ];
			}
		}

		/// <summary>
		/// Setup constructor, from basis vectors and a translation
		/// </summary>
		public InvariantMatrix44( Vector3 xAxis, Vector3 yAxis, Vector3 zAxis, Point3 pos )
		{
			m_Elements[ 0 ] = xAxis.X; m_Elements[ 1 ] = xAxis.Y; m_Elements[ 2 ] = xAxis.Z; m_Elements[ 3 ] = 0;
			m_Elements[ 4 ] = yAxis.X; m_Elements[ 5 ] = yAxis.Y; m_Elements[ 6 ] = yAxis.Z; m_Elements[ 7 ] = 0;
			m_Elements[ 8 ] = zAxis.X; m_Elements[ 9 ] = zAxis.Y; m_Elements[ 10 ] = zAxis.Z; m_Elements[ 11 ] = 0;
			m_Elements[ 12 ] = pos.X; m_Elements[ 13 ] = pos.Y; m_Elements[ 14 ] = pos.Z; m_Elements[ 15 ] = 1;
		}

		/// <summary>
		/// Sets up all the matrices' elements
		/// </summary>
		public InvariantMatrix44( float M00, float M10, float M20, float M30,
						 float M01, float M11, float M21, float M31,
						 float M02, float M12, float M22, float M32,
						 float M03, float M13, float M23, float M33 )
		{
			m_Elements[ 0 ]  = M00; m_Elements[ 1 ]  = M10; m_Elements[ 2 ]  = M20; m_Elements[ 3 ]  = M30;
			m_Elements[ 4 ]  = M01; m_Elements[ 5 ]  = M11; m_Elements[ 6 ]  = M21; m_Elements[ 7 ]  = M31;
			m_Elements[ 8 ]  = M02; m_Elements[ 9 ]  = M12; m_Elements[ 10 ] = M22; m_Elements[ 11 ] = M32;
			m_Elements[ 12 ] = M03; m_Elements[ 13 ] = M13; m_Elements[ 14 ] = M23; m_Elements[ 15 ] = M33;
		}

		#endregion

		#region Building

		/// <summary>
		/// Creates a new identity matrix
		/// </summary>
		public static InvariantMatrix44 MakeIdentityMatrix( )
		{
			return new InvariantMatrix44( );
		}

		/// <summary>
		/// Makes a translation matrix from a point
		/// </summary>
		public static InvariantMatrix44 MakeTranslationMatrix( Point3 translation )
		{
			return MakeTranslationMatrix( translation.X, translation.Y, translation.Z );
		}

		/// <summary>
		/// Makes a translation matrix from a vector
		/// </summary>
		public static InvariantMatrix44 MakeTranslationMatrix( Vector3 translation )
		{
			return MakeTranslationMatrix( translation.X, translation.Y, translation.Z );
		}

		/// <summary>
		/// Makes a translation matrix from a vector
		/// </summary>
		public static InvariantMatrix44 MakeTranslationMatrix( float x, float y, float z )
		{
			InvariantMatrix44 matrix = new InvariantMatrix44( );
			MakeTranslationMatrix( matrix, x, y, z );
			return matrix;
		}

		/// <summary>
		/// Makes a scale matrix from a vector
		/// </summary>
		public static InvariantMatrix44 MakeScaleMatrix( float x, float y, float z )
		{
			InvariantMatrix44 matrix = new InvariantMatrix44( );
			MakeScaleMatrix( matrix, x, y, z );
			return matrix;
		}

		/// <summary>
		/// Makes a matrix storing a rotation around the x-axis
		/// </summary>
		public static InvariantMatrix44 MakeRotationAroundXAxisMatrix( float angleInRadians )
		{
			InvariantMatrix44 matrix = new InvariantMatrix44( );
			MakeRotationAroundXAxisMatrix( matrix, angleInRadians );
			return matrix;
		}

		/// <summary>
		/// Makes a matrix storing a rotation around the y-axis
		/// </summary>
		public static InvariantMatrix44 MakeRotationAroundYAxisMatrix( float angleInRadians )
		{
			InvariantMatrix44 matrix = new InvariantMatrix44( );
			MakeRotationAroundYAxisMatrix( matrix, angleInRadians );
			return matrix;
		}

		/// <summary>
		/// Makes a matrix storing a rotation around the z-axis
		/// </summary>
		public static InvariantMatrix44 MakeRotationAroundZAxisMatrix( float angleInRadians )
		{
			InvariantMatrix44 matrix = new InvariantMatrix44( );
			MakeRotationAroundZAxisMatrix( matrix, angleInRadians );
			return matrix;
		}

		/// <summary>
		/// Makes a matrix from a quaternion
		/// </summary>
		public static InvariantMatrix44 MakeQuaternionMatrix( Quaternion quaternion )
		{
			InvariantMatrix44 result = new InvariantMatrix44( );
			MakeQuaternionMatrix( result, quaternion );
			return result;
		}

		/// <summary>
		/// Stores the reflection of src in result
		/// </summary>
		public static InvariantMatrix44 MakeReflectionMatrix( Point3 pointOnPlane, Vector3 planeNormal )
		{
			InvariantMatrix44 result = new InvariantMatrix44( );
			MakeReflectionMatrix( result, pointOnPlane, planeNormal );
			return result;
		}

		#endregion

		#region Accessors

		/// <summary>
		/// Gets a single element from the matrix
		/// </summary>
		/// <param name="index">Element index</param>
		/// <returns>Element value at the specified index</returns>
		public float this[ int index ]
		{
			get { return m_Elements[ index ]; }
		}

		/// <summary>
		/// Gets a single element from the matrix
		/// </summary>
		/// <param name="x">X position of element</param>
		/// <param name="y">Y position of element</param>
		/// <returns>Element value at (x,y)</returns>
		public float this[ int x, int y ]
		{
			get { return m_Elements[ x + y * 4 ]; }
		}

		/// <summary>
		/// Extracts the x axis from this matrix, if the matrix were transposed 
		/// </summary>
		public Vector3 TransposedXAxis
		{
			get { return new Vector3( m_Elements[ 0 ], Elements[ 4 ], Elements[ 8 ] ); }
		}

		/// <summary>
		/// Extracts the y axis from this matrix, if the matrix were transposed 
		/// </summary>
		public Vector3 TransposedYAxis
		{
			get { return new Vector3( m_Elements[ 1 ], m_Elements[ 5 ], m_Elements[ 9 ] ); }
		}

		/// <summary>
		/// Extracts the z axis from this matrix, if the matrix were transposed 
		/// </summary>
		public Vector3 TransposedZAxis
		{
			get { return new Vector3( m_Elements[ 2 ], m_Elements[ 6 ], m_Elements[ 10 ] ); }
		}
		
		/// <summary>
		/// Extracts the x axis from this matrix
		/// </summary>
		public Vector3 XAxis
		{
			get { return new Vector3( m_Elements[ 0 ], m_Elements[ 1 ], m_Elements[ 2 ] ); }
		}

		/// <summary>
		/// Extracts the y axis from this matrix
		/// </summary>
		public Vector3 YAxis
		{
			get { return new Vector3( m_Elements[ 4 ], m_Elements[ 5 ], m_Elements[ 6 ] ); }
		}

		/// <summary>
		/// Extracts the z axis from this matrix
		/// </summary>
		public Vector3 ZAxis
		{
			get { return new Vector3( m_Elements[ 8 ], m_Elements[ 9 ], m_Elements[ 10 ] ); }
		}
		
		/// <summary>
		/// Extracts the translation from this matrix
		/// </summary>
		public Point3 Translation
		{
			get { return new Point3( m_Elements[ 12 ], m_Elements[ 13 ], m_Elements[ 14 ] ); }
		}

		/// <summary>
		/// Gets the determinant of this matrix
		/// </summary>
		/// <remarks>
		/// The determinant of an NxN matrix A is the sum of aij.Pij, with i=1 and j=0..N-1,  where aij is the matrix element at (i,j), and
		/// Pij is the determinant of A with row i and column j removed, multiplied by -1 if i+j is odd, or 1 if i+j is even.
		/// As with the adjoint operation in StoreInverse(), I was too lazy to fully expand the calculations this implies for a 4x4 matrix, so
		/// I grabbed and adapted the following code from http://www.geometrictools.com
		/// <note>
		/// The determinant is not cached in the matrix, but is calculated on the fly. Don't access this property too often.
		/// </note>
		/// </remarks>
		public float Determinant
		{
			get
			{
				float a0 = this[ 0 ] * this[ 5 ] - this[ 1 ] * this[ 4 ];
				float a1 = this[ 0 ] * this[ 6 ] - this[ 2 ] * this[ 4 ];
				float a2 = this[ 0 ] * this[ 7 ] - this[ 3 ] * this[ 4 ];
				float a3 = this[ 1 ] * this[ 6 ] - this[ 2 ] * this[ 5 ];
				float a4 = this[ 1 ] * this[ 7 ] - this[ 3 ] * this[ 5 ];
				float a5 = this[ 2 ] * this[ 7 ] - this[ 3 ] * this[ 6 ];
				float b0 = this[ 8 ] * this[ 13 ] - this[ 9 ] * this[ 12 ];
				float b1 = this[ 8 ] * this[ 14 ] - this[ 10 ] * this[ 12 ];
				float b2 = this[ 8 ] * this[ 15 ] - this[ 11 ] * this[ 12 ];
				float b3 = this[ 9 ] * this[ 14 ] - this[ 10 ] * this[ 13 ];
				float b4 = this[ 9 ] * this[ 15 ] - this[ 11 ] * this[ 13 ];
				float b5 = this[ 10 ] * this[ 15 ] - this[ 11 ] * this[ 14 ];

				float det = a0 * b5 - a1 * b4 + a2 * b3 + a3 * b2 - a4 * b1 + a5 * b0;
				return det;
			}
		}

		#endregion

		#region Operations

		#region Point and vector multiplication

		/// <summary>
		/// Multiplies a vector by this matrix, returning a new vector that stores the result (does not apply translation)
		/// </summary>
		public Vector3 Multiply( Vector3 vec )
		{
			float x = ( vec.X * Elements[ 0 ] ) + ( vec.Y * Elements[ 4 ] ) + ( vec.Z * Elements[ 8 ] );
			float y = ( vec.X * Elements[ 1 ] ) + ( vec.Y * Elements[ 5 ] ) + ( vec.Z * Elements[ 9 ] );
			float z = ( vec.X * Elements[ 2 ] ) + ( vec.Y * Elements[ 6 ] ) + ( vec.Z * Elements[ 10 ] );

			return new Vector3( x, y, z );
		}

		/// <summary>
		/// Multiplies a point by this matrix, returning a new point that stores the result
		/// </summary>
		public Point3 Multiply( Point3 pt )
		{
			float x = ( pt.X * Elements[ 0 ] ) + ( pt.Y * Elements[ 4 ] ) + ( pt.Z * Elements[ 8 ] ) + ( Elements[ 12 ] );
			float y = ( pt.X * Elements[ 1 ] ) + ( pt.Y * Elements[ 5 ] ) + ( pt.Z * Elements[ 9 ] ) + ( Elements[ 13 ] );
			float z = ( pt.X * Elements[ 2 ] ) + ( pt.Y * Elements[ 6 ] ) + ( pt.Z * Elements[ 10 ] ) + ( Elements[ 14 ] );

			return new Point3( x, y, z );
		}

		/// <summary>
		/// Multiplies a homogeneous coordinate by this matrix
		/// </summary>
		public Point3 HomogenousMultiply( Point3 pt )
		{
			float x = ( pt.X * Elements[ 0 ] ) + ( pt.Y * Elements[ 4 ] ) + ( pt.Z * Elements[ 8 ] ) + ( Elements[ 12 ] );
			float y = ( pt.X * Elements[ 1 ] ) + ( pt.Y * Elements[ 5 ] ) + ( pt.Z * Elements[ 9 ] ) + ( Elements[ 13 ] );
			float z = ( pt.X * Elements[ 2 ] ) + ( pt.Y * Elements[ 6 ] ) + ( pt.Z * Elements[ 10 ] ) + ( Elements[ 14 ] );
			float w = ( pt.X * Elements[ 3 ] ) + ( pt.Y * Elements[ 7 ] ) + ( pt.Z * Elements[ 11 ] ) + ( Elements[ 15 ] );
			w = 1.0f / w;

			return new Point3( x * w, y * w, z * w );
		}

		#endregion

		/// <summary>
		/// Returns true if the two matrices are equal, within a given tolerance per element
		/// </summary>
		public bool IsCloseTo( InvariantMatrix44 mat, float tol )
		{
			for ( int index = 0; index < 16; ++index )
			{
				if ( Math.Abs( this[ index ] - mat[ index ] ) > tol )
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Creates a transposed copy of this matrix
		/// </summary>
		public InvariantMatrix44 Transpose( )
		{
			InvariantMatrix44 result = new InvariantMatrix44( );
			MakeTransposeMatrix( result, this );
			return result;
		}

		/// <summary>
		/// Creates an inverted copy of this matrix
		/// </summary>
		public InvariantMatrix44 Invert( )
		{
			InvariantMatrix44 result = new InvariantMatrix44( );
			MakeInverse( result, this );
			return result;
		}

		/// <summary>
		/// Converts the matrix into a string representation
		/// </summary>
		/// <returns>Returns string</returns>
		public override string ToString( )
		{
			return string.Format( "X:{0} Y: {1} Z: {2} T:{3}", XAxis, YAxis, ZAxis, Translation );
		}

		/// <summary>
		/// Returns the elements of this matrix as a new float array
		/// </summary>
		public float[] ToFloatArray( )
		{
			float[] elements = new float[ 16 ];
			for ( int index = 0; index < 16; ++index )
			{
				elements[ index ] = m_Elements[ index ];
			}
			return elements;
		}

		#endregion

		#region Operators

		/// <summary>
		/// Returns the multiple of lhs and rhs
		/// </summary>
		public static InvariantMatrix44 operator * ( InvariantMatrix44 lhs, InvariantMatrix44 rhs )
		{
			InvariantMatrix44 result = new InvariantMatrix44( );
			Multiply( result, lhs, rhs );
			return result;
		}

		/// <summary>
		/// Returns the multiple of lhs and rhs
		/// </summary>
		public static Point3 operator * ( InvariantMatrix44 lhs, Point3 rhs )
		{
			return lhs.Multiply( rhs );
		}

		/// <summary>
		/// Returns the multiple of lhs and rhs
		/// </summary>
		public static Vector3 operator *( InvariantMatrix44 lhs, Vector3 rhs )
		{
			return lhs.Multiply( rhs );
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Stores a quaternion in the specified matrix
		/// </summary>
		protected static void MakeQuaternionMatrix( InvariantMatrix44 matrix, Quaternion quaternion )
		{
			float fTx = 2.0f * quaternion.X;
			float fTy = 2.0f * quaternion.Y;
			float fTz = 2.0f * quaternion.Z;
			float fTwx = fTx * quaternion.W;
			float fTwy = fTy * quaternion.W;
			float fTwz = fTz * quaternion.W;
			float fTxx = fTx * quaternion.X;
			float fTxy = fTy * quaternion.X;
			float fTxz = fTz * quaternion.X;
			float fTyy = fTy * quaternion.Y;
			float fTyz = fTz * quaternion.Y;
			float fTzz = fTz * quaternion.Z;

			float[] elements = matrix.m_Elements;

			elements[ 0 ] = 1.0f - ( fTyy + fTzz );
			elements[ 1 ] = fTxy - fTwz;
			elements[ 2 ] = fTxz + fTwy;

			elements[ 4 ] = fTxy + fTwz;
			elements[ 5 ] = 1.0f - ( fTxx + fTzz );
			elements[ 6 ] = fTyz - fTwx;

			elements[ 8 ] = fTxz - fTwy;
			elements[ 9 ] = fTyz + fTwx;
			elements[ 10 ] = 1.0f - ( fTxx + fTyy );
		}

		/// <summary>
		/// Makes a matrix storing a rotation around the x-axis
		/// </summary>
		protected static void MakeRotationAroundXAxisMatrix( InvariantMatrix44 matrix, float angleInRadians )
		{
			float sinA = Functions.Sin( angleInRadians );
			float cosA = Functions.Cos( angleInRadians );

			float[] elements = matrix.Elements;
			elements[ 0 ] = 1; elements[ 1 ] = 0; elements[ 2 ] = 0; elements[ 3 ] = 0;
			elements[ 4 ] = 0; elements[ 5 ] = cosA; elements[ 6 ] = sinA; elements[ 7 ] = 0;
			elements[ 8 ] = 0; elements[ 9 ] = -sinA; elements[ 10 ] = cosA; elements[ 11 ] = 0;
			elements[ 12 ] = 0; elements[ 13 ] = 0; elements[ 14 ] = 0; elements[ 15 ] = 1;
		}

		/// <summary>
		/// Makes a matrix storing a rotation around the y-axis
		/// </summary>
		protected static void MakeRotationAroundYAxisMatrix( InvariantMatrix44 matrix, float angleInRadians )
		{
			float sinA = Functions.Sin( angleInRadians );
			float cosA = Functions.Cos( angleInRadians );

			float[] elements = matrix.Elements;
			elements[ 0 ] = cosA;  elements[ 1 ] = 0;  elements[ 2 ] = sinA;  elements[ 3 ] = 0;
			elements[ 4 ] = 0;     elements[ 5 ] = 1;  elements[ 6 ] = 0;     elements[ 7 ] = 0;
			elements[ 8 ] = -sinA; elements[ 9 ] = 0;  elements[ 10 ] = cosA; elements[ 11 ] = 0;
			elements[ 12 ] = 0;    elements[ 13 ] = 0; elements[ 14 ] = 0;    elements[ 15 ] = 1;
		}

		/// <summary>
		/// Makes a matrix storing a rotation around the z-axis
		/// </summary>
		protected static void MakeRotationAroundZAxisMatrix( InvariantMatrix44 matrix, float angleInRadians )
		{
			float sinA = Functions.Sin( angleInRadians );
			float cosA = Functions.Cos( angleInRadians );

			float[] elements = matrix.Elements;
			elements[ 0 ] = cosA;  elements[ 1 ] = sinA; elements[ 2 ] = 0;  elements[ 3 ] = 0;
			elements[ 4 ] = -sinA; elements[ 5 ] = cosA; elements[ 6 ] = 0;  elements[ 7 ] = 0;
			elements[ 8 ] = 0;     elements[ 9 ] = 0;    elements[ 10 ] = 1; elements[ 11 ] = 0;
			elements[ 12 ] = 0;    elements[ 13 ] = 0;   elements[ 14 ] = 0; elements[ 15 ] = 1;
		}

		/// <summary>
		/// Makes the specified matrix a scale matrix
		/// </summary>
		protected static void MakeScaleMatrix( InvariantMatrix44 matrix, float x, float y, float z )
		{
			float[] elements = matrix.Elements;
			elements[ 0 ] = x; elements[ 1 ] = 0; elements[ 2 ] = 0; elements[ 3 ] = 0;
			elements[ 4 ] = 0; elements[ 5 ] = y; elements[ 6 ] = 0; elements[ 7 ] = 0;
			elements[ 8 ] = 0; elements[ 9 ] = 0; elements[ 10 ] = z; elements[ 11 ] = 0;
			elements[ 12 ] = 0; elements[ 13 ] = 0; elements[ 14 ] = 0; elements[ 15 ] = 1; 
		}

		/// <summary>
		/// Makes the specified matrix into a translation matrix
		/// </summary>
		protected static void MakeTranslationMatrix( InvariantMatrix44 matrix, float x, float y, float z )
		{
			float[] elements = matrix.Elements;
			elements[ 0 ] = 1; elements[ 1 ] = 0; elements[ 2 ] = 0; elements[ 3 ] = 0;
			elements[ 4 ] = 0; elements[ 5 ] = 1; elements[ 6 ] = 0; elements[ 7 ] = 0;
			elements[ 8 ] = 0; elements[ 9 ] = 0; elements[ 10 ] = 1; elements[ 11 ] = 0;
			elements[ 12 ] = x; elements[ 13 ] = y; elements[ 14 ] = z; elements[ 15 ] = 1; 
		}

		/// <summary>
		/// Sets up a matrix as a look-at matrix
		/// </summary>
		protected static void MakeLookAtMatrix( InvariantMatrix44 matrix, Point3 origin, Point3 lookAt, Vector3 up )
		{
			Vector3 zAxis = ( origin - lookAt ).MakeNormal( );
			Vector3 yAxis = up;
			Vector3 xAxis = Vector3.Cross( yAxis, zAxis );
			yAxis = Vector3.Cross( zAxis, xAxis );

			InvariantMatrix44 frame = new InvariantMatrix44(xAxis, yAxis, zAxis, Point3.Origin);
			InvariantMatrix44 translation = MakeTranslationMatrix( -origin );

			Multiply( matrix, frame, translation );
		}

		/// <summary>
		/// Creates a matrix from a planar reflection
		/// </summary>
		protected static void MakeReflectionMatrix( InvariantMatrix44 matrix, Point3 pointOnPlane, Vector3 planeNormal )
		{
			float[] elements = matrix.Elements;
			float np2 = 2.0f * planeNormal.Dot( pointOnPlane );
			elements[ 0 ] = 1.0f - 2.0f * planeNormal.X * planeNormal.X;
			elements[ 4 ] = -2.0f * planeNormal.X * planeNormal.Y;
			elements[ 8 ] = -2.0f * planeNormal.X * planeNormal.Z;
			elements[ 12 ] = np2 * planeNormal.X;

			elements[ 1 ] = -2.0f * planeNormal.Y * planeNormal.X;
			elements[ 5 ] = 1.0f - 2.0f * planeNormal.Y * planeNormal.Y;
			elements[ 9 ] = -2.0f * planeNormal.Y * planeNormal.Z;
			elements[ 13 ] = np2 * planeNormal.Y;

			elements[ 2 ] = -2.0f * planeNormal.Z * planeNormal.X;
			elements[ 6 ] = -2.0f * planeNormal.Z * planeNormal.Y;
			elements[ 10 ] = 1.0f - 2.0f * planeNormal.Z * planeNormal.Z;
			elements[ 14 ] = np2 * planeNormal.Z;

			elements[ 3 ] = 0.0f;
			elements[ 7 ] = 0.0f;
			elements[ 11 ] = 0.0f;
			elements[ 15 ] = 1.0f;
		}

		/// <summary>
		/// Stores the inverse of src in result
		/// </summary>
		/// <remarks>
		/// The generic inverse of a matrix A is found by dividing the adjoint of A by the determinant of A. The adjoint B of a matrix A is
		/// defined by B=bij, where bij is the determinant of A with row i and column j removed (the co-factors of A).
		/// I was too lazy to fully expand the calculations this implies for a 4x4 matrix, so I grabbed and adapted the following code from
		/// http://www.geometrictools.com
		/// </remarks>	
		protected static void MakeInverse( InvariantMatrix44 result, InvariantMatrix44 src )
		{
			//	The inverse of an nxn matrix A is the adjoint of A divided through by the determinant of A
			//	Because the adjoint and determinant use the same sub-matrix determinants, we can store these values and use them in both calculations:
			float[] elements = result.Elements;
			float[] srcElements = src.Elements;
			float a0 = srcElements[ 0 ] * srcElements[ 5 ] - srcElements[ 1 ] * srcElements[ 4 ];
			float a1 = srcElements[ 0 ] * srcElements[ 6 ] - srcElements[ 2 ] * srcElements[ 4 ];
			float a2 = srcElements[ 0 ] * srcElements[ 7 ] - srcElements[ 3 ] * srcElements[ 4 ];
			float a3 = srcElements[ 1 ] * srcElements[ 6 ] - srcElements[ 2 ] * srcElements[ 5 ];
			float a4 = srcElements[ 1 ] * srcElements[ 7 ] - srcElements[ 3 ] * srcElements[ 5 ];
			float a5 = srcElements[ 2 ] * srcElements[ 7 ] - srcElements[ 3 ] * srcElements[ 6 ];
			float b0 = srcElements[ 8 ] * srcElements[ 13 ] - srcElements[ 9 ] * srcElements[ 12 ];
			float b1 = srcElements[ 8 ] * srcElements[ 14 ] - srcElements[ 10 ] * srcElements[ 12 ];
			float b2 = srcElements[ 8 ] * srcElements[ 15 ] - srcElements[ 11 ] * srcElements[ 12 ];
			float b3 = srcElements[ 9 ] * srcElements[ 14 ] - srcElements[ 10 ] * srcElements[ 13 ];
			float b4 = srcElements[ 9 ] * srcElements[ 15 ] - srcElements[ 11 ] * srcElements[ 13 ];
			float b5 = srcElements[ 10 ] * srcElements[ 15 ] - srcElements[ 11 ] * srcElements[ 14 ];

			float det = a0 * b5 - a1 * b4 + a2 * b3 + a3 * b2 - a4 * b1 + a5 * b0;
			if ( ( det > -0.000001f ) && ( det < 0.000001f ) )
			{
				//	Maybe store zero matrix instead?
				throw new ApplicationException( "Tried to take the inverse of a matrix with determinant of zero" );
			}

			//	Store the reciprocal of the determinant
			float rcpDet = 1.0f / det;

			//	Store the adjoint of mat in this matrix
			elements[ 0 ] = ( +srcElements[ 5 ] * b5 - srcElements[ 6 ] * b4 + srcElements[ 7 ] * b3 ) * rcpDet;
			elements[ 4 ] = ( -srcElements[ 4 ] * b5 + srcElements[ 6 ] * b2 - srcElements[ 7 ] * b1 ) * rcpDet;
			elements[ 8 ] = ( +srcElements[ 4 ] * b4 - srcElements[ 5 ] * b2 + srcElements[ 7 ] * b0 ) * rcpDet;
			elements[ 12 ] = ( -srcElements[ 4 ] * b3 + srcElements[ 5 ] * b1 - srcElements[ 6 ] * b0 ) * rcpDet;
			elements[ 1 ] = ( -srcElements[ 1 ] * b5 + srcElements[ 2 ] * b4 - srcElements[ 3 ] * b3 ) * rcpDet;
			elements[ 5 ] = ( +srcElements[ 0 ] * b5 - srcElements[ 2 ] * b2 + srcElements[ 3 ] * b1 ) * rcpDet;
			elements[ 9 ] = ( -srcElements[ 0 ] * b4 + srcElements[ 1 ] * b2 - srcElements[ 3 ] * b0 ) * rcpDet;
			elements[ 13 ] = ( +srcElements[ 0 ] * b3 - srcElements[ 1 ] * b1 + srcElements[ 2 ] * b0 ) * rcpDet;
			elements[ 2 ] = ( +srcElements[ 13 ] * a5 - srcElements[ 14 ] * a4 + srcElements[ 15 ] * a3 ) * rcpDet;
			elements[ 6 ] = ( -srcElements[ 12 ] * a5 + srcElements[ 14 ] * a2 - srcElements[ 15 ] * a1 ) * rcpDet;
			elements[ 10 ] = ( +srcElements[ 12 ] * a4 - srcElements[ 13 ] * a2 + srcElements[ 15 ] * a0 ) * rcpDet;
			elements[ 14 ] = ( -srcElements[ 12 ] * a3 + srcElements[ 13 ] * a1 - srcElements[ 14 ] * a0 ) * rcpDet;
			elements[ 3 ] = ( -srcElements[ 9 ] * a5 + srcElements[ 10 ] * a4 - srcElements[ 11 ] * a3 ) * rcpDet;
			elements[ 7 ] = ( +srcElements[ 8 ] * a5 - srcElements[ 10 ] * a2 + srcElements[ 11 ] * a1 ) * rcpDet;
			elements[ 11 ] = ( -srcElements[ 8 ] * a4 + srcElements[ 9 ] * a2 - srcElements[ 11 ] * a0 ) * rcpDet;
			elements[ 15 ] = ( +srcElements[ 8 ] * a3 - srcElements[ 9 ] * a1 + srcElements[ 10 ] * a0 ) * rcpDet;
		}

		/// <summary>
		/// Stores the transpose of src in result
		/// </summary>
		protected static void MakeTransposeMatrix( InvariantMatrix44 result, InvariantMatrix44 src )
		{
			for ( int row = 0; row < 4; ++row )
			{
				for ( int col = 0; col < 4; ++col )
				{
					result.m_Elements[ row + col * 4 ] = src[ col, row ];
				}
			}
		}

		/// <summary>
		/// Stores the multiple of lhs by rhs in result
		/// </summary>
		protected static void Multiply( InvariantMatrix44 result, InvariantMatrix44 lhs, InvariantMatrix44 rhs )
		{
			float[] elements = result.Elements;
			for ( int row = 0; row < 4; ++row )
			{
				int col = 0;
				elements[ col + row * 4 ] = ( lhs[ col, 0 ] * rhs[ 0, row ] ) + ( lhs[ col, 1 ] * rhs[ 1, row ] ) + ( lhs[ col, 2 ] * rhs[ 2, row ] ) + ( lhs[ col, 3 ] * rhs[ 3, row ] ); ++col;
				elements[ col + row * 4 ] = ( lhs[ col, 0 ] * rhs[ 0, row ] ) + ( lhs[ col, 1 ] * rhs[ 1, row ] ) + ( lhs[ col, 2 ] * rhs[ 2, row ] ) + ( lhs[ col, 3 ] * rhs[ 3, row ] ); ++col;
				elements[ col + row * 4 ] = ( lhs[ col, 0 ] * rhs[ 0, row ] ) + ( lhs[ col, 1 ] * rhs[ 1, row ] ) + ( lhs[ col, 2 ] * rhs[ 2, row ] ) + ( lhs[ col, 3 ] * rhs[ 3, row ] ); ++col;
				elements[ col + row * 4 ] = ( lhs[ col, 0 ] * rhs[ 0, row ] ) + ( lhs[ col, 1 ] * rhs[ 1, row ] ) + ( lhs[ col, 2 ] * rhs[ 2, row ] ) + ( lhs[ col, 3 ] * rhs[ 3, row ] ); ++col;
			}
		}

		/// <summary>
		/// Gets the element array
		/// </summary>
		protected float[] Elements
		{
			get { return m_Elements; }
		}

		#endregion

		#region Private Members

		private readonly float[] m_Elements = new float[ 16 ];

		#endregion
	}
}
