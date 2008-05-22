using System;
using Poc1.Particles.Interfaces;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Handy abstract base class for particle systems
	/// </summary
	public class ParticleSystem : IParticleSystem
	{
		#region IParticleSystem Members

		/// <summary>
		/// Particle system centre
		/// </summary>
		public Matrix44 Frame
		{
			get { return m_Frame; }
		}

		/// <summary>
		/// Gets/sets the maximum number of particles. The default for this is 256
		/// </summary>
		public int MaximumNumberOfParticles
		{
			get { return m_Particles.MaximumSize; }
			set { m_Particles.MaximumSize = value; }
		}

		/// <summary>
		/// Enables/disables particle spawning.
		/// </summary>
		public bool EnableSpawning
		{
			get { return m_EnableSpawning; }
			set { m_EnableSpawning = value; }
		}

		/// <summary>
		/// Gets/sets the object used to create new particles, and destroy old particles
		/// </summary>
		public IParticleFactory ParticleFactory
		{
			get { return m_ParticleFactory; }
			set { m_ParticleFactory = value; }
		}

		/// <summary>
		/// Gets/sets the object used to determine the spawn rate for particles
		/// </summary>
		public ISpawnRate SpawnRate
		{
			get { return m_SpawnRate; }
			set { m_SpawnRate = value; }
		}

		/// <summary>
		/// The object that sets up the initial state of newly spawned particles
		/// </summary>
		public IParticleSpawner Spawner
		{
			get { return m_Spawner; }
			set { m_Spawner = value; }
		}

		/// <summary>
		/// The object that updates particle properties.
		/// </summary>
		public IParticleUpdater Updater
		{
			get { return m_Updater; }
			set { m_Updater = value; }
		}

		/// <summary>
		/// Gets/sets the object that determines the lifespan of particles. 
		/// </summary>
		public IParticleKiller Killer
		{
			get { return m_Killer; }
			set { m_Killer = value; }
		}

		/// <summary>
		/// Gets/sets the object that renders the particle system
		/// </summary>
		public IParticleRenderer Renderer
		{
			get { return m_Renderer; }
			set { m_Renderer = value; }
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders this particle system
		/// </summary>
		public void Render( IRenderContext context )
		{
			if ( Renderer == null )
			{
				return;
			}
			Renderer.RenderParticles( context, this, m_Particles );
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Updates this particle system
		/// </summary>
		protected virtual void Update( )
		{
			if ( !EnableSpawning )
			{
				return;
			}
			if ( Spawner != null )
			{
				int count = SpawnRate == null ? 0 : SpawnRate.GetNumberOfParticlesToSpawn( );
				Spawner.SpawnParticles( ParticleFactory, count, m_Particles );
			}
			if ( Killer != null )
			{
				Killer.KillParticles( ParticleFactory, m_Particles );
			}
			if ( Updater != null )
			{
				Updater.Update( this, m_Particles );
			}
		}

		#endregion

		#region Private Members

		#region ParticleSet class

		private class ParticleSet : System.Collections.IList
		{
			public int MaximumSize
			{
				get { return m_Particles.Length; }
				set
				{
					object[] particles = new object[ value ];
					int max = value < m_Particles.Length ? value : m_Particles.Length;
					for ( int i = 0; i < max; ++i )
					{
						if ( m_Particles[ i ] != null )
						{
							particles[ i ] = m_Particles[ i ];
						}
					}
					m_Particles = particles;
					if ( m_Count > value )
					{
						m_Count = value;
					}
				}
			}

			private int m_Count;
			private int m_AddPosition;
			private object[] m_Particles = new object[ 256 ];

			#region IList Members

			public int Add( object value )
			{
				int index = m_AddPosition++;
				m_Particles[ index ] = value;
				if ( m_Count < MaximumSize )
				{
					++m_Count;
				}
				return index;
			}

			public void Clear( )
			{
				for ( int i = 0; i < m_Particles.Length; ++i )
				{
					m_Particles[ i ] = null;
				}
				m_Count = 0;
			}

			public bool Contains( object value )
			{
				return Array.IndexOf( m_Particles, value ) != -1;
			}

			public int IndexOf( object value )
			{
				return Array.IndexOf( m_Particles, value );
			}

			public void Insert( int index, object value )
			{
				throw new NotSupportedException( );
			}

			public bool IsFixedSize
			{
				get { return true; }
			}

			public bool IsReadOnly
			{
				get { return false; }
			}

			public void Remove( object value )
			{
				int index = IndexOf( value );
				if ( index != -1 )
				{
					RemoveAt( index );
				}
			}

			public void RemoveAt( int index )
			{
				m_Particles[ index ] = null;
			}

			public object this[ int index ]
			{
				get { return m_Particles[ index ]; }
				set { m_Particles[ index ] = value; }
			}

			#endregion

			#region ICollection Members

			public void CopyTo( Array array, int index )
			{
				throw new NotSupportedException( );
			}

			public int Count
			{
				get { return m_Count; }
			}

			public bool IsSynchronized
			{
				get { return false; }
			}

			public object SyncRoot
			{
				get { return null; }
			}

			#endregion

			#region IEnumerable Members

			public System.Collections.IEnumerator GetEnumerator( )
			{
				throw new NotSupportedException( );
			}

			#endregion
		}

		#endregion

		private readonly ParticleSet	m_Particles			= new ParticleSet( );
		private readonly Matrix44		m_Frame				= new Matrix44( );
		private bool					m_EnableSpawning	= true;
		private IParticleFactory 		m_ParticleFactory	= ms_DefaultFactory;
		private IParticleSpawner		m_Spawner			= ms_DefaultSpawner;
		private ISpawnRate				m_SpawnRate			= ms_DefaultSpawnRate;
		private IParticleKiller			m_Killer			= ms_DefaultKiller;
		private IParticleUpdater 		m_Updater			= ms_DefaultUpdater;
		private IParticleRenderer		m_Renderer;

		private readonly static IParticleFactory	ms_DefaultFactory;
		private readonly static IParticleSpawner	ms_DefaultSpawner;
		private readonly static IParticleUpdater	ms_DefaultUpdater;
		private readonly static IParticleKiller		ms_DefaultKiller;
		private readonly static ISpawnRate			ms_DefaultSpawnRate;

		static ParticleSystem( )
		{
			ms_DefaultFactory	= null;
			ms_DefaultSpawner	= null;
			ms_DefaultUpdater	= null;
			ms_DefaultKiller	= null;
			ms_DefaultSpawnRate = new RandomSpawnRate( 1 );
		}

		#endregion

	}
}
