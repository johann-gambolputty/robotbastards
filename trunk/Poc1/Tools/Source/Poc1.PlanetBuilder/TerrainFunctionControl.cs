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

			heightFunctionComboBox.Items.Add( NewTerrainFunctionItem< SimpleFractalTerrainFunction >( ) );
		//	heightFunctionComboBox.Items.Add( NewTerrainFunctionItem< RidgedFractal >( ) );
			heightFunctionComboBox.SelectedIndex = 0;

			groundFunctionComboBox.Items.Add( NewTerrainFunctionItem<SimpleFractalTerrainFunction>( ) );
		//	groundFunctionComboBox.Items.Add( NewTerrainFunctionItem< RidgedFractal >( ) );
			groundFunctionComboBox.SelectedIndex = 0;
		}

		#region Private Members

		#region TerrainFunctionItem Class

		private static TerrainFunctionItem NewTerrainFunctionItem< T >( )
			where T : TerrainFunction
		{
			return new TerrainFunctionItem( typeof( T ) );
		}

		/// <summary>
		/// Wraps up a type, returning its short name from ToString(). Used in type combo-boxes
		/// </summary>
		private class TerrainFunctionItem
		{
			/// <summary>
			/// Type item setup constructor
			/// </summary>
			/// <param name="type">Encapsulated type</param>
			public TerrainFunctionItem( Type type )
			{
				m_TerrainFunction = ( TerrainFunction )Activator.CreateInstance( type );
				m_Parameters = m_TerrainFunction.CreateParameters( );
			}

			/// <summary>
			/// Returns the short name of the type
			/// </summary>
			public override string ToString( )
			{
				return m_TerrainFunction.Name;
			}

			/// <summary>
			/// Gets the stored type
			/// </summary>
			public object TerrainFunctionParameters
			{
				get { return m_Parameters; }
			}

			#region Private Members

			private readonly TerrainFunction m_TerrainFunction;
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
			heightFunctionPropertyGrid.SelectedObject = ( ( TerrainFunctionItem )heightFunctionComboBox.SelectedItem ).TerrainFunctionParameters;
		}

		private void groundFunctionComboBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			groundFunctionPropertyGrid.SelectedObject = ( ( TerrainFunctionItem )groundFunctionComboBox.SelectedItem ).TerrainFunctionParameters;
		}
	}
}
