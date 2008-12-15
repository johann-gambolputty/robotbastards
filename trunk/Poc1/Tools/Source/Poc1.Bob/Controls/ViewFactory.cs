
using Poc1.Bob.Controls.Biomes;
using Poc1.Bob.Controls.Waves;
using Poc1.Bob.Core.Classes.Biomes.Controllers;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Classes.Waves;
using Poc1.Bob.Core.Interfaces;
using Poc1.Bob.Core.Interfaces.Biomes;
using Poc1.Bob.Core.Interfaces.Biomes.Views;
using Poc1.Bob.Core.Interfaces.Waves;
using Poc1.Tools.Waves;

namespace Poc1.Bob.Controls
{
	/// <summary>
	/// Creates views
	/// </summary>
	public class ViewFactory : IViewFactory
	{
		/// <summary>
		/// Creates an wave animator view
		/// </summary>
		public IWaveAnimatorView CreateWaveAnimatorView( WaveAnimationParameters model )
		{
			IWaveAnimatorView view = new WaveAnimatorControl( );
			new WaveAnimatorController( view, model );
			return view;
		}

		/// <summary>
		/// Creates a biome list view
		/// </summary>
		public IBiomeListView CreateBiomeListView( ISelectedBiomeContext context, BiomeListModel model )
		{
			IBiomeListView view = new BiomeListViewControl( );
			new BiomeListController( context, model, view );
			return view;
		}

		/// <summary>
		/// Creates a biome manager view
		/// </summary>
		public IBiomeManagerView CreateBiomeManagerView( ISelectedBiomeContext context )
		{
			IBiomeManagerView view = new BiomeManagerControl( );
			new BiomeManagerController( context, view );
			return view;
		}
	}
}
