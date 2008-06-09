using System.Windows.Forms;
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Interaction;
using Rb.Interaction.Windows;
using Rb.Rendering.Cameras;

namespace Rb.Tools.Cameras
{
	/// <summary>
	/// User input controller for sphere cameras
	/// </summary>
	public class SphereCameraController : Component
	{
		/// <summary>
		/// Camera commands
		/// </summary>
		public enum Commands
		{
			[CommandDescription("Rotate", "Rotates the camera")]
			Rotate,

			[CommandDescription("Pan", "Pans the camera")]
			Pan,

			[CommandDescription("Zoom", "Zooms the camera in and out")]
			Zoom
		}

		/// <summary>
		/// Default constructor. Caller must set up a mapping from user inputs to controller commands, using the HandleCameraCommand method
		/// </summary>
		public SphereCameraController( )
		{
		}

		/// <summary>
		/// Camera controller setup constructor. Creates a default input mapping to controller commands
		/// </summary>
		public SphereCameraController( InputContext context, CommandUser user )
		{
			CommandList commands = CommandListManager.Instance.FindOrCreateFromEnum( typeof( Commands ) );
			
			CommandInputTemplateMap templateMap = new CommandInputTemplateMap( );
			templateMap.Add( commands.FindByCommandId( ( int )Commands.Zoom ), new MouseScrollDeltaInputTemplate( MouseButtons.None, 0.1f ) );
			templateMap.Add( commands.FindByCommandId( ( int )Commands.Pan ), new MouseCursorInputTemplate( MouseButtons.Left ) );
			templateMap.Add( commands.FindByCommandId( ( int )Commands.Rotate ), new MouseCursorInputTemplate( MouseButtons.Right ) );

			templateMap.BindToInput( context, user );

			user.AddActiveListener( commands, HandleCameraCommand );
		}


		/// <summary>
		/// Called when this object is added to a parent component
		/// </summary>
		/// <param name="parent">Parent object</param>
		/// <remarks>
		/// If this controller is added as a child of a sphere camera, the camera is set automatically if it is not null.
		/// </remarks>
		public override void AddedToParent( object parent )
		{
			if ( ( parent is SphereCamera ) && ( Camera == null ) )
			{
				Camera = ( SphereCamera )parent;
			}
			base.AddedToParent( parent );
		}

		/// <summary>
		/// Gets/sets the associated camera
		/// </summary>
		/// <remarks>
		/// If this controller is added as a child of a camera, the camera is set automatically if it is not null.
		/// </remarks>
		public SphereCamera Camera
		{
			get { return m_Camera; }
			set { m_Camera = value; }
		}

		/// <summary>
		/// Handles command messages, from the <see cref="Commands"/> enum
		/// </summary>
        [Dispatch]
        public void HandleCameraCommand( CommandMessage msg )
        {
            switch ( ( Commands )msg.CommandId )
            {
                case Commands.Zoom :
                    {
                        Camera.Zoom += ( ( ScalarCommandMessage )msg ).Value;
                        break;
                    }
                case Commands.Pan :
                    {
                        CursorCommandMessage cursorMsg = ( CursorCommandMessage )msg;
                        float deltaX = cursorMsg.X - cursorMsg.LastX;
                        float deltaY = cursorMsg.Y - cursorMsg.LastY;

                        Point3 newLookAt = Camera.LookAt;

						newLookAt += Camera.Frame.XAxis * deltaX;
						newLookAt += Camera.Frame.YAxis * deltaY;

                        Camera.LookAt = newLookAt;
                        break;
                    }
                case Commands.Rotate :
                    {
                        CursorCommandMessage cursorMsg = ( CursorCommandMessage )msg;
                        float deltaX = cursorMsg.X - cursorMsg.LastX;
                        float deltaY = cursorMsg.Y - cursorMsg.LastY;
                        Camera.S += deltaX * 0.01f;
                        Camera.T -= deltaY * 0.01f;

                        break;
                    }
            }
        }

		private SphereCamera m_Camera;
	}
}
