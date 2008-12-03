
using System;
using Rb.Common.Controls.Graphs.Classes.Renderers;
using Rb.Common.Controls.Graphs.Interfaces;

namespace Rb.Common.Controls.Graphs.Classes.Sources
{
	/// <summary>
	/// Basic implementation of <see cref="IGraphX2dSource"/>
	/// </summary>
	public abstract class GraphX2dSourceAbstract : Graph2dSourceAbstract, IGraphX2dSource
	{
		/// <summary>
		/// Creates a line renderer for the graph
		/// </summary>
		public override IGraph2dRenderer CreateRenderer( )
		{
			return new GraphX2dLineRenderer( );
		}

		#region IGraphX2dSource Members

		/// <summary>
		/// Checks if a point in data space hits the graph
		/// </summary>
		public override bool IsHit( float x, float y, float tolerance )
		{
			return Math.Abs( Evaluate( x ) - y ) < tolerance;
		}
		
		/// <summary>
		/// Gets the display value of the graph, when the data cursor is at (x,y)
		/// </summary>
		public override string GetDisplayValueAt( float x, float y )
		{
			return Evaluate( x ).ToString( "G4" );
		}

		/// <summary>
		/// Evaluates the graph function with input x
		/// </summary>
		public abstract float Evaluate( float x );

		#endregion
	}
}
