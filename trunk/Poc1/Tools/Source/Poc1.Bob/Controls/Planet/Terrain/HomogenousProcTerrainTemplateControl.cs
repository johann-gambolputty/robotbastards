using System;
using System.Windows.Forms;
using Poc1.Bob.Core.Interfaces.Planets.Terrain;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Poc1.Fast.Terrain;

namespace Poc1.Bob.Controls.Planet.Terrain
{
	public partial class HomogenousProcTerrainTemplateControl : UserControl, IHomogenousProceduralTerrainView
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

		#region IHomogenousProceduralTerrainView Members

		/// <summary>
		/// Event raised when the user requests rebuild
		/// </summary>
		public event EventHandler Rebuild;

		/// <summary>
		/// Gets/sets the planet terrain model
		/// </summary>
		public IPlanetHomogenousProceduralTerrainTemplate Template
		{
			get { return m_Template; }
			set
			{
				m_Template = value;
				UpdateControlFromCurrentTemplate( );
			}
		}

		#endregion

		#region Private Members

		private IPlanetHomogenousProceduralTerrainTemplate m_Template;

		/// <summary>
		/// Updates function type and function property grids from the current template
		/// </summary>
		private void UpdateControlFromCurrentTemplate( )
		{
			if ( m_Template == null || m_Template.HeightFunction == null )
			{
				heightFunctionTypeComboBox.SelectedValue = TerrainFunctionType.Flat;
				heightFunctionPropertyGrid.SelectedObject = null;
			}
			else
			{
				heightFunctionTypeComboBox.SelectedValue = m_Template.HeightFunction.FunctionType;
				heightFunctionPropertyGrid.SelectedObject = m_Template.HeightFunction.Parameters;
			}

			if ( m_Template == null || m_Template.GroundOffsetFunction == null )
			{
				groundFunctionTypeComboBox.SelectedValue = TerrainFunctionType.Flat;
				groundFunctionPropertyGrid.SelectedObject = null;
			}
			else
			{
				groundFunctionTypeComboBox.SelectedValue = m_Template.GroundOffsetFunction.FunctionType;
				groundFunctionPropertyGrid.SelectedObject = m_Template.GroundOffsetFunction.Parameters;
			}
		}

		#region Event Handlers

		private void rebuildModelsButton_Click( object sender, EventArgs e )
		{
			if ( Rebuild != null )
			{
				Rebuild( this, EventArgs.Empty );
			}
		}

		private void heightFunctionTypeComboBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			if ( m_Template == null )
			{
				return;
			}
			TerrainFunctionType heightFunctionType = heightFunctionTypeComboBox.SelectedItem == null ? TerrainFunctionType.Flat : ( TerrainFunctionType )heightFunctionTypeComboBox.SelectedItem;
			m_Template.HeightFunction = new TerrainFunction( heightFunctionType );

			UpdateControlFromCurrentTemplate( );
		}

		private void groundFunctionTypeComboBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			if ( m_Template == null )
			{
				return;
			}
			TerrainFunctionType groundFunctionType = groundFunctionTypeComboBox.SelectedItem == null ? TerrainFunctionType.Flat : ( TerrainFunctionType )groundFunctionTypeComboBox.SelectedItem;
			m_Template.GroundOffsetFunction = new TerrainFunction( groundFunctionType );

			UpdateControlFromCurrentTemplate( );
		}

		#endregion


		#endregion

	}
}
