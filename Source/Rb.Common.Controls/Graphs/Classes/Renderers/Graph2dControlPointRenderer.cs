using System.Drawing;
using Rb.Common.Controls.Graphs.Classes.Controllers;
using Rb.Common.Controls.Graphs.Classes.Sources;
using Rb.Common.Controls.Graphs.Interfaces;
using Rb.Core.Maths;

namespace Rb.Common.Controls.Graphs.Classes.Renderers
{
	/// <summary>
	/// Piecewise linear control point renderer
	/// </summary>
	public class Graph2dControlPointRenderer : IGraph2dRenderer
	{
		#region IGraph2dRenderer Members


		/// <summary>
		/// Gets/sets the principle colour of the graph
		/// </summary>
		public Color Colour
		{
			get { return m_Colour; }
			set
			{
				m_Colour = GraphUtils.ScaleColour( value, 0.1f );
				if ( m_CpBrush != null )
				{
					m_CpBrush.Dispose( );
				}
				m_CpBrush = new SolidBrush( m_Colour );
			}
		}

		/// <summary>
		/// Renders control points in a graph that derives from Graph2dPiecewiseLinear
		/// </summary>
		public void Render( IGraphCanvas graphics, GraphTransform transform, IGraph2dSource data, PointF cursorDataPt, bool enabled )
		{
			if ( data.Disabled )
			{
				return;
			}
			GraphX2dSourceFunction1dAdapter pwlGraph = ( GraphX2dSourceFunction1dAdapter )data;
			PiecewiseLinearFunction1d pwlFunc = ( PiecewiseLinearFunction1d )pwlGraph.Function;

			float tolerance = transform.ScreenToDataYScale * GraphX2dControlPointController.ScreenSelectionTolerance;
			int highlightCpIndex = pwlFunc.FindControlPoint( new Point2( cursorDataPt.X, cursorDataPt.Y ), tolerance );

			for ( int cpIndex = 0; cpIndex < pwlFunc.NumControlPoints; ++cpIndex )
			{
				Point2 cp = pwlFunc[ cpIndex ];
				PointF screenPt = transform.DataToScreen( new PointF( cp.X, cp.Y ) );
				graphics.FillCircle( m_CpBrush, screenPt.X, screenPt.Y, 2 );

				if ( cpIndex == highlightCpIndex )
				{
					graphics.DrawCircle( s_HighlightPen, screenPt.X, screenPt.Y, 4 );
				}
			}
		}

		#endregion

		#region Private Members

		private Color m_Colour;
		private SolidBrush m_CpBrush = new SolidBrush( Color.Black );

		private static Pen s_HighlightPen = new Pen( Color.Blue, 2.0f );

		#endregion
	}
}
