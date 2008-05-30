namespace Poc1.Particles.Interfaces
{
	/// <summary>
	/// Interface for classes responsible for creating particles
	/// </summary>
	public interface IParticleFactory
	{
		/// <summary>
		/// Creates a single particle
		/// </summary>
		IParticle CreateParticle( );

		/// <summary>
		/// Destroys a single particles
		/// </summary>
		void DestroyParticle( IParticle particle );
	}
}
