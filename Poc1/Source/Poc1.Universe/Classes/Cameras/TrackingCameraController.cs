using Rb.Core.Components;
using Rb.Core.Utils;
using Rb.Interaction;

namespace Poc1.Universe.Classes.Cameras
{
	/// <summary>
	/// Handles user input to control a tracking camera
	/// </summary>
	/// <remarks>
	/// Cheat: Actually used to control a sphere camera... must be added as a child of a <see cref="PointTrackingCamera"/> object
	/// </remarks>
	public class TrackingCameraController : Component
	{
		/// <summary>
		/// Camera commands
		/// </summary>
		public enum Commands
		{
			[CommandDescription( "Zoom In", "Zooms the camera in" )]
			ZoomIn,

			[CommandDescription( "Zoom Out", "Zooms the camera out" )]
			ZoomOut,

			[CommandDescription( "Zoom", "Changes the camera zoom" )]
			Zoom,

			[CommandDescription( "Pan", "Pans the camera" )]
			Pan,

			[CommandDescription( "Rotate", "Rotates the camera" )]
			Rotate
		}

		/// <summary>
		/// Handles command messages, from the <see cref="Commands"/> enum
		/// </summary>
		[Dispatch]
		public void HandleCameraCommand( CommandMessage msg )
		{
			switch ( ( Commands )msg.CommandId )
			{
				case Commands.Zoom:
					{
						m_Camera.Radius += ( ( ScalarCommandMessage )msg ).Value;
						break;
					}
				case Commands.Pan:
					{
						if ( !m_Camera.CanModifyLookAtPoint )
						{
							break;
						}

						CursorCommandMessage cursorMsg = ( CursorCommandMessage )msg;
						float deltaX = cursorMsg.X - cursorMsg.LastX;
						float deltaY = cursorMsg.Y - cursorMsg.LastY;

						UniPoint3 newLookAt = m_Camera.LookAtPoint;

						newLookAt += m_Camera.Frame.TransposedXAxis * UniUnits.Metres.ToUniUnits( deltaX );
						newLookAt += m_Camera.Frame.TransposedYAxis * UniUnits.Metres.ToUniUnits( deltaY );

						m_Camera.LookAtPoint = newLookAt;
						break;
					}
				case Commands.Rotate:
					{
						CursorCommandMessage cursorMsg = ( CursorCommandMessage )msg;
						float deltaX = cursorMsg.X - cursorMsg.LastX;
						float deltaY = cursorMsg.Y - cursorMsg.LastY;

						m_Camera.S -= deltaX * 0.01f;
						m_Camera.T -= deltaY * 0.01f;

						break;
					}
			}
		}

		#region IChild Members

		/// <summary>
		/// Called when this object is added to a parent object. Assumes that parent is of type <see cref="PointTrackingCamera"/>
		/// </summary>
		public override void AddedToParent( object parent )
		{
			base.AddedToParent( parent );
			m_Camera = ( PointTrackingCamera )parent;
		}

		/// <summary>
		/// Called when this object is removed from a parent object
		/// </summary>
		public override void RemovedFromParent( object parent )
		{
			base.RemovedFromParent( parent );
			m_Camera = null;
		}

		#endregion

		#region Private Members

		private PointTrackingCamera m_Camera;

		#endregion
	}
}
