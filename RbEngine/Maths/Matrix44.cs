using System;

namespace RbEngine.Maths
{
	/// <summary>
	/// 4x4 matrix, used for transformation in R3
	/// </summary>
	public class Matrix44
	{
		#region	Predefined matrices

		/// <summary>
		/// Identity matrix
		/// </summary>
		public static readonly Matrix44 Identity = new Matrix44( );

		#endregion

		#region	Public properties

		/// <summary>
		/// The elements of this matrix
		/// </summary>
		public float[ ] Elements = new float[ 16 ];


		/// <summary>
		/// 1-dimensional indexer for this matrix
		/// </summary>
		public float this[ int index ]
		{
			get
			{
				return Elements[ index ];
			}
			set
			{
				Elements[ index ] = value;
			}
		}

		/// <summary>
		/// 2-dimensional indexer for this matrix
		/// </summary>
		public float this[ int Col, int Row ]
		{
			get
			{
				return Elements[ Col + ( Row * 4 ) ];
			}
			set
			{
				Elements[ Col + ( Row * 4 ) ] = value;
			}
		}

		/// <summary>
		/// Extracts the x axis from this matrix
		/// </summary>
		public Vector3 XAxis
		{
			get
			{
				return new Vector3( Elements[ 0 ], Elements[ 1 ], Elements[ 2 ] );
			}
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
		public Vector3 YAxis
		{
			get
			{
				return new Vector3( Elements[ 4 ], Elements[ 5 ], Elements[ 6 ] );
			}
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
		public Vector3 ZAxis
		{
			get
			{
				return new Vector3( Elements[ 8 ], Elements[ 9 ], Elements[ 10 ] );
			}
			set
			{
				Elements[ 8 ]  = value[ 0 ];
				Elements[ 9 ]  = value[ 1 ];
				Elements[ 10 ] = value[ 2 ];
			}
		}

		/// <summary>
		/// Extracts the z axis from this matrix
		/// </summary>
		public Point3 Translation
		{
			get
			{
				return new Point3( Elements[ 12 ], Elements[ 13 ], Elements[ 14 ] );
			}
			set
			{
				Elements[ 12 ]  = value[ 0 ];
				Elements[ 13 ]  = value[ 1 ];
				Elements[ 14 ]	= value[ 2 ];

			}
		}

		#endregion

		#region	Construction

		/// <summary>
		/// Makes this an identity matrix
		/// </summary>
		public Matrix44( )
		{
			Elements[ 0 ]  = 1; Elements[ 1 ]  = 0; Elements[ 2 ]  = 0; Elements[ 3 ]  = 0;
			Elements[ 4 ]  = 0; Elements[ 5 ]  = 1; Elements[ 6 ]  = 0; Elements[ 7 ]  = 0;
			Elements[ 8 ]  = 0; Elements[ 9 ]  = 0; Elements[ 10 ] = 1; Elements[ 11 ] = 0;
			Elements[ 12 ] = 0; Elements[ 13 ] = 0; Elements[ 14 ] = 0; Elements[ 15 ] = 1;
		}

		/// <summary>
		/// Copies the specified source matrix
		/// </summary>
		public Matrix44( Matrix44 Src )
		{
			for ( int Index = 0; Index < 16; ++Index )
			{
				Elements[ Index ] = Src.Elements[ Index ];
			}
		}

		/// <summary>
		/// Sets up the matrix
		/// </summary>
		public Matrix44( Point3 pos, Vector3 xAxis, Vector3 yAxis, Vector3 zAxis )
		{
			Elements[ 0 ]  = xAxis.X; 	Elements[ 1 ]  = xAxis.Y; 	Elements[ 2 ]  = xAxis.Z; 	Elements[ 3 ]  = 0;
			Elements[ 4 ]  = yAxis.X; 	Elements[ 5 ]  = yAxis.Y; 	Elements[ 6 ]  = yAxis.Z; 	Elements[ 7 ]  = 0;
			Elements[ 8 ]  = zAxis.X; 	Elements[ 9 ]  = zAxis.Y; 	Elements[ 10 ] = zAxis.Z; 	Elements[ 11 ] = 0;
			Elements[ 12 ] = pos.X;		Elements[ 13 ] = pos.Y;		Elements[ 14 ] = pos.Z;		Elements[ 15 ] = 1;
		}
		/// <summary>
		/// Sets up all the matrices' elements
		/// </summary>
		public Matrix44( float M00, float M10, float M20, float M30,
						 float M01, float M11, float M21, float M31,
						 float M02, float M12, float M22, float M32,
						 float M03, float M13, float M23, float M33 )
		{
			Elements[ 0 ]  = M00; Elements[ 1 ]  = M10; Elements[ 2 ]  = M20; Elements[ 3 ]  = M30;
			Elements[ 4 ]  = M01; Elements[ 5 ]  = M11; Elements[ 6 ]  = M21; Elements[ 7 ]  = M31;
			Elements[ 8 ]  = M02; Elements[ 9 ]  = M12; Elements[ 10 ] = M22; Elements[ 11 ] = M32;
			Elements[ 12 ] = M03; Elements[ 13 ] = M13; Elements[ 14 ] = M23; Elements[ 15 ] = M33;
		}

		#endregion

		#region	Setup

		/// <summary>
		/// Sets the position part of this matrix
		/// </summary>
		public void SetPosition( float X, float Y, float Z )
		{
			Elements[ 12 ] = X; Elements[ 13 ] = Y; Elements[ 14 ] = Z;
		}

		/// <summary>
		/// Sets the position part of this matrix
		/// </summary>
		public void SetPosition( Vector3 Pos )
		{
			SetPosition( Pos.X, Pos.Y, Pos.Z );
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
			/*
			Vector3 zAxis = ( lookAt - origin ).MakeNormal( );
			Vector3 yAxis = up;
			Vector3 xAxis = Vector3.Cross( yAxis, zAxis );
			yAxis = Vector3.Cross( zAxis, xAxis );

			Elements[ 0 ]  = xAxis.X; 	Elements[ 1 ]  = xAxis.Y; 	Elements[ 2 ]  = xAxis.Z; 	Elements[ 3 ]  = 0;
			Elements[ 4 ]  = yAxis.X; 	Elements[ 5 ]  = yAxis.Y; 	Elements[ 6 ]  = yAxis.Z; 	Elements[ 7 ]  = 0;
			Elements[ 8 ]  = zAxis.X; 	Elements[ 9 ]  = zAxis.Y; 	Elements[ 10 ] = zAxis.Z; 	Elements[ 11 ] = 0;
			Elements[ 12 ] = 0;			Elements[ 13 ] = 0;			Elements[ 14 ] = 0;			Elements[ 15 ] = 1;

			Translate( -origin.X, -origin.Y, -origin.Z );
			*/
		}

		#endregion

		#region	Operations

		/// <summary>
		/// Translation. Stores the result of this * T, where T is the translation matrix for (x,y,z)
		/// </summary>
		public void		Translate( float x, float y, float z )
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
		public void StoreMultiply( Matrix44 lhs, Matrix44 rhs )
		{
			for ( int row = 0; row < 4; ++row )
			{
				int col = 0;
				this[ col, row ] = ( lhs[ col, 0 ] * rhs[ 0, row ] ) + ( lhs[ col, 1 ] * rhs[ 1, row ] )  + ( lhs[ col, 2 ] * rhs[ 2, row ] ) + ( lhs[ col, 3 ] * rhs[ 3, row ] ); ++col;
				this[ col, row ] = ( lhs[ col, 0 ] * rhs[ 0, row ] ) + ( lhs[ col, 1 ] * rhs[ 1, row ] )  + ( lhs[ col, 2 ] * rhs[ 2, row ] ) + ( lhs[ col, 3 ] * rhs[ 3, row ] ); ++col;
				this[ col, row ] = ( lhs[ col, 0 ] * rhs[ 0, row ] ) + ( lhs[ col, 1 ] * rhs[ 1, row ] )  + ( lhs[ col, 2 ] * rhs[ 2, row ] ) + ( lhs[ col, 3 ] * rhs[ 3, row ] ); ++col;
				this[ col, row ] = ( lhs[ col, 0 ] * rhs[ 0, row ] ) + ( lhs[ col, 1 ] * rhs[ 1, row ] )  + ( lhs[ col, 2 ] * rhs[ 2, row ] ) + ( lhs[ col, 3 ] * rhs[ 3, row ] ); ++col;
			}
		}

		/// <summary>
		/// Inverts this matrix (so M.M' = I)
		/// </summary>
		public void Invert( )
		{
			Matrix44 tmp = new Matrix44( this );
			StoreInverse( tmp );
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
			//	The inverse of an nxn matrix A is the adjoint of A divided through by the determinant of A
			//	Because the adjoint and determinant use the same sub-matrix determinants, we can store these values and use them in both calculations:

			float a0 = mat[  0 ] * mat[  5 ] - mat[  1 ] * mat[  4 ];
			float a1 = mat[  0 ] * mat[  6 ] - mat[  2 ] * mat[  4 ];
			float a2 = mat[  0 ] * mat[  7 ] - mat[  3 ] * mat[  4 ];
			float a3 = mat[  1 ] * mat[  6 ] - mat[  2 ] * mat[  5 ];
			float a4 = mat[  1 ] * mat[  7 ] - mat[  3 ] * mat[  5 ];
			float a5 = mat[  2 ] * mat[  7 ] - mat[  3 ] * mat[  6 ];
			float b0 = mat[  8 ] * mat[ 13 ] - mat[  9 ] * mat[ 12 ];
			float b1 = mat[  8 ] * mat[ 14 ] - mat[ 10 ] * mat[ 12 ];
			float b2 = mat[  8 ] * mat[ 15 ] - mat[ 11 ] * mat[ 12 ];
			float b3 = mat[  9 ] * mat[ 14 ] - mat[ 10 ] * mat[ 13 ];
			float b4 = mat[  9 ] * mat[ 15 ] - mat[ 11 ] * mat[ 13 ];
			float b5 = mat[ 10 ] * mat[ 15 ] - mat[ 11 ] * mat[ 14 ];

			float det = a0 * b5 - a1 * b4 + a2 * b3 + a3 * b2 - a4 * b1 + a5 * b0;
			if ( ( det > -0.0001f ) && ( det < 0.0001f ) )
			{
				//	Maybe store zero matrix instead?
				throw new ApplicationException( "Tried to take the inverse of a matrix with determinant of zero" );
			}

			//	Store the reciprocal of the determinant
			float rcpDet = 1.0f / det;

			//	Store the adjoint of mat in this matrix
			this[  0 ] = ( +mat[  5 ] * b5 - mat[  6 ] * b4 + mat[  7 ] * b3 ) * rcpDet;
			this[  4 ] = ( -mat[  4 ] * b5 + mat[  6 ] * b2 - mat[  7 ] * b1 ) * rcpDet;
			this[  8 ] = ( +mat[  4 ] * b4 - mat[  5 ] * b2 + mat[  7 ] * b0 ) * rcpDet;
			this[ 12 ] = ( -mat[  4 ] * b3 + mat[  5 ] * b1 - mat[  6 ] * b0 ) * rcpDet;
			this[  1 ] = ( -mat[  1 ] * b5 + mat[  2 ] * b4 - mat[  3 ] * b3 ) * rcpDet;
			this[  5 ] = ( +mat[  0 ] * b5 - mat[  2 ] * b2 + mat[  3 ] * b1 ) * rcpDet;
			this[  9 ] = ( -mat[  0 ] * b4 + mat[  1 ] * b2 - mat[  3 ] * b0 ) * rcpDet;
			this[ 13 ] = ( +mat[  0 ] * b3 - mat[  1 ] * b1 + mat[  2 ] * b0 ) * rcpDet;
			this[  2 ] = ( +mat[ 13 ] * a5 - mat[ 14 ] * a4 + mat[ 15 ] * a3 ) * rcpDet;
			this[  6 ] = ( -mat[ 12 ] * a5 + mat[ 14 ] * a2 - mat[ 15 ] * a1 ) * rcpDet;
			this[ 10 ] = ( +mat[ 12 ] * a4 - mat[ 13 ] * a2 + mat[ 15 ] * a0 ) * rcpDet;
			this[ 14 ] = ( -mat[ 12 ] * a3 + mat[ 13 ] * a1 - mat[ 14 ] * a0 ) * rcpDet;
			this[  3 ] = ( -mat[  9 ] * a5 + mat[ 10 ] * a4 - mat[ 11 ] * a3 ) * rcpDet;
			this[  7 ] = ( +mat[  8 ] * a5 - mat[ 10 ] * a2 + mat[ 11 ] * a1 ) * rcpDet;
			this[ 11 ] = ( -mat[  8 ] * a4 + mat[  9 ] * a2 - mat[ 11 ] * a0 ) * rcpDet;
			this[ 15 ] = ( +mat[  8 ] * a3 - mat[  9 ] * a1 + mat[ 10 ] * a0 ) * rcpDet;
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
				float a0 = this[  0 ] * this[  5 ] - this[  1 ] * this[  4 ];
				float a1 = this[  0 ] * this[  6 ] - this[  2 ] * this[  4 ];
				float a2 = this[  0 ] * this[  7 ] - this[  3 ] * this[  4 ];
				float a3 = this[  1 ] * this[  6 ] - this[  2 ] * this[  5 ];
				float a4 = this[  1 ] * this[  7 ] - this[  3 ] * this[  5 ];
				float a5 = this[  2 ] * this[  7 ] - this[  3 ] * this[  6 ];
				float b0 = this[  8 ] * this[ 13 ] - this[  9 ] * this[ 12 ];
				float b1 = this[  8 ] * this[ 14 ] - this[ 10 ] * this[ 12 ];
				float b2 = this[  8 ] * this[ 15 ] - this[ 11 ] * this[ 12 ];
				float b3 = this[  9 ] * this[ 14 ] - this[ 10 ] * this[ 13 ];
				float b4 = this[  9 ] * this[ 15 ] - this[ 11 ] * this[ 13 ];
				float b5 = this[ 10 ] * this[ 15 ] - this[ 11 ] * this[ 14 ];

				float det = a0 * b5 - a1 * b4 + a2 * b3 + a3 * b2 - a4 * b1 + a5 * b0;
				return det;
			}
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
		/// Multiplies a vector by this matrix, returning a new vector that stores the result
		/// </summary>
		public Vector3	Multiply( Vector3 In )
		{
			float x = ( In.X * Elements[ 0 ] ) + ( In.Y * Elements[ 4 ] ) + ( In.Z * Elements[ 8  ] ) + ( Elements[ 12 ] );
			float y = ( In.X * Elements[ 1 ] ) + ( In.Y * Elements[ 5 ] ) + ( In.Z * Elements[ 9  ] ) + ( Elements[ 13 ] );
			float z = ( In.X * Elements[ 2 ] ) + ( In.Y * Elements[ 6 ] ) + ( In.Z * Elements[ 10 ] ) + ( Elements[ 14 ] );

			return new Vector3( x, y, z );
		}

		/// <summary>
		/// Multiplies a point by this matrix, returning a new point that stores the result
		/// </summary>
		public Point3	Multiply( Point3 In )
		{
			float x = ( In.X * Elements[ 0 ] ) + ( In.Y * Elements[ 4 ] ) + ( In.Z * Elements[ 8  ] ) + ( Elements[ 12 ] );
			float y = ( In.X * Elements[ 1 ] ) + ( In.Y * Elements[ 5 ] ) + ( In.Z * Elements[ 9  ] ) + ( Elements[ 13 ] );
			float z = ( In.X * Elements[ 2 ] ) + ( In.Y * Elements[ 6 ] ) + ( In.Z * Elements[ 10 ] ) + ( Elements[ 14 ] );

			return new Point3( x, y, z );
		}

		/// <summary>
		/// Creates a new Matrix44 and stores the transpose of this matrix in it
		/// </summary>
		/// <remarks>
		/// If this matrix is orthogonal, then Tranpose() returns the inverse of this matrix (i.e. Tranpose(A).A == Identity)
		/// </remarks>
		public Matrix44 Transpose( )
		{
			Matrix44 result = new Matrix44( );

			for ( int row = 0; row < 4; ++row )
			{
				for ( int col = 0; col < 4; ++col )
				{
					result.Set( row, col, Get( col, row ) );
				}
			}

			return result;
		}

		/// <summary>
		/// Copies a matrix
		/// </summary>
		public void	Copy( Matrix44 src )
		{
			for ( int index = 0; index < 16; ++index )
			{
				Elements[ index ] = src.Elements[ index ];
			}
		}

		/// <summary>
		/// Creates a copy of this matrix
		/// </summary>
		public Matrix44	Copy( )
		{
			return new Matrix44( this );
		}

		#endregion

	}
}
