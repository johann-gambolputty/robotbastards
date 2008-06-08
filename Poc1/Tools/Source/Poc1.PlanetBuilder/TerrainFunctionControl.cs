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

			heightFunctionComboBox.Items.Add( NewTerrainFunctionItem( TerrainFunctionType.SimpleFractal ) );
			heightFunctionComboBox.Items.Add( NewTerrainFunctionItem( TerrainFunctionType.RidgedFractal ) );
			heightFunctionComboBox.SelectedIndex = 0;

			groundFunctionComboBox.Items.Add( NewTerrainFunctionItem( TerrainFunctionType.SimpleFractal ) );
			groundFunctionComboBox.Items.Add( NewTerrainFunctionItem( TerrainFunctionType.RidgedFractal ) );
			groundFunctionComboBox.SelectedIndex = 0;
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
				m_TerrainFunction = functionType;
				m_Parameters = TerrainFunction.CreateParameters( functionType );
			}

			/// <summary>
			/// Returns the short name of the type
			/// </summary>
			public override string ToString( )
			{
				return TerrainFunction.Name( m_TerrainFunction );
			}

			/// <summary>
			/// Gets the stored type
			/// </summary>
			public object TerrainFunctionParameters
			{
				get { return m_Parameters; }
			}

			#region Private Members

			private readonly TerrainFunctionType m_TerrainFunction;
			private readonly object m_Parameters;

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

		private void heightFunctionComboBox_SelectedIndexChanged( object sender, EventArgs e )
		{
		//	heightFunctionPropertyGrid.SelectedObject = ( ( TerrainFunctionItem )heightFunctionComboBox.SelectedItem ).TerrainFunctionParameters;
			heightFunctionPropertyGrid.SelectedObject = heightFunctionComboBox.SelectedItem;
		}

		private void groundFunctionComboBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			groundFunctionPropertyGrid.SelectedObject = ( ( TerrainFunctionItem )groundFunctionComboBox.SelectedItem ).TerrainFunctionParameters;
		}

		private static void regenerateMeshButton_Click( object sender, EventArgs e )
		{
			BuilderState.Instance.TerrainMesh.RegenerateMesh( );
		}
	}
}
