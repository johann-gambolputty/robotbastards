
namespace Poc1.Core.Interfaces.Astronomical.Planets.Renderers.Terrain.Patches
{
	/// <summary>
	/// Terrain patch vertex generation interface
	/// </summary>
	public unsafe interface ITerrainPatchGenerator
	{
		/// <summary>
		/// Gets patch build parameters
		/// </summary>
		TerrainPatchBuildParameters BuildParameters
		{
			get;
		}

		/// <summary>
		/// Generates vertices for a patch
		/// </summary>
		/// <param name="patch">Patch</param>
		/// <param name="res">Patch resolution</param>
		/// <param name="firstPatchVertex">Patch vertices</param>
		void GenerateTerrainPatchVertices( ITerrainPatch patch, int res, TerrainPatchVertex* firstPatchVertex );

		/// <summary>
		/// Generates vertices for a patch. Calculates maximum error between this patch and next higher detail patch
		/// </summary>
		/// <param name="patch">Patch</param>
		/// <param name="res">Patch resolution</param>
		/// <param name="firstPatchVertex">Patch vertices</param>
		/// <param name="error">Maximum error value between this patch and higher level patch</param>
		void GenerateTerrainPatchVertices( ITerrainPatch patch, int res, TerrainPatchVertex* firstPatchVertex, out float error );
	}
}
