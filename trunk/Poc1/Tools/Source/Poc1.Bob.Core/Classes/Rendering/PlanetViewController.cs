using System;
using Poc1.Bob.Core.Interfaces.Rendering;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets;
using Rb.Core.Utils;
using Rb.Rendering;

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
					//StarBox stars = new StarBox( );
					view.Renderable = new RenderableList( planet );
					SetDefaultCameraPosition( view.UniCamera );
				};

			m_Planet = planet;

			planet.PlanetModel.ModelChanged += OnModelChanged;
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
			if ( Planet.PlanetModel.TerrainModel != null )
			{
				cameraHeight = Planet.PlanetModel.TerrainModel.MaximumHeight;
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
