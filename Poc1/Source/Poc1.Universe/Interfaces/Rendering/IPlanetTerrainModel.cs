using Poc1.Fast.Terrain;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces.Rendering
{
	/// <summary>
	/// Planet terrain geometry interface
	/// </summary>
	public unsafe interface IPlanetTerrainModel
	{
		/// <summary>
		/// Gets/sets the terrain types texture
		/// </summary>
		ITexture2d TerrainTypesTexture
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the terrain pack texture
		/// </summary>
		ITexture2d TerrainPackTexture
		{
			get; set;
		}

		/// <summary>
		/// Sets up the terrain functions
		/// </summary>
		/// <param name="maxHeight">Maximum height of the terrain</param>
		/// <param name="heightFunction">The terrain height function</param>
		/// <param name="groundFunction">The terrain ground offset function</param>
		void SetupTerrain( float maxHeight, TerrainFunction heightFunction, TerrainFunction groundFunction );

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
