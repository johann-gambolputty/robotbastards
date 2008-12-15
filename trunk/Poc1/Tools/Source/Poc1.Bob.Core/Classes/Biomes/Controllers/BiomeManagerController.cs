using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Interfaces.Biomes;
using Poc1.Bob.Core.Interfaces.Biomes.Views;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Biomes.Controllers
{
	public class BiomeManagerController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="context">Biome selector context</param>
		/// <param name="view">Biome view</param>
		/// <exception cref="System.ArgumentNullException">Thrown if context or view is null</exception>
		public BiomeManagerController( ISelectedBiomeContext context, IBiomeManagerView view )
		{
			Arguments.CheckNotNull( context, "context" );
			context.BiomeSelected += OnBiomeSelected;

			m_View = view;

			new BiomeTerrainTextureController( context, view.TerrainTextureView );
		}

		#region Private Members

		private readonly IBiomeManagerView m_View;

		/// <summary>
		/// Called when a biome is selected
		/// </summary>
		/// <param name="selectedBiome">The selected biome</param>
		private void OnBiomeSelected( BiomeModel selectedBiome )
		{
			m_View.CurrentBiome = selectedBiome;
		}

		#endregion
	}
}
