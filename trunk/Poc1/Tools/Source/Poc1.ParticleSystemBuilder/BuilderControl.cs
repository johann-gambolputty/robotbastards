using System;
using System.Windows.Forms;
using Poc1.Particles.Classes;

namespace Poc1.ParticleSystemBuilder
{

	public partial class BuilderControl : UserControl
	{
		public BuilderControl()
		{
			InitializeComponent();
		}

		private void BuilderControl_Load(object sender, EventArgs e)
		{
			spawnRateControlEditor.ControlTypes	= new Type[] { typeof( RandomSpawnRate ), typeof( SpawnRateFunction ) };
			spawnControlEditor.ControlTypes		= new Type[] { typeof( PointParticleSpawner ) };
			updateControlEditor.ControlTypes 	= new Type[] { typeof( NullParticleUpdater ) };
			killerControlEditor.ControlTypes 	= new Type[] { typeof( ParticleAgeKiller ) };
			renderControlEditor.ControlTypes 	= new Type[] { typeof( TestRenderer ) };
		}

		public class TestRenderer { };
	}
}
