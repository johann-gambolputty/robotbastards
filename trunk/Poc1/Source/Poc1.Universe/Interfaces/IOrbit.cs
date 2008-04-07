
namespace Poc1.Universe.Interfaces
{
	/// <summary>
	/// The orbit defines how an entity moves in relation to another celestial body
	/// </summary>
	public interface IOrbit
	{
		/// <summary>
		/// Updates the transform of an orbiting entity
		/// </summary>
		/// <param name="entity">Orbiting entity</param>
		/// <param name="updateTime">Update time, in ticks (<see cref="Rb.Core.Utils.TinyTime"/>)</param>
		void Update( IEntity entity, long updateTime );
	}
}
