using System;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Models.Templates;
using Rb.Core.Maths;
using Rb.Core.Utils;

namespace Poc1.Universe.Planets.Models.Templates
{
	/// <summary>
	/// Planet cloud model template
	/// </summary>
	/// <remarks>
	/// The low and high ranges in this template determine minimum and maximum
	/// cloud coverage in a model instanced by this template.
	/// </remarks>
	public class PlanetCloudModelTemplate : PlanetEnvironmentModelTemplate, IPlanetCloudModelTemplate
	{
		#region IPlanetCloudModelTemplate Members

		/// <summary>
		/// Gets/sets the range of values that the models' cloud layer height can take
		/// </summary>
		/// <see cref="IPlanetCloudModel.CloudLayerMinHeight"/>
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
		/// <see cref="IPlanetCloudModel.CoverageRange"/>
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
		/// <see cref="IPlanetCloudModel.CoverageRange"/>
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
		/// <param name="planetModel">Planet model to create</param>
		/// <param name="context">Instancing context</param>
		public override void SetupInstance( IPlanetEnvironmentModel model, ModelTemplateInstanceContext context )
		{
			Arguments.CheckNotNull( planetModel, "planetModel" );
			Arguments.CheckNotNull( context, "context" );

			IPlanetCloudModel model = CreateCloudModel( );
			SetupCloudModel( model, context );

			planetModel.CloudModel = model;
		}

		/// <summary>
		/// Calls a visitor object
		/// </summary>
		public override T InvokeVisit<T>( IPlanetEnvironmentModelTemplateVisitor<T> visitor )
		{
			return visitor.Visit( this );
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Creates a planet cloud model from this template
		/// </summary>
		protected virtual IPlanetCloudModel CreateCloudModel( )
		{
			return new PlanetCloudModel( );
		}

		/// <summary>
		/// Sets up a cloud model created by <see cref="CreateCloudModel"/>
		/// </summary>
		protected virtual void SetupCloudModel( IPlanetCloudModel model, ModelTemplateInstanceContext context )
		{
			model.CloudLayerMinHeight = Range.Mid( CloudLayerHeightRange, context.Random.NextDouble( ) );

			float min = Range.Mid( LowCoverageRange, ( float )context.Random.NextDouble( ) );
			float max = Range.Mid( HighCoverageRange, ( float )context.Random.NextDouble( ) );
			model.CoverageRange = new Range<float>( Utils.Min( min, max ), Utils.Max( min, max ) );
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
