using System;
using System.Windows.Forms;

using Poc1.Fast.Terrain;

namespace Poc1.PlanetBuilder
{
	public partial class TerrainFunctionControl : UserControl
	{
		public TerrainFunctionControl( )
		{
			InitializeComponent( );
		}

		#region Private Members

		#region TerrainFunctionItem Class

		private static TerrainFunctionItem NewTerrainFunctionItem( TerrainFunctionType functionType )
		{
			return new TerrainFunctionItem( functionType );
		}

		/// <summary>
		/// Wraps up a type, returning its short name from ToString(). Used in type combo-boxes
		/// </summary>
		private class TerrainFunctionItem
		{
			/// <summary>
			/// Type item setup constructor
			/// </summary>
			public TerrainFunctionItem( TerrainFunctionType functionType )
			{
				m_Function = new TerrainFunction( functionType );
			}

			/// <summary>
			/// Returns the short name of the type
			/// </summary>
			public override string ToString( )
			{
				return TerrainFunction.Name( m_Function.FunctionType );
			}

			/// <summary>
			/// Gets the stored terrain function's parameters
			/// </summary>
			public TerrainFunctionParameters Parameters
			{
				get { return m_Function.Parameters; }
			}

			/// <summary>
			/// Gets the stored terrain function
			/// </summary>
			public TerrainFunction Function
			{
				get { return m_Function; }
			}

			#region Private Members

			private readonly TerrainFunction m_Function;

			#endregion
		}

		#endregion

		#endregion

		#region Control Event Handlers

		private void groundOffsetEnableCheckBox_CheckedChanged( object sender, EventArgs e )
		{
			groundOffsetFunctionGroupBox.Enabled = groundOffsetEnableCheckBox.Checked;
		}

		#endregion

		private TerrainFunction CurrentHeightFunction
		{
			get
			{
				TerrainFunctionItem functionItem = ( ( TerrainFunctionItem )heightFunctionComboBox.SelectedItem );
				if ( functionItem == null )
				{
					return null;
				}
				return functionItem.Function;
			}
		}

		private TerrainFunction CurrentGroundFunction
		{
			get
			{
				if ( !groundOffsetEnableCheckBox.Checked )
				{
					return null;
				}
				TerrainFunctionItem functionItem = ( ( TerrainFunctionItem )groundFunctionComboBox.SelectedItem );
				if ( functionItem == null )
				{
					return null;
				}
				return functionItem.Function;
			}
		}

		private void heightFunctionComboBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			heightFunctionPropertyGrid.SelectedObject = ( ( TerrainFunctionItem )heightFunctionComboBox.SelectedItem ).Parameters;
		}

		private void groundFunctionComboBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			groundFunctionPropertyGrid.SelectedObject = ( ( TerrainFunctionItem )groundFunctionComboBox.SelectedItem ).Parameters;
		}

		private void regenerateMeshButton_Click( object sender, EventArgs e )
		{
			BuilderState.Instance.Planet.Terrain.SetupTerrain( BuilderState.TerrainMaxHeight, CurrentHeightFunction, CurrentGroundFunction );
			BuilderState.Instance.Planet.RegenerateTerrain( );
		}

		private void TerrainFunctionControl_Load( object sender, EventArgs e )
		{
			heightFunctionComboBox.Items.Add( NewTerrainFunctionItem( TerrainFunctionType.SimpleFractal ) );
			heightFunctionComboBox.Items.Add( NewTerrainFunctionItem( TerrainFunctionType.RidgedFractal ) );
			heightFunctionComboBox.SelectedIndex = 0;

			groundFunctionComboBox.Items.Add( NewTerrainFunctionItem( TerrainFunctionType.SimpleFractal ) );
			groundFunctionComboBox.Items.Add( NewTerrainFunctionItem( TerrainFunctionType.RidgedFractal ) );
			groundFunctionComboBox.SelectedIndex = 0;
		}
	}
}
