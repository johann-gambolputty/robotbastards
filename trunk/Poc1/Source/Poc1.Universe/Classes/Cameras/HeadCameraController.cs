
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
			ZoomOut,

			[CommandDescription( "Zoom", "Changes the camera zoom" )]
			Zoom,

			[CommandDescription( "Pan", "Pans the camera" )]
			Pan,

			[CommandDescription( "Rotate", "Rotates the camera" )]
			Rotate
		}

		[Dispatch]
		public void HandleCommand( CommandMessage msg )
		{
		}
	}
}
