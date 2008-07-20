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
			for ( int i = 8; i <= 1024; i *= 2 )
			{
				heightSamplesComboBox.Items.Add( i );
				viewAngleSamplesComboBox.Items.Add( i );
				sunAngleSamplesComboBox.Items.Add( i );
			}
			heightSamplesComboBox.SelectedItem = 8;
			viewAngleSamplesComboBox.SelectedItem = 8;
			sunAngleSamplesComboBox.SelectedItem = 8;

			Atmosphere = new AtmosphereBuilder( );

			m_Builder = new BackgroundWorker( );
			m_Builder.WorkerReportsProgress = true;
			m_Builder.WorkerSupportsCancellation = true;
			m_Builder.DoWork += BuildWorkItem;
			m_Builder.RunWorkerCompleted += WorkItemComplete;
			m_Builder.ProgressChanged += BuildProgressChanged;
		}

		/// <summary>
		/// Gets the atmosphere builder
		/// </summary>
		public AtmosphereBuilder Atmosphere
		{
			get { return m_Atmosphere; }
			set { m_Atmosphere = value; }
		}

		#region Private Members

		private AtmosphereBuilder m_Atmosphere;
		private readonly BackgroundWorker m_Builder;

		#region WorkItem class

		/// <summary>
		/// Atmosphere model work item for the background worker
		/// </summary>
		private class WorkItem
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			public WorkItem( BackgroundWorker worker, AtmosphereBuilder atmosphere, int attenuationSamples, int heightSamples, int viewAngleSamples, int sunAngleSamples )
			{
				m_Worker = worker;
				m_Atmosphere = atmosphere;
				m_AttenuationSamples = attenuationSamples;
				m_HeightSamples = heightSamples;
				m_ViewAngleSamples = viewAngleSamples;
				m_SunAngleSamples = sunAngleSamples;
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
				m_Atmosphere.InnerRadius = ( float )UniUnits.Metres.FromUniUnits( spherePlanet.Radius );
				m_Atmosphere.OuterRadius = ( float )UniUnits.Metres.FromUniUnits( spherePlanet.Radius + sphereAtmosphere.Radius );
				m_Atmosphere.AttenuationSamples = m_AttenuationSamples;
				return m_Atmosphere.BuildLookupTexture( m_ViewAngleSamples, m_SunAngleSamples, m_HeightSamples, progress );
			}
			#region Private Members

			private readonly BackgroundWorker m_Worker;
			private readonly AtmosphereBuilder m_Atmosphere;
			private readonly int m_AttenuationSamples;
			private readonly int m_HeightSamples;
			private readonly int m_ViewAngleSamples;
			private readonly int m_SunAngleSamples;

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
			if ( m_Builder.IsBusy )
			{
				if ( !m_Builder.CancellationPending )
				{
					m_Builder.CancelAsync( );
				}
				return;
			}
			int attenuationSamples = ( int )attenuationUpDown.Value;
			int heightSamples = ( int )heightSamplesComboBox.SelectedItem;
			int viewAngleSamples = ( int )viewAngleSamplesComboBox.SelectedItem;
			int sunAngleSamples = ( int )sunAngleSamplesComboBox.SelectedItem;

			buildButton.Text = Resources.Cancel;

			WorkItem item = new WorkItem( m_Builder, m_Atmosphere, attenuationSamples, heightSamples, viewAngleSamples, sunAngleSamples );
			m_Builder.RunWorkerAsync( item );
		}

		#endregion
	}
}
