
namespace Poc1.Core.Interfaces.Astronomical.Planets.Renderers.Terrain.Patches
{
	/// <summary>
	/// Contains parameters used to control the building of terrain patches
	/// </summary>
	public class TerrainPatchBuildParameters
	{
		/// <summary>
		/// Gets/sets the flag that disables terrain skirt generation
		/// </summary>
		/// <remarks>
		/// Terrain skirts are strips running around the edges of terrain patches. They hide
		/// any cracks between terrain patches.
		/// </remarks>
		public bool DisableTerrainSkirts
		{
			get { return m_DisableTerrainSkirts; }
			set { m_DisableTerrainSkirts = value; }
		}

		#region Private Members

		private bool m_DisableTerrainSkirts;

		#endregion
	}
}
