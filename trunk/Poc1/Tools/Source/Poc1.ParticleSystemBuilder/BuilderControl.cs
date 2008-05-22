using System;
using System.Windows.Forms;
using Poc1.Particles.Classes;
using Poc1.Particles.Interfaces;

namespace Poc1.ParticleSystemBuilder
{

	public partial class BuilderControl : UserControl
	{
		public BuilderControl( )
		{
			InitializeComponent( );
			
			spawnRateControlEditor.ControlTypes	= new Type[] { typeof( RandomSpawnRate ), typeof( SpawnRateFunction ) };
			spawnControlEditor.ControlTypes		= new Type[] { typeof( PointParticleSpawner ) };
			updateControlEditor.ControlTypes 	= new Type[] { typeof( NullParticleUpdater ) };
			killerControlEditor.ControlTypes 	= new Type[] { typeof( ParticleAgeKiller ) };
			renderControlEditor.ControlTypes 	= new Type[] { typeof( ParticleDebugRenderer ) };
		}

		public IParticleSystem ParticleSystem
		{
			get { return m_ParticleSystem; }
			set
			{
				m_ParticleSystem = value;

				if ( m_ParticleSystem == null )
				{
					return;
				}
				m_ParticleSystem.SpawnRate	= ( ISpawnRate )spawnRateControlEditor.ControlObject;
				m_ParticleSystem.Spawner	= ( IParticleSpawner )spawnControlEditor.ControlObject;
				m_ParticleSystem.Updater	= ( IParticleUpdater )updateControlEditor.ControlObject;
				m_ParticleSystem.Killer		= ( IParticleKiller )killerControlEditor.ControlObject;
				m_ParticleSystem.Renderer	= ( IParticleRenderer )renderControlEditor.ControlObject;
			}
		}

		private IParticleSystem m_ParticleSystem;

		private void BuilderControl_Load( object sender, EventArgs e )
		{
		}

	}
}
