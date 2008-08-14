using System;
using System.Windows.Forms;

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
		public float SeaLevel
		{
			get
			{
				float range = seaLevelTrackBar.Maximum - seaLevelTrackBar.Minimum;
				return BuilderState.TerrainMaxHeight * ( ( seaLevelTrackBar.Value - seaLevelTrackBar.Minimum ) / range );
			}
		}

		private void seaLevelTrackBar_Scroll( object sender, EventArgs e )
		{
			BuilderState.Instance.Planet.SeaLevel = SeaLevel;
		}
	}
}
