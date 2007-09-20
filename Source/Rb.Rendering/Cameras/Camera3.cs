using Rb.Core.Maths;

namespace Rb.Rendering.Cameras
{
	/// <summary>
	/// 3d camera base class
	/// </summary>
	public abstract class Camera3 : CameraBase, ICamera3
	{

		#region	Unprojection

		/// <summary>
		/// Unprojects a screen space coordinate into world space
		/// </summary>
		/// <param name="x">Screen X position</param>
		/// <param name="y">Screen Y position</param>
		/// <param name="depth">View depth</param>
		/// <returns>Returns the unprojected world position</returns>
		public Point3 Unproject( int x, int y, float depth )
        {
            //	TODO: AP: This is a bodge - don't abuse the rendering pipeline like this!
			Begin( );
			Point3 result = Renderer.Instance.Unproject( x, y, depth );
			End( );
			return result;
		}

		/// <summary>
		/// Creates a pick ray from a screen position
		/// </summary>
		/// <param name="x">Screen X position</param>
		/// <param name="y">Screen Y position</param>
		/// <returns>Returns world ray</returns>
		public Ray3 PickRay( int x, int y )
		{
			//	TODO: AP: This is a bodge - don't abuse the rendering pipeline like this!
			Begin( );
			Ray3 result = new Ray3( Position, ( Renderer.Instance.Unproject( x, y, 1 ) - Position ).MakeNormal( ) );
			End( );
			return result;
		}

		#endregion

		#region	Camera frame

		/// <summary>
		///	Gets the camera frame's x axis
		/// </summary>
		public Vector3	XAxis
		{
			get { return m_XAxis; }
		}

		/// <summary>
		///	Gets the camera frame's y axis
		/// </summary>
		public Vector3	YAxis
		{
			get { return m_YAxis; }
		}

		/// <summary>
		///	Gets the camera frame's z axis
		/// </summary>
		public Vector3	ZAxis
		{
			get { return m_ZAxis; }
		}

		/// <summary>
		///	Gets the camera's position
		/// </summary>
		public Point3	Position
		{
			get
			{
				return m_Pos;
			}
		}

		#endregion

		#region	Protected stuff
		
		/// <summary>
		/// Camera X axis
		/// </summary>
		protected Vector3	m_XAxis		= new Vector3( 1, 0, 0 );
		
		/// <summary>
		/// Camera Y axis
		/// </summary>
		protected Vector3	m_YAxis		= new Vector3( 0, 1, 0 );
		
		/// <summary>
		/// Camera Z axis
		/// </summary>
		protected Vector3	m_ZAxis		= new Vector3( 0, 0, 1 );
		
		/// <summary>
		/// Camera position
		/// </summary>
		protected Point3	m_Pos		= new Point3( );

		#endregion
	}
}
