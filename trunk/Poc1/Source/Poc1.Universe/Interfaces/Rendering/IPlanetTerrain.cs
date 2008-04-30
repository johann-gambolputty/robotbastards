using Rb.Core.Maths;

namespace Poc1.Universe.Interfaces.Rendering
{
	/// <summary>
	/// Planet terrain geometry interface
	/// </summary>
	public unsafe interface IPlanetTerrain
	{
		/// <summary>
		/// Generates vertices for a patch
		/// </summary>
		/// <param name="origin">Patch origin (on unit geometry)</param>
		/// <param name="uStep">Offset between row vertices</param>
		/// <param name="vStep">Offset between column vertices</param>
		/// <param name="res">Patch resolution</param>
		/// <param name="firstVertex">Patch vertices</param>
		/// <returns>Centre point of the patch, in render unit space</returns>
		Point3 GenerateTerrainPatchVertices( Point3 origin, Vector3 uStep, Vector3 vStep, int res, TerrainVertex* firstVertex );

	}
}
