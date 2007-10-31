
using System.Drawing;

namespace Rb.Rendering.Lights
{
	/// <summary>
	/// Base light interface
	/// </summary>
	public interface ILight
	{
		/// <summary>
		/// Gets/sets shadowcaster flag
		/// </summary>
		bool CastsShadows
		{
			get; set;
		}

		/// <summary>
		/// Light colour
		/// </summary>
		Color Colour
		{
			get; set;
		}
	}
}
