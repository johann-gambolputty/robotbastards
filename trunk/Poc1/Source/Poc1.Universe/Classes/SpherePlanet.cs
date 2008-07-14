using Poc1.Universe.Classes.Rendering;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Classes
{
	public class SpherePlanet : Planet
	{
		/// <summary>
		/// Creates a spherical planet, with a given orbit, name and radius, in metres
		/// </summary>
		public SpherePlanet( IOrbit orbit, string name, double radius ) :
			base( orbit, name )
		{
			m_Radius = UniUnits.Metres.ToUniUnits( radius );

			m_Atmosphere = new SphereAtmosphereModel( );
			m_Terrain = new SphereTerrainModel( this );
			m_Renderer = new SpherePlanetRenderer( this );
		}

		/// <summary>
		/// Gets the radius of this planet, in universe units
		/// </summary>
		public long Radius
		{
			get { return m_Radius; }
		}

		/// <summary>
		/// Gets the spherical atmosphere model (typed alias of <see cref="Atmosphere"/>)
		/// </summary>
		public SphereAtmosphereModel SphereAtmosphere
		{
			get { return m_Atmosphere; }
		}

		/// <summary>
		/// Gets the planetary terrain renderer
		/// </summary>
		public override IPlanetTerrainModel Terrain
		{
			get { return m_Terrain; }
		}

		/// <summary>
		/// Gets the atmosphere model
		/// </summary>
		public override IPlanetAtmosphereModel Atmosphere
		{
			get { return m_Atmosphere; }
		}


		/// <summary>
		/// Regenerates the planet's terrain
		/// </summary>
		public override void RegenerateTerrain( )
		{
			m_Renderer.RegenerateTerrain( );
		}

		/// <summary>
		/// Disposes of this planet. Away with you, I say
		/// </summary>
		public override void Dispose( )
		{
			m_Renderer.Dispose( );
		}

		#region IRenderable Members

		public override void Render( IRenderContext context )
		{
			base.Render( context );
			m_Renderer.Render( context );
		}

		#endregion

		#region Private Members

		private readonly long m_Radius;
		private readonly SpherePlanetRenderer m_Renderer;
		private readonly SphereTerrainModel m_Terrain;
		private readonly SphereAtmosphereModel m_Atmosphere;


		#endregion

	}
}
