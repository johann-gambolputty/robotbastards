using System;
using RbEngine.Maths;
using System.Windows.Forms;

namespace RbEngine.Cameras
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
		/// Creates a SphereCameraControl to manipulate this camera
		/// </summary>
		public override Object CreateDefaultController( Control control, Scene.SceneDb scene )
		{
			return new SphereCameraControl( this, control, scene );
		}

		/// <summary>
		/// Applies a look at transform to the renderer, as well as the standard perspective transform in the Camera base class
		/// </summary>
		public override void Apply( int width, int height )
		{
			Rendering.Renderer.Inst.SetLookAtTransform( m_LookAt, m_Pos, m_YAxis );
			base.Apply( width, height );
		}

		#region IUpdater Members

		/// <summary>
		/// Updates this object
		/// </summary>
		public void Update( )
		{
		}

		#endregion

		#region	Public properties

		/// <summary>
		/// Access to the camera S spherical coordinate
		/// </summary>
		public float	S
		{
			get
			{
				return m_S;
			}
			set
			{
				m_S = Maths.Utils.Wrap( value, 0, Maths.Constants.kTwoPi );
				UpdateCameraFrame( );
			}
		}

		/// <summary>
		/// Access to the camera T spherical coordinate
		/// </summary>
		public float	T
		{
			get
			{
				return m_T;
			}
			set
			{
				m_T = Maths.Utils.Clamp( value, 0.1f, Maths.Constants.kPi - 0.1f );
				UpdateCameraFrame( );
			}
		}

		/// <summary>
		/// Access to the camera look at point. If this is set to null, the camera becomes a 'free look' camera. Otherwise, the 
		/// camera will rotate around the look at point, keeping it in the centre of the viewport
		/// </summary>
		public Vector3	LookAt
		{
			get
			{
				return m_LookAt;
			}
			set
			{
				m_LookAt = value;
				UpdateCameraFrame( );
			}
		}

		/// <summary>
		/// Access to the zoom factor. This determines the distance around the look at point that the camera maintains when moving
		/// </summary>
		public float	Zoom
		{
			get
			{
				return m_Zoom;
			}
			set
			{
				m_Zoom = value;
				UpdateCameraFrame( );
			}
		}

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
		public Vector3	Position
		{
			get
			{
				return m_Pos;
			}
		}

		#endregion

		#region	Private stuff

		private float	m_S			= Maths.Constants.kPi;
		private float	m_T			= 0.1f;
		private Vector3	m_LookAt	= new Vector3( );
		private float	m_Zoom		= 100.0f;
		private Vector3	m_XAxis		= new Vector3( 1, 0, 0 );
		private Vector3	m_YAxis		= new Vector3( 0, 1, 0 );
		private Vector3	m_ZAxis		= new Vector3( 0, 0, 1 );
		private Vector3	m_Pos		= new Vector3( );

		/// <summary>
		/// Updates camera frame
		/// </summary>
		private void	UpdateCameraFrame( )
		{
			m_ZAxis.X	= ( float )( Math.Cos( m_S ) * Math.Sin( m_T ) );
			m_ZAxis.Y	= ( float )( Math.Cos( m_T ) );
			m_ZAxis.Z	= ( float )( Math.Sin( m_S ) * Math.Sin( m_T ) );

			m_YAxis.X	= ( float )-(  Math.Cos( m_S ) * Math.Cos( m_T ) );
			m_YAxis.Y	= ( float )-( -Math.Sin( m_T ) );
			m_YAxis.Z	= ( float )-(  Math.Sin( m_S ) * Math.Cos( m_T ) );

			m_XAxis		= Vector3.Cross( m_ZAxis, m_YAxis );
			m_Pos		= ( m_ZAxis * m_Zoom )  + m_LookAt;
		}

		#endregion
	}
}
