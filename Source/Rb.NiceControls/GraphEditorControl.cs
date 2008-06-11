using System;
using System.Windows.Forms;
using Rb.Core.Maths;

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

		public IFunction1d Function
		{
			get { return m_Function; }
			set
			{
				if ( value == null )
				{
					throw new ArgumentNullException( "value" );
				}
				m_Function = value;
				
			}
		}

		private IFunction1d m_Function;
		private Control m_FunctionControl;

		private void graphTypeComboBox_SelectedIndexChanged( object sender, EventArgs e )
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
