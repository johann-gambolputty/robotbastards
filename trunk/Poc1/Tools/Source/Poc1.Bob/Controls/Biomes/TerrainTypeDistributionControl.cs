using System.Windows.Forms;
using Poc1.Bob.Core.Interfaces.Biomes.Views;

namespace Poc1.Bob.Controls.Biomes
{
	public partial class TerrainTypeDistributionControl : UserControl, ITerrainTypeDistributionView
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public TerrainTypeDistributionControl( )
		{
			InitializeComponent( );
		}

	}
}
