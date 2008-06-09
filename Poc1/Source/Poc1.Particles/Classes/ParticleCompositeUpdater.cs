using System.Collections.Generic;
using System.ComponentModel;
using Poc1.Particles.Interfaces;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Composite updater
	/// </summary>
	[Browsable(false)]
	public class ParticleCompositeUpdater : IParticleUpdater
	{
		public void AddUpdater( IParticleUpdater updater )
		{
			if ( m_Particles != null )
			{
				updater.Attach( m_Particles );
			}
			m_Updaters.Add( updater );
		}

		public void RemoveUpdater( IParticleUpdater updater )
		{
			m_Updaters.Remove( updater );
		}

		#region IParticleUpdater Members

		public void Update( IParticleSystem ps )
		{
			foreach ( IParticleUpdater updater in m_Updaters )
			{
				updater.Update( ps );
			}
		}

		#endregion

		#region IParticleSystemComponent Members

		public void Attach( IParticleSystem particles )
		{
			m_Particles = particles;
			foreach ( IParticleUpdater updater in m_Updaters )
			{
				updater.Attach( particles );
			}
		}

		#endregion
		
		#region Private Members

		private IParticleSystem m_Particles;
		private readonly List<IParticleUpdater> m_Updaters = new List<IParticleUpdater>( );

		#endregion
	}
}
