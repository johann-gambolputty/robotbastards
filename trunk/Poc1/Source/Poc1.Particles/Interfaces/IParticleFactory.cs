namespace Poc1.Particles.Interfaces
{
	/// <summary>
	/// Interface for classes responsible for creating particles
	/// </summary>
	public interface IParticleFactory
	{
		/// <summary>
		/// Creates a number of particles
		/// </summary>
		object[] CreateParticles( int count );

		/// <summary>
		/// Destroys a number of particles
		/// </summary>
		void DestroyParticles( System.Collections.IEnumerable particles );
	}
}
