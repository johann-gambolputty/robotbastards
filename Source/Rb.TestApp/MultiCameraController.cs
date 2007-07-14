using System.Collections.Generic;
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Interaction;
using Rb.Log;
using Rb.Rendering.Cameras;

namespace Rb.TestApp
{
	/// <summary>
	/// Commands that MultiCameraController can process
	/// </summary>
	public enum MultiCameraCommands
	{
		[CommandDescription( "Next Camera", "Switches to the next camera" )]
		NextCamera,
		
		[CommandDescription( "Previous Camera", "Switches to the previous camera" )]
		PreviousCamera,

		[CommandDescription("Camera 0", "Selects camera 0")]
		Camera0,

		[CommandDescription("Camera 1", "Selects camera 1")]
		Camera1,

		[CommandDescription("Camera 2", "Selects camera 2")]
		Camera2,

		[CommandDescription("Camera 3", "Selects camera 3")]
		Camera3,

		[CommandDescription("Camera 4", "Selects camera 4")]
		Camera4,

		[CommandDescription("Camera 5", "Selects camera 5")]
		Camera5,

		[CommandDescription("Camera 6", "Selects camera 6")]
		Camera6,

		[CommandDescription("Camera 7", "Selects camera 7")]
		Camera7,

		[CommandDescription("Camera 8", "Selects camera 8")]
		Camera8,

		[CommandDescription("Camera 9", "Selects camera 9")]
		Camera9,
	}

	/// <summary>
	/// Switches between cameras in a <see cref="MultiCamera"/>
	/// </summary>
	public class MultiCameraController : CameraController
	{
		public override CameraBase Camera
		{
			get
			{
				return base.Camera;
			}
			set
			{
				base.Camera = value;
				MultiCamera multiCam = ( MultiCamera )Camera;
				foreach ( CameraBase subCamera in multiCam.Cameras )
				{
					GetCameraController( subCamera ).Enabled = false;
				}
				if ( multiCam.ActiveCamera != null )
				{
					GetCameraController( multiCam.ActiveCamera ).Enabled = true;
				}
				Camera.OnChildAdded += OnChildAddedToCamera;
			}
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

			MultiCamera camera = ( MultiCamera )Camera;

			AppLog.Verbose( "Disabling camera {0}", camera.ActiveCameraIndex );

			GetCameraController( camera.ActiveCamera ).Enabled = false;

			switch ( ( MultiCameraCommands )msg.CommandId )
			{
				case MultiCameraCommands.NextCamera:
					{
						camera.ActiveCameraIndex = ( camera.ActiveCameraIndex + 1 ) % camera.CameraCount;
						break;
					}

				case MultiCameraCommands.PreviousCamera:
					{
						camera.ActiveCameraIndex = ( camera.ActiveCameraIndex == 0 ? camera.CameraCount : camera.ActiveCameraIndex ) - 1;
						break;
					}
				case MultiCameraCommands.Camera0 :
				case MultiCameraCommands.Camera1 :
				case MultiCameraCommands.Camera2 :
				case MultiCameraCommands.Camera3 :
				case MultiCameraCommands.Camera4 :
				case MultiCameraCommands.Camera5 :
				case MultiCameraCommands.Camera6 :
				case MultiCameraCommands.Camera7 :
				case MultiCameraCommands.Camera8 :
				case MultiCameraCommands.Camera9 :
					{
						camera.ActiveCameraIndex = msg.CommandId - ( int )MultiCameraCommands.Camera0;
						break;
					}
			}

			AppLog.Verbose( "Enabling camera {0}", camera.ActiveCameraIndex );
			GetCameraController( camera.ActiveCamera ).Enabled = true;
		}

		/// <summary>
		/// Gets an <see cref="ICameraController"/> from the active camera in the specified camera
		/// </summary>
		private static ICameraController GetCameraController( IParent camera )
		{
			return ParentHelpers.GetChildOfType< ICameraController >( camera );
		}

		/// <summary>
		/// Called when a child is added to the specified camera
		/// </summary>
		private static void OnChildAddedToCamera( object camera, object obj )
		{
			if ( obj is ICameraController )
			{
				( ( ICameraController )obj ).Enabled = ( ( MultiCamera )camera ).CameraCount == 0;
			}
		}
	}
}
