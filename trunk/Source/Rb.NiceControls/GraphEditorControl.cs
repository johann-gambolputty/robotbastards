using System.Windows.Forms;

namespace Rb.NiceControls
{
	public partial class GraphEditorControl : UserControl
	{
		public GraphEditorControl( )
		{
			InitializeComponent( );

			graphTypeComboBox.Items.Add( new PiecewiseLinearFunctionDescriptor( ) );
			graphTypeComboBox.SelectedIndex = 0;
		}

		private Control m_FunctionControl;

		private void graphTypeComboBox_SelectedIndexChanged( object sender, System.EventArgs e )
		{
			if ( graphTypeComboBox.SelectedItem == null )
			{
				graphPanel.Controls.Remove( m_FunctionControl );
				return;
			}
			m_FunctionControl = ( ( FunctionDescriptor )graphTypeComboBox.SelectedItem ).CreateControl( );
			m_FunctionControl.Dock = DockStyle.Fill;
			graphPanel.Controls.Add( m_FunctionControl );
		}
	}
}
