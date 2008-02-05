
namespace Rb.Core.Maths
{
	/// <summary>
	/// A collection of 6 planes making up a truncated pyramid. Used to represent the
	/// viewing area of a perspective camera 
	/// </summary>
	public class Frustum
	{
		#region Construction

		/// <summary>
		/// Sets up a frustum from the properties of a perspective camera
		/// </summary>
		/// <param name="fov">Field of view</param>
		/// <param name="aspect">Aspect ratio</param>
		/// <param name="near">Near plane distance</param>
		/// <param name="far">Far plane distance</param>
		public Frustum( float fov, float aspect, float near, float far )
		{
			//	Set near and far planes
			Front = new Plane3( 0, 0, 1, -near );	//	TODO: AP: Wrong way around?
			Back = new Plane3( 0, 0, 1, far );

			//	Set top and bottom planes
			float hFov = fov / 2;
			float sinHFov = Functions.Sin( hFov );
			float cosHFov = Functions.Cos( hFov );

			Top = new Plane3( 0, -cosHFov, -sinHFov, 0 );
			Bottom = new Plane3( 0, cosHFov, -sinHFov, 0 );

			//	Set left and right planes
			float xHFov = Functions.Atan( aspect * Functions.Tan( hFov ) );
			float sinXHFov = Functions.Sin( xHFov );
			float cosXHFov = Functions.Cos( xHFov );

			Left = new Plane3( cosXHFov, 0, -sinXHFov, 0 );
			Right = new Plane3( -cosXHFov, 0, -sinXHFov, 0 );
		}

		/// <summary>
		/// Sets up a frustum from the projection matrix of a perspective camera
		/// </summary>
		/// <param name="matrix">Camera projection matrix</param>
		public Frustum( Matrix44 matrix )
		{
			Left	= new Plane3( matrix[ 4, 1 ] + matrix[ 1, 1 ], matrix[ 4, 2 ] + matrix[ 1, 2 ], matrix[ 4, 3 ] + matrix[ 1, 3 ], matrix[ 4, 4 ] + matrix[ 1, 4 ] );
			Right	= new Plane3( matrix[ 4, 1 ] - matrix[ 1, 1 ], matrix[ 4, 2 ] - matrix[ 1, 2 ], matrix[ 4, 3 ] - matrix[ 1, 3 ], matrix[ 4, 4 ] - matrix[ 1, 4 ] );
			Bottom	= new Plane3( matrix[ 4, 1 ] + matrix[ 2, 1 ], matrix[ 4, 2 ] + matrix[ 2, 2 ], matrix[ 4, 3 ] + matrix[ 2, 3 ], matrix[ 4, 4 ] + matrix[ 2, 4 ] );
			Top		= new Plane3( matrix[ 4, 1 ] - matrix[ 2, 1 ], matrix[ 4, 2 ] - matrix[ 2, 2 ], matrix[ 4, 3 ] - matrix[ 2, 3 ], matrix[ 4, 4 ] - matrix[ 2, 4 ] );
			Front	= new Plane3( matrix[ 4, 1 ] + matrix[ 3, 1 ], matrix[ 4, 2 ] + matrix[ 3, 2 ], matrix[ 4, 3 ] + matrix[ 3, 3 ], matrix[ 4, 4 ] + matrix[ 3, 4 ] );
			Back	= new Plane3( matrix[ 4, 1 ] - matrix[ 3, 1 ], matrix[ 4, 2 ] - matrix[ 3, 2 ], matrix[ 4, 3 ] - matrix[ 3, 3 ], matrix[ 4, 4 ] - matrix[ 3, 4 ] );
		}

		#endregion

		#region Planes

		/// <summary>
		/// Gets a plane
		/// </summary>
		/// <param name="index">Plane index [0,6)</param>
		/// <returns>Returns indexed plane</returns>
		public Plane3 GetPlane( int index )
		{
			return m_Planes[ index ];
		}

		/// <summary>
		/// The front plane (z min)
		/// </summary>
		public Plane3 Front
		{
			get { return m_Planes[ 0 ]; }
			set { m_Planes[ 0 ] = value; }
		}

		/// <summary>
		/// The back plane (z max)
		/// </summary>
		public Plane3 Back
		{
			get { return m_Planes[ 1 ]; }
			set { m_Planes[ 1 ] = value; }
		}

		/// <summary>
		/// The left plane (x min)
		/// </summary>
		public Plane3 Left
		{
			get { return m_Planes[ 2 ]; }
			set { m_Planes[ 2 ] = value; }
		}

		/// <summary>
		/// The right plane (x max)
		/// </summary>
		public Plane3 Right
		{
			get { return m_Planes[ 3 ]; }
			set { m_Planes[ 3 ] = value; }
		}

		/// <summary>
		/// The top plane (y min)
		/// </summary>
		public Plane3 Top
		{
			get { return m_Planes[ 4 ]; }
			set { m_Planes[ 4 ] = value; }
		}

		/// <summary>
		/// The bottom plane (y max)
		/// </summary>
		public Plane3 Bottom
		{
			get { return m_Planes[ 5 ]; }
			set { m_Planes[ 5 ] = value; }
		}

		#endregion

		#region Private members

		private readonly Plane3[] m_Planes = new Plane3[ 6 ];

		#endregion
	}
}
