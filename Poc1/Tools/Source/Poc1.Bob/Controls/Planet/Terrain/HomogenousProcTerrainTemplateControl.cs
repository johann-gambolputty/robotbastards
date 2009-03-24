using System.Windows.Forms;
using Poc1.Bob.Core.Interfaces.Planets.Terrain;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;

namespace Poc1.Bob.Controls.Planet.Terrain
{
	public partial class HomogenousProcTerrainTemplateControl : UserControl, IHomogenousProcTerrainView
	{
		public HomogenousProcTerrainTemplateControl( )
		{
			InitializeComponent( );
		}

		#region IHomogenousProcTerrainView Members

		/// <summary>
		/// Gets/sets the planet terrain model
		/// </summary>
		public IPlanetProcTerrainTemplate Template
		{
			get { return m_Template; }
			set
			{
				m_Template = value;
				terrainPropertyGrid.SelectedObject = m_Template;
			}
		}

		#endregion

		#region Private Members

		private IPlanetProcTerrainTemplate m_Template;

		#endregion
	}
}
