using System.Windows.Forms;
using Rb.Core.Maths;
using Rb.NiceControls.Graph;

namespace Rb.NiceControls
{
	public class PiecewiseLinearFunctionDescriptor : FunctionDescriptor
	{
		public PiecewiseLinearFunctionDescriptor( ) :
			base( "Piecewise Linear", new LineFunction1d( ) )
		{
		}

		/// <summary>
		/// Returns true if the specified function is supported
		/// </summary>
		public override bool SupportsFunction( IFunction1d function )
		{
			return ( function is PiecewiseLinearFunction1d );
		}

		/// <summary>
		/// Creates a control for this function
		/// </summary>
		public override Control CreateControl( )
		{
			GraphControl control = new GraphControl( );
			control.Graph = new PiecewiseGraphInputHandler( ( PiecewiseLinearFunction1d  )Function );
			return control;
		}
	}
}
