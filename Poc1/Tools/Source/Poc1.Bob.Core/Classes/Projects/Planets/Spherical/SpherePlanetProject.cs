using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Core.Classes.Astronomical.Planets.Models.Templates;
using Poc1.Core.Classes.Astronomical.Planets.Spherical.Models;
using Poc1.Core.Classes.Astronomical.Planets.Spherical.Models.Templates;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Models;
using Poc1.Universe.Planets.Spherical.Renderers;

namespace Poc1.Bob.Core.Classes.Projects.Planets.Spherical
{
	/// <summary>
	/// Sphere planet project instance
	/// </summary>
	public class SpherePlanetProject : PlanetProject
	{
		/// <summary>
		/// Creates an empty planet project. Planet has no environment components
		/// </summary>
		public static SpherePlanetProject EmptyPlanetProject( SpherePlanetProjectType projectType, string name )
		{
			return new SpherePlanetProject( projectType, name );
		}

		/// <summary>
		/// Creates a terrestrial planet project. Planet has terrain, atmosphere, cloud and ocean components
		/// </summary>
		public static SpherePlanetProject TerrestrialPlanetProject( SpherePlanetProjectType projectType, string name )
		{
			ISpherePlanetModelTemplate template = new SpherePlanetModelTemplate( );
			template.Add( new PlanetHomogenousProceduralTerrainTemplate( ) );

			ISpherePlanet planet = new SpherePlanet( );

			return new SpherePlanetProject( projectType, name, template, planet );
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="projectType">Project type that created this project</param>
		/// <param name="name">Project name</param>
		public SpherePlanetProject( SpherePlanetProjectType projectType, string name ) :
			this( projectType, name, new SpherePlanetModelTemplate( ), new SpherePlanet( ) )
		{
		}

		/// <summary>
		/// Gets the entire list of biomes that can be used
		/// </summary>
		public BiomeListModel AllBiomes
		{
			get { return m_AllBiomes; }
		}

		/// <summary>
		/// Gets the subset of biomes that are being used by this project
		/// </summary>
		public BiomeListModel CurrentBiomes
		{
			get { return m_CurrentBiomes; }
		}

		/// <summary>
		/// Gets the biome distributions model
		/// </summary>
		public BiomeListLatitudeDistributionModel BiomeDistributions
		{
			get { return m_BiomeDistributions; }
		}

		#region Private Members

		private readonly BiomeListLatitudeDistributionModel m_BiomeDistributions;
		private readonly BiomeListModel m_AllBiomes = new BiomeListModel( );
		private readonly BiomeListModel m_CurrentBiomes = new BiomeListModel( );

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="projectType">Project type that created this project</param>
		/// <param name="name">Project name</param>
		public SpherePlanetProject( SpherePlanetProjectType projectType, string name, ISpherePlanetModelTemplate  template, ISpherePlanet planet ) :
			base( projectType, name, new SimpleSpherePlanetEnvironmentModelFactory( ), new SimpleSpherePlanetEnvironmentRendererFactory( ), template, planet )
		{
			m_BiomeDistributions = new BiomeListLatitudeDistributionModel( m_CurrentBiomes );
		}

		#endregion
	}
}
