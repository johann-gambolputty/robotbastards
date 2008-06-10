using System.Windows.Forms;
using Rb.NiceControls.Graph;

namespace Rb.NiceControls
{
	public class PiecewiseLinearFunctionDescriptor : FunctionDescriptor
	{
		public PiecewiseLinearFunctionDescriptor( ) :
			base( "Piecewise Linear Function" )
		{
		}

		/// <summary>
		/// Creates a control for this function
		/// </summary>
		public override Control CreateControl( )
		{
			GraphControl control = new GraphControl( );
			control.Graph = new LineGraph( );
			return control;
		}
	}
}
