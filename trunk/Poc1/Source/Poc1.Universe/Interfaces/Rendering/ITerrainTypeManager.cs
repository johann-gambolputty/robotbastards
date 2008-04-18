
namespace Poc1.Universe.Interfaces.Rendering
{
	/// <summary>
	/// Manages terrain types for a planet 
	/// </summary>
	public unsafe interface ITerrainTypeManager
	{
		/// <summary>
		/// Gets the array of terrain types supported by this manager
		/// </summary>
		ITerrainType[] TerrainTypes
		{
			get; set;
		}
	}
}