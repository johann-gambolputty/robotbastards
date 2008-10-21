namespace Poc1.Universe.Planets.Spherical.Renderers.Patches
{
	/// <summary>
	/// Terrain patch constants
	/// </summary>
	public static class TerrainPatchConstants
	{
		/// <summary>
		/// Width and height resolution of the patch in vertices
		/// </summary>
		public const int PatchResolution = 41;

		/// <summary>
		/// Area of a patch
		/// </summary>
		public const int PatchArea = PatchResolution * PatchResolution;

		/// <summary>
		/// Total number of vertices used by the patch. This includes the skirt vertices
		/// </summary>
		public const int PatchTotalVertexCount = PatchArea + ( PatchResolution * 4 );

		/// <summary>
		/// Patch LOD increase error threshold (pixels)
		/// </summary>
		public const float ErrorThreshold = 8;

	}
}
