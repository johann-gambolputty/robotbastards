namespace Poc1.Universe.Planets.Spherical.Renderers.Patches
{
	/// <summary>
	/// Terrain patch constants
	/// </summary>
	public static class TerrainPatchConstants
	{
		/// <summary>
		/// Width resolution of the patch in vertices
		/// </summary>
		public const int PatchWidth = 33;

		/// <summary>
		/// Height resolution of the patch in vertices
		/// </summary>
		public const int PatchHeight = 33;

		/// <summary>
		/// Area of a patch
		/// </summary>
		public const int PatchArea = PatchWidth * PatchHeight;

		/// <summary>
		/// Total number of vertices used by the patch. This includes the skirt vertices
		/// </summary>
		public const int PatchTotalVertexCount = ( PatchWidth * PatchHeight ) + ( PatchWidth * 2 + PatchHeight * 2 );

		/// <summary>
		/// Patch LOD increase error threshold (pixels)
		/// </summary>
		public const float ErrorThreshold = 8;

	}
}
