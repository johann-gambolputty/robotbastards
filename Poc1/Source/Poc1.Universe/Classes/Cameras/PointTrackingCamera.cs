using Poc1.Universe.Interfaces;
using Rb.Core.Maths;

namespace Poc1.Universe.Classes.Cameras
{
	public class PointTrackingCamera : UniCamera
	{
		/// <summary>
		/// Gets the point that the camera is focused on
		/// </summary>
		/// <remarks>
		/// If the position changes, the camera's frame will not update until the
		/// next <see cref="UpdateFrame"/> or <see cref="Begin"/> call.
		/// </remarks>
		public virtual UniPoint3 LookAtPoint
		{
			get { return m_LookAt; }
			set
			{
				m_UpdateFrame |= ( m_LookAt != value );
				m_LookAt = value;
			}
		}

		/// <summary>
		/// Gets/sets the S spherical coordinate of the camera (forces update of camera frame)
		/// </summary>
		public float S
		{
			get { return m_S; }
			set
			{
				m_UpdateFrame |= ( m_S != value );
				m_S = value;
			}
		}

		/// <summary>
		/// Gets/sets the T spherical coordinate of the camera (forces update of camera frame)
		/// </summary>
		public float T
		{
			get { return m_T; }
			set
			{
				m_UpdateFrame |= ( m_T != value );
				m_T = value;
			}
		}

		/// <summary>
		/// Gets/sets the Radius spherical coordinate of the camera (forces update of camera frame)
		/// </summary>
		public float Radius
		{
			get { return m_Radius; }
			set
			{
				m_UpdateFrame |= ( m_Radius != value );
				m_Radius = value;
			}
		}

		/// <summary>
		/// Gets the current camera transform
		/// </summary>
		public override UniTransform Frame
		{
			get
			{
				if ( m_UpdateFrame )
				{
					UpdateFrame( );
				}
				return base.Frame;
			}
		}

		/// <summary>
		/// Forces an update of the current camera transform
		/// </summary>
		public void UpdateFrame( )
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
					-( Functions.Sin( m_S ) * Functions.Cos( m_T ) )
				);

			Vector3 xAxis = Vector3.Cross( zAxis, yAxis );
			UniPoint3 pt = m_LookAt + ( zAxis * m_Radius );

			m_UpdateFrame = false;
			Frame.Set( pt, xAxis, yAxis, zAxis );
		}

		/// <summary>
		/// Applies this camera
		/// </summary>
		public override void Begin( )
		{
			UpdateFrame( );
			base.Begin( );
		}

		#region Protected Members

		/// <summary>
		/// Makes the camera transform dirty. The next time <see cref="Frame"/> is accessed, the frame will be updated
		/// </summary>
		protected void DirtyFrame( bool dirty )
		{
			m_UpdateFrame |= dirty;
		}

		#endregion

		#region Private Members

		private float m_S;
		private float m_T;
		private float m_Radius;
		private bool m_UpdateFrame;
		private UniPoint3 m_LookAt = new UniPoint3( );

		#endregion
	}
}
