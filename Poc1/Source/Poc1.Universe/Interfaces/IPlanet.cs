
namespace Poc1.Universe.Interfaces
{
	public interface IPlanet : IEntity
	{
		/// <summary>
		/// Gets the name of this planet
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// A planet orbits around another entity (sun, another planet)
		/// </summary>
		IEntity OrbitCentre
		{
			get;
		}
	}
}
