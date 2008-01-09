using Rb.Core.Components;
using Rb.Core.Utils;
using Rb.Interaction;

namespace Poc0.Core.Cameras
{
	/// <summary>
	/// Allows zooming of a <see cref="FollowCamera"/>
	/// </summary>
	public class FollowCameraControl : Component
	{
		/// <summary>
		/// Camera commands
		/// </summary>
		public enum Commands
		{
			[CommandDescription( "Zoom", "Changes the camera zoom" )]
			Zoom,
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="camera">The camera to control</param>
		/// <param name="user">Control will respond to commands from this user</param>
		public FollowCameraControl( FollowCamera camera, CommandUser user )
		{
			m_Camera = camera;

			//	Create an input listener
			CommandInputListener listener = new CommandInputListener( );
			listener.CommandListEnumType = typeof( Commands );
			listener.User = user;
			AddChild( listener );
		}

		/// <summary>
		/// Handles command messages, from the <see cref="Commands"/> enum
		/// </summary>
		[Dispatch]
		public void HandleCameraCommand( CommandMessage msg )
		{
            if ( ( Commands )msg.CommandId == Commands.Zoom )
            {
                m_Camera.Zoom = ( ( ScalarCommandMessage )msg ).Value;
            }
		}

		private readonly FollowCamera m_Camera;
	}
}
