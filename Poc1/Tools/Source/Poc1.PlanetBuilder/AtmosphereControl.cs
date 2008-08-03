using System;
using System.Windows.Forms;
using Poc1.PlanetBuilder.Properties;
using Poc1.Tools.Atmosphere;
using System.ComponentModel;
using Poc1.Universe;
using Poc1.Universe.Classes;
using Poc1.Universe.Classes.Rendering;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.PlanetBuilder
{
	public partial class AtmosphereControl : UserControl
	{
		public AtmosphereControl( )
		{
			InitializeComponent( );

			//	Populate texture resolution combo boxes
			for ( int i = 4; i <= 1024; i *= 2 )
			{
				heightSamplesComboBox.Items.Add( i );
				viewAngleSamplesComboBox.Items.Add( i );
				sunAngleSamplesComboBox.Items.Add( i );
			}
			heightSamplesComboBox.SelectedItem = m_AtmosphereBuildParameters.HeightSamples;
			viewAngleSamplesComboBox.SelectedItem = m_AtmosphereBuildParameters.ViewAngleSamples;
			sunAngleSamplesComboBox.SelectedItem = m_AtmosphereBuildParameters.SunAngleSamples;

			m_AtmosphereBuilder = new AtmosphereBuilder( );

			mH0UpDown.Value = ( decimal )m_AtmosphereModel.MieDensityScaleHeightFraction;
			rH0UpDown.Value = ( decimal )m_AtmosphereModel.RayleighDensityScaleHeightFraction;
			//	TODO: AP: Remove bodges
			inscatterDistanceFudgeUpDown.Value = ( decimal )m_AtmosphereModel.InscatterDistanceFudgeFactor;
			outscatterDistanceFudgeUpDown.Value = ( decimal )m_AtmosphereModel.OutscatterDistanceFudgeFactor;
			outscatterFudgeUpDown.Value = ( decimal )m_AtmosphereModel.OutscatterFudgeFactor;
			mieFudgeUpDown.Value = ( decimal )m_AtmosphereModel.MieFudgeFactor;
			rayleighFudgeUpDown.Value = ( decimal )m_AtmosphereModel.RayleighFudgeFactor;

			m_Worker = new BackgroundWorker( );
			m_Worker.WorkerReportsProgress = true;
			m_Worker.WorkerSupportsCancellation = true;
			m_Worker.DoWork += BuildWorkItem;
			m_Worker.RunWorkerCompleted += WorkItemComplete;
			m_Worker.ProgressChanged += BuildProgressChanged;
		}

		/// <summary>
		/// Gets the atmosphere builder
		/// </summary>
		[Browsable( false )]
		public AtmosphereBuilder Atmosphere
		{
			get { return m_AtmosphereBuilder; }
			set
			{
				if ( value == null )
				{
					throw new ArgumentNullException( "value" );
				}
				m_AtmosphereBuilder = value;
			}
		}

		#region Private Members

		private AtmosphereBuilder m_AtmosphereBuilder;
		private readonly AtmosphereBuildModel m_AtmosphereModel = new AtmosphereBuildModel( );
		private readonly AtmosphereBuildParameters m_AtmosphereBuildParameters = new AtmosphereBuildParameters( );
		private readonly BackgroundWorker m_Worker;

		#region WorkItem class

		/// <summary>
		/// Atmosphere model work item for the background worker
		/// </summary>
		private class WorkItem
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			public WorkItem( BackgroundWorker worker, AtmosphereBuildModel model, AtmosphereBuildParameters parameters )
			{
				m_Worker = worker;
				m_AtmosphereModel = model;
				m_AtmosphereBuildParameters = parameters;
			}

			/// <summary>
			/// Builds the atmosphere lookup texture
			/// </summary>
			public Texture3dData Build( )
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

				SpherePlanet spherePlanet = ( SpherePlanet )BuilderState.Instance.Planet;
				SphereAtmosphereModel sphereAtmosphere = ( SphereAtmosphereModel )spherePlanet.Atmosphere;
				m_AtmosphereModel.InnerRadius = ( float )UniUnits.Metres.FromUniUnits( spherePlanet.Radius );
				m_AtmosphereModel.OuterRadius = ( float )UniUnits.Metres.FromUniUnits( spherePlanet.Radius + sphereAtmosphere.Radius );
				return m_Atmosphere.BuildLookupTexture( m_AtmosphereModel, m_AtmosphereBuildParameters, progress );
			}

			#region Private Members

			private readonly BackgroundWorker m_Worker;
			private readonly AtmosphereBuilder m_Atmosphere = new AtmosphereBuilder( );
			private readonly AtmosphereBuildModel m_AtmosphereModel;
			private readonly AtmosphereBuildParameters m_AtmosphereBuildParameters;

			#endregion
		}
		
		#endregion

		/// <summary>
		/// Called when the atmosphere lookup texture has been built by the background worker
		/// </summary>
		private void WorkItemComplete( object sender, RunWorkerCompletedEventArgs args )
		{
			buildButton.Text = Resources.Build;
			buildProgressBar.Value = 0;

			//	Create the atmosphere texture
			ITexture3d lookupTexture = Graphics.Factory.CreateTexture3d( );
			lookupTexture.Create( ( Texture3dData )args.Result );

			( ( ISphereAtmosphereRenderer )BuilderState.Instance.Planet.AtmosphereRenderer ).LookupTexture = lookupTexture;
		}

		/// <summary>
		/// Builds a work item on the background worker
		/// </summary>
		private static void BuildWorkItem( object sender, DoWorkEventArgs args )
		{
			args.Result = ( ( WorkItem )args.Argument ).Build( );
		}

		/// <summary>
		/// Callback from the background worker. Updates the progress bar
		/// </summary>
		private void BuildProgressChanged( object sender, ProgressChangedEventArgs args )
		{
			buildProgressBar.Value = args.ProgressPercentage;
		}

		private void buildButton_Click( object sender, EventArgs e )
		{
			if ( m_Worker.IsBusy )
			{
				if ( !m_Worker.CancellationPending )
				{
					m_Worker.CancelAsync( );
				}
				return;
			}

			//	TODO: AP: Remove bodge
			m_AtmosphereModel.OutscatterFudgeFactor = ( float )outscatterFudgeUpDown.Value;
			m_AtmosphereModel.MieFudgeFactor = ( float )mieFudgeUpDown.Value;
			m_AtmosphereModel.RayleighFudgeFactor = ( float )rayleighFudgeUpDown.Value;
			m_AtmosphereModel.InscatterDistanceFudgeFactor = ( float )inscatterDistanceFudgeUpDown.Value;
			m_AtmosphereModel.OutscatterDistanceFudgeFactor = ( float )outscatterDistanceFudgeUpDown.Value;
			m_AtmosphereModel.MieDensityScaleHeightFraction = ( float )mH0UpDown.Value;
			m_AtmosphereModel.RayleighDensityScaleHeightFraction = ( float )rH0UpDown.Value;
			m_AtmosphereBuildParameters.AttenuationSamples = ( int )attenuationUpDown.Value;
			m_AtmosphereBuildParameters.HeightSamples = ( int )heightSamplesComboBox.SelectedItem;
			m_AtmosphereBuildParameters.ViewAngleSamples = ( int )viewAngleSamplesComboBox.SelectedItem;
			m_AtmosphereBuildParameters.SunAngleSamples = ( int )sunAngleSamplesComboBox.SelectedItem;

			buildButton.Text = Resources.Cancel;

			WorkItem item = new WorkItem( m_Worker, m_AtmosphereModel, m_AtmosphereBuildParameters );
			m_Worker.RunWorkerAsync( item );
		}

		private void phaseCoeffUpDown_ValueChanged( object sender, EventArgs e )
		{
			BuilderState.Instance.Planet.Atmosphere.PhaseCoefficient = ( float )phaseCoeffUpDown.Value;
		}

		private void phaseWeightUpDown_ValueChanged( object sender, EventArgs e )
		{
			BuilderState.Instance.Planet.Atmosphere.PhaseWeight = ( float )phaseWeightUpDown.Value;
		}

		#endregion

		private void AtmosphereControl_Load( object sender, EventArgs e )
		{
			SpherePlanet planet = ( SpherePlanet )BuilderState.Instance.Planet;
			m_AtmosphereModel.InnerRadius = ( float )UniUnits.RenderUnits.FromUniUnits( planet.Radius );
			m_AtmosphereModel.OuterRadius = ( float )UniUnits.RenderUnits.FromUniUnits( planet.Radius + planet.SphereAtmosphere.Radius );
			phaseCoeffUpDown.Value = ( decimal )BuilderState.Instance.Planet.Atmosphere.PhaseCoefficient;
			phaseWeightUpDown.Value = ( decimal )BuilderState.Instance.Planet.Atmosphere.PhaseWeight;
		}

	}
}
