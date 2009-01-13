
using Rb.Interaction.Classes;

namespace Poc1.Universe.Classes.Cameras
{
	/// <summary>
	/// Tracking camera commands
	/// </summary>
	public static class TrackingCameraCommands
	{
		public readonly static CommandGroup Commands;
		public readonly static Command ZoomIn;
		public readonly static Command ZoomOut;
		public readonly static Command Zoom;
		public readonly static Command Pan;
		public readonly static Command Rotate;

		#region Private Members
		
		static TrackingCameraCommands( )
		{
			Commands	= new CommandGroup( "trackingCamera", "Tracking Camera Commands", CommandRegistry.Instance );
			Zoom		= Commands.NewCommand( "zoom", "Zoom", "Zooms the camera in and out" );
			ZoomIn		= Commands.NewCommand( "zoomIn", "Zoom In", "Zooms the camera in" );
			ZoomOut		= Commands.NewCommand( "zoomOut", "Zoom Out", "Zooms the camera out" );
			Pan			= Commands.NewCommand( "pan", "Pan", "Pans the camera" );
			Rotate		= Commands.NewCommand( "rotate", "Rotate", "Rotates the camera" );
		}

		#endregion
	}
}
