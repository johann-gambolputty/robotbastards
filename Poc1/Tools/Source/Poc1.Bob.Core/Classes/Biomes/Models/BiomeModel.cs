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

		//	TODO: AP: Add vegetation

		/// <summary>
		/// Returns the name of this biome
		/// </summary>
		public override string ToString( )
		{
			return m_Name;
		}

		#region Private Members

		private string m_Name;
		private readonly TerrainTypeList m_TerrainTypes = new TerrainTypeList( );

		#endregion
	}
}
