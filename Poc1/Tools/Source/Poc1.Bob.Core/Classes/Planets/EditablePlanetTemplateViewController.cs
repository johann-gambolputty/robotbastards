using System;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Classes.Components;
using Poc1.Bob.Core.Interfaces;
using Poc1.Bob.Core.Interfaces.Components;
using Poc1.Universe.Interfaces.Planets.Models.Templates;
using Poc1.Universe.Interfaces.Planets.Spherical.Models.Templates;
using Poc1.Universe.Planets.Models.Templates;
using Poc1.Universe.Planets.Spherical.Models.Templates;
using Rb.Core.Components;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Planets
{
	/// <summary>
	/// Extends <see cref="EditableCompositeViewController"/> to provide support for planet templates
	///</summary>
	public class EditablePlanetTemplateViewController : EditableCompositeViewController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public EditablePlanetTemplateViewController( IViewFactory viewFactory, IEditableCompositeView view, IPlanetModelTemplate template, IPlanetEnvironmentModelTemplateVisitor<bool> actionVisitor ) :
			base( viewFactory, view, template )
		{
			Arguments.CheckNotNull( actionVisitor, "actionVisitor" );
			view.ComponentAction += OnComponentAction;
			m_ActionVisitor = actionVisitor;
		}

		/// <summary>
		/// Gets the component types supported by this controller
		/// </summary>
		protected override ComponentType[] ComponentTypes
		{
			get
			{
				//	TODO: AP: Remove type hardcoding
				if ( Composite is ISpherePlanetModelTemplate )
				{
					return new ComponentType[]
						{
							new ComponentType( typeof( SpherePlanetCloudModelTemplate ) ), 
							new ComponentType( typeof( PlanetAtmosphereModelTemplate ) ),
							new ComponentType( typeof( SpherePlanetOceanModelTemplate ) ), 
							new ComponentType( typeof( BiomeListModel ) ),
							new ComponentType( typeof( BiomeListLatitudeDistributionModel ) )
						};
				}
				throw new NotSupportedException( string.Format( "Planet model template type {0} is not supported", Composite.GetType( ) ) );
			}
		}

		#region Private Members

		private readonly IPlanetEnvironmentModelTemplateVisitor<bool> m_ActionVisitor;

		/// <summary>
		/// Opens up a view on the specified component
		/// </summary>
		private void OnComponentAction( object component )
		{
			IPlanetEnvironmentModelTemplate modelTemplate = component as IPlanetEnvironmentModelTemplate;
			if ( modelTemplate == null )
			{
				return;
			}
			modelTemplate.InvokeVisit( m_ActionVisitor );
		}

		#endregion
	}

}
