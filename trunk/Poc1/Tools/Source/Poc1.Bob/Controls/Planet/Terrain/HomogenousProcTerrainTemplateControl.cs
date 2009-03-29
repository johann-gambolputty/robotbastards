using System;
using System.Windows.Forms;
using Poc1.Bob.Core.Interfaces.Planets.Terrain;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Poc1.Fast.Terrain;

namespace Poc1.Bob.Controls.Planet.Terrain
{
	public partial class HomogenousProcTerrainTemplateControl : UserControl, IHomogenousProcTerrainView
	{
		public HomogenousProcTerrainTemplateControl( )
		{
			InitializeComponent( );

			foreach ( TerrainFunctionType functionType in Enum.GetValues( typeof( TerrainFunctionType ) ) )
			{
				heightFunctionTypeComboBox.Items.Add( functionType );
				groundFunctionTypeComboBox.Items.Add( functionType );
			}
			heightFunctionTypeComboBox.SelectedItem = TerrainFunctionType.Flat;
			groundFunctionTypeComboBox.SelectedItem = TerrainFunctionType.Flat;
		}

		#region IHomogenousProcTerrainView Members

		/// <summary>
		/// Gets/sets the planet terrain model
		/// </summary>
		public IPlanetHomogenousProceduralTerrainTemplate Template
		{
			get { return m_Template; }
			set
			{
				m_Template = value;
				if ( m_Template == null )
				{
					heightFunctionPropertyGrid.SelectedObject	= null;
					groundFunctionPropertyGrid.SelectedObject	= null;
				}
				else
				{
					heightFunctionTypeComboBox.SelectedValue = m_Template.HeightFunction.FunctionType;
					heightFunctionPropertyGrid.SelectedObject = m_Template.HeightFunction.Parameters;

					if ( m_Template.GroundOffsetFunction == null )
					{
						groundFunctionTypeComboBox.SelectedValue = TerrainFunctionType.Flat;
					}
					else
					{
						groundFunctionTypeComboBox.SelectedValue = m_Template.GroundOffsetFunction.FunctionType;
						groundFunctionPropertyGrid.SelectedObject = m_Template.GroundOffsetFunction.Parameters;
					}

				}
			}
		}

		#endregion

		#region Private Members

		private IPlanetHomogenousProceduralTerrainTemplate m_Template;

		#region Event Handlers

		private void rebuildModelsButton_Click( object sender, EventArgs e )
		{
		}

		#endregion

		#endregion

	}
}
