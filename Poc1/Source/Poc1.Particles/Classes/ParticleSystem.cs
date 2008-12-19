using Poc1.Particles.Interfaces;
using Rb.Core.Maths;
using Rb.Core.Utils;
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
			set { m_Spawner = Attach( value ); }
		}

		/// <summary>
		/// The object that updates particle properties.
		/// </summary>
		public IParticleUpdater Updater
		{
			get { return m_Updater; }
			set { m_Updater = Attach( value ); }
		}

		/// <summary>
		/// Gets/sets the object that determines the lifespan of particles. 
		/// </summary>
		public IParticleKiller Killer
		{
			get { return m_Killer; }
			set { m_Killer = Attach( value ); }
		}

		/// <summary>
		/// Gets/sets the object that renders the particle system
		/// </summary>
		public IParticleRenderer Renderer
		{
			get { return m_Renderer; }
			set { m_Renderer = Attach( value ); }
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders this particle system
		/// </summary>
		public void Render( IRenderContext context )
		{
			Update( context.RenderTime );
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
		protected virtual void Update( long updateTimeInTicks )
		{
			float updateTime = 0;
			if ( m_LastUpdate != -1 )
			{
				updateTime = ( float )TinyTime.ToSeconds( updateTimeInTicks - m_LastUpdate );
			}

			using ( Buffer.Prepare( ) )
			{
				if ( EnableSpawning )
				{
					if ( ( Spawner != null ) && ( Buffer.MaximumNumberOfParticles > 0 ) )
					{
						int count = SpawnRate == null ? 0 : SpawnRate.GetNumberOfParticlesToSpawn( updateTime );
						Spawner.SpawnParticles( this, count );
					}
				}
				if ( Killer != null )
				{
					Killer.KillParticles( this );
				}
				if ( Updater != null )
				{
					Updater.Update( this, updateTime );
				}
			}
			m_LastUpdate = updateTimeInTicks;
		}

		#endregion

		#region Private Members

		private long						m_LastUpdate		= -1;
		private readonly Matrix44			m_Frame				= new Matrix44( );
		private bool						m_EnableSpawning	= true;
		private readonly IParticleBuffer	m_Buffer			= new SerialParticleBuffer( 1024 );
		private IParticleSpawner			m_Spawner;
		private ISpawnRate					m_SpawnRate;
		private IParticleKiller				m_Killer;
		private IParticleUpdater 			m_Updater;
		private IParticleRenderer			m_Renderer;

		private T Attach<T>( T component ) where T : class, IParticleSystemComponent
		{
			if ( component != null )
			{
				component.Attach( this );
			}
			return component;
		}

		#endregion

	}
}
