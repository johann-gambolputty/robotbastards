using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Interaction;
using Rb.Interaction.Classes;
using Rb.Interaction.Interfaces;

namespace Poc1.Universe.Classes.Cameras
{
	/// <summary>
	/// Controller class for <see cref="FirstPersonCamera"/>
	/// </summary>
	public class FirstPersonCameraController
	{
		/// <summary>
		/// Attaches this controller to a user
		/// </summary>
		/// <param name="user">User to attach to</param>
		public FirstPersonCameraController( ICommandUser user ) : this( user, null )
		{
		}

		/// <summary>
		/// Attaches this controller to a user
		/// </summary>
		/// <param name="user">User to attach to</param>
		/// <param name="camera">Camera to control. Can be null</param>
		public FirstPersonCameraController( ICommandUser user, FirstPersonCamera camera )
		{
			Arguments.CheckNotNull( user, "user" );
			user.CommandTriggered += HandleCommand;
			InteractionUpdateTimer.Instance.Update += OnInteractionUpdate;
			Camera = camera;
		}

		/// <summary>
		/// Gets/sets the maximum forward speed of the camera
		/// </summary>
		public float MaxForwardSpeed
		{
			get { return m_MaxForwardSpeed; }
			set { m_MaxForwardSpeed = value; }
		}

		/// <summary>
		/// Gets/sets the maximum slip speed of the camera (slip is movement along the x and y axis of the camera)
		/// </summary>
		public float MaxSlipSpeed
		{
			get { return m_MaxSlipSpeed; }
			set { m_MaxSlipSpeed = value; }
		}

		/// <summary>
		/// Gets/sets the maximum speed that the camera can turn (pitch and yaw)
		/// </summary>
		public float MaxTurnSpeed
		{
			get { return m_MaxTurnSpeed; }
			set { m_MaxTurnSpeed = value; }
		}

		/// <summary>
		/// Gets/sets the camera controlled by this object
		/// </summary>
		public FirstPersonCamera Camera
		{
			get { return m_Camera; }
			set { m_Camera = value; }
		}

		#region Private Members

		private float m_SecondsSinceLastUpdate = 1;
		private long m_LastUpdate;
		private FirstPersonCamera m_Camera;
		private float m_MaxSlipSpeed = 3000000;
		private float m_MaxForwardSpeed = 3000000;
		private float m_MaxTurnSpeed = 0.8f;

		/// <summary>
		/// Handles update of the interaction system
		/// </summary>
		private void OnInteractionUpdate( )
		{
			m_SecondsSinceLastUpdate = ( float )TinyTime.ToSeconds( TinyTime.CurrentTime - m_LastUpdate );
			m_LastUpdate = TinyTime.CurrentTime;
		}

		/// <summary>
		/// Handles camera commands
		/// </summary>
		private void HandleCommand( CommandTriggerData triggerData )
		{
			if ( m_Camera == null )
			{
				return;
			}

			float secondsSinceLastUpdate = m_SecondsSinceLastUpdate;

			Matrix44 movementFrame = m_Camera.InverseFrame;
			if ( triggerData.Command == FirstPersonCameraCommands.Forwards )
			{
				m_Camera.Position.Add( movementFrame.ZAxis * -MaxForwardSpeed * secondsSinceLastUpdate );
			}
			else if ( triggerData.Command == FirstPersonCameraCommands.Backwards )
			{
				m_Camera.Position.Add( movementFrame.ZAxis * MaxForwardSpeed * secondsSinceLastUpdate );
			}
			else if ( triggerData.Command == FirstPersonCameraCommands.PitchUp )
			{
				m_Camera.ChangePitch( -MaxTurnSpeed * secondsSinceLastUpdate );
			}
			else if ( triggerData.Command == FirstPersonCameraCommands.PitchDown )
			{
				m_Camera.ChangePitch( MaxTurnSpeed * secondsSinceLastUpdate );
			}
			else if ( triggerData.Command == FirstPersonCameraCommands.YawLeft )
			{
				m_Camera.ChangeYaw( MaxTurnSpeed * secondsSinceLastUpdate );
			}
			else if ( triggerData.Command == FirstPersonCameraCommands.YawRight )
			{
				m_Camera.ChangeYaw( -MaxTurnSpeed * secondsSinceLastUpdate );
			}
			else if ( triggerData.Command == FirstPersonCameraCommands.RollClockwise )
			{
				m_Camera.ChangeRoll( -MaxTurnSpeed * secondsSinceLastUpdate );
			}
			else if ( triggerData.Command == FirstPersonCameraCommands.RollAnticlockwise )
			{
				m_Camera.ChangeRoll( MaxTurnSpeed * secondsSinceLastUpdate );
			}
			else if ( triggerData.Command == FirstPersonCameraCommands.SlipLeft )
			{
				m_Camera.Position -= movementFrame.XAxis * MaxSlipSpeed * secondsSinceLastUpdate;
			}
			else if ( triggerData.Command == FirstPersonCameraCommands.SlipRight )
			{
				m_Camera.Position += movementFrame.XAxis * MaxSlipSpeed * secondsSinceLastUpdate;
			}
			else if ( triggerData.Command == FirstPersonCameraCommands.Turn )
			{
				CommandPointInputState pointState = ( CommandPointInputState )triggerData.InputState;
				Vector2 delta = pointState.Delta * 0.01f * secondsSinceLastUpdate;

				m_Camera.ChangeYaw( -delta.X );
				m_Camera.ChangePitch( -delta.Y );
			}
		}

		#endregion
	}
}
