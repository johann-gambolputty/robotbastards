
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Spherical;
using Poc1.Universe.Planets.Models.Templates;
using Rb.Core.Maths;

namespace Poc1.Universe.Planets.Spherical.Models.Templates
{
	/// <summary>
	/// Template for spherical planets
	/// </summary>
	public class SpherePlanetModelTemplate : PlanetModelTemplate
	{
		/// <summary>
		/// Get/sets the range of valid radii for a spherical planet instance
		/// </summary>
		public Range<Units.Metres> Radius
		{
			get { return m_Radius; }
			set { m_Radius = value; }
		}


		#region Private Members

		private Range<Units.Metres> m_Radius = new Range<Units.Metres>( new Units.Metres( 5000 ), new Units.Metres( 10000000 ) );

		#endregion

		/// <summary>
		/// Creates a sphere planet model instance
		/// </summary>
		protected override IPlanetModel CreatePlanetModel( IPlanet planet )
		{
			return new SpherePlanetModel( ( ISpherePlanet )planet );
		}

		///// <summary>
		///// Builds the planet model
		///// </summary>
		//protected override void BuildPlanetModel( IPlanetModel planetModel, Poc1.Universe.Interfaces.Planets.Models.Templates.ModelTemplateInstanceContext context )
		//{
		//    base.BuildPlanetModel( planetModel, context );
		//}
	}
}
