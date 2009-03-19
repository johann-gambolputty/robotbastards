namespace Poc1.Core.Interfaces.Astronomical
{
	/// <summary>
	/// Simple epicyclic orbit interface
	/// </summary>
	public interface IEpicyclicOrbit : IOrbit
	{
		/// <summary>
		/// The radius of the orbit around the orbital centre
		/// </summary>
		Units.Metres Radius
		{
			get;
		}
	}
}
