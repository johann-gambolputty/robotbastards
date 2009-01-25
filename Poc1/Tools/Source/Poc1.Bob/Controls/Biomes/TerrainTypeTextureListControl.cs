
using System.Windows.Forms;
using Poc1.Tools.TerrainTextures.Core;

namespace Poc1.Bob.Controls.Biomes
{
	public partial class TerrainTypeTextureListControl : TerrainTypeListControl
	{
		/// <summary>
		/// Initialises the control
		/// </summary>
		public TerrainTypeTextureListControl( )
		{
			InitializeComponent( );
		}

		#region Protected Members

		/// <summary>
		/// Creates a new control for a terrain type instance
		/// </summary>
		protected override Control CreateControlForTerrainType( TerrainType terrainType )
		{
			TerrainTypeTextureItemControl control = new TerrainTypeTextureItemControl( );
			control.TerrainType = terrainType;
			return control;
		}

		#endregion

	}
}
