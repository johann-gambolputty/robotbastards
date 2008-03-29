using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Interaction;
using Rb.Rendering.Cameras;

namespace Rb.Tools.LevelEditor.Core.Cameras
{
	//	TODO: AP: Currently requires a sphere camera class. The controller should determine S/T/R values from any Camera3, though

	/// <summary>
	/// Simple sphere camera controller
	/// </summary>
	public class LookAtCameraController : CameraController
	{
		/// <summary>
		/// Camera commands
		/// </summary>
		public enum Commands
		{
			[ CommandDescription( "Zoom In", "Zooms the camera in" ) ]
			ZoomIn,

			[ CommandDescription( "Zoom Out", "Zooms the camera out" ) ]
			ZoomOut,

			[ CommandDescription( "Zoom", "Changes the camera zoom" ) ]
			Zoom,

			[ CommandDescription( "Pan", "Pans the camera" ) ]
			Pan,

			[ CommandDescription( "Rotate", "Rotates the camera" ) ]
			Rotate
		}

		/// <summary>
		/// Handles command messages, from the <see cref="Commands"/> enum
		/// </summary>
        [Dispatch]
        public void HandleCameraCommand( CommandMessage msg )
		{
			if ( !Enabled )
			{
				return;
			}
            switch ( ( Commands )msg.CommandId )
            {
                case Commands.Zoom :
                {
                    ( ( SphereCamera )Camera ).Zoom = ( ( ScalarCommandMessage )msg ).Value;
                    break;
                }
                case Commands.Pan :
                {
                    CursorCommandMessage cursorMsg = ( CursorCommandMessage )msg;
                    float deltaX = cursorMsg.X - cursorMsg.LastX;
                    float deltaY = cursorMsg.Y - cursorMsg.LastY;

                    SphereCamera camera = ( ( SphereCamera )Camera );

                    Point3 newLookAt = camera.LookAt;

                    newLookAt += camera.Frame.XAxis * deltaX;
                    newLookAt += camera.Frame.YAxis * deltaY;

                    camera.LookAt = newLookAt;
                    break;
                }
                case Commands.Rotate :
                {
                    CursorCommandMessage cursorMsg = ( CursorCommandMessage )msg;
                    float deltaX = cursorMsg.X - cursorMsg.LastX;
                    float deltaY = cursorMsg.Y - cursorMsg.LastY;

                    SphereCamera camera = ( ( SphereCamera )Camera );

                    camera.S += deltaX * 0.01f;
                    camera.T -= deltaY * 0.01f;

                    break;
                }
			}
		}
	}
}
