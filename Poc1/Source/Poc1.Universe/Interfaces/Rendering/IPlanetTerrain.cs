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
		/// <param name="uvRes">UV coordinate resolution over entire patch</param>
		/// <param name="firstVertex">Patch vertices</param>
		/// <returns>Centre point of the patch, in render unit space</returns>
		Point3 GenerateTerrainPatchVertices( Point3 origin, Vector3 uStep, Vector3 vStep, int res, float uvRes, TerrainVertex* firstVertex );
		
		/// <summary>
		/// Generates vertices for a patch. Calculates maximum error between this patch and next higher detail patch
		/// </summary>
		/// <param name="origin">Patch origin (on unit geometry)</param>
		/// <param name="uStep">Offset between row vertices</param>
		/// <param name="vStep">Offset between column vertices</param>
		/// <param name="res">Patch resolution</param>
		/// <param name="uvRes">UV coordinate resolution over entire patch</param>
		/// <param name="firstVertex">Patch vertices</param>
		/// <param name="error">Maximum error value between this patch and higher level patch</param>
		/// <returns>Centre point of the patch, in render unit space</returns>
		Point3 GenerateTerrainPatchVertices( Point3 origin, Vector3 uStep, Vector3 vStep, int res, float uvRes, TerrainVertex* firstVertex, out float error );

	}
}
