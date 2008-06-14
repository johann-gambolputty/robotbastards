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
		/// Adds this function to an existing control (created by a function of the same type)
		/// </summary>
		public override void AddToControl( Control control )
		{
			GraphControl graph = ( GraphControl )control;
			graph.AddGraph( CreateInputHandler( ) );
		}

		/// <summary>
		/// Removes this function from an existing control (created by a function of the same type)
		/// </summary>
		public override void RemoveFromControl( Control control )
		{
			GraphControl graph = ( GraphControl )control;
			graph.RemoveFunction( Function );
		}

		/// <summary>
		/// Creates a control for this function
		/// </summary>
		public override Control CreateControl( )
		{
			GraphControl control = new GraphControl( );
			control.AddGraph( CreateInputHandler( ) );
			return control;
		}

		#region Private Members

		/// <summary>
		/// Creates an input handler for the function, that can be added to graph controls
		/// </summary>
		private IGraphInputHandler CreateInputHandler( )
		{
			return new PiecewiseGraphInputHandler( ( PiecewiseLinearFunction1d )Function );
		} 

		#endregion
	}
}
