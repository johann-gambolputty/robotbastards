using System;

namespace Rb.Core.Maths
{
	/// <summary>
	/// 4x4 matrix, used for transformation in R3
	/// </summary>
	[Serializable]
	public class Matrix44 : InvariantMatrix44
	{
		#region Builder methods

		/// <summary>
		/// Returns a new identity matrix
		/// </summary>
		public static new Matrix44 MakeIdentityMatrix( )
		{
			return new Matrix44( );
		}

		/// <summary>
		/// Makes the specified matrix an identity matrix
		/// </summary>
		public static void MakeIdentityMatrix( Matrix44 matrix )
		{
			matrix.Set( 1, 0, 0, 0,
						0, 1, 0, 0,
						0, 0, 1, 0,
						0, 0, 0, 1 );
		}

		/// <summary>
		/// Makes a matrix from a quaternion
		/// </summary>
		public static new Matrix44 MakeQuaternionMatrix( Quaternion quaternion )
		{
			Matrix44 result = new Matrix44( );
			MakeQuaternionMatrix( ( InvariantMatrix44 )result, quaternion );
			return result;
		}

		/// <summary>
		/// Makes a matrix from a quaternion
		/// </summary>
		public static void MakeQuaternionMatrix( Matrix44 matrix, Quaternion quaternion )
		{
			MakeQuaternionMatrix( ( InvariantMatrix44 )matrix, quaternion );
		}

		#endregion

		#region	Public properties

		/// <summary>
		/// 1-dimensional indexer for this matrix
		/// </summary>
		public new float this[ int index ]
		{
			get { return base[ index ]; }
			set { Elements[ index ] = value; }
		}

		/// <summary>
		/// 2-dimensional indexer for this matrix
		/// </summary>
		public new float this[ int Col, int Row ]
		{
			get { return base[ Col, Row ]; }
			set { Elements[ Col + ( Row * 4 ) ] = value; }
		}

		/// <summary>
		/// Extracts the x axis from this matrix
		/// </summary>
		public new Vector3 XAxis
		{
			get { return base.XAxis; }
			set
			{
				Elements[ 0 ] = value[ 0 ];
				Elements[ 1 ] = value[ 1 ];
				Elements[ 2 ] = value[ 2 ];
			}
		}

		/// <summary>
		/// Extracts the y axis from this matrix
		/// </summary>
		public new Vector3 YAxis
		{
			get { return base.YAxis; }
			set
			{
				Elements[ 4 ] = value[ 0 ];
				Elements[ 5 ] = value[ 1 ];
				Elements[ 6 ] = value[ 2 ];
			}
		}

		/// <summary>
		/// Extracts the z axis from this matrix
		/// </summary>
		public new Vector3 ZAxis
		{
			get { return base.ZAxis; }
			set
			{
				Elements[ 8 ]  = value[ 0 ];
				Elements[ 9 ]  = value[ 1 ];
				Elements[ 10 ] = value[ 2 ];
			}
		}

		/// <summary>
		/// Extracts the translation from this matrix
		/// </summary>
		public new Point3 Translation
		{
			get { return base.Translation; }
			set
			{
				Elements[ 12 ]  = value[ 0 ];
				Elements[ 13 ]  = value[ 1 ];
				Elements[ 14 ]	= value[ 2 ];
			}
		}

		/// <summary>
		/// Returns matrix elements
		/// </summary>
		public new float[] Elements
		{
			get { return base.Elements; }
		}

		#endregion

		#region	Construction

		/// <summary>
		/// Makes this an identity matrix
		/// </summary>
		public Matrix44( )
		{
		}

		/// <summary>
		/// Copies the specified source matrix
		/// </summary>
		public Matrix44( InvariantMatrix44 src ) :
			base( src )
		{
		}

		/// <summary>
		/// Sets up the matrix
		/// </summary>
		public Matrix44( Vector3 xAxis, Vector3 yAxis, Vector3 zAxis, Point3 pos ) :
			base( xAxis, yAxis, zAxis, pos)
		{
		}

		/// <summary>
		/// Sets up all the matrices' elements
		/// </summary>
		public Matrix44( float M00, float M10, float M20, float M30,
						 float M01, float M11, float M21, float M31,
						 float M02, float M12, float M22, float M32,
						 float M03, float M13, float M23, float M33 ) :
			base( M00, M10, M20, M30, M01, M11, M21, M31, M02, M12, M22, M32, M03, M13, M23, M33 )
		{
		}

		#endregion

		#region	Setup

		/// <summary>
		/// Sets the position part of this matrix
		/// </summary>
		public void SetTranslation( float x, float y, float z )
		{
			Elements[ 12 ] = x; Elements[ 13 ] = y; Elements[ 14 ] = z;
		}

