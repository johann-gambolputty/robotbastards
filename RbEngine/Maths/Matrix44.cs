using System;

namespace RbEngine.Maths
{
	/// <summary>
	/// 4x4 matrix, used for transformation in R3
	/// </summary>
	public class Matrix44
	{
		#region	Public properties

		/// <summary>
		/// The elements of this matrix
		/// </summary>
		public float[ ] Elements = new float[ 16 ];

		/// <summary>
		/// An identity matrix
		/// </summary>
		public static Matrix44 Identity
		{
			get
			{
				return new Matrix44( );
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
			/*
			for ( int row = 0; row < 4; ++row )
			{
				int col = 0;
				this[ col, row ] = ( lhs[ 0, row ] * rhs[ col, 0 ] ) + ( lhs[ 1, row ] * rhs[ col, 1 ] )  + ( lhs[ 2, row ] * rhs[ col, 2 ] ) + ( lhs[ 3, row ] * rhs[ col, 3 ] ); ++col;
				this[ col, row ] = ( lhs[ 0, row ] * rhs[ col, 0 ] ) + ( lhs[ 1, row ] * rhs[ col, 1 ] )  + ( lhs[ 2, row ] * rhs[ col, 2 ] ) + ( lhs[ 3, row ] * rhs[ col, 3 ] ); ++col;
				this[ col, row ] = ( lhs[ 0, row ] * rhs[ col, 0 ] ) + ( lhs[ 1, row ] * rhs[ col, 1 ] )  + ( lhs[ 2, row ] * rhs[ col, 2 ] ) + ( lhs[ 3, row ] * rhs[ col, 3 ] ); ++col;
				this[ col, row ] = ( lhs[ 0, row ] * rhs[ col, 0 ] ) + ( lhs[ 1, row ] * rhs[ col, 1 ] )  + ( lhs[ 2, row ] * rhs[ col, 2 ] ) + ( lhs[ 3, row ] * rhs[ col, 3 ] ); ++col;
			}
			*/
			for ( int row = 0; row < 4; ++row )
			{
				for ( int col = 0; col < 4; ++col )
				{
					this[ col, row ] = 0;
					for ( int mul = 0; mul < 4; ++mul )
					{
						this[ col, row ] += lhs[ col, mul ] * rhs[ mul, row ];
					}
				}
			}
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
			Matrix44 Result = new Matrix44( );

			for ( int Row = 0; Row < 4; ++Row )
			{
				for ( int Col = 0; Col < 4; ++Col )
				{
					Result.Set( Row, Col, Get( Col, Row ) );
				}
			}

			return Result;
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
