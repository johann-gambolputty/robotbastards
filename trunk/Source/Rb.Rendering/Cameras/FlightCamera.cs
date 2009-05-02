using Rb.Core.Maths;
using Rb.Rendering.Interfaces;

namespace Rb.Rendering.Cameras
{
	/// <summary>
	/// Camera implementation, with yaw/pitch/roll controls implemented using quaternions
	/// </summary>
	public class FlightCamera : ProjectionCamera
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
		public override void Begin( )
		{
			Matrix44 matrix = Matrix44.MakeQuaternionMatrix( m_Orientation );
			matrix.Translation = -m_Position;	//	TODO: AP: This is bobbins
			Frame = matrix;
			Graphics.Renderer.PushTransform( TransformType.WorldToView, InverseFrame );
			base.Begin( );
		}

		/// <summary>
		/// Unapplies the camera transform
		/// </summary>
		public override void End( )
		{
			Graphics.Renderer.PopTransform( TransformType.WorldToView );
			base.End( );
		}

		/// <summary>
		/// Sets/gets the camera position
		/// </summary>
		public Point3 Position
		{
			get { return m_Position; }
			set { m_Position = value; }
		}

		#region Private Members

		private Point3 m_Position = Point3.Origin;
		private Quaternion m_Orientation = new Quaternion( 0, 0, 0, 1 );

		#endregion
	}
}
