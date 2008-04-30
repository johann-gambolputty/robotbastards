
using Rb.Core.Maths;

namespace Poc1.Universe.Classes.Cameras
{
	public class HeadCamera : UniCamera
	{
		public void ChangePitch( float amount )
		{
			m_Orientation = m_Orientation * Quaternion.FromAxisAngle( Vector3.XAxis, amount );
		}
		
		public void ChangeYaw( float amount )
		{
			m_Orientation = m_Orientation * Quaternion.FromAxisAngle( Vector3.YAxis, amount );
		}

		public void ChangeRoll( float amount )
		{
			m_Orientation = m_Orientation * Quaternion.FromAxisAngle( Vector3.ZAxis, amount );
		}
		
		public override void Begin()
		{
			SetViewFrame( m_Orientation );
			base.Begin( );
		}

		private Quaternion m_Orientation = new Quaternion( 0, 0, 0, 1 );
	}
}
