
using System.Collections.Generic;
namespace Poc1.Particles.Interfaces
{
	public interface IParticleSystemCompositeComponent : IParticleSystemComponent
	{
		void Add( IParticleSystemComponent component );

		void Remove( IParticleSystemComponent component );

		IEnumerable<IParticleSystemComponent> Components
		{
			get;
		}
	}
}
