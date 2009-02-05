using Poc1.Bob.Core.Interfaces.Rendering;
using Poc1.Universe.Classes.Cameras;
using Rb.Core.Utils;
using Rb.Interaction.Classes;
using Rb.Interaction.Interfaces;

namespace Poc1.Bob.Core.Classes.Rendering
{
	/// <summary>
	/// Controller for MVC with a <see cref="IUniCameraView"/> view
	/// </summary>
	public class UniCameraViewController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="view">Camera view</param>
		/// <exception cref="System.ArgumentNullException">Thrown if view or planet are null</exception>
		public UniCameraViewController( IUniCameraView view )
		{
			Arguments.CheckNotNull( view, "view" );

			m_View = view;

			//	Create a camera to add to the view
			UniCamera camera = new FirstPersonCamera( );
			camera.PerspectiveZNear = 1.0f;
			camera.PerspectiveZFar = 15000.0f;
			view.Camera = camera;

			//	Bind camera commands to a camera controller
			ICommandUser user = CommandUser.Default;
			camera.AddChild( new FirstPersonCameraController( user ) );
			view.InputSource.AddBindings( user, m_CameraControlBindings );
			view.InputSource.Start( );
		}

		#region Protected Members

		/// <summary>
		/// Gets the view
		/// </summary>
		protected IUniCameraView View
		{
			get { return m_View; }
		}

		#endregion

		#region Private Members

		private readonly IUniCameraView m_View;
		private readonly CommandInputBinding[] m_CameraControlBindings = FirstPersonCameraCommands.DefaultBindings;

		#endregion
	}
}
