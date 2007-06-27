using Rb.Core.Utils;
using Rb.Core.Maths;
using Rb.Core.Components;
using Rb.Interaction;
using Rb.Rendering.Cameras;

namespace Rb.TestApp
{
    public class SphereCameraController : Component
    {
        [Dispatch]
        public void HandleCameraCommand( CommandMessage msg )
        {
            switch ( ( CameraCommands )msg.CommandId )
            {
                case CameraCommands.Zoom :
                    {
                        ( ( SphereCamera )Parent ).Zoom = ( ( ScalarCommandMessage )msg ).Value;
                        break;
                    }
                case CameraCommands.Pan :
                    {
                        CursorCommandMessage cursorMsg = ( CursorCommandMessage )msg;
                        float deltaX = cursorMsg.X - cursorMsg.LastX;
                        float deltaY = cursorMsg.Y - cursorMsg.LastY;

                        SphereCamera camera = ( ( SphereCamera )Parent );

                        Point3 newLookAt = camera.LookAt;

                        newLookAt += camera.XAxis * deltaX;
                        newLookAt += camera.YAxis * deltaY;

                        camera.LookAt = newLookAt;
                        break;
                    }
                case CameraCommands.Rotate :
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
    }
}
