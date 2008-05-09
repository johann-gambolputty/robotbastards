
using Rb.Core.Components;
using Rb.Core.Utils;
using Rb.Interaction;

namespace Poc1.Universe.Classes.Cameras
{
	public class HeadCameraController : Component
	{
		/// <summary>
		/// Camera commands
		/// </summary>
		public enum Commands
		{
			[CommandDescription( "Forwards", "Moves forwards" )]
			Forwards,

			[CommandDescription( "Backwards", "Moves backwards" )]
			Backwards,

			[CommandDescription( "Zoom", "Changes the camera zoom" )]
			Zoom,

			[CommandDescription( "Pitch Up", "Pitches the camera up" )]
			PitchUp,

			[CommandDescription( "Pitch Down", "Pitches the camera down" )]
			PitchDown,

			[CommandDescription( "Roll Clockwise", "Rolls the camera clockwise" )]
			RollClockwise,

			[CommandDescription( "Roll AntiClockwise", "Spins the camera anti-clockwise" )]
			RollAnticlockwise,

			[CommandDescription( "Yaw left", "Turns the camera left" )]
			YawLeft,
			
			[CommandDescription( "Yaw right", "Turns the camera right" )]
			YawRight,

			[CommandDescription( "Slip left", "Slips the camera left")]
			SlipLeft,

			[CommandDescription( "Slip right", "Slips the camera right" )]
			SlipRight,
				
			[CommandDescription( "Turn", "Turns the camera (affects yaw and pitch)" )]
			Turn
		}

		public float MaxForwardSpeed
		{
			get { return m_MaxForwardSpeed; }
			set { m_MaxForwardSpeed = value; }
		}

		public float MaxSlipSpeed
		{
			get { return m_MaxSlipSpeed; }
			set { m_MaxSlipSpeed = value; }
		}

		/// <summary>
		/// Handles camera commands
		/// </summary>
		[Dispatch]
		public void HandleCommand( CommandMessage msg )
		{
			switch ( ( Commands )msg.CommandId )
			{
				case Commands.Forwards :
					m_Camera.Position -= m_Camera.InverseFrame.ZAxis * MaxForwardSpeed;
					break;

				case Commands.Backwards:
					m_Camera.Position += m_Camera.InverseFrame.ZAxis * MaxForwardSpeed;
					break;

				case Commands.PitchUp :
					m_Camera.ChangePitch( -0.1f );
					break;

				case Commands.PitchDown:
					m_Camera.ChangePitch( 0.1f );
					break;

				case Commands.YawLeft:
					m_Camera.ChangeYaw( 0.1f );
					break;
					
				case Commands.YawRight:
					m_Camera.ChangeYaw( -0.1f );
					break;

				case Commands.RollClockwise :
					m_Camera.ChangeRoll( -0.1f );
					break;

				case Commands.RollAnticlockwise :
					m_Camera.ChangeRoll( 0.1f );
					break;

				case Commands.Turn:
					CursorCommandMessage cursorMsg = (CursorCommandMessage)msg;
					float deltaX = ( cursorMsg.X - cursorMsg.LastX ) * 0.01f;
					float deltaY = ( cursorMsg.Y - cursorMsg.LastY ) * 0.01f;

					m_Camera.ChangeYaw( -deltaX );
					m_Camera.ChangePitch( -deltaY );
					break;

				case Commands.SlipLeft:
					m_Camera.Position -= m_Camera.InverseFrame.XAxis * MaxSlipSpeed;
					break;

				case Commands.SlipRight:
					m_Camera.Position += m_Camera.InverseFrame.XAxis * MaxSlipSpeed;
					break;
			}
		}
		
		#region IChild Members

		/// <summary>
		/// Called when this object is added to a parent object. Assumes that parent is of type <see cref="PointTrackingCamera"/>
		/// </summary>
		public override void AddedToParent( object parent )
		{
			base.AddedToParent( parent );
			m_Camera = ( HeadCamera )parent;
		}

		/// <summary>
		/// Called when this object is removed from a parent object
		/// </summary>
		public override void RemovedFromParent( object parent )
		{
			base.RemovedFromParent( parent );
			m_Camera = null;
		}

		#endregion

		#region Private Members

		private HeadCamera m_Camera;
		private float m_MaxSlipSpeed = 300000;
		private float m_MaxForwardSpeed = 300000;

		#endregion
	}
}
