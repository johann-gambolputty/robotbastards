
using System.Drawing;

namespace Poc1.Universe.Interfaces
{
	/// <summary>
	/// A sun also constitutes a directional light source (well, it's a point light source, but at astronomical
	/// distances, light rays are effectively parallel)
	/// </summary>
	public interface ISun : IEntity
	{
		/// <summary>
		/// Gets/sets the colour of the sun
		/// </summary>
		Color Colour
		{
			get; set;
		}
	}
}
