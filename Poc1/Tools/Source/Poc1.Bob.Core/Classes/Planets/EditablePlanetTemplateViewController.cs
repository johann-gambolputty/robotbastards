using System;
using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Classes.Components;
using Poc1.Bob.Core.Interfaces;
using Poc1.Bob.Core.Interfaces.Components;
using Poc1.Core.Classes.Astronomical.Planets.Models.Templates;
using Poc1.Core.Classes.Astronomical.Planets.Spherical.Models.Templates;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Models;
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
		/// Action handler delegate
		/// </summary>
		public delegate void TemplateActionDelegate( IWorkspace workspace, object templateComponent );

		/// <summary>
		/// Setup constructor
		/// </summary>
		public EditablePlanetTemplateViewController( IWorkspace workspace, IViewFactory viewFactory, IEditableCompositeView view, IPlanetModelTemplate template, TemplateActionDelegate onAction ) :
			base( viewFactory, view, template )
		{
			Arguments.CheckNotNull( workspace, "workspace" );
			Arguments.CheckNotNull( onAction, "onAction" );
			view.ComponentSelected += OnComponentSelected;
			m_Workspace = workspace;
			m_ActionHandler = onAction;
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
							new ComponentType( typeof( SpherePlanetRingTemplate ) ),
							new ComponentType( typeof( PlanetHomogenousProceduralTerrainTemplate ) ), 
							new ComponentType( typeof( PlanetSimpleCloudTemplate ) ), 
							new ComponentType( typeof( PlanetAtmosphereScatteringTemplate ) ),
							new ComponentType( typeof( SpherePlanetOceanTemplate ) ), 
							new ComponentType( typeof( BiomeListModel ) ),
							new ComponentType( typeof( BiomeListLatitudeDistributionModel ) )
						};
				}
				throw new NotSupportedException( string.Format( "Planet model template type {0} is not supported", Composite.GetType( ) ) );
			}
		}

		#region Private Members

		private readonly IWorkspace m_Workspace;
		private readonly TemplateActionDelegate m_ActionHandler;

		/// <summary>
		/// Opens up a view on the specified component
		/// </summary>
		private void OnComponentSelected( object component )
		{
			m_ActionHandler( m_Workspace, component );
		}

		#endregion
	}

}
