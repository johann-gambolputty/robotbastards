using Poc1.Bob.Core.Classes.Biomes.Models;
using Rb.Core.Sets.Interfaces;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes
{
	/// <summary>
	/// Workspace service. Keeps track of the currently selected biome
	/// </summary>
	public class SelectedBiomeContext : IObjectSetService
	{
		/// <summary>
		/// Event raised when the selected biome is changed
		/// </summary>
		public event ActionDelegates.Action<BiomeModel> BiomeSelected;

		/// <summary>
		/// Gets/sets the currently selected biome
		/// </summary>
		public BiomeModel SelectedBiome
		{
			get { return m_SelectedBiome; }
			set
			{
				if ( m_SelectedBiome != value )
				{
					m_SelectedBiome = value;
					if ( BiomeSelected != null )
					{
						BiomeSelected( m_SelectedBiome );
					}
				}
			}
		}

		#region Private Members

		private BiomeModel m_SelectedBiome;

		#endregion
	}
}
