
namespace Rb.TestApp
{
	/// <summary>
	/// Test command enumeration
	/// </summary>
	public enum TestCommands
	{
		[Interaction.CommandDescription( "Forwards", "Moves forwards" )]
		Forward,

		[Interaction.CommandDescription( "Back", "Moves backwards" )]
		Back,

		[Interaction.CommandDescription( "Left", "Moves left" )]
		Left,

		[Interaction.CommandDescription( "Right", "Moves right" )]
		Right,

		[Interaction.CommandDescription( "Fire", "Fires the current weapon" )]
		Fire,

		[Interaction.CommandDescription( "Jump", "Jumps" )]
		Jump,

		[Interaction.CommandDescription( "Look at", "Looks at a point" )]
		LookAt
	}
}
