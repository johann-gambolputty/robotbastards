using Poc1.Bob.Core.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Models.Templates;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Planets
{
	/// <summary>
	/// Planet template view conroller
	/// </summary>
	public class PlanetTemplateViewController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="view">Planet view</param>
		/// <param name="template">Planetary template</param>
		/// <param name="model">Planet model instance</param>
		public PlanetTemplateViewController( IPlanetModelTemplateView view, IPlanetModelTemplate template, IPlanetModel model )
		{
			Arguments.CheckNotNull( view, "view" );
			Arguments.CheckNotNull( template, "template" );
			Arguments.CheckNotNull( model, "model" );
			m_View = view;
			m_Template = template;
			m_Model = model;

			m_View.PlanetModelTemplate = template;
			m_View.PlanetModel = model;
		}

		#region Private Members

		private readonly IPlanetModelTemplateView m_View;
		private readonly IPlanetModelTemplate m_Template;
		private readonly IPlanetModel m_Model;
		
		#endregion
	}
}