		/// <summary>
		/// Sets the position part of this matrix
		/// </summary>
		public void SetTranslation( Point3 Pos )
		{
			SetTranslation( Pos.X, Pos.Y, Pos.Z );
		}

		/// <summary>
		/// Sets the position part of this matrix
		/// </summary>
		public void SetTranslation( Vector3 Pos )
		{
			SetTranslation( Pos.X, Pos.Y, Pos.Z );
		}

		/// <summary>
		/// Sets up all the matrices' elements
		/// </summary>
		public void Set( Point3 pos, Vector3 xAxis, Vector3 yAxis, Vector3 zAxis )
		{
			Elements[ 0 ] = xAxis.X; Elements[ 1 ] = xAxis.Y; Elements[ 2 ] = xAxis.Z; Elements[ 3 ] = 0;
			Elements[ 4 ] = yAxis.X; Elements[ 5 ] = yAxis.Y; Elements[ 6 ] = yAxis.Z; Elements[ 7 ] = 0;
			Elements[ 8 ] = zAxis.X; Elements[ 9 ] = zAxis.Y; Elements[ 10 ] = zAxis.Z; Elements[ 11 ] = 0;
			Elements[ 12 ] = pos.X; Elements[ 13 ] = pos.Y; Elements[ 14 ] = pos.Z; Elements[ 15 ] = 1;
		}

		/// <summary>
		/// Sets up all the matrices' elements
		/// </summary>
		public void Set( float M00, float M10, float M20, float M30,
						 float M01, float M11, float M21, float M31,
						 float M02, float M12, float M22, float M32,
						 float M03, float M13, float M23, float M33 )
		{
			Elements[ 0 ] = M00; Elements[ 1 ] = M10; Elements[ 2 ] = M20; Elements[ 3 ] = M30;
			Elements[ 4 ] = M01; Elements[ 5 ] = M11; Elements[ 6 ] = M21; Elements[ 7 ] = M31;
			Elements[ 8 ] = M02; Elements[ 9 ] = M12; Elements[ 10 ] = M22; Elements[ 11 ] = M32;
			Elements[ 12 ] = M03; Elements[ 13 ] = M13; Elements[ 14 ] = M23; Elements[ 15 ] = M33;
		}

		/// <summary>
		/// Sets an element in this matrix
		/// </summary>
		public void Set( int X, int Y, float Val )
		{
			Elements[ X + ( Y * 4 ) ] = Val;
		}

		/// <summary>
		/// Gets an element in this matrix
		/// </summary>
		public float Get( int X, int Y )
		{
			return Elements[ X + ( Y * 4 ) ];
		}

		/// <summary>
		/// Sets up this matrix as a look-at matrix
		/// </summary>
		public void SetLookAt( Point3 origin, Point3 lookAt, Vector3 up )
		{
			Vector3 zAxis = ( origin - lookAt ).MakeNormal( );
			Vector3 yAxis = up;
			Vector3 xAxis = Vector3.Cross( yAxis, zAxis );
			yAxis = Vector3.Cross( zAxis, xAxis );

			Elements[ 0 ]  = xAxis.X; 	Elements[ 1 ]  = yAxis.X; 	Elements[ 2 ]  = zAxis.X; 	Elements[ 3 ]  = 0;
			Elements[ 4 ]  = xAxis.Y; 	Elements[ 5 ]  = yAxis.Y; 	Elements[ 6 ]  = zAxis.Y; 	Elements[ 7 ]  = 0;
			Elements[ 8 ]  = xAxis.Z; 	Elements[ 9 ]  = yAxis.Z; 	Elements[ 10 ] = zAxis.Z; 	Elements[ 11 ] = 0;
			Elements[ 12 ] = 0;			Elements[ 13 ] = 0;			Elements[ 14 ] = 0;			Elements[ 15 ] = 1;

			Translate( -origin.X, -origin.Y, -origin.Z );
		}

		/// <summary>
		/// Sets up this matrix as a scale matrix
		/// </summary>
		public void SetScale( float x, float y, float z )
		{
			Elements[ 0 ]  = x; 	Elements[ 1 ]  = 0; 	Elements[ 2 ]  = 0; 	Elements[ 3 ]  = 0;
			Elements[ 4 ]  = 0; 	Elements[ 5 ]  = y; 	Elements[ 6 ]  = 0; 	Elements[ 7 ]  = 0;
			Elements[ 8 ]  = 0; 	Elements[ 9 ]  = 0; 	Elements[ 10 ] = z; 	Elements[ 11 ] = 0;
			Elements[ 12 ] = 0;		Elements[ 13 ] = 0;		Elements[ 14 ] = 0;		Elements[ 15 ] = 1;
		}

