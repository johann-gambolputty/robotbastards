namespace Poc1.Particles.Interfaces
{
	/// <summary>
	/// Factory interface for particle systems
	/// </summary>
	public interface IParticleSystemFactory
	{
		IInertParticleSystem CreateInertParticleSystem( );
	}
}
