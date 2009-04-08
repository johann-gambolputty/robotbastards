using System.ComponentModel;
using System.Windows.Forms;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Poc1.Tools.Atmosphere;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Bob.Controls.Atmosphere
{
	public partial class ScatteringAtmosphereBuildControl : UserControl
	{
		public ScatteringAtmosphereBuildControl( )
		{
			InitializeComponent( );

			//	Populate texture resolution combo boxes
			for ( int i = 4; i <= 1024; i *= 2 )
			{
				opticalDepthResolutionComboBox.Items.Add( i );
				scatteringResolutionComboBox.Items.Add( i );
			}
			scatteringResolutionComboBox.SelectedItem = 16;
			opticalDepthResolutionComboBox.SelectedItem = 256;

			m_AtmosphereBuilder = new AtmosphereBuilder( );

			m_Worker = new BackgroundWorker( );
			m_Worker.WorkerReportsProgress = true;
			m_Worker.WorkerSupportsCancellation = true;
			m_Worker.DoWork += BuildWorkItem;
			m_Worker.RunWorkerCompleted += WorkItemComplete;
			m_Worker.ProgressChanged += BuildProgressChanged;

			atmosphereParametersPropertyGrid.SelectedObject = m_AtmosphereModel;
		}

		/// <summary>
		/// Gets/sets the current atmosphere scattering template
		/// </summary>
		public IPlanetAtmosphereScatteringTemplate Template
		{
			get { return m_Template; }
			set
			{
				m_Template = value;
			}
		}

		/// <summary>
		/// Gets/sets the current atmosphere scattering model
		/// </summary>
		public IPlanetAtmosphereScatteringModel Model
		{
			get { return m_Model; }
			set { m_Model = value; }
		}

		#region Private Members

		private IPlanetAtmosphereScatteringTemplate m_Template;
		private IPlanetAtmosphereScatteringModel m_Model;
		private AtmosphereBuilder m_AtmosphereBuilder;
		private readonly AtmosphereBuildModel m_AtmosphereModel = new AtmosphereBuildModel( );
		private readonly AtmosphereBuildParameters m_AtmosphereBuildParameters = new AtmosphereBuildParameters( );
		private readonly BackgroundWorker m_Worker;

		/// <summary>
		/// Callback from the background worker. Updates the progress bar
		/// </summary>
		private void BuildProgressChanged( object sender, ProgressChangedEventArgs args )
		{
			buildProgressBar.Value = args.ProgressPercentage;
		}

		/// <summary>
		/// Builds the atmosphere
		/// </summary>
		private void BuildWorkItem( object sender, DoWorkEventArgs args )
		{
			AtmosphereBuildProgress progress = new AtmosphereBuildProgress( );
			progress.SliceCompleted +=
				delegate( float p )
				{
					m_Worker.ReportProgress( ( int )( p * 100.0f ) );
					if ( m_Worker.CancellationPending )
					{
						progress.Cancel = true;
					}
				};

			AtmosphereBuildOutputs outputs = m_AtmosphereBuilder.Build( m_AtmosphereModel, m_AtmosphereBuildParameters, progress );
			args.Result = outputs;
		}

		/// <summary>
		/// Called when the atmosphere lookup texture has been built by the background worker
		/// </summary>
		private void WorkItemComplete( object sender, RunWorkerCompletedEventArgs args )
		{
			buildButton.Text = "Build";
			buildProgressBar.Value = 0;

			if ( args.Result == null )
			{
				return;
			}

			AtmosphereBuildOutputs buildOutputs = ( AtmosphereBuildOutputs )args.Result;

			//	Create the atmosphere lookup textures
			ITexture3d scatteringTexture = Graphics.Factory.CreateTexture3d( );
			scatteringTexture.Create( buildOutputs.ScatteringTexture );

			ITexture2d opticalDepthTexture = Graphics.Factory.CreateTexture2d( );
			opticalDepthTexture.Create( buildOutputs.OpticalDepthTexture, false );
		//	opticalDepthTexture.ToBitmap( false )[ 0 ].Save( "OpticalDepthTexture.png" );

			if ( m_Model != null )
			{
				m_Model.OpticalDepthTexture = opticalDepthTexture;
				m_Model.ScatteringTexture = scatteringTexture;
			}
		//	ISpherePlanetAtmosphereRenderer atmoRenderer = BuilderState.Instance.SpherePlanet.PlanetRenderer.SphereAtmosphereRenderer;
		//	atmoRenderer.SetLookupTextures( scatteringTexture, opticalDepthTexture );
		}

		#region Event Handlers

		private void buildButton_Click( object sender, System.EventArgs e )
		{

			if ( m_Worker.IsBusy )
			{
				if ( !m_Worker.CancellationPending )
				{
					m_Worker.CancelAsync( );
				}
				return;
			}
			buildButton.Text = "Cancel";
			m_Worker.RunWorkerAsync( m_AtmosphereBuildParameters );
		}

		#endregion

		#endregion

	}
}
