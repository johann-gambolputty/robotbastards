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

			spawnRateControlEditor.Setup( ms_SpawnRateTypes, typeof( RandomSpawnRate ) );
			spawnRateControlEditor.SelectedControlChanged += delegate( object controller ) { ParticleSystem.SpawnRate = ( ISpawnRate )controller; };

			spawnControlEditor.Setup( ms_SpawnerTypes, typeof( PointParticleSpawner ) );
			spawnControlEditor.SelectedControlChanged += delegate( object controller ) { ParticleSystem.Spawner = ( IParticleSpawner )controller; };

			updateControlEditor.Setup( ms_UpdaterTypes, typeof( NullParticleUpdater ) );
			updateControlEditor.SelectedControlChanged += delegate( object controller ) { ParticleSystem.Updater = ( IParticleUpdater )controller; };

			killerControlEditor.Setup( ms_KillerTypes, typeof( ParticleAgeKiller ) );
			killerControlEditor.SelectedControlChanged += delegate( object controller ) { ParticleSystem.Killer = ( IParticleKiller )controller; };

			renderControlEditor.Setup( ms_RendererTypes, typeof( ParticleDebugRenderer ) );
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
		
		private static Type[] ms_SpawnRateTypes;
		private static Type[] ms_SpawnerTypes;
		private static Type[] ms_UpdaterTypes;
		private static Type[] ms_KillerTypes;
		private static Type[] ms_RendererTypes;

		static void UpdateTypes( )
		{
			ms_SpawnRateTypes = GetComponentTypes( typeof( ISpawnRate ) );
			ms_SpawnerTypes = GetComponentTypes( typeof( IParticleSpawner ) );
			ms_UpdaterTypes = GetComponentTypes( typeof( IParticleUpdater ) );
			ms_KillerTypes = GetComponentTypes( typeof( IParticleKiller ) );
			ms_RendererTypes = GetComponentTypes( typeof( IParticleRenderer ) );
		}

		private static Type[] GetComponentTypes( Type componentType )
		{
			Type[] types = AppDomainUtils.FindTypesImplementingInterface( componentType );
			List<Type> filtered = new List<Type>( );
			foreach ( Type type in types )
			{
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
