using Rb.Core.Components;
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
	public class FirstPersonCameraController : Component
	{
		/// <summary>
		/// Attaches this controller to a user
		/// </summary>
		/// <param name="user">User to attach to</param>
		public FirstPersonCameraController( ICommandUser user )
		{
			Arguments.CheckNotNull( user, "user" );
			user.CommandTriggered += HandleCommand;
			InteractionUpdateTimer.Instance.Update += OnInteractionUpdate;
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

		#region IChild Members

		/// <summary>
		/// Called when this object is added to a parent object. Assumes that parent is of type <see cref="FirstPersonCamera"/>
		/// </summary>
		public override void AddedToParent( object parent )
		{
			base.AddedToParent( parent );
			m_Camera = ( FirstPersonCamera )parent;
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

		private FirstPersonCamera m_Camera;
		private float m_MaxSlipSpeed = 3000000;
		private float m_MaxForwardSpeed = 3000000;
		private float m_MaxTurnSpeed = 0.7f;
		private long m_LastUpdate;

		/// <summary>
		/// Handles update of the interaction system
		/// </summary>
		private void OnInteractionUpdate( )
		{
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

			float secondsSinceLastUpdate = 0.1f; // ( float )TinyTime.ToSeconds( TinyTime.CurrentTime - m_LastUpdate );

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
