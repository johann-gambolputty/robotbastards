using Poc1.Bob.Core.Interfaces.Planets;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Spherical.Models;
using Poc1.Universe.Interfaces.Planets.Spherical.Models.Templates;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Planets
{
	/// <summary>
	/// Sphere planet template view controller
	/// </summary>
	public class SpherePlanetTemplateViewController : PlanetTemplateViewController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="view">Planet view</param>
		/// <param name="template">Planetary template</param>
		/// <param name="model">Planet model instance</param>
		public SpherePlanetTemplateViewController( ISpherePlanetModelTemplateView view, ISpherePlanetModelTemplate template, ISpherePlanetModel model ) :
			base( view, template, model )
		{
			Arguments.CheckNotNull( view, "view" );
			Arguments.CheckNotNull( template, "template" );
			Arguments.CheckNotNull( model, "model" );
		//	m_Template = template;
			m_Model = model;
			view.ModelRadiusChanged += OnModelRadiusChanged;
		}

		#region Private Members

		//private readonly ISpherePlanetModelTemplate m_Template;
		private readonly ISpherePlanetModel m_Model;

		/// <summary>
		/// Handles the model radius changing in the view
		/// </summary>
		private void OnModelRadiusChanged( Units.Metres radius )
		{
			m_Model.Radius = radius;
		} 

		#endregion
	}
}
