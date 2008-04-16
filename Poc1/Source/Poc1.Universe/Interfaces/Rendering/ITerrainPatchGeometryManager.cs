
namespace Poc1.Universe.Interfaces.Rendering
{
	/// <summary>
	/// Interface for managing allocations in a vertex or index buffer
	/// </summary>
	public interface ITerrainPatchGeometryManager
	{
		/// <summary>
		/// Allocates a range of vertices and indices from the associated planet buffers
		/// </summary>
		/// <param name="lod">Level of detail to allocate buffers for</param>
		/// <returns>Returns a new ITerrainPatchGeometry object for the specified level of detail</returns>
		ITerrainPatchGeometry CreateGeometry( int lod );

		/// <summary>
		/// Releases terrain patch geometry allocated by <see cref="CreateGeometry"/>
		/// </summary>
		/// <param name="geometry">Geometry object to release</param>
		void ReleaseGeometry( ITerrainPatchGeometry geometry );

		/// <summary>
		/// Sets up the rendering pipeline for patches using geometry created by this manager
		/// </summary>
		void BeginPatchRendering( );

		/// <summary>
		/// Reverts the rendering pipeline after <see cref="BeginPatchRendering"/>
		/// </summary>
		void EndPatchRendering( );
	}
}