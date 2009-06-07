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
		public EditablePlanetTemplateViewController( IWorkspace workspace, IViewFactory viewFactory, IEditableCompositeView view, IPlanetModelTemplate template ) :
			base( workspace, viewFactory, view, template )
		{
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
	}

}
