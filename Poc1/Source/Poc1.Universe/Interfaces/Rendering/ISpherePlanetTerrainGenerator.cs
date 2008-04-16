namespace Poc1.Universe.Interfaces.Rendering
{
	internal enum PlanetMapFace
	{
		NegX,
		PosX,
		NegY,
		PosY,
		NegZ,
		PosZ
	}

	/// <summary>
	/// Interfaces for classes that generate terrains for spherical planets
	/// </summary>
	public interface ISpherePlanetTerrainGenerator
	{
		void GenerateSide( SpherePlanetFace face, byte* pixels, int width, int height, int stride );
	}
}
