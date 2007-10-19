
using Rb.Interaction;

namespace Poc0.Core.Controllers
{
	/// <summary>
	/// Entity commands
	/// </summary>
	public enum EntityCommands
	{
		[CommandDescription( "Move forwards", "Moves forwards" )]
		MoveForward,

		[CommandDescription( "Move backwards", "Moves backwards" )]
		MoveBackward,

		[CommandDescription( "Strafe left", "Sidesteps left" )]
		StrafeLeft,

		[CommandDescription( "Strafe right", "Sidesteps right" )]
		StrafeRight,

		[CommandDescription( "Turn left", "Turns left" )]
		TurnLeft,

		[CommandDescription( "Turn right", "Turns right" )]
		TurnRight,

		[CommandDescription( "Jump", "Jumps" )]
		Jump,

		[CommandDescription( "Fire", "Fires main weapon" )]
		Fire
	}
}
