using System;
using System.Windows.Forms;
using Poc1.Bob.Core.Interfaces.Rendering;
using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Spherical;
using Rb.Core.Utils;
using Rb.Interaction;
using Rb.Interaction.Classes;
using Rb.Interaction.Windows;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Cameras;

namespace Poc1.Bob.Controls.Rendering
{
	/// <summary>
	/// Terrain sampler view
	/// </summary>
	public partial class UniCameraViewControl : UserControl, IUniCameraView
	{
		/// <summary>
		/// Default constructor. Initializes control components
		/// </summary>
		public UniCameraViewControl( )
		{
			InitializeComponent( );
			m_InputSource = new CommandControlInputSource( this, null );
		}

		#region Private Members

		private readonly CommandControlInputSource m_InputSource;
		private IUniCamera m_Camera;
		private IRenderable m_Renderable;

		/// <summary>
		/// Adds the current renderable to all the viewers
		/// </summary>
		private void AddRenderableToViewers( )
		{
			foreach ( Viewer viewer in display.Viewers )
			{
				if ( viewer.Renderable == m_Renderable )
				{
					continue;
				}
				viewer.Renderable = m_Renderable;

				if ( m_Renderable != null )
				{
					Units.UniUnits height = new Units.UniUnits( );
					IPlanet planet = m_Renderable as IPlanet;
					if ( planet != null )
					{
						ISpherePlanet spherePlanet = m_Renderable as ISpherePlanet;
						if ( spherePlanet != null )
						{
							height += spherePlanet.PlanetModel.Radius.ToUniUnits;
						}
						if ( planet.PlanetModel.TerrainModel != null )
						{
							height += planet.PlanetModel.TerrainModel.MaximumHeight.ToUniUnits;
						}
						( ( UniCamera )viewer.Camera ).Position = new UniPoint3( 0, height, 0 );
					}
				}
			}
		}

		/// <summary>
		/// Creates the camera used by the main display
		/// </summary>
		private static IUniCamera CreateCamera( )
		{
			//	FlightCamera camera = new FlightCamera( );
			UniCamera camera = new FirstPersonCamera( );
			camera.PerspectiveZNear = 1.0f;
			camera.PerspectiveZFar = 15000.0f;
			camera.AddChild( new FirstPersonCameraController( CommandUser.Default ) );
			//camera.AddChild( new HeadCameraController( context, user ) );
			return camera;
		}

		#region Event Handlers

		/// <summary>
		/// Handles loading this control. Creates a viewer that can render the terrain model
		/// </summary>
		private void UniCameraViewControl_Load( object sender, EventArgs e )
		{
			m_Camera = CreateCamera( );

			Viewer viewer = new Viewer( );
			display.AddViewer( viewer );

			display.AllowArrowKeyInputs = true;

			viewer.Camera = m_Camera;

			CommandControlInputSource.StartMonitoring( CommandUser.Default, display, FirstPersonCameraCommands.DefaultBindings );
			CommandUser.Default.CommandTriggered += OnCommandTriggered;

			//	If planet was already assigned prior to Load, add it to all views
			AddRenderableToViewers( );

			//	TODO: AP: Horrible bodge to work around InteractionUpdateTimer not working properly without manual intervention
			display.OnBeginRender += delegate { InteractionUpdateTimer.Instance.OnUpdate( ); };

			if ( InitializeRendering != null )
			{
				InitializeRendering( this, EventArgs.Empty );
			}
		}

		private void OnCommandTriggered( CommandTriggerData triggerData )
		{
			display.Invalidate( );
		}

		#endregion

		#endregion

		#region IUniCameraView Members

		/// <summary>
		/// Returns the <see cref="ICameraView.Camera"/> property as an <see cref="IUniCamera"/>
		/// </summary>
		public IUniCamera UniCamera
		{
			get { return m_Camera; }
		}

		#endregion

		#region ICameraView Members

		/// <summary>
		/// Event raised when the camera is ready to initialize rendering
		/// </summary>
		/// <remarks>
		/// Rendering resources like effects can only be initialized once a rendering context has been
		/// created by the underlying Display object used by this view.
		/// </remarks>
		public event EventHandler InitializeRendering;

		/// <summary>
		/// Gets/sets the camera used by the view
		/// </summary>
		public ICamera Camera
		{
			get { return m_Camera; }
			set
			{
				m_Camera = Arguments.CheckedNonNullCast<IUniCamera>( value, "value" );
			}
		}

		/// <summary>
		/// Gets the control input source
		/// </summary>
		public CommandInputSource InputSource
		{
			get { return m_InputSource; }
		}

		/// <summary>
		/// Renderable object displayed by this control
		/// </summary>
		public IRenderable Renderable
		{
			get { return m_Renderable; }
			set
			{
				m_Renderable = value;
				AddRenderableToViewers( );
			}
		}

		#endregion

	}
}