		/// <summary>
		/// Scales this matrix
		/// </summary>
		public void Scale( float x, float y, float z )
		{
			//	TODO: CHEAT! AGAIN!
			Matrix44 mat = new Matrix44( this );
			Matrix44 scaleMat = new Matrix44( x, 0, 0, 0,
											  0, y, 0, 0,
											  0, 0, z, 0,
											  0, 0, 0, 1 );

			StoreMultiply( mat, scaleMat );
		}

		/// <summary>
		/// Multiplication operator
		/// </summary>
		public static Matrix44	operator * ( Matrix44 lhs, Matrix44 rhs )
		{
			Matrix44 mat = new Matrix44( );
			mat.StoreMultiply( lhs, rhs );
			return mat;
		}

		/// <summary>
		/// Returns true if the two matrices are equal, within a given tolerance per element
		/// </summary>
		public bool IsCloseTo( Matrix44 mat, float tol )
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

		#endregion

		#region	Operations

		//	TODO: AP: Add rotation code

		/// <summary>
		/// Translation. Stores the result of this * T, where T is the translation matrix for (x,y,z)
		/// </summary>
		public void	Translate( float x, float y, float z )
		{
			//	TODO: This is pretty lazy :)
			Matrix44 lhs = new Matrix44( this );
			Matrix44 rhs = new Matrix44( 1, 0, 0, 0, 
										 0, 1, 0, 0,
										 0, 0, 1, 0,
										 x, y, z, 1 );
			StoreMultiply( lhs, rhs );
		}

		/// <summary>
		/// Stores the result of multiplying lhs * rhs in this matrix
		/// </summary>
		public void StoreMultiply( InvariantMatrix44 lhs, InvariantMatrix44 rhs )
		{
			Multiply( this, lhs, rhs );
		}

		/// <summary>
		/// Stores the inverse of mat (mat.mat'=I) in this matrix
		/// </summary>
		/// <remarks>
		/// The generic inverse of a matrix A is found by dividing the adjoint of A by the determinant of A. The adjoint B of a matrix A is
		/// defined by B=bij, where bij is the determinant of A with row i and column j removed (the co-factors of A).
		/// I was too lazy to fully expand the calculations this implies for a 4x4 matrix, so I grabbed and adapted the following code from
		/// http://www.geometrictools.com
		/// </remarks>
		public void StoreInverse( Matrix44 mat )
		{
			MakeInverse( this, mat );
		}

		/// <summary>
		/// Stores the inverse transpose of mat in this matrix. This is used to create a matrix that can be used to transform vertex normals
		/// </summary>
		public void StoreInverseTranspose( Matrix44 mat )
		{
		}

		/// <summary>
		/// Stores the result of multiplying this * rhs in this matrix
		/// </summary>
		public void StoreMultiply( Matrix44 rhs )
		{
			//	TODO: CHEATER!
			Matrix44 tmp = new Matrix44( this );
			StoreMultiply( tmp, rhs );
		}

		/// <summary>
		/// Stores the transpose of mat in this matrix
		/// </summary>
		/// <param name="mat"></param>
		public void StoreTranspose( Matrix44 mat )
		{
			for ( int row = 0; row < 4; ++row )
			{
				for ( int col = 0; col < 4; ++col )
				{
					this[ row, col ] = mat[ col, row ];
				}
			}
		}

		/// <summary>
		/// Creates a transposed copy of this matrix
		/// </summary>
		public new Matrix44 Transpose( )
		{
			Matrix44 result = new Matrix44( );
			MakeTransposeMatrix( result, this );
			return result;
		}

		/// <summary>
		/// Creates an inverted copy of this matrix
		/// </summary>
		public new Matrix44 Invert( )
		{
			Matrix44 result = new Matrix44( );
			MakeInverse( result, this );
			return result;
		}

		/// <summary>
		/// Copies a matrix
		/// </summary>
		public void	Copy( InvariantMatrix44 src )
		{
			for ( int index = 0; index < 16; ++index )
			{
				Elements[ index ] = src[ index ];
			}
		}

		/// <summary>
		/// Creates a copy of this matrix
		/// </summary>
		public Matrix44	Clone( )
		{
			return new Matrix44( this );
		}

		#endregion

		#region Operators

		/// <summary>
		/// Transforms a point by a matrix
		/// </summary>
		/// <param name="mat">Transformation matrix</param>
		/// <param name="pt">Source point</param>
		/// <returns>Transformed point</returns>
		public static Point3 operator * ( Matrix44 mat, Point3 pt )
		{
			return mat.Multiply( pt );
		}

		/// <summary>
		/// Transforms a vector by a matrix
		/// </summary>
		/// <param name="mat">Transformation matrix</param>
		/// <param name="vec">Source vector</param>
		/// <returns>Transformed vector</returns>
		public static Vector3 operator * ( Matrix44 mat, Vector3 vec )
		{
			return mat.Multiply( vec );
		}

		#endregion
	}
}
