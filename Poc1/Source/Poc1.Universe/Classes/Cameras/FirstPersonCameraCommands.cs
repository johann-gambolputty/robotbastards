using Rb.Interaction.Classes;
using Rb.Interaction.Classes.InputBindings;

namespace Poc1.Universe.Classes.Cameras
{
	/// <summary>
	/// A set of commands for first-person cameras like <see cref="FirstPersonCamera"/>
	/// </summary>
	public class FirstPersonCameraCommands
	{
		public readonly static CommandList Commands;
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


		#region Private Members
		
		static FirstPersonCameraCommands( )
		{
			Commands			= new CommandList( "fpCamCommands", "First Person Camera Commands", CommandRegistry.Instance );
			Forwards			= Commands.NewCommand( "forwards", "Forwards", "Moves forwards" );
			Backwards			= Commands.NewCommand( "backwards", "Backwards", "Moves backwards" );
			PitchUp				= Commands.NewCommand( "pitchUp", "Pitch up", "Pitches the camera up" );
			PitchDown			= Commands.NewCommand( "pitchDown", "Pitch down", "Pitches the camera down" );
			RollClockwise		= Commands.NewCommand( "rollClockwise", "Roll clockwise", "Rolls the camera clockwise" );
			RollAnticlockwise	= Commands.NewCommand( "rollAnticlockwise", "Roll anticlockwise", "Rolls the camera anti-clockwise" ); ;
			YawLeft				= Commands.NewCommand( "yawLeft", "Yaw left", "Yaws the camera left" );
			YawRight			= Commands.NewCommand( "yawRight", "Yaw right", "Yaws the camera right" );
			SlipLeft			= Commands.NewCommand( "slipLeft", "Slip left", "Slips the camera left" );
			SlipRight			= Commands.NewCommand( "slipRight", "Slip right", "Slips the camera right" );
			Turn				= Commands.NewCommand( "turn", "Turn", "Turns the camera" );
		}

		#endregion
	}
}
