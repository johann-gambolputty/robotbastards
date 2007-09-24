using System;
using Rb.Core.Maths;

namespace Rb.Rendering.Cameras
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
			Graphics.Renderer.SetLookAtTransform( m_LookAt, m_Pos, m_YAxis );
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
				m_S = Utils.Wrap( value, 0, Constants.TwoPi );
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
				m_T = Utils.Clamp( value, 0.1f, Constants.Pi - 0.1f );
				UpdateCameraFrame( );
			}
		}

		/// <summary>
		/// Access to the camera look at point. If this is set to null, the camera becomes a 'free look' camera. Otherwise, the 
		/// camera will rotate around the look at point, keeping it in the centre of the viewport
		/// </summary>
		public Point3	LookAt
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

		#endregion

		#region	Private stuff

		private float	m_S			= Constants.Pi;
		private float	m_T			= 0.1f;
		private Point3	m_LookAt	= new Point3( );
		private float	m_Zoom		= 100.0f;

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
			m_Pos		= m_LookAt + ( m_ZAxis * m_Zoom );
		}

		#endregion
	}
}
