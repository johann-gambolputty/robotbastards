using System;
using Poc1.Bob.Core.Interfaces.Rendering;
using Poc1.Core.Classes.Astronomical;
using Poc1.Core.Classes.Rendering;
using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Rendering.Cameras;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Rendering
{
	/// <summary>
	/// Controls a <see cref="IUniCameraView"/>, that displays an instance of a planet template
	/// </summary>
	public class PlanetViewController : UniCameraViewController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="view">Camera view</param>
		/// <param name="planet">Planet instance to view</param>
		/// <exception cref="System.ArgumentNullException">Thrown if view or planet are null</exception>
		public PlanetViewController( IUniCameraView view, IPlanet planet ) :
			base( view )
		{
			Arguments.CheckNotNull( view, "view" );
			Arguments.CheckNotNull( planet, "planet" );

			view.InitializeRendering +=
				delegate
				{
				//	SolarSystem scene = new SolarSystem( null );	//	Initializes without stars
					SolarSystem scene = new SolarSystem( );			//	Initializes with stars
					scene.Add( planet );
				//	view.Renderable = new UniRenderer( scene, view.UniCamera, new BaseSolarSystemRenderer( ) );
					view.Renderable = new UniRenderer( scene, view.UniCamera, new SolarSystemRenderer( false ) );
					SetDefaultCameraPosition( view.UniCamera );
				};

			m_Planet = planet;

			planet.Model.ModelChanged += OnModelChanged;
		}

		/// <summary>
		/// Gets the planet being displayed in the view
		/// </summary>
		protected IPlanet Planet
		{
			get { return m_Planet; }
		}

		/// <summary>
		/// Sets the default camera position above a planet
		/// </summary>
		/// <param name="camera"></param>
		protected virtual void SetDefaultCameraPosition( IUniCamera camera )
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
			camera.Position.Set( 0, cameraHeight.ToUniUnits, 0 );
		}

		/// <summary>
		/// Handles model changes
		/// </summary>
		protected virtual void OnModelChanged( object sender, EventArgs args )
		{
		}

		private IPlanet m_Planet;
	}
}
