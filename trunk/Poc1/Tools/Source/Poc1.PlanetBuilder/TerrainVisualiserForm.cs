using System.Windows.Forms;
using Poc1.Universe.Interfaces.Planets.Models;

namespace Poc1.PlanetBuilder
{
	public partial class TerrainVisualiserForm : Form
	{
		public TerrainVisualiserForm( )
		{
			InitializeComponent( );
		}

		/// <summary>
		/// Gets/sets the terrain model visualised by this form
		/// </summary>
		public IPlanetTerrainModel TerrainModel
		{
			get { return terrainVisualiserControl1.TerrainModel; }	
			set { terrainVisualiserControl1.TerrainModel = value; }	
		}
	}
}