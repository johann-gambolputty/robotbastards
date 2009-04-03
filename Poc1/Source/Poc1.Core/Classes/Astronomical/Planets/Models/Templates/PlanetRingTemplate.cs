using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Rb.Core.Maths;

namespace Poc1.Core.Classes.Astronomical.Planets.Models.Templates
{
	/// <summary>
	/// Base template for planetary rings
	/// </summary>
	public abstract class PlanetRingTemplate : PlanetEnvironmentModelTemplate, IPlanetRingTemplate
	{
		/// <summary>
		/// Gets/sets the range of width values that the planetary rings can take
		/// </summary>
		public Range<Units.Metres> RingWidth
		{
			get { return m_RingWidth; }
			set
			{
				if ( m_RingWidth != value )
				{
					m_RingWidth = value;
					OnTemplateChanged( );
				}
			}
		}

		/// <summary>
		/// Gets/sets the probability that this template will create a ring model for the planet
		/// </summary>
		public float ProbabilityOfRings
		{
			get { return m_Probability; }
			set
			{
				if ( m_Probability != value )
				{
					m_Probability = value;
					OnTemplateChanged( );
				}
			}
		}
		
		/// <summary>
		/// Sets up a ring model instance
		/// </summary>
		public override void SetupInstance( IPlanetEnvironmentModel model, ModelTemplateInstanceContext context )
		{
			IPlanetRingModel ringModel = ( IPlanetRingModel )model;
			double width = RingWidth.Minimum + ( RingWidth.Maximum - RingWidth.Minimum ) * context.Random.NextDouble( );
			ringModel.Width = new Units.Metres( width );
		}

		#region Private Members

		private float m_Probability = 1.0f;
		private Range<Units.Metres> m_RingWidth = new Range<Units.Metres>( new Units.Metres( 500 ), new Units.Metres( 5000 ) );

		#endregion
	}
}
