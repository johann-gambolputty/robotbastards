using System.ComponentModel;
using Poc1.Particles.Interfaces;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Composite updater
	/// </summary>
	[Browsable(false)]
	public class ParticleCompositeUpdater : ParticleSystemCompositeComponent<IParticleUpdater>, IParticleUpdater
	{
		#region IParticleUpdater Members

		public void Update( IParticleSystem ps, float updateTime )
		{
			foreach ( IParticleUpdater updater in Components )
			{
				updater.Update( ps, updateTime );
			}
		}

		#endregion
	}
}
