using System;
using Rb.Common.Controls.Graphs.Classes.Controllers;
using Rb.Common.Controls.Graphs.Classes.Renderers;
using Rb.Common.Controls.Graphs.Interfaces;
using Rb.Core.Maths;

namespace Rb.Common.Controls.Graphs.Classes.Sources
{
	/// <summary>
	/// Adapts an <see cref="IFunction1d"/> to provide data for a graph control
	/// </summary>
	public class GraphX2dSourceFunction1dAdapter : GraphX2dSourceAbstract
	{
		/// <summary>
		/// Sets the function to use as a graph source
		/// </summary>
		/// <param name="function">Graph data source</param>
		public GraphX2dSourceFunction1dAdapter( IFunction1d function )
		{
			if ( function == null )
			{
				throw new ArgumentNullException( "function" );
			}
			m_Function = function;
		}

		/// <summary>
		/// Gets the associated function
		/// </summary>
		public IFunction1d Function
		{
			get { return m_Function; }
		}

		#region IGraphX2dSource Members

		/// <summary>
		/// Creates a renderer for this source
		/// </summary>
		public override IGraph2dRenderer CreateRenderer( )
		{
			PiecewiseLinearFunction1d pwlFunction = Function as PiecewiseLinearFunction1d;
			if ( pwlFunction == null )
			{
				return base.CreateRenderer( );
			}
			return new Graph2dRendererList( new Graph2dPiecewiseLinear1dRenderer( ), new Graph2dControlPointRenderer( ) );
		}

		/// <summary>
		/// Creates a controller for this source
		/// </summary>
		public override IGraph2dController CreateController( )
		{
			PiecewiseLinearFunction1d pwlFunction = Function as PiecewiseLinearFunction1d;
			if ( pwlFunction == null )
			{
				return base.CreateController( );
			}
			return new GraphX2dControlPointController( );
		}

		/// <summary>
		/// Evaluates the graph function with input x
		/// </summary>
		public override float Evaluate( float x )
		{
			return Function.GetValue( x );
		}

		#endregion

		#region Private Members

		private readonly IFunction1d m_Function;

		#endregion
	}
}
