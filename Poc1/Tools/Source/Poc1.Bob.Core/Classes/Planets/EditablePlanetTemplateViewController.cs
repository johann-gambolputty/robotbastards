using System;
using Poc1.Bob.Core.Classes.Components;
using Poc1.Bob.Core.Interfaces;
using Poc1.Bob.Core.Interfaces.Components;
using Poc1.Universe.Interfaces.Planets.Models.Templates;
using Poc1.Universe.Interfaces.Planets.Spherical.Models.Templates;
using Poc1.Universe.Planets.Spherical.Models.Templates;
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
		public EditablePlanetTemplateViewController( IViewFactory viewFactory, IEditableCompositeView view, IPlanetModelTemplate template ) :
			base( viewFactory, view, template )
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
							new ComponentType( typeof( SpherePlanetCloudModelTemplate ) ), 
							new ComponentType( typeof( SpherePlanetAtmosphereModelTemplate ) ), 
						};
				}
				throw new NotSupportedException( string.Format( "Planet model template type {0} is not supported", Composite.GetType( ) ) );
			}
		}
	}

}
