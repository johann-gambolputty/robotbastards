using Poc1.Core.Classes.Astronomical.Planets.Models.Templates;
using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Models;
using Rb.Core.Maths;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Models.Templates
{
	/// <summary>
	/// Template for spherical planets
	/// </summary>
	public class SpherePlanetModelTemplate : AbstractPlanetModelTemplate, ISpherePlanetModelTemplate
	{
		/// <summary>
		/// Get/sets the range of valid radii for a spherical planet instance
		/// </summary>
		public Range<Units.Metres> Radius
		{
			get { return m_Radius; }
			set { m_Radius = value; }
		}

		#region Protected Members

		/// <summary>
		/// Builds a planet model
		/// </summary>
		protected override void BuildPlanetModel( IPlanetModel planetModel, IPlanetEnvironmentModelFactory modelFactory, ModelTemplateInstanceContext context )
		{
			Units.Metres radius = Range.Mid( m_Radius, context.Random.NextDouble( ) );
			( ( ISpherePlanetModel )planetModel ).Radius = radius;
			base.BuildPlanetModel( planetModel, modelFactory, context );
		}

		#endregion

		#region Private Members

		//0.0094339622641509433962264150943396
	//	private Range<Units.Metres> m_Radius = new Range<Units.Metres>( new Units.Metres( 5000 ), new Units.Metres( 10000000 ) );
		private Range<Units.Metres> m_Radius = new Range<Units.Metres>( new Units.Metres( 400000 ), new Units.Metres( 400000 ) );
	//	private Range<Units.Metres> m_Radius = new Range<Units.Metres>( new Units.Metres( 2120000 ), new Units.Metres( 2120000 ) );
	//	private Range<Units.Metres> m_Radius = new Range<Units.Metres>( new Units.Metres( 6360000 ), new Units.Metres( 6360000 ) );

		#endregion
	}
}
