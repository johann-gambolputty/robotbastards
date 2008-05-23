using Rb.Core.Maths;

namespace Poc1.Universe.Interfaces.Rendering
{
	/// <summary>
	/// Planet terrain geometry interface
	/// </summary>
	public unsafe interface IPlanetTerrain
	{

		/// <summary>
		/// Patches are defined in a local space. This determines the planet-space parameters of a patch
		/// </summary>
		void SetPatchPlanetParameters( ITerrainPatch patch );

		/// <summary>
		/// Generates vertices for a patch
		/// </summary>
		/// <param name="patch">Patch</param>
		/// <param name="res">Patch resolution</param>
		/// <param name="uvRes">UV coordinate resolution over entire patch</param>
		/// <param name="firstVertex">Patch vertices</param>
		void GenerateTerrainPatchVertices( ITerrainPatch patch, int res, float uvRes, TerrainVertex* firstVertex );
		
		/// <summary>
		/// Generates vertices for a patch. Calculates maximum error between this patch and next higher detail patch
		/// </summary>
		/// <param name="patch">Patch</param>
		/// <param name="res">Patch resolution</param>
		/// <param name="uvRes">UV coordinate resolution over entire patch</param>
		/// <param name="firstVertex">Patch vertices</param>
		/// <param name="error">Maximum error value between this patch and higher level patch</param>
		void GenerateTerrainPatchVertices( ITerrainPatch patch, int res, float uvRes, TerrainVertex* firstVertex, out float error );

	}
}
