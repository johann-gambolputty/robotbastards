using Rb.Core.Maths;
using Rb.Rendering.Interfaces;

namespace Rb.Rendering.Base.Cameras
{
	/// <summary>
	/// Drives camera transform using spherical coordinates
	/// </summary>
	public class SphereCamera : ProjectionCamera
	{

		/// <summary>
		/// Sets up initial camera frame
		/// </summary>
		public SphereCamera( )
		{
			UpdateCameraFrame( );
		}

		/// <summary>
		/// Applies a look at transform to the renderer, as well as the standard perspective transform in the Camera base class
		/// </summary>
		public override void Begin( )
		{
			Graphics.Renderer.PushTransform( Transform.WorldToView );
			Graphics.Renderer.SetLookAtTransform( m_LookAt, Frame.Translation, Frame.YAxis );
			m_ViewMatrix = Graphics.Renderer.GetTransform( Transform.WorldToView );
			//	TODO: AP: View matrix should be calculated by SetFrame()
			base.Begin( );
		}

		/// <summary>
		/// Pops camera transforms
		/// </summary>
		public override void End( )
		{
			Graphics.Renderer.PopTransform( Transform.WorldToView );
			base.End( );
		}

		#region	Public properties

		/// <summary>
		/// Access to the camera S spherical coordinate
		/// </summary>
		public float S
		{
			get { return m_S; }
			set
			{
				m_S = Utils.Wrap( value, 0, Constants.TwoPi );
				UpdateCameraFrame( );
			}
		}

		/// <summary>
		/// Access to the camera T spherical coordinate
		/// </summary>
		public float T
		{
			get { return m_T; }
			set
			{
				m_T = Utils.Clamp( value, 0.1f, Constants.Pi - 0.1f );
				UpdateCameraFrame( );
			}
		}

		/// <summary>
		/// Access to the camera look at point. If this is set to null, the camera becomes a 'free look' camera. Otherwise, the 
		/// camera will rotate around the look at point, keeping it in the centre of the viewport
		/// </summary>
		public Point3 LookAt
		{
			get { return m_LookAt; }
			set
			{
				m_LookAt = value;
				UpdateCameraFrame( );
			}
		}

		/// <summary>
		/// Access to the zoom factor. This determines the distance around the look at point that the camera maintains when moving
		/// </summary>
		public float Zoom
		{
			get { return m_Zoom; }
			set
			{
				m_Zoom = value;
				UpdateCameraFrame( );
			}
		}

		#endregion

		#region	Private stuff

		private float	m_S			= Constants.Pi;
		private float	m_T			= 0.1f;
		private Point3	m_LookAt	= new Point3( );
		private float	m_Zoom		= 100.0f;

		/// <summary>
		/// Updates camera frame
		/// </summary>
		private void UpdateCameraFrame( )
		{
			Vector3 zAxis = new Vector3
			    (
			        ( Functions.Cos( m_S ) * Functions.Sin( m_T ) ),
			        ( Functions.Cos( m_T ) ),
			        ( Functions.Sin( m_S ) * Functions.Sin( m_T ) )
			    );

			Vector3 yAxis = new Vector3
			    (
			        -( Functions.Cos( m_S ) * Functions.Cos( m_T ) ),
			        -( -Functions.Sin( m_T ) ),
			        -(  Functions.Sin( m_S ) * Functions.Cos( m_T ) )
			    );

			Vector3 xAxis = Vector3.Cross( zAxis, yAxis );
			Point3 pt = m_LookAt + ( zAxis * m_Zoom );

			SetFrame( pt, xAxis, yAxis, zAxis );
		}

		#endregion
	}
}
