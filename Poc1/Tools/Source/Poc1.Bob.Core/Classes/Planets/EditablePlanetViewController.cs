
using System;
using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Core.Classes.Components;
using Poc1.Bob.Core.Interfaces;
using Poc1.Bob.Core.Interfaces.Components;
using Poc1.Core.Classes.Astronomical.Planets.Spherical.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical;
using Rb.Core.Components;

namespace Poc1.Bob.Core.Classes.Planets
{
	/// <summary>
	/// Controller for an editable planet view
	/// </summary>
	public class EditablePlanetViewController : EditableCompositeViewController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="workspace">Current workspace</param>
		/// <param name="viewFactory">View factory</param>
		/// <param name="view">View to control</param>
		/// <param name="planet">Planet composite to edit</param>
		public EditablePlanetViewController( IWorkspace workspace, IViewFactory viewFactory, IEditableCompositeView view, IComposite planet ) :
			base( workspace, viewFactory, view, planet )
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
				if ( Composite is ISpherePlanet )
				{
					return new ComponentType[]
						{
							new ComponentType( typeof( SpherePlanetRingModel ) )
						};
				}
				throw new NotSupportedException( string.Format( "Planet model type {0} is not supported", Composite.GetType( ) ) );
			}
		}
	}
}
