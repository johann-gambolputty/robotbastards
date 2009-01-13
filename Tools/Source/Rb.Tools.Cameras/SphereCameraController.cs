using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Interaction.Classes;
using Rb.Interaction.Classes.InputBindings;
using Rb.Interaction.Interfaces;
using Rb.Rendering.Cameras;

namespace Rb.Tools.Cameras
{
	/// <summary>
	/// User input controller for sphere cameras
	/// </summary>
	public class SphereCameraController : Component
	{

		#region Commands

		/// <summary>
		/// Controller command list
		/// </summary>
		public readonly static CommandGroup Commands;

		/// <summary>
		/// Controller rotation command
		/// </summary>
		public readonly static Command Rotate;

		/// <summary>
		/// Controller pan command
		/// </summary>
		public readonly static Command Pan;

		/// <summary>
		/// Controller zoom command
		/// </summary>
		public readonly static Command Zoom;

		#endregion

		/// <summary>
		/// Setup constructor. Sets the user that controls the camera
		/// </summary>
		public SphereCameraController( ICommandUser user )
		{
			user.CommandTriggered += HandleCameraCommand;
		}
		
		/// <summary>
		/// Gets default input bindings for the <see cref="Command"/> command list
		/// </summary>
		public static CommandInputBinding[] DefaultInputBindings
		{
			get
			{
				return new CommandInputBinding[]
				{
					new CommandMouseWheelInputBinding( Zoom ),
					new CommandMouseButtonInputBinding( Pan, MouseButtons.Left, BinaryInputState.Down ),
					new CommandMouseButtonInputBinding( Rotate, MouseButtons.Right, BinaryInputState.Held )
				};
			}
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
		/// Gets/sets the panning speed of the camera
		/// </summary>
		public float PanSpeed
		{
			get { return m_PanSpeed; }
			set { m_PanSpeed = value; }
		}

		#region Private Members

		/// <summary>
		/// Handles command messages, from the <see cref="Commands"/> enum
		/// </summary>
        private void HandleCameraCommand( CommandTriggerData data )
        {
			if ( data .Command == Zoom )
			{
				Camera.Zoom += ( ( CommandScalarInputState )data.InputState ).Value;	
			}
			else if ( data.Command == Pan )
			{
				CommandPointInputState cursorMsg = ( CommandPointInputState )data.InputState;
				Vector2 delta = cursorMsg.Delta * PanSpeed;

				Point3 newLookAt = Camera.LookAt;

				newLookAt += Camera.Frame.XAxis * delta.X;
				newLookAt += Camera.Frame.YAxis * delta.Y;

				Camera.LookAt = newLookAt;
			}
			else if ( data.Command == Rotate )
			{
				CommandPointInputState cursorMsg = ( CommandPointInputState )data.InputState;
				Vector2 delta = cursorMsg.Delta * PanSpeed;
                Camera.S += delta.X * 0.01f;
                Camera.T -= delta.Y * 0.01f;
			}
        }

		private float m_PanSpeed = 0.1f;
		private SphereCamera m_Camera;

		static SphereCameraController( )
		{
			Commands = new CommandGroup( "sphereCameraCommands", "Spherical Camera Commands", CommandRegistry.Instance );
			Rotate = Commands.NewCommand( "rotate", "Rotate", "Rotates the camera" );
			Pan = Commands.NewCommand( "pan", "Pan", "Pans the camera" );
			Zoom = Commands.NewCommand( "zoom", "Zoom", "Zooms the camera in and out" );
		}

		#endregion
	}
}
