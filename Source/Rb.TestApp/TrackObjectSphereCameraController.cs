using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Interaction;
using Rb.Rendering.Cameras;

namespace Rb.TestApp
{
	/// <summary>
	/// Commands used by the <see cref="TrackObjectSphereCameraController"/>
	/// </summary>
	public enum TrackCameraCommands
	{
		[CommandDescription("Zoom", "Zooms the camera in and out")]
		Zoom,

		[CommandDescription("Rotate", "Rotates the camera")]
		Rotate
	}

	/// <summary>
	/// Sphere camera controller, that locks onto an object
	/// </summary>
	public class TrackObjectSphereCameraController : CameraController
	{
		/// <summary>
		/// Access to the object being tracked
		/// </summary>
		public object TrackedObject
		{
			get { return m_TrackedObject; }
			set { m_TrackedObject = value; }
		}

		/// <summary>
		/// Handles a camera command message
		/// </summary>
        [Dispatch]
        public void HandleCameraCommand( CommandMessage msg )
        {
			if ( !Enabled )
			{
				return;
			}
            switch ( ( TrackCameraCommands )msg.CommandId )
            {
				case TrackCameraCommands.Zoom:
                    {
                        ( ( SphereCamera )Parent ).Zoom = ( ( ScalarCommandMessage )msg ).Value;
                        break;
                    }
				case TrackCameraCommands.Rotate:
                    {
                        CursorCommandMessage cursorMsg = ( CursorCommandMessage )msg;
                        float deltaX = cursorMsg.X - cursorMsg.LastX;
                        float deltaY = cursorMsg.Y - cursorMsg.LastY;

                        SphereCamera camera = ( ( SphereCamera )Parent );

                        camera.S += deltaX * 0.01f;
                        camera.T -= deltaY * 0.01f;

                        break;
                    }
            }
        }

		private object m_TrackedObject;
	}
}
