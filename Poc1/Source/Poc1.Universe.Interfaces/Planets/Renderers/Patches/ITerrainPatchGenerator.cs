using Poc1.Universe.Interfaces.Rendering;

namespace Poc1.Universe.Interfaces.Planets.Renderers.Patches
{
	/// <summary>
	/// Terrain patch vertex generation interface
	/// </summary>
	public unsafe interface ITerrainPatchGenerator
	{
		/// <summary>
		/// Generates vertices for a patch
		/// </summary>
		/// <param name="patch">Patch</param>
		/// <param name="res">Patch resolution</param>
		/// <param name="firstVertex">Patch vertices</param>
		void GenerateTerrainPatchVertices( ITerrainPatch patch, int res, TerrainVertex* firstVertex );

		/// <summary>
		/// Generates vertices for a patch. Calculates maximum error between this patch and next higher detail patch
		/// </summary>
		/// <param name="patch">Patch</param>
		/// <param name="res">Patch resolution</param>
		/// <param name="firstVertex">Patch vertices</param>
		/// <param name="error">Maximum error value between this patch and higher level patch</param>
		void GenerateTerrainPatchVertices( ITerrainPatch patch, int res, TerrainVertex* firstVertex, out float error );
	}
}
