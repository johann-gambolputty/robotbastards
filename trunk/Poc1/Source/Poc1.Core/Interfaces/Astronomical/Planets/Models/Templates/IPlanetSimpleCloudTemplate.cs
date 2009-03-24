
using Rb.Core.Maths;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates
{
	/// <summary>
	/// Cloud model template for spherical planets
	/// </summary>
	/// <remarks>
	/// Defines a template from which planet models can be created
	/// </remarks>
	public interface IPlanetSimpleCloudTemplate : IPlanetEnvironmentModelTemplate
	{
		/// <summary>
		/// Gets the low cloud coverage range for the model template
		/// </summary>
		/// <remarks>
		/// This is the range of values that the least amount of cloud cover 
		/// over the planet can take. If the planet can be completely free
		/// of clouds, set the minimum of this range to 0.
		/// 
		/// When a planet model is instanced from this template, it will get
		/// a single cloud coverage range. The minimum of the model instance range
		/// will be randomly selected from this template's range.
		/// 
		/// </remarks>
		Range<float> LowCoverageRange
		{
			get; set;
		}

		/// <summary>
		/// Gets the high cloud coverage range for the model template
		/// </summary>
		/// <remarks>
		/// This is the range of values that the most amount of cloud cover 
		/// over the planet can take. If the planet can be completely covered
		/// by clouds, set the maximum of this range to 1.
		/// 
		/// When a planet model is instanced from this template, it will get
		/// a single cloud coverage range. The maximum of the model instance range
		/// will be randomly selected from this template's range.
		/// 
		/// </remarks>
		Range<float> HighCoverageRange
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the range of values that the models' cloud layer height can take
		/// </summary>
		Range<Units.Metres> CloudLayerHeightRange
		{
			get; set;
		}
	}
}
