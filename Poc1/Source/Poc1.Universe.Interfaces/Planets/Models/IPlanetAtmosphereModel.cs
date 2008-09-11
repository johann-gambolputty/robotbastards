
namespace Poc1.Universe.Interfaces.Planets.Models
{
	/// <summary>
	/// Planet atmosphere model
	/// </summary>
	public interface IPlanetAtmosphereModel
	{
		/// <summary>
		/// Event, invoked when the atmosphere model changes
		/// </summary>
		event System.EventHandler ModelChanged;
	}
}
