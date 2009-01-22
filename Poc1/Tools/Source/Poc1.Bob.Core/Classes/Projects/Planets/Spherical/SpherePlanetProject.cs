using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Interfaces.Projects;
using Poc1.Universe.Planets.Spherical.Models.Templates;

namespace Poc1.Bob.Core.Classes.Projects.Planets.Spherical
{
	/// <summary>
	/// Sphere planet project instance
	/// </summary>
	public class SpherePlanetProject : Project
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="projectType">Project type that created this project</param>
		/// <param name="name">Project name</param>
		public SpherePlanetProject( SpherePlanetProjectType projectType, string name ) :
			base( projectType, name, null, null )
		{
			m_BiomeDistributions = new BiomeListLatitudeDistributionModel( m_Biomes );
		}

		/// <summary>
		/// Gets the biome list
		/// </summary>
		public BiomeListModel Biomes
		{
			get { return m_Biomes; }
		}

		/// <summary>
		/// Gets the sphere planet template
		/// </summary>
		public SpherePlanetModelTemplate PlanetTemplate
		{
			get { return m_PlanetTemplate; }
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
		private readonly BiomeListModel m_Biomes = new BiomeListModel( );
		private readonly SpherePlanetModelTemplate m_PlanetTemplate = new SpherePlanetModelTemplate( );

		#endregion

	}
}
