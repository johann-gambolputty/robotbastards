
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Interfaces.Biomes;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes
{
	/// <summary>
	/// Bob workspace
	/// </summary>
	public class Workspace : ISelectedBiomeContext
	{
		#region ISelectedBiomeContext Members

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

		#endregion

		#region Private Members

		private BiomeModel m_SelectedBiome;

		#endregion
	}
}
