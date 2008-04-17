using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces.Rendering
{
	/// <summary>
	/// Interfaces for classes that generate terrains for spherical planets
	/// </summary>
	public unsafe interface ISpherePlanetTerrainGenerator
	{
		/// <summary>
		/// Gets the height of a point on the surface of the surface
		/// </summary>
		float GetHeight( float x, float y, float z );

		/// <summary>
		/// Displaces a set of terrain vertices
		/// </summary>
		/// <remarks>
		/// Vertices should already be initialised with their position on the sphere (position property), and
		/// their initial normal (sphere normal)
		/// </remarks>
		void DisplaceTerrainVertices( int vertexCount, TerrainVertex* vertices );

		/// <summary>
		/// Generates the side of a cube map
		/// </summary>
		void GenerateSide( CubeMapFace face, byte* pixels, int width, int height, int stride );
	}
}
