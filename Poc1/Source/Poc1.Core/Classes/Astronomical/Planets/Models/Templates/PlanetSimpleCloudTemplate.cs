using System;
using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Rb.Core.Maths;
using Rb.Core.Utils;

namespace Poc1.Core.Classes.Astronomical.Planets.Models.Templates
{
	/// <summary>
	/// Planet cloud model template
	/// </summary>
	/// <remarks>
	/// The low and high ranges in this template determine minimum and maximum
	/// cloud coverage in a model instanced by this template.
	/// </remarks>
	public class PlanetSimpleCloudTemplate : PlanetEnvironmentModelTemplate, IPlanetSimpleCloudTemplate
	{
		#region IPlanetCloudModelTemplate Members

		/// <summary>
		/// Gets/sets the range of values that the models' cloud layer height can take
		/// </summary>
		/// <see cref="IPlanetSimpleCloudModel.CloudLayerHeight"/>
		public Range<Units.Metres> CloudLayerHeightRange
		{
			get { return m_CloudLayerHeightRange; }
			set
			{
				if ( m_CloudLayerHeightRange != value )
				{
					m_CloudLayerHeightRange = value;
					OnTemplateChanged( EventArgs.Empty );
				}
			}
		}

		/// <summary>
		/// Gets the low cloud coverage range for the model template
		/// </summary>
		/// <see cref="IPlanetSimpleCloudModel.CloudCoverRange"/>
		public Range<float> LowCoverageRange
		{
			get { return m_LowCoverageRange; }
			set
			{
				if ( m_LowCoverageRange != value )
				{
					ValidateCoverageRange( value );
					m_LowCoverageRange = value;
					OnTemplateChanged( EventArgs.Empty );
				}
			}
		}

		/// <summary>
		/// Gets the high cloud coverage range for the model template
		/// </summary>
		/// <see cref="IPlanetSimpleCloudModel.CloudCoverRange"/>
		public Range<float> HighCoverageRange
		{
			get { return m_HighCoverageRange; }
			set
			{
				if ( m_HighCoverageRange != value )
				{
					ValidateCoverageRange( value );
					m_HighCoverageRange = value;
					OnTemplateChanged( EventArgs.Empty );
				}
			}
		}

		#endregion

		#region IPlanetEnvironmentModelTemplate Members

		/// <summary>
		/// Creates an instance of this template
		/// </summary>
		/// <param name="model">Planet model to create</param>
		/// <param name="context">Instancing context</param>
		public override void SetupInstance( IPlanetEnvironmentModel model, ModelTemplateInstanceContext context )
		{
			Arguments.CheckNotNull( model, "planetModel" );
			Arguments.CheckNotNull( context, "context" );

			SetupCloudModel( ( IPlanetSimpleCloudModel )model, context );
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Sets up a simple cloud model
		/// </summary>
		protected virtual void SetupCloudModel( IPlanetSimpleCloudModel model, ModelTemplateInstanceContext context )
		{
			model.CloudLayerHeight = Range.Mid( CloudLayerHeightRange, context.Random.NextDouble( ) );

			float min = Range.Mid( LowCoverageRange, ( float )context.Random.NextDouble( ) );
			float max = Range.Mid( HighCoverageRange, ( float )context.Random.NextDouble( ) );
			model.CloudCoverRange = new Range<float>( Utils.Min( min, max ), Utils.Max( min, max ) );
		}

		#endregion

		#region Private Members

		private Range<Units.Metres> m_CloudLayerHeightRange = new Range<Units.Metres>( new Units.Metres( 4000 ), new Units.Metres( 5000 ) );
		private Range<float> m_LowCoverageRange = new Range<float>( 0, 0.5f );
		private Range<float> m_HighCoverageRange = new Range<float>( 0.5f, 1.0f );

		/// <summary>
		/// Validates a range passed to ValidateCoverageRange()
		/// </summary>
		private static void ValidateCoverageRange( Range<float> coverageRange )
		{
			if ( coverageRange.Minimum < 0 )
			{
				throw new ArgumentOutOfRangeException( "coverageRange", "Range minimum must be >= 0" );
			}
			if ( coverageRange.Maximum > 1 )
			{
				throw new ArgumentOutOfRangeException( "coverageRange", "Range maximum must be <= 1" );
			}
		}

		#endregion
	}
}
