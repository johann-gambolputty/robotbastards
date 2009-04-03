
using Poc1.Bob.Core.Interfaces.Planets.Terrain;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Poc1.Core.Interfaces.Astronomical.Planets.Renderers;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Planets.Terrain
{
	/// <summary>
	/// Controller for an <see cref="IHomogenousProceduralTerrainView
	/// </summary>
	public class HomogenousProceduralTerrainViewController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="view">View to control</param>
		/// <param name="template">Terrain template</param>
		/// <param name="model">Terrain model</param>
		public HomogenousProceduralTerrainViewController( IHomogenousProceduralTerrainView view, IPlanetHomogenousProceduralTerrainTemplate template, IPlanetHomogenousProceduralTerrainModel model )
		{
			Arguments.CheckNotNull( view, "view" );
			Arguments.CheckNotNull( template, "template" );
			Arguments.CheckNotNull( model, "model" );

			view.Template = template;
			view.Rebuild += OnRebuild;
			m_Template = template;
			m_Model = model;
		}

		#region Private Members

		private readonly IPlanetHomogenousProceduralTerrainTemplate m_Template;
		private readonly IPlanetHomogenousProceduralTerrainModel m_Model;

		/// <summary>
		/// Called when the user requests a rebuild of the current model
		/// </summary>
		private void OnRebuild( object sender, System.EventArgs e )
		{
			m_Template.SetupInstance( m_Model, ModelTemplateInstanceContext.Default );

			//	TODO: AP: Ermm.... not sure...
			//		- Explicit refresh (as done here)
			//		- Renderer listens for model changes
			//		- Model invokes renderer refresh on change
			IPlanetHomogenousProceduralTerrainRenderer renderer = m_Model.Planet.Renderer.GetRenderer<IPlanetHomogenousProceduralTerrainRenderer>( );
			if ( renderer != null )
			{
				renderer.Refresh( );
			}
		}

		#endregion
	}
}
