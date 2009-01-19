using Bob.Core.Ui.Interfaces;
using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Controls.Biomes;
using Poc1.Bob.Controls.Projects;
using Poc1.Bob.Controls.Rendering;
using Poc1.Bob.Controls.Waves;
using Poc1.Bob.Core.Classes;
using Poc1.Bob.Core.Classes.Biomes.Controllers;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Classes.Projects;
using Poc1.Bob.Core.Classes.Rendering;
using Poc1.Bob.Core.Classes.Waves;
using Poc1.Bob.Core.Interfaces;
using Poc1.Bob.Core.Interfaces.Biomes.Views;
using Poc1.Bob.Core.Interfaces.Projects;
using Poc1.Bob.Core.Interfaces.Rendering;
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
		/// Shows a create project view as a modal dialog
		/// </summary>
		public void ShowCreateProjectView( IWorkspace workspace, ProjectContext context, ProjectGroupContainer rootGroup )
		{
			ICreateProjectView view = new CreateProjectForm( );
			new CreateProjectController( workspace, context, view, rootGroup ).Show( );
		}

		/// <summary>
		/// Creates a view with a universe camera
		/// </summary>
		public IUniCameraView CreateUniCameraView( )
		{
			IUniCameraView view = new UniCameraViewControl( );
			new UniCameraViewController( view );
			return view;
		}

		/// <summary>
		/// Creates a project type selector view
		/// </summary>
		public IProjectTypeSelectorView CreateProjectTypeSelectorView( ProjectGroupContainer rootGroup )
		{
			IProjectTypeSelectorView view = new ProjectTypeSelectorView( );
			new ProjectTypeSelectorController( view, rootGroup );
			return view;
		}

		/// <summary>
		/// Creates a biome terrain texture view
		/// </summary>
		public IBiomeTerrainTextureView CreateBiomeTerrainTextureView( SelectedBiomeContext context )
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
		public IBiomeListView CreateBiomeListView( SelectedBiomeContext context, BiomeListModel model )
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
