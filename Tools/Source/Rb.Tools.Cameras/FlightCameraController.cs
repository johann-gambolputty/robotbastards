
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Interaction;
using Rb.Interaction.Classes;
using Rb.Interaction.Classes.InputBindings;
using Rb.Interaction.Interfaces;
using Rb.Rendering.Cameras;

namespace Rb.Tools.Cameras
{
	/// <summary>
	/// Flight camera controller
	/// </summary>
	public class FlightCameraController
	{
		#region Commands

		public readonly static CommandGroup Commands;
		public readonly static Command Forwards;
		public readonly static Command Backwards;
		public readonly static Command PitchUp;
		public readonly static Command PitchDown;
		public readonly static Command RollClockwise;
		public readonly static Command RollAnticlockwise;
		public readonly static Command YawLeft;
		public readonly static Command YawRight;
		public readonly static Command SlipLeft;
		public readonly static Command SlipRight;
		public readonly static Command Turn;

		#endregion

		/// <summary>
		/// Returns an array of default command bindings
		/// </summary>
		public static CommandInputBinding[] DefaultBindings
		{
			get
			{
				return new CommandInputBinding[]
					{
						new CommandKeyInputBinding( Forwards, "W", BinaryInputState.Held ),
						new CommandKeyInputBinding( Backwards, "S", BinaryInputState.Held ),
						new CommandKeyInputBinding( SlipLeft, "A", BinaryInputState.Held ),
						new CommandKeyInputBinding( SlipRight, "D", BinaryInputState.Held ),
						new CommandKeyInputBinding( RollClockwise, "E", BinaryInputState.Held ),
						new CommandKeyInputBinding( RollAnticlockwise, "Q", BinaryInputState.Held ),
						new CommandKeyInputBinding( PitchUp, "Up", BinaryInputState.Held ),
						new CommandKeyInputBinding( PitchDown, "Down", BinaryInputState.Held ),
						new CommandKeyInputBinding( YawLeft, "Left", BinaryInputState.Held ),
						new CommandKeyInputBinding( YawRight, "Right", BinaryInputState.Held ),
						new CommandMouseButtonInputBinding( Turn, MouseButtons.Middle, BinaryInputState.Held )
					};
			}
		}

		/// <summary>
		/// Setup constructor. Sets the user that controls the camera
		/// </summary>
		public FlightCameraController( ICommandUser user ) : this( user, null )
		{
		}


		/// <summary>
		/// Setup constructor. Sets the user that controls the camera
		/// </summary>
		public FlightCameraController( ICommandUser user, FlightCamera camera )
		{
			Camera = camera;
			user.CommandTriggered += HandleCameraCommand;
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

		/// <summary>
		/// Gets/sets the camera controlled by this object
		/// </summary>
		public FlightCamera Camera
		{
			get { return m_Camera; }
			set { m_Camera = value; }
		}


		#region Private Members

		private float m_SecondsSinceLastUpdate = 1;
		private long m_LastUpdate;
		private FlightCamera m_Camera;
		private float m_MaxSlipSpeed = 4;
		private float m_MaxForwardSpeed = 4;
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
		/// Handles command messages, from the <see cref="Commands"/> enum
		/// </summary>
        private void HandleCameraCommand( CommandTriggerData triggerData )
		{
			if ( m_Camera == null )
			{
				return;
			}

			float secondsSinceLastUpdate = m_SecondsSinceLastUpdate;

			InvariantMatrix44 movementFrame = m_Camera.Frame;
			if ( triggerData.Command == Forwards )
			{
				m_Camera.Position += ( movementFrame.ZAxis * MaxForwardSpeed * secondsSinceLastUpdate );
			}
			else if ( triggerData.Command == Backwards )
			{
				m_Camera.Position += ( movementFrame.ZAxis * -MaxForwardSpeed * secondsSinceLastUpdate );
			}
			else if ( triggerData.Command == PitchUp )
			{
				m_Camera.ChangePitch( MaxTurnSpeed * secondsSinceLastUpdate );
			}
			else if ( triggerData.Command == PitchDown )
			{
				m_Camera.ChangePitch( -MaxTurnSpeed * secondsSinceLastUpdate );
			}
			else if ( triggerData.Command == YawLeft )
			{
				m_Camera.ChangeYaw( -MaxTurnSpeed * secondsSinceLastUpdate );
			}
			else if ( triggerData.Command == YawRight )
			{
				m_Camera.ChangeYaw( MaxTurnSpeed * secondsSinceLastUpdate );
			}
			else if ( triggerData.Command == RollClockwise )
			{
				m_Camera.ChangeRoll( MaxTurnSpeed * secondsSinceLastUpdate );
			}
			else if ( triggerData.Command == RollAnticlockwise )
			{
				m_Camera.ChangeRoll( -MaxTurnSpeed * secondsSinceLastUpdate );
			}
			else if ( triggerData.Command == SlipLeft )
			{
				m_Camera.Position -= movementFrame.XAxis * MaxSlipSpeed * secondsSinceLastUpdate;
			}
			else if ( triggerData.Command == SlipRight )
			{
				m_Camera.Position += movementFrame.XAxis * MaxSlipSpeed * secondsSinceLastUpdate;
			}
			else if ( triggerData.Command == Turn )
			{
				CommandPointInputState pointState = ( CommandPointInputState )triggerData.InputState;
				Vector2 delta = pointState.Delta * 0.01f * secondsSinceLastUpdate;

				m_Camera.ChangeYaw( -delta.X );
				m_Camera.ChangePitch( -delta.Y );
			}
        }

		static FlightCameraController( )
		{
			Commands = new CommandGroup( "flightCameraCommands", "Flight Camera Commands", CommandRegistry.Instance );
			Forwards = Commands.NewCommand( "forwards", "Forwards", "Moves forwards" );
			Backwards = Commands.NewCommand( "backwards", "Backwards", "Moves backwards" );
			PitchUp = Commands.NewCommand( "pitchUp", "Pitch up", "Pitches the camera up" );
			PitchDown = Commands.NewCommand( "pitchDown", "Pitch down", "Pitches the camera down" );
			RollClockwise = Commands.NewCommand( "rollClockwise", "Roll clockwise", "Rolls the camera clockwise" );
			RollAnticlockwise = Commands.NewCommand( "rollAnticlockwise", "Roll anticlockwise", "Rolls the camera anti-clockwise" ); ;
			YawLeft = Commands.NewCommand( "yawLeft", "Yaw left", "Yaws the camera left" );
			YawRight = Commands.NewCommand( "yawRight", "Yaw right", "Yaws the camera right" );
			SlipLeft = Commands.NewCommand( "slipLeft", "Slip left", "Slips the camera left" );
			SlipRight = Commands.NewCommand( "slipRight", "Slip right", "Slips the camera right" );
			Turn = Commands.NewCommand( "turn", "Turn", "Turns the camera" );
		}

		#endregion

	}
}
