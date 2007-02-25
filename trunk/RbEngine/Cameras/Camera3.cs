using System;
using RbEngine.Maths;

namespace RbEngine.Cameras
{
	/// <summary>
	/// 3d camera base class
	/// </summary>
	// TODO: Probably a bit of a redundant class...
	public abstract class Camera3 : CameraBase
	{

		#region	Unprojection

		/// <summary>
		/// Unprojects a screen space coordinate into world space
		/// </summary>
		/// <param name="x">Screen X position</param>
		/// <param name="y">Screen Y position</param>
		/// <param name="depth">View depth</param>
		/// <returns>Returns the unprojected world position</returns>
		public Maths.Point3			Unproject( int x, int y, float depth )
		{
			//	TODO: This is a bodge - don't abuse the rendering pipeline like this!
			Apply( );
			return Rendering.Renderer.Inst.Unproject( x, y, depth );
		}

		/// <summary>
		/// Creates a pick ray from a screen position
		/// </summary>
		/// <param name="x">Screen X position</param>
		/// <param name="y">Screen Y position</param>
		/// <returns>Returns world ray</returns>
		public Maths.Ray3			PickRay( int x, int y )
		{
			//	TODO: This is a bodge - don't abuse the rendering pipeline like this!
			Apply( );
			return new Maths.Ray3( Position, ( Rendering.Renderer.Inst.Unproject( x, y, 1 ) - Position ).MakeNormal( ) );
		}

		#endregion

		#region	Camera frame

		/// <summary>
		///	Gets the camera frame's x axis
		/// </summary>
		public Vector3	XAxis
		{
			get
			{
				return m_XAxis;
			}
		}

		/// <summary>
		///	Gets the camera frame's y axis
		/// </summary>
		public Vector3	YAxis
		{
			get
			{
				return m_YAxis;
			}
		}

		/// <summary>
		///	Gets the camera frame's z axis
		/// </summary>
		public Vector3	ZAxis
		{
			get
			{
				return m_ZAxis;
			}
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
		
		protected Vector3	m_XAxis		= new Vector3( 1, 0, 0 );
		protected Vector3	m_YAxis		= new Vector3( 0, 1, 0 );
		protected Vector3	m_ZAxis		= new Vector3( 0, 0, 1 );
		protected Point3	m_Pos		= new Point3( );

		#endregion
	}
}
