using System.ComponentModel;
using System.Windows.Forms;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Poc1.Tools.Atmosphere;
using Rb.Core.Maths;
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
		private ScatteringAtmosphereAnalysisForm m_AnalysisForm;
		private AtmosphereBuildOutputs m_LastBuildOutput;

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
			m_LastBuildOutput = buildOutputs;
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

			m_AtmosphereBuildParameters.HeightSamples = ( int )scatteringResolutionComboBox.SelectedItem;
			m_AtmosphereBuildParameters.ViewAngleSamples = ( int )scatteringResolutionComboBox.SelectedItem;
			m_AtmosphereBuildParameters.SunAngleSamples = ( int )scatteringResolutionComboBox.SelectedItem;
			m_AtmosphereBuildParameters.OpticalDepthResolution = ( int )opticalDepthResolutionComboBox.SelectedItem;
			m_AtmosphereBuildParameters.AttenuationSamples = ( int )attenuationUpDown.Value;

			buildButton.Text = "Cancel";
			m_Worker.RunWorkerAsync( );
		}

		private float GetIntensity( float height, float viewDirection, float sunDirection, int offsetToElement )
		{
			if ( m_LastBuildOutput == null )
			{
				return 0;
			}
			Texture3dData tex = m_LastBuildOutput.ScatteringTexture;
			byte[] data = tex.Bytes;
			int x = ( int )Utils.Clamp( viewDirection * tex.Width, 0, tex.Width - 1 );
			int y = ( int )Utils.Clamp( sunDirection * tex.Height, 0, tex.Height - 1 );
			int z = ( int )Utils.Clamp( height * tex.Depth, 0, tex.Depth - 1 );

			int offset = offsetToElement;
			offset += z * 4;
			offset += y * 4 * tex.Depth;
			offset += x * 4 * tex.Depth * tex.Height;

			byte value = data[ offset ];
			return value / 255.0f;
		}

		private float GetRedIntensity( float height, float viewDirection, float sunDirection )
		{
			return GetIntensity( height, viewDirection, sunDirection, 3 );
		}

		private float GetGreenIntensity( float height, float viewDirection, float sunDirection )
		{
			return GetIntensity( height, viewDirection, sunDirection, 2 );
		}

		private float GetBlueIntensity( float height, float viewDirection, float sunDirection )
		{
			return GetIntensity( height, viewDirection, sunDirection, 1 );
		}

		private float GetMieIntensity( float height, float viewDirection, float sunDirection )
		{
			return GetIntensity( height, viewDirection, sunDirection, 0 );
		}

		private void analyzeButton_Click( object sender, System.EventArgs e )
		{
			if ( ( m_AnalysisForm != null ) && ( m_AnalysisForm.IsHandleCreated ) )
			{
				return;
			}

			m_AnalysisForm = new ScatteringAtmosphereAnalysisForm( );
			m_AnalysisForm.RedIntensityCalculator = GetRedIntensity;
			m_AnalysisForm.GreenIntensityCalculator = GetGreenIntensity;
			m_AnalysisForm.BlueIntensityCalculator = GetBlueIntensity;
			m_AnalysisForm.MieIntensityCalculator = GetMieIntensity;
			m_AnalysisForm.Show( ParentForm );
		}

		#endregion

		#endregion

	}
}
