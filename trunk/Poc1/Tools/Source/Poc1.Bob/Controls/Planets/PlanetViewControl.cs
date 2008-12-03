using System;
using System.Windows.Forms;
using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Spherical;
using Rb.Interaction;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects.Cameras;
using Poc1.Bob.Core.Controls.Classes;

namespace Poc1.Bob.Controls.Planets
{
	/// <summary>
	/// Terrain sampler view
	/// </summary>
	public partial class PlanetViewControl : UserControl
	{
		/// <summary>
		/// Default constructor. Initializes control components
		/// </summary>
		public PlanetViewControl( )
		{
			InitializeComponent( );
		}

		#region ITerrainSamplerView Members

		/// <summary>
		/// Gets/sets the planet to display
		/// </summary>
		public IPlanet Planet
		{
			get { return m_Planet; }
			set
			{
				if ( m_Planet != null )
				{
					m_Planet.PlanetChanged -= OnModelChanged;
				}
				m_Planet = value;
				if ( m_Planet != null )
				{
					m_Planet.PlanetChanged += OnModelChanged;
				}
				AddPlanetToViewers( );
			}
		}

		#endregion

		#region Private Members

		private IPlanet m_Planet;

		/// <summary>
		/// Adds the current planet to all the viewers
		/// </summary>
		private void AddPlanetToViewers( )
		{
			foreach ( Viewer viewer in terrainDisplay.Viewers )
			{
				if ( viewer.Renderable == m_Planet )
				{
					continue;
				}
				viewer.Renderable = m_Planet;

				if ( m_Planet != null )
				{
					Units.UniUnits height = new Units.UniUnits( );
					ISpherePlanet spherePlanet = m_Planet as ISpherePlanet;
					if ( spherePlanet != null )
					{
						height += spherePlanet.Radius.ToUniUnits;
					}
					if ( m_Planet.TerrainModel != null )
					{
						height += m_Planet.TerrainModel.MaximumHeight.ToUniUnits;
					}
					( ( UniCamera )viewer.Camera ).Position = new UniPoint3( 0, height, 0 );
				}
			}
		}

		/// <summary>
		/// Creates the camera used by the main display
		/// </summary>
		private static ICamera CreateCamera( InputContext context, CommandUser user )
		{
			//	FlightCamera camera = new FlightCamera( );
			UniCamera camera = new HeadCamera( );
			camera.PerspectiveZNear = 1.0f;
			camera.PerspectiveZFar = 15000.0f;
			camera.AddChild( new BuilderCameraController( context, user ) );
			//camera.AddChild( new HeadCameraController( context, user ) );
			return camera;
		}

		#region Event Handlers

		/// <summary>
		/// Handles the event <see cref="IPlanet.PlanetChanged"/>
		/// </summary>
		private void OnModelChanged( object sender, EventArgs args )
		{
			Invalidate( );
		}

		/// <summary>
		/// Handles loading this control. Creates a viewer that can render the terrain model
		/// </summary>
		private void TerrainSamplerViewControl_Load( object sender, EventArgs e )
		{
			Viewer viewer = new Viewer( );
			terrainDisplay.AddViewer( viewer );

			terrainDisplay.AllowArrowKeyInputs = true;

			InputContext context = new InputContext( viewer );
			CommandUser user = new CommandUser( );

			viewer.Camera = CreateCamera( context, user );
			user.AddActiveListener( CommandListManager.Instance.FindOrCreateFromEnum( typeof( BuilderCameraController.Commands ) ), OnCameraCommandActive );

			//	If planet was already assigned prior to Load, add it to all views
			AddPlanetToViewers( );
		}

		private void OnCameraCommandActive( CommandMessage msg )
		{
			terrainDisplay.Invalidate( );
		}

		#endregion

		#endregion
	}
}
