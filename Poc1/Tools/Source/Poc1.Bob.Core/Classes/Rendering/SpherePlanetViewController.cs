using Poc1.Bob.Core.Interfaces.Rendering;
using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical;
using Poc1.Core.Interfaces.Rendering.Cameras;

namespace Poc1.Bob.Core.Classes.Rendering
{
	/// <summary>
	/// View controller for a spherical planet
	/// </summary>
	public class SpherePlanetViewController : PlanetViewController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="view">Camera view</param>
		/// <param name="planet">Planet instance to view</param>
		/// <exception cref="System.ArgumentNullException">Thrown if view or planet are null</exception>
		public SpherePlanetViewController( IUniCameraView view, ISpherePlanet planet ) :
			base( view, planet )
		{
			m_Planet = planet;
		}

		#region Protected Members

		/// <summary>
		/// Sets the default camera position above a planet
		/// </summary>
		/// <param name="camera"></param>
		protected override void SetDefaultCameraPosition( IUniCamera camera )
		{
			//	Move the camera up to the surface of the planet
			Units.Metres cameraHeight;
			IPlanetTerrainModel terrain = Planet.Model.GetModel<IPlanetTerrainModel>( );
			if ( terrain != null )
			{
				cameraHeight = terrain.MaximumHeight;
			}
			else
			{
				cameraHeight = new Units.Metres( 1000 );
			}
			cameraHeight += m_Planet.Model.Radius;
			camera.Position.Set( 0, cameraHeight.ToUniUnits, 0 );
		}

		#endregion

		#region Private Members

		private ISpherePlanet m_Planet;

		#endregion
	}
}
