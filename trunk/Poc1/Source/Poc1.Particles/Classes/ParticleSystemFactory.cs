using Poc1.Particles.Interfaces;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Particle system factory class
	/// </summary>
	public class ParticleSystemFactory : IParticleSystemFactory
	{

		#region IParticleSystemFactory Members

		/// <summary>
		/// Creates a new inert particle system
		/// </summary>
		public IInertParticleSystem CreateInertParticleSystem( )
		{
			return new InertParticleSystem( );
		}

		#endregion
	}
}
