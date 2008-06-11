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

			Function = new LineFunction1d( );
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
				foreach ( FunctionDescriptor descriptor in graphTypeComboBox.Items )
				{
					if ( descriptor.SupportsFunction( value ) )
					{
						m_Function = value;
						descriptor.Function = value;
						if ( graphTypeComboBox.SelectedItem != descriptor )
						{
							graphTypeComboBox.SelectedItem = descriptor;
						}
						else
						{
							CreateControl( descriptor );
						}
						return;
					}
				}
				throw new NotSupportedException( string.Format( "Function type \"{0}\" is not supported", value.GetType( ) ) );
			}
		}

		private IFunction1d m_Function;
		private Control m_FunctionControl;

		private void CreateControl( FunctionDescriptor descriptor )
		{
			if ( m_FunctionControl != null )
			{
				graphPanel.Controls.Remove( m_FunctionControl );
			}
			if ( descriptor != null )
			{
				m_FunctionControl = descriptor.CreateControl( );
				m_FunctionControl.Dock = DockStyle.Fill;
				graphPanel.Controls.Add( m_FunctionControl );
			}
		}

		private void graphTypeComboBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			CreateControl( ( FunctionDescriptor )graphTypeComboBox.SelectedItem );
		}
	}
}
