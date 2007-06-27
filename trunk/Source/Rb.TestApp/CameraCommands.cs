
using Rb.Interaction;

namespace Rb.TestApp
{
    public enum CameraCommands
    {
        [CommandDescription("Rotate", "Rotates the camera")]
        Rotate,

        [CommandDescription("Pan", "Pans the camera")]
        Pan,

        [CommandDescription("Zoom", "Zooms the camera in and out")]
        Zoom
    }
}
