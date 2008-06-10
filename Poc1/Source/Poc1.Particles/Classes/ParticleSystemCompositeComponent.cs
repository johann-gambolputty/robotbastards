
using System.Collections.Generic;
using Poc1.Particles.Interfaces;

namespace Poc1.Particles.Classes
{
	public class ParticleSystemCompositeComponent<T> : IParticleSystemCompositeComponent
		where T : IParticleSystemComponent
	{
		public void Add( IParticleSystemComponent component )
		{
			T componentT = ( T )component;
			if ( m_Particles != null )
			{
				componentT.Attach( m_Particles );
			}
			m_Components.Add( componentT );
		}

		public void Remove( IParticleSystemComponent component )
		{
			T componentT = ( T )component;
			m_Components.Remove( componentT );
		}

		public IEnumerable<IParticleSystemComponent> Components
		{
			get
			{
				foreach ( T updater in m_Components )
				{
					yield return updater;
				}
			}
		}
		
		#region IParticleSystemComponent Members

		public void Attach( IParticleSystem particles )
		{
			m_Particles = particles;
			foreach ( T component in m_Components )
			{
				component.Attach( particles );
			}
		}

		#endregion
		

		#region Private Members

		private IParticleSystem m_Particles;
		private readonly List<T> m_Components = new List<T>( );

		#endregion
	}
}
