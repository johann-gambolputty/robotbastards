using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Poc1.Particles.Classes;
using Poc1.Particles.Interfaces;
using Rb.Core.Utils;

namespace Poc1.ParticleSystemBuilder
{

	public partial class BuilderControl : UserControl
	{
		public BuilderControl( )
		{
			InitializeComponent( );

			spawnRateControlEditor.Setup( s_SpawnRateTypes, typeof( RandomSpawnRate ) );
			spawnRateControlEditor.SelectedControlChanged += delegate( object controller ) { ParticleSystem.SpawnRate = ( ISpawnRate )controller; };

			spawnControlEditor.Setup( s_SpawnerTypes, typeof( PointParticleSpawner ) );
			spawnControlEditor.SelectedControlChanged += delegate( object controller ) { ParticleSystem.Spawner = ( IParticleSpawner )controller; };

			updateControlEditor.Setup( s_UpdaterTypes, typeof( SimplePhysicsUpdater ) );
			updateControlEditor.SelectedControlChanged += delegate( object controller ) { ParticleSystem.Updater = ( IParticleUpdater )controller; };

			killerControlEditor.Setup( s_KillerTypes, typeof( ParticleAgeKiller ) );
			killerControlEditor.SelectedControlChanged += delegate( object controller ) { ParticleSystem.Killer = ( IParticleKiller )controller; };

			renderControlEditor.Setup( s_RendererTypes, typeof( ParticleDebugRenderer ) );
			renderControlEditor.SelectedControlChanged += delegate( object controller ) { ParticleSystem.Renderer = ( IParticleRenderer )controller; };
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
		
		private static Type[] s_SpawnRateTypes;
		private static Type[] s_SpawnerTypes;
		private static Type[] s_UpdaterTypes;
		private static Type[] s_KillerTypes;
		private static Type[] s_RendererTypes;

		static void UpdateTypes( )
		{
			s_SpawnRateTypes = GetComponentTypes( typeof( ISpawnRate ) );
			s_SpawnerTypes = GetComponentTypes( typeof( IParticleSpawner ) );
			s_UpdaterTypes = GetComponentTypes( typeof( IParticleUpdater ) );
			s_KillerTypes = GetComponentTypes( typeof( IParticleKiller ) );
			s_RendererTypes = GetComponentTypes( typeof( IParticleRenderer ) );
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

		static BuilderControl( )
		{
			UpdateTypes( );
		}

		private void BuilderControl_Load( object sender, EventArgs e )
		{
		}

	}
}
