using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Interfaces.Biomes.Views;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Biomes.Controllers
{
	/// <summary>
	/// Biome distribution controller
	/// </summary>
	public class BiomeDistributionController
	{
		/// <summary>
		/// Biomes distribution controller
		/// </summary>
		public BiomeDistributionController( IBiomeDistributionView<BiomeLatitudeRangeDistribution> view, BiomeListLatitudeDistributionModel distributions )
		{
			Arguments.CheckNotNull( view, "view" );
			Arguments.CheckNotNull( distributions, "distributions" );

			m_Distributions = distributions;
			m_View = view;

			foreach ( BiomeLatitudeRangeDistribution distribution in m_Distributions.Distributions )
			{
				view.AddDistribution( distribution );
			}

			distributions.DistributionChanged += OnDistributionChanged;
			distributions.DistributionAdded += OnDistributionAdded;
			distributions.DistributionRemoved += OnDistributionRemoved;
		}

		#region Private Members

		private readonly IBiomeDistributionView<BiomeLatitudeRangeDistribution> m_View;
		private readonly BiomeListLatitudeDistributionModel m_Distributions;

		private void OnDistributionChanged( BiomeLatitudeRangeDistribution distribution )
		{
			m_View.RefreshDistribution( distribution );
		}

		private void OnDistributionAdded( BiomeLatitudeRangeDistribution distribution )
		{
			m_View.AddDistribution( distribution );
		}

		private void OnDistributionRemoved( BiomeLatitudeRangeDistribution distribution )
		{
			m_View.RemoveDistribution( distribution );
		}

		#endregion
	}
}
