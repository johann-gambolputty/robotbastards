using System.Windows.Forms;
using Bob.Core.Ui.Interfaces;
using Bob.Core.Windows.Forms.Ui.Docking;
using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Core.Classes;
using Poc1.Bob.Core.Classes.Templates.Planets.Spherical;
using Poc1.Bob.Core.Interfaces;
using Poc1.Bob.Core.Interfaces.Views;
using Rb.Core.Utils;

namespace Poc1.Bob.Templates
{
	/// <summary>
	/// Adds dockable views to the SpherePlanetTemplate
	/// </summary>
	public class SpherePlanetDockingTemplate : SpherePlanetTemplate
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="viewFactory">View factory</param>
		public SpherePlanetDockingTemplate( IViewFactory viewFactory )
		{
			Arguments.CheckNotNull( viewFactory, "viewFactory" );
			m_ViewFactory = viewFactory;
			m_Views = new DockingViewInfo[]
				{
					new DockingViewInfo( "Atmosphere Template View", CreateAtmosphereTemplateView )
				};
		}

		/// <summary>
		/// Gets the views associated with this template
		/// </summary>
		public override IViewInfo[] Views
		{
			get { return m_Views; }
		}

		#region Private Members

		private readonly IViewFactory m_ViewFactory;
		private readonly DockingViewInfo[] m_Views;

		/// <summary>
		/// Creates an atmosphere template view control
		/// </summary>
		private Control CreateAtmosphereTemplateView( IWorkspace workspace )
		{
			return ( Control )m_ViewFactory.CreateBiomeListView( );
		}

		#endregion
	}
}
