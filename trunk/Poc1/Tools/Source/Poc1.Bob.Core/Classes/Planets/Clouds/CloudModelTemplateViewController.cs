using System.Diagnostics;
using Poc1.Bob.Core.Interfaces.Planets.Clouds;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Models.Templates;
using Rb.Core.Utils;
using Poc1.Universe.Interfaces.Planets.Models;
using Rb.Core.Maths;

namespace Poc1.Bob.Core.Classes.Planets.Clouds
{
	/// <summary>
	/// Controller for a <see cref="ICloudModelTemplateView"/>
	/// </summary>
	public class CloudModelTemplateViewController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="view">View to control</param>
		/// <param name="template">Template to control</param>
		/// <param name="model">Model to control</param>
		/// <exception cref="System.ArgumentNullException">Thrown if any argument is null</exception>
		public CloudModelTemplateViewController( ICloudModelTemplateView view, IPlanetCloudModelTemplate template, IPlanetCloudModel model )
		{
			Arguments.CheckNotNull( view, "view" );
			Arguments.CheckNotNull( template, "template" );

			m_Model = model;
			m_Template = template;

			view.TemplateCloudLayerHeightChanged += OnTemplateCloudLayerHeightChanged;
			view.TemplateLowCoverageChanged += OnTemplateLowCoverageChanged;
			view.TemplateHighCoverageChanged += OnTemplateHighCoverageChanged;
			view.ModelCloudLayerHeightChanged += OnModelCloudLayerHeightChanged;
			view.ModelLowCoverageChanged += OnModelLowCoverageChanged;
			view.ModelHighCoverageChanged += OnModelHighCoverageChanged;

			view.EnableModelChanges = ( model != null );
		}

		#region Private Members
		
		private readonly IPlanetCloudModel m_Model;
		private readonly IPlanetCloudModelTemplate m_Template;

		private void OnModelHighCoverageChanged( float coverage )
		{
			Debug.Assert( m_Model == null, "Expected model to be non-null, if model UI value is changing" );
			Range<float> range = new Range<float>( m_Model.CoverageRange.Minimum, coverage );
			m_Model.CoverageRange = range;
		}

		private void OnModelLowCoverageChanged( float coverage )
		{
			Debug.Assert( m_Model == null, "Expected model to be non-null, if model UI value is changing" );
			Range<float> range = new Range<float>( coverage, m_Model.CoverageRange.Maximum );
			m_Model.CoverageRange = range;
		}

		private void OnModelCloudLayerHeightChanged( Units.Metres height )
		{
			Debug.Assert( m_Model == null, "Expected model to be non-null, if model UI value is changing" );
			m_Model.CloudLayerMinHeight = height;
		}

		private void OnTemplateHighCoverageChanged( Range<float> highCoverageRange )
		{
			m_Template.HighCoverageRange = highCoverageRange;
			UpdateModelCoverageRange( );
		}

		private void OnTemplateLowCoverageChanged( Range<float> lowCoverageRange )
		{
			m_Template.LowCoverageRange = lowCoverageRange;
			UpdateModelCoverageRange( );
		}

		private void OnTemplateCloudLayerHeightChanged( Range<Units.Metres> heightRange )
		{
			m_Template.CloudLayerHeightRange = heightRange;
			m_Model.CloudLayerMinHeight = heightRange.Clamp( m_Model.CloudLayerMinHeight );
		}

		/// <summary>
		/// Clamps the cloud model's coverage range to the low and high coverage ranges in the template
		/// </summary>
		private void UpdateModelCoverageRange( )
		{
			float min = m_Template.LowCoverageRange.Clamp( m_Model.CoverageRange.Minimum );
			float max = m_Template.HighCoverageRange.Clamp( m_Model.CoverageRange.Maximum );

			m_Model.CoverageRange = new Range<float>( min, max );
		}

		#endregion
	}
}
