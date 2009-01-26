using System;
using System.Windows.Forms;
using Poc1.Universe.Interfaces;

namespace Poc1.PlanetBuilder
{
	public partial class OceanControl : UserControl
	{
		public OceanControl( )
		{
			InitializeComponent( );
		}

		/// <summary>
		/// Gets the height of the ocean
		/// </summary>
		public Units.Metres SeaLevel
		{
			get
			{
				float range = seaLevelTrackBar.Maximum - seaLevelTrackBar.Minimum;
				Units.Metres maximumHeight = BuilderState.Instance.Planet.PlanetModel.TerrainModel.MaximumHeight.ToMetres;
				return maximumHeight * ( ( seaLevelTrackBar.Value - seaLevelTrackBar.Minimum ) / range );
			}
		}

		private void seaLevelTrackBar_Scroll( object sender, EventArgs e )
		{
			BuilderState.Instance.Planet.PlanetModel.OceanModel.SeaLevel = SeaLevel;
		}
	}
}
