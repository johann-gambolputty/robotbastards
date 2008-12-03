using System.Collections.Generic;
using System.Drawing;
using Rb.Common.Controls.Graphs.Classes.Controllers;
using Rb.Common.Controls.Graphs.Classes.Sources;
using Rb.Common.Controls.Graphs.Interfaces;
using Rb.Core.Maths;

namespace Rb.Common.Controls.Graphs.Classes.Renderers
{
	/// <summary>
	/// Renderer for <see cref="GraphX2dSourceFunction1dAdapter"/> graphs, where the
	/// function type is <see cref="PiecewiseLinearFunction1d"/>. Draws straight line
	/// segments between control points
	/// </summary>
	public class Graph2dPiecewiseLinear1dRenderer : IGraph2dRenderer
	{
		/// <summary>
		/// Gets/sets the flag that controls whether or not the rendered graph has endpoints
		/// </summary>
		public bool NoEndPoints
		{
			get { return m_NoEndPoints; }
			set { m_NoEndPoints = value; }
		}

		#region IGraph2dRenderer Members

		/// <summary>
		/// Gets/sets the renderer colour
		/// </summary>
		public Color Colour
		{
			get { return m_Pen.Color; }
			set
			{
				m_Pen.Color = value;
				m_Brush.Color = value;
			}
		}

		/// <summary>
		/// Renders the graph
		/// </summary>
		public void Render( IGraphCanvas graphics, GraphTransform transform, IGraph2dSource data, PointF cursorDataPt, bool enabled )
		{
			Pen pen = ( enabled && !data.Disabled ) ? m_Pen : s_DisabledPen;
			Pen shadowPen = GetShadowPen( data );

			GraphX2dSourceFunction1dAdapter cpData = ( GraphX2dSourceFunction1dAdapter )data;
			PiecewiseLinearFunction1d pwl = ( PiecewiseLinearFunction1d )cpData.Function;

			float tolerance = transform.ScreenToDataYScale * GraphX2dControlPointController.ScreenSelectionTolerance;
			int highlightCpIndex = IndexOfControlPointNearPoint( pwl.ControlPoints, new Point2( cursorDataPt.X, cursorDataPt.Y ), tolerance );

			Point2 cp = pwl[ 0 ];
			Point lastPt = transform.DataToScreen( new PointF( cp.X, cp.Y ) );
			if ( NoEndPoints )
			{
				Point edgePt = transform.DataToScreen( new PointF( transform.DataBounds.Left, cp.Y ) );
				DrawGraphLine( graphics, pen, shadowPen, enabled, lastPt, edgePt );
			}

			for ( int cpIndex = 1; cpIndex < pwl.NumControlPoints; ++cpIndex )
			{
				cp = pwl[ cpIndex ];
				Point pt = transform.DataToScreen( new PointF( cp.X, cp.Y ) );

				DrawGraphLine( graphics, pen, shadowPen, enabled, lastPt, pt );
				DrawControlPoint( graphics, lastPt, enabled ? m_Brush : s_DisabledBrush, highlightCpIndex == ( cpIndex - 1 ) );

				lastPt = pt;
			}
			
			if ( NoEndPoints )
			{
				Point edgePt = transform.DataToScreen( new PointF( transform.DataBounds.Right, cp.Y ) );
				DrawGraphLine( graphics, pen, shadowPen, enabled, lastPt, edgePt );
			}
			DrawControlPoint( graphics, lastPt, Brushes.Blue, highlightCpIndex == ( pwl.NumControlPoints - 1 ) );
		}

		#endregion

		#region Private Members

		private bool m_NoEndPoints = true;
		private readonly Pen m_Pen = new Pen( Color.Red, 2.0f );
		private readonly SolidBrush m_Brush = new SolidBrush( Color.Red );

		private static Pen s_DisabledPen = new Pen( GraphUtils.ScaleColour( SystemColors.Control, 0.90f ), 2 );
		private static Brush s_DisabledBrush = new SolidBrush( GraphUtils.ScaleColour( SystemColors.Control, 0.80f ) );
		private static Pen s_HighlightedPen = new Pen( Color.Yellow );
		private static Pen s_SelectedPen = new Pen( Color.Black );

		/// <summary>
		/// Draws a control point
		/// </summary>
		private static void DrawControlPoint( IGraphCanvas graphics, Point screenPt, Brush brush, bool highlight )
		{
			graphics.FillCircle( brush, screenPt.X, screenPt.Y, 2 );
			if ( highlight )
			{
				graphics.DrawCircle( s_HighlightedPen, screenPt.X, screenPt.Y, 4 );
			}
		}
		
		/// <summary>
		/// Draws a graph line
		/// </summary>
		private static void DrawGraphLine( IGraphCanvas graphics, Pen pen, Pen shadowPen, bool enabled, Point lastPt, Point pt )
		{
			graphics.DrawLine( pen, pt.X, pt.Y, lastPt.X, lastPt.Y );
			
			if ( enabled && shadowPen != null )
			{
				graphics.DrawLine( shadowPen, lastPt.X, lastPt.Y + 1, pt.X, pt.Y + 1 );
				graphics.DrawLine( shadowPen, lastPt.X, lastPt.Y - 1, pt.X, pt.Y - 1 );
			}
		}

		/// <summary>
		/// Gets the pen used to shade the graph lines
		/// </summary>
		private static Pen GetShadowPen( IGraph2dSource data )
		{
			Pen shadowPen = null;
			if ( !data.Disabled )
			{
				if ( data.Selected )
				{
					shadowPen = s_SelectedPen;
				}
				else if ( data.Highlighted )
				{
					shadowPen = s_HighlightedPen;
				}
			}
			return shadowPen;
		}

		/// <summary>
		/// Returns the index of the control point within tolerance distance of a given point
		/// </summary>
		private static int IndexOfControlPointNearPoint( IEnumerable<Point2> controlPoints, Point2 pt, float tolerance )
		{
			int cpIndex = 0;
			float tol2 = tolerance * tolerance;
			foreach ( Point2 controlPoint in controlPoints )
			{
				if ( controlPoint.SqrDistanceTo( pt ) < tol2 )
				{
					return cpIndex;
				}
				++cpIndex;
			}
			return -1;
		}

		#endregion
	}
}
