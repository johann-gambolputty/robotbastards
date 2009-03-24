using Poc1.Core.Interfaces;
using Rb.Core.Maths;
using Rb.Interaction.Classes;
using Rb.Interaction.Interfaces;

namespace Poc1.Core.Classes.Rendering.Cameras
{
	/// <summary>
	/// Handles user input to control a tracking camera
	/// </summary>
	/// <remarks>
	/// Cheat: Actually used to control a sphere camera... must be added as a child of a <see cref="PointTrackingCamera"/> object
	/// </remarks>
	public class TrackingCameraController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="user">User controlling the camera</param>
		public TrackingCameraController( ICommandUser user )
		{
			user.CommandTriggered += HandleCommand;
		}

		/// <summary>
		/// Gets/sets the camera controlled by this controller
		/// </summary>
		public PointTrackingCamera Camera
		{
			get { return m_Camera; }
			set { m_Camera = value; }
		}
		
		#region Private Members

		private PointTrackingCamera m_Camera;

		/// <summary>
		/// Handles command messages from <see cref="TrackingCameraCommands"/>
		/// </summary>
		private void HandleCommand( CommandTriggerData triggerData )
		{
			if ( triggerData.Command == TrackingCameraCommands.Zoom )
			{
				m_Camera.Radius += ( ( CommandScalarInputState )triggerData.InputState ).Value;
			}
			else if ( triggerData.Command == TrackingCameraCommands.Pan )
			{
				if ( !m_Camera.CanModifyLookAtPoint )
				{
					return;
				}

				CommandPointInputState cursorMsg = ( CommandPointInputState )triggerData.InputState;
				Vector2 delta = cursorMsg.Delta;

				UniPoint3 newLookAt = m_Camera.LookAtPoint;

				newLookAt += m_Camera.Frame.TransposedXAxis * Units.Convert.MetresToUni( delta.X );
				newLookAt += m_Camera.Frame.TransposedYAxis * Units.Convert.MetresToUni( delta.Y );

				m_Camera.LookAtPoint = newLookAt;
			}
			else if ( triggerData.Command == TrackingCameraCommands.Rotate )
			{
				CommandPointInputState cursorMsg = ( CommandPointInputState )triggerData.InputState;
				Vector2 delta = cursorMsg.Delta;

				m_Camera.S -= delta.X * 0.01f;
				m_Camera.T -= delta.Y * 0.01f;
			}
		}

		#endregion
	}
}
