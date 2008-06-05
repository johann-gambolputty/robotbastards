
namespace Poc1.Particles.Interfaces
{
	/// <summary>
	/// Base interface for particle system components
	/// </summary>
	public interface IParticleSystemComponent
	{
		/// <summary>
		/// Called when this component is attached to a particle system
		/// </summary>
		void Attach( IParticleSystem particles );
	}
}
