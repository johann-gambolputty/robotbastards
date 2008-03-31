
namespace Poc1.Universe.Interfaces
{
	public interface IPlanet : IEntity
	{
		IEntity Parent
		{
			get;
		}
	}
}
