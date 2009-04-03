using Poc1.Core.Classes.Astronomical.Planets.Models.Templates;
using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Models;
using Rb.Core.Maths;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Models.Templates
{
	/// <summary>
	/// Template for ring models for spherical planets
	/// </summary>
	public class SpherePlanetRingTemplate : PlanetRingTemplate
	{
		/// <summary>
		/// Gets/sets the range of values that are used to calcualte the inner radius of the ring
		/// </summary>
		/// <remarks>
		/// Values are multiples of the radius of the planet
		/// </remarks>
		public Range<float> InnerRadiusMultiple
		{
			get { return m_InnerRadiusMultiple; }
			set { m_InnerRadiusMultiple = value; }
		}

		/// <summary>
		/// Sets up a ring model instance
		/// </summary>
		public override void SetupInstance( IPlanetEnvironmentModel model, ModelTemplateInstanceContext context )
		{
			base.SetupInstance( model, context );

			ISpherePlanetRingModel ringModel = ( ISpherePlanetRingModel )model;
			double radiusMultiplier = Range.Mid( InnerRadiusMultiple, ( float )context.Random.NextDouble( ) );
			ringModel.InnerRadius = new Units.Metres( ringModel.PlanetModel.Radius * radiusMultiplier );
		}

		#region Private Members

		private Range<float> m_InnerRadiusMultiple = new Range<float>( 1.5f, 3.0f );

		#endregion
	}
}
