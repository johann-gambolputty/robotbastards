
using Rb.Core.Maths;

namespace Poc1.Universe.Classes.Cameras
{
	public class HeadCamera : UniCamera
	{
		/// <summary>
		/// Changes the camera pitch 
		/// </summary>
		public void ChangePitch( float radians )
		{
			m_Orientation = m_Orientation * Quaternion.FromAxisAngle( Vector3.XAxis, radians );
		}

		/// <summary>
		/// Changes the camera yaw 
		/// </summary>
		public void ChangeYaw( float radians )
		{
			m_Orientation = m_Orientation * Quaternion.FromAxisAngle( Vector3.YAxis, radians );
		}

		/// <summary>
		/// Changes the camera roll 
		/// </summary>
		public void ChangeRoll( float radians )
		{
			m_Orientation = m_Orientation * Quaternion.FromAxisAngle( Vector3.ZAxis, radians );
		}
		
		/// <summary>
		/// Applies the camera transform
		/// </summary>
		public override void Begin()
		{
			SetViewFrame( m_Orientation );
			base.Begin( );
		}

		#region Private Members

		private Quaternion m_Orientation = new Quaternion( 0, 0, 0, 1 ); 

		#endregion
	}
}
