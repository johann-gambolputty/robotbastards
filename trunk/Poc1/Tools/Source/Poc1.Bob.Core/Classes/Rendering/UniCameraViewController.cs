using Poc1.Bob.Core.Interfaces.Rendering;
using Poc1.Universe.Classes.Cameras;
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
		public UniCameraViewController( IUniCameraView view )
		{
			//	Create a camera to add to the view
			UniCamera camera = new FirstPersonCamera( );
			camera.PerspectiveZNear = 1.0f;
			camera.PerspectiveZFar = 15000.0f;

			//Units.Metres cameraPos = BuilderState.Instance.SpherePlanet.Radius;
			//if ( BuilderState.Instance.SpherePlanet.TerrainModel != null )
			//{
			//    cameraPos += BuilderState.Instance.SpherePlanet.TerrainModel.MaximumHeight;
			//}
			//else
			//{
			//    cameraPos += new Units.Metres( 1000000 );
			//}
			//camera.Position = new UniPoint3( cameraPos.ToUniUnits, 0, 0 );
			view.Camera = camera;

			//	Bind camera commands to a camera controller
			ICommandUser user = CommandUser.Default;
			camera.AddChild( new FirstPersonCameraController( user ) );
			view.InputSource.AddBindings( user, m_CameraControlBindings );
			view.InputSource.Start( );
		}

		#region Private Members

		private readonly CommandInputBinding[] m_CameraControlBindings = FirstPersonCameraCommands.DefaultBindings;

		#endregion
	}
}
