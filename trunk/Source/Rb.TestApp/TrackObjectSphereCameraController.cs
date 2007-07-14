using System;
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Interaction;
using Rb.Rendering.Cameras;
using Rb.World;
using Rb.World.Entities;

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
	public class TrackObjectSphereCameraController : CameraController, ISceneObject
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
						m_SOffset -= deltaX * 0.01f;
                        camera.T -= deltaY * 0.01f;

                        break;
                    }
            }
        }

		private float m_SOffset = 0;
		private object m_TrackedObject;

		private void OnUpdate( Clock updateClock )
		{
			if ( TrackedObject != null )
			{
				//	TODO: AP: This is a really cheesy way to do it...
				SphereCamera camera = ( ( SphereCamera )Camera );
				Entity3d frame = ( ( Entity3d )TrackedObject );
				camera.LookAt = frame.NextPosition;

				//	Determine camera S from frame forward vector
				camera.S = ( float )Math.Atan2( frame.Ahead.Z, frame.Ahead.X ) + m_SOffset;


			}
		}

		#region ISceneObject Members

		/// <summary>
		/// Sets the scene context of this object
		/// </summary>
		public void SetSceneContext( Scene scene )
		{
			scene.GetClock( "updateClock" ).Subscribe( OnUpdate );
		}

		#endregion
	}
}
