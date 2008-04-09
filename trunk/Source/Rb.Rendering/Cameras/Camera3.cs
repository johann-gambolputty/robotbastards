using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects.Cameras;

namespace Rb.Rendering.Cameras
{
	/// <summary>
	/// 3d camera base class
	/// </summary>
	public abstract class Camera3 : CameraBase, ICamera3
	{
		/// <summary>
		/// Calculates the inverse of the project.view matrix, for unprojection operations
		/// </summary>
		public override void Begin( )
		{
			m_InvProjView.StoreMultiply( m_ProjectionMatrix, m_ViewMatrix );
			m_InvProjView.Invert( );
			base.Begin( );
		}

		/// <summary>
		/// Returns the inverse of the camera's view.projection matrices
		/// </summary>
		public Matrix44 InverseCameraMatrix
		{
			get { return m_InvProjView; }
		}

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
			Point3 result = Graphics.Renderer.Unproject( x, y, depth );
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
			Matrix44 matrix = m_InvProjView;

			float width = Graphics.Renderer.Viewport.Width;
			float height = Graphics.Renderer.Viewport.Height;
			Point3 pt = new Point3( ( 2 * x / width ) - 1, ( 2 * ( height - y ) / height ) - 1, 1.0f );
			Point3 invPt = matrix.NormalizedMultiple( pt );
			Ray3 result = new Ray3( Frame.Translation, ( invPt - Frame.Translation ).MakeNormal( ) );

			return result;
		}

		#endregion

		#region	Camera frame

		/// <summary>
		/// Gets the camera frame
		/// </summary>
		public Matrix44 Frame
		{
			get { return m_CameraMatrix; }
		}

		#endregion

		#region	Protected stuff

		private readonly Matrix44	m_CameraMatrix		= new Matrix44( );
		protected Matrix44			m_ViewMatrix		= Matrix44.Identity;
		protected Matrix44			m_ProjectionMatrix	= Matrix44.Identity;
		private readonly Matrix44	m_InvProjView		= new Matrix44( );

		protected void SetFrame( Point3 pt, Vector3 xAxis, Vector3 yAxis, Vector3 zAxis )
		{
			m_CameraMatrix.Translation = pt;
			m_CameraMatrix.XAxis = xAxis;
			m_CameraMatrix.YAxis = yAxis;
			m_CameraMatrix.ZAxis = zAxis;
		}

		#endregion
	}
}
