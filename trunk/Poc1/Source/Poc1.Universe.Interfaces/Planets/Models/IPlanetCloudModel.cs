
namespace Poc1.Universe.Interfaces.Planets.Models
{
	/// <summary>
	/// Planet cloud model
	/// </summary>
	public interface IPlanetCloudModel : IPlanetEnvironmentModel
	{
		/// <summary>
		/// Gets/sets the minimum height of the cloud layer
		/// </summary>
		Units.Metres CloudLayerMinHeight
		{
			get; set;
		}
	}
}
