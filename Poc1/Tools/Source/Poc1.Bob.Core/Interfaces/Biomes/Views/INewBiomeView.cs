namespace Poc1.Bob.Core.Interfaces.Biomes.Views
{
	/// <summary>
	/// Interface for a view used to specify initialization parameters for a biome
	/// </summary>
	public interface INewBiomeView
	{
		/// <summary>
		/// Gets the biome name
		/// </summary>
		string BiomeName
		{
			get;
		}

		/// <summary>
		/// Shows the view
		/// </summary>
		/// <returns>
		/// true if biome should be created
		/// </returns>
		bool ShowView( );
	}
}
