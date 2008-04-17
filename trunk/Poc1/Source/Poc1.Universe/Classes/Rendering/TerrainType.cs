using Rb.Core.Maths;
using Poc1.Universe.Classes.Rendering;

namespace Poc1.Universe.Interfaces.Rendering
{
	/// <summary>
	/// A type of terrain found on a planet
	/// </summary>
	public interface ITerrainType
	{
		/// <summary>
		/// Gets the name of this terrain type
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Gets the distribution of this terrain type
		/// </summary>
		TerrainDistribution Distribution
		{
			get;
		}

		/// <summary>
		/// Gets the UV rectangle for this terrain type, in the specified terrain set texture
		/// </summary>
		Rectangle GetUvRectangle( ITexture terrainSetTexture );
	}

	/// <summary>
	/// Manages terrain types for a planet 
	/// </summary>
	public unsafe interface ITerrainTypeManager
	{
		/// <summary>
		/// Assigns terrain types to terrain vertices
		/// </summary>
		void AssignTerrainVertexTypes( int numVertices, TerrainVertex* vertices );

		/// <summary>
		/// Sets a pixel to a terrain type colour, based on elevation, latitude and slope at that pixel
		/// </summary>
		void SetTerrainTypeColour( float e, float l, float s, byte* pixel );
	}
}