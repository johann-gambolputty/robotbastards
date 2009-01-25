
namespace Poc1.Bob.Core.Interfaces.Biomes.Views
{
	/// <summary>
	/// Biome distribution view interface
	/// </summary>
	public interface IBiomeDistributionView
	{
	}

	/// <summary>
	/// Typed biome distribution view
	/// </summary>
	public interface IBiomeDistributionView<DistributionModel> : IBiomeDistributionView
	{
		/// <summary>
		/// Adds a distribution model to the view
		/// </summary>
		void AddDistribution( DistributionModel model );
		
		/// <summary>
		/// Removes a distribution model from the view
		/// </summary>
		void RemoveDistribution( DistributionModel model );

		/// <summary>
		/// Refreshes the distribution
		/// </summary>
		void RefreshDistribution( DistributionModel model );
	}
}
