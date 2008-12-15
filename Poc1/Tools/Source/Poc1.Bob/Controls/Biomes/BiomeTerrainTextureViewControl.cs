using System.Windows.Forms;
using Poc1.Bob.Core.Interfaces.Biomes.Views;

namespace Poc1.Bob.Controls.Biomes
{
	public partial class BiomeTerrainTextureViewControl : UserControl, IBiomeTerrainTextureView
	{
		public BiomeTerrainTextureViewControl( )
		{
			InitializeComponent( );
		}

		#region IBiomeTerrainTextureView Members

		/// <summary>
		/// Gets the terrain types view
		/// </summary>
		public ITerrainTypeListView TerrainTypesView
		{
			get { return terrainTypeTextureListControl1; }
		}

		/// <summary>
		/// Gets the terrain type distributions over altitude
		/// </summary>
		public ITerrainTypeDistributionView AltitudeDistributionView
		{
			get { return altitudeDistributionControl; }
		}

		/// <summary>
		/// Gets the terrain type distributions over slope
		/// </summary>
		public ITerrainTypeDistributionView SlopeDistributionView
		{
			get { return slopeDistributionControl; }
		}

		#endregion
	}
}
