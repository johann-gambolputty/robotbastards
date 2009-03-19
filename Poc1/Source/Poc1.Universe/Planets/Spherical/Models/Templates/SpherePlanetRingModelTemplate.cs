using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Models.Templates;
using Poc1.Universe.Interfaces.Planets.Spherical.Models;
using Poc1.Universe.Planets.Models.Templates;
using Rb.Core.Maths;

namespace Poc1.Universe.Planets.Spherical.Models.Templates
{
	/// <summary>
	/// Template for ring models for spherical planets
	/// </summary>
	public class SpherePlanetRingModelTemplate : PlanetRingModelTemplate
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
			double radius = Range.Mid( InnerRadiusMultiple, ( float )context.Random.NextDouble( ) );
			ringModel.InnerRadius = new Units.Metres( radius );
		}

		#region Private Members

		private Range<float> m_InnerRadiusMultiple;

		#endregion
	}
}
