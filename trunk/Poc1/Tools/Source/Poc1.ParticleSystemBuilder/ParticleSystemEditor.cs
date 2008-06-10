using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Poc1.Particles.Classes;
using Poc1.Particles.Interfaces;
using Rb.Core.Utils;

namespace Poc1.ParticleSystemBuilder
{
	public partial class ParticleSystemEditor : UserControl
	{
		public ParticleSystemEditor( )
		{
			InitializeComponent( );
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

				m_ParticleSystem.Updater = updaterComponentsControl.BuildComponent<ParticleCompositeUpdater>( m_ParticleSystem.Updater );
				m_ParticleSystem.Killer = killerComponentsControl.BuildComponent<ParticleCompositeKiller>( m_ParticleSystem.Killer );
				m_ParticleSystem.Renderer = rendererComponentsControl.BuildComponent<ParticleCompositeRenderer>( m_ParticleSystem.Renderer );
			//	m_ParticleSystem.SpawnRate	= ( ISpawnRate )spawnRateComponentControl.ControlObject;
			//	m_ParticleSystem.Spawner	= ( IParticleSpawner )spawnControlEditor.ControlObject;
			//	m_ParticleSystem.Updater	= ( IParticleUpdater )updateControlEditor.ControlObject;
			//	m_ParticleSystem.Killer		= ( IParticleKiller )killerControlEditor.ControlObject;
			//	m_ParticleSystem.Renderer	= ( IParticleRenderer )renderControlEditor.ControlObject;
			}
		}
		private static Type[] GetComponentTypes( Type componentType )
		{
			Type[] types = AppDomainUtils.FindTypesImplementingInterface( componentType );
			List<Type> filtered = new List<Type>( );
			foreach ( Type type in types )
			{
				if ( type.IsAbstract || type.IsInterface )
				{
					continue;
				}
				object[] attributes = type.GetCustomAttributes( typeof( BrowsableAttribute ), true );
				if ( ( attributes.Length == 0 ) || ( ( ( BrowsableAttribute )attributes[ 0 ] ).Browsable ) )
				{
					filtered.Add( type );
				}
			}
			return filtered.ToArray( );
		}

		private IParticleSystem m_ParticleSystem;
		private readonly static Type[] ms_SpawnRateTypes;
		private readonly static Type[] ms_SpawnerTypes;
		private readonly static Type[] ms_UpdaterTypes;
		private readonly static Type[] ms_KillerTypes;
		private readonly static Type[] ms_RendererTypes;

		static ParticleSystemEditor( )
		{
			ms_SpawnRateTypes = GetComponentTypes( typeof( ISpawnRate ) );
			ms_SpawnerTypes = GetComponentTypes( typeof( IParticleSpawner ) );
			ms_UpdaterTypes = GetComponentTypes( typeof( IParticleUpdater ) );
			ms_KillerTypes = GetComponentTypes( typeof( IParticleKiller ) );
			ms_RendererTypes = GetComponentTypes( typeof( IParticleRenderer ) );
		}

		private void ParticleSystemEditor_Load( object sender, EventArgs e )
		{
			spawnRateComponentControl.Setup( ms_SpawnRateTypes, typeof( RandomSpawnRate ) );
			spawnRateComponentControl.SelectedControlChanged += delegate( object controller ) { ParticleSystem.SpawnRate = ( ISpawnRate )controller; };

			spawnerComponentControl.Setup( ms_SpawnerTypes, typeof( PointParticleSpawner ) );
			spawnerComponentControl.SelectedControlChanged += delegate( object controller ) { ParticleSystem.Spawner = ( IParticleSpawner )controller; };

			updaterComponentsControl.ComponentTypes = ms_UpdaterTypes;
			killerComponentsControl.ComponentTypes = ms_KillerTypes;
			rendererComponentsControl.ComponentTypes = ms_RendererTypes;
		}
	}
}
