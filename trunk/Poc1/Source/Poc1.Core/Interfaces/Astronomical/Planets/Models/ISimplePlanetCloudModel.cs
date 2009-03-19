
using Rb.Core.Maths;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Models
{
	/// <summary>
	/// Simple planet cloud model interface
	/// </summary>
	public interface ISimplePlanetCloudModel : IPlanetEnvironmentModel
	{
		/// <summary>
		/// Gets/sets the height of the cloud layer over sea level in metres
		/// </summary>
		Units.Metres CloudLayerHeight
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the cloud coverage range. 0 is clear skies, 1 is complete cover
		/// </summary>
		Range<float> CloudCoverRange
		{
			get; set;
		}
	}

}
