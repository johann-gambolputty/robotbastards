using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces.Rendering
{
	/// <summary>
	/// Planet terrain geometry interface
	/// </summary>
	public unsafe interface IPlanetTerrain
	{
		/// <summary>
		/// Generates the position and normal from a point on a terrain patch
		/// </summary>
		void MakeTerrainVertexFromPatchPoint( Point3 patchPt, TerrainVertex* vertex );

		/// <summary>
		/// Renders the planet using textures only (no terrain)
		/// </summary>
		void RenderFlat( IRenderContext context );
	}
}
