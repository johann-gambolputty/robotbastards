using System.Drawing;
using Poc1.Universe.Classes.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Rectangle=Rb.Core.Maths.Rectangle;

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
		/// Gets/sets the colour representation of this terrain type
		/// </summary>
		Color Colour
		{
			get; set;
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
}