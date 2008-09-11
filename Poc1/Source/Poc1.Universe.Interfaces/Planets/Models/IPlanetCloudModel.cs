
namespace Poc1.Universe.Interfaces.Planets.Models
{
	/// <summary>
	/// Planet cloud model
	/// </summary>
	public interface IPlanetCloudModel
	{
		/// <summary>
		/// Event, invoked when the cloud model changes
		/// </summary>
		event System.EventHandler ModelChanged;
	}
}
