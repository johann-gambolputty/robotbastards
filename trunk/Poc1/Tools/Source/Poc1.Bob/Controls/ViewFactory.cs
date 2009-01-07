
using Poc1.Bob.Controls.Biomes;
using Poc1.Bob.Controls.Templates;
using Poc1.Bob.Controls.Waves;
using Poc1.Bob.Core.Classes.Biomes.Controllers;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Classes.Templates;
using Poc1.Bob.Core.Classes.Waves;
using Poc1.Bob.Core.Interfaces;
using Poc1.Bob.Core.Interfaces.Biomes;
using Poc1.Bob.Core.Interfaces.Biomes.Views;
using Poc1.Bob.Core.Interfaces.Templates;
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
		/// Setup constructor
		/// </summary>
		/// <param name="uiProvider">Message UI provider. If null, no messages are displayed to the user</param>
		/// <remarks>
		/// All views created by this factory use the specified UI provider to display simple
		/// messages to the user.
		/// </remarks>
		public ViewFactory( IMessageUiProvider uiProvider )
		{
			m_UiProvider = uiProvider;
		}

		/// <summary>
		/// Shows a create template instance view as a modal dialog
		/// </summary>
		public void ShowCreateTemplateInstanceView( TemplateGroupContainer rootGroup )
		{
			ICreateTemplateInstanceView view = new CreateTemplateInstanceForm( );
			new CreateTemplateInstanceController( view, rootGroup );
		}

		/// <summary>
		/// Creates a template selector view
		/// </summary>
		public ITemplateSelectorView CreateTemplateSelectorView( TemplateGroupContainer rootGroup )
		{
			ITemplateSelectorView view = new TemplateSelectorView( );
			new TemplateSelectorController( view, rootGroup );
			return view;
		}

		/// <summary>
		/// Creates a biome terrain texture view
		/// </summary>
		public IBiomeTerrainTextureView CreateBiomeTerrainTextureView( ISelectedBiomeContext context )
		{
			IBiomeTerrainTextureView view = new BiomeTerrainTextureViewControl( );
			new BiomeTerrainTextureController( context, view );
			return view;
		}

		/// <summary>
		/// Creates an wave animator view
		/// </summary>
		public IWaveAnimatorView CreateWaveAnimatorView( WaveAnimationParameters model )
		{
			IWaveAnimatorView view = new WaveAnimatorControl( );
			new WaveAnimatorController( m_UiProvider, view, model );
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

		#region Private Members

		private readonly IMessageUiProvider m_UiProvider;

		#endregion

	}
}
