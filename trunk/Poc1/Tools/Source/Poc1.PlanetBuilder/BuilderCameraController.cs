
using System.Windows.Forms;
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Interaction;
using Rb.Rendering.Cameras;
using Rb.Interaction.Windows;

namespace Poc1.PlanetBuilder
{
	class BuilderCameraController : Component
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

			[CommandDescription( "Slip left", "Slips the camera left" )]
			SlipLeft,

			[CommandDescription( "Slip right", "Slips the camera right" )]
			SlipRight,

			[CommandDescription( "Turn", "Turns the camera (affects yaw and pitch)" )]
			Turn
		}

		public BuilderCameraController( InputContext context, CommandUser user )
		{
			CommandList commands = CommandListManager.Instance.FindOrCreateFromEnum( typeof( Commands ) );
			
			CommandInputTemplateMap templateMap = new CommandInputTemplateMap( );
			templateMap.Add( commands.FindByCommandId( ( int )Commands.Forwards ), new KeyInputTemplate( Keys.W, KeyState.Held ) );
			templateMap.Add( commands.FindByCommandId( ( int )Commands.Backwards ), new KeyInputTemplate( Keys.S, KeyState.Held ) );
			templateMap.Add( commands.FindByCommandId( ( int )Commands.SlipLeft ), new KeyInputTemplate( Keys.A, KeyState.Held ) );
			templateMap.Add( commands.FindByCommandId( ( int )Commands.SlipRight ), new KeyInputTemplate( Keys.D, KeyState.Held ) );
			templateMap.Add( commands.FindByCommandId( ( int )Commands.RollClockwise ), new KeyInputTemplate( Keys.E, KeyState.Held ) );
			templateMap.Add( commands.FindByCommandId( ( int )Commands.RollAnticlockwise ), new KeyInputTemplate( Keys.Q, KeyState.Held ) );
			templateMap.Add( commands.FindByCommandId( ( int )Commands.PitchDown ), new KeyInputTemplate( Keys.Down, KeyState.Held ) );
			templateMap.Add( commands.FindByCommandId( ( int )Commands.PitchUp ), new KeyInputTemplate( Keys.Up, KeyState.Held ) );
			templateMap.Add( commands.FindByCommandId( ( int )Commands.YawLeft ), new KeyInputTemplate( Keys.Left, KeyState.Held ) );
			templateMap.Add( commands.FindByCommandId( ( int )Commands.YawRight ), new KeyInputTemplate( Keys.Right, KeyState.Held ) );
			templateMap.Add( commands.FindByCommandId( ( int )Commands.Turn ), new MouseCursorInputTemplate( MouseButtons.Middle ) );

			templateMap.BindToInput( context, user );

			user.AddActiveListener( commands, HandleCommand );
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

		public float MaxTurnSpeed
		{
			get { return m_MaxTurnSpeed; }
			set { m_MaxTurnSpeed = value; }
		}


		/// <summary>
		/// Handles camera commands
		/// </summary>
		[Dispatch]
		public void HandleCommand( CommandMessage msg )
		{
			Matrix44 movementFrame = m_Camera.Frame;
			switch ( ( Commands )msg.CommandId )
			{
				case Commands.Forwards:
					m_Camera.Position += movementFrame.ZAxis * MaxForwardSpeed;
					break;

				case Commands.Backwards:
					m_Camera.Position -= movementFrame.ZAxis * MaxForwardSpeed;
					break;

				case Commands.PitchUp:
					m_Camera.ChangePitch( -MaxTurnSpeed );
					break;

				case Commands.PitchDown:
					m_Camera.ChangePitch( MaxTurnSpeed );
					break;

				case Commands.YawLeft:
					m_Camera.ChangeYaw( MaxTurnSpeed );
					break;

				case Commands.YawRight:
					m_Camera.ChangeYaw( -MaxTurnSpeed );
					break;

				case Commands.RollClockwise:
					m_Camera.ChangeRoll( -MaxTurnSpeed );
					break;

				case Commands.RollAnticlockwise:
					m_Camera.ChangeRoll( MaxTurnSpeed );
					break;

				case Commands.Turn:
					CursorCommandMessage cursorMsg = ( CursorCommandMessage )msg;
					float deltaX = ( cursorMsg.X - cursorMsg.LastX ) * 0.01f;
					float deltaY = ( cursorMsg.Y - cursorMsg.LastY ) * 0.01f;

					m_Camera.ChangeYaw( -deltaX );
					m_Camera.ChangePitch( -deltaY );
					break;

				case Commands.SlipLeft:
					m_Camera.Position += movementFrame.XAxis * MaxSlipSpeed;
					break;

				case Commands.SlipRight:
					m_Camera.Position -= movementFrame.XAxis * MaxSlipSpeed;
					break;
			}
		}

		#region IChild Members

		/// <summary>
		/// Called when this object is added to a parent object
		/// </summary>
		public override void AddedToParent( object parent )
		{
			base.AddedToParent( parent );
			m_Camera = ( FlightCamera )parent;
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

		private FlightCamera m_Camera;
		private float m_MaxSlipSpeed = 1;
		private float m_MaxForwardSpeed = 1;
		private float m_MaxTurnSpeed = 0.08f;

		#endregion

	}
}
