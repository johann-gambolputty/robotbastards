using Poc1.Tools.TerrainTextures.Core;

namespace Poc1.Bob.Core.Classes.Biomes.Models
{
	/// <summary>
	/// Biome model
	/// </summary>
	public class BiomeModel
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public BiomeModel( )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="name">Biome name</param>
		public BiomeModel( string name )
		{
			m_Name = name;
		}

		/// <summary>
		/// Gets/sets the name of this biome
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		/// <summary>
		/// Gets the terrain type set
		/// </summary>
		public TerrainTypeList TerrainTypes
		{
			get { return m_TerrainTypes; }
		}

		/// <summary>
		/// Gets/sets the lowest latitude bound of this biome (0 = pole, 1 = equator)
		/// </summary>
		public float LowestLatitude
		{
			get { return m_LowestLatitude; }
			set { m_LowestLatitude = value; }
		}

		/// <summary>
		/// Gets/sets the highest latitude bound of this biome (0 = pole, 1 = equator)
		/// </summary>
		public float HighestLatitude
		{
			get { return m_HighestLatitude; }
			set { m_HighestLatitude = value; }
		}

		//	TODO: AP: Add vegetation

		#region Private Members

		private string m_Name;
		private readonly TerrainTypeList m_TerrainTypes = new TerrainTypeList( );
		private float m_LowestLatitude = 0;
		private float m_HighestLatitude = 1;

		#endregion
	}
}
