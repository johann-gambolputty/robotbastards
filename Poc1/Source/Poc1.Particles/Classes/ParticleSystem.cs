using System;
using System.Collections.Generic;
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
		/// Enables/disables particle spawning.
		/// </summary>
		public bool EnableSpawning
		{
			get { return m_EnableSpawning; }
			set { m_EnableSpawning = value; }
		}

		/// <summary>
		/// Gets the buffer containing all the particles in this system
		/// </summary>
		public IParticleBuffer Buffer
		{
			get { return m_Buffer; }
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
			Update( );
			if ( Renderer == null )
			{
				return;
			}
			Renderer.RenderParticles( context, this );
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
				Spawner.SpawnParticles( this, count );
			}
			if ( Killer != null )
			{
				Killer.KillParticles( this );
			}
			if ( Updater != null )
			{
				Updater.Update( this, m_Particles );
			}

			//	Age all the particles
			foreach ( ParticleBase particle in m_Particles )
			{
				++particle.Age;
			}
		}

		#endregion

		#region Private Members

		private readonly Matrix44			m_Frame				= new Matrix44( );
		private bool						m_EnableSpawning	= true;
		private IParticleSpawner			m_Spawner			= ms_DefaultSpawner;
		private ISpawnRate					m_SpawnRate			= ms_DefaultSpawnRate;
		private IParticleKiller				m_Killer			= ms_DefaultKiller;
		private IParticleUpdater 			m_Updater			= ms_DefaultUpdater;
		private IParticleRenderer			m_Renderer;
		private readonly IParticleBuffer	m_Buffer			= new ParticleBuffer( );

		private readonly static IParticleSpawner	ms_DefaultSpawner;
		private readonly static IParticleUpdater	ms_DefaultUpdater;
		private readonly static IParticleKiller		ms_DefaultKiller;
		private readonly static ISpawnRate			ms_DefaultSpawnRate;

		static ParticleSystem( )
		{
			ms_DefaultSpawner	= null;
			ms_DefaultUpdater	= null;
			ms_DefaultKiller	= null;
			ms_DefaultSpawnRate = new RandomSpawnRate( 1 );
		}

		#endregion

	}
}
