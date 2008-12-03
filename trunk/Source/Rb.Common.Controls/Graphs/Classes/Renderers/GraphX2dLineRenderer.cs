using System;
using System.Drawing;
using Rb.Common.Controls.Graphs.Interfaces;

namespace Rb.Common.Controls.Graphs.Classes.Renderers
{
	/// <summary>
	/// Renders a line graph
	/// </summary>
	public class GraphX2dLineRenderer : IGraph2dRenderer, IDisposable
	{
		/// <summary>
		/// Gets/sets the pen used to render the graph
		/// </summary>
		public Pen Pen
		{
			get { return m_Pen; }
			set { m_Pen = value; }
		}

		#region IGraph2dRenderer Members

		/// <summary>
		/// Gets/sets the principle colour of the graph
		/// </summary>
		public Color Colour
		{
			get { return m_Pen.Color; }
			set { m_Pen.Color = value; }
		}

		/// <summary>
		/// Renders graph data
		/// </summary>
		/// <param name="graphics">Graphics object to render into</param>
		/// <param name="transform">Graph view transform</param>
		/// <param name="data">Data to render</param>
		/// <param name="cursorDataPt">Position of the mouse cursor in data space</param>
		/// <param name="enabled">The enabled state of the graph control</param>
		public virtual void Render( IGraphCanvas graphics, GraphTransform transform, IGraph2dSource data, PointF cursorDataPt, bool enabled )
		{
			IGraphX2dSource eval = ( IGraphX2dSource )data;
			Pen pen = ( enabled && !data.Disabled ) ? m_Pen : s_DisabledPen;

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

			float dataX = transform.DataBounds.Left;
			float dataXInc = transform.DataBounds.Width / transform.DrawBounds.Width;

			Point lastPt = transform.DataToScreen( new PointF( dataX, eval.Evaluate( dataX ) ) ) ;
			dataX += dataXInc;

			for ( int drawX = 1; drawX < transform.DrawBounds.Width; ++drawX, dataX += dataXInc )
			{
				Point pt = transform.DataToScreen( new PointF( dataX, eval.Evaluate( dataX ) ) );
				graphics.DrawLine( pen, lastPt.X, lastPt.Y, pt.X, pt.Y );
				if ( enabled && shadowPen != null )
				{
					graphics.DrawLine( shadowPen, lastPt.X, lastPt.Y + 1, pt.X, pt.Y + 1 );
					graphics.DrawLine( shadowPen, lastPt.X, lastPt.Y - 1, pt.X, pt.Y - 1 );
				}
				lastPt = pt;
			}
		}

		#endregion

		#region Private Members

		private static Pen s_DisabledPen = new Pen( GraphUtils.ScaleColour( SystemColors.Control, 0.90f ), 2 );
		private static Pen s_HighlightedPen = new Pen( Color.Yellow );
		private static Pen s_SelectedPen = new Pen( Color.Black );

		private Pen m_Pen = new Pen( Color.Red, 2.0f );

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Disposes of this object
		/// </summary>
		public void Dispose( )
		{
			if ( m_Pen != null )
			{
				m_Pen.Dispose( );
				m_Pen = null;
			}
		}

		#endregion
	}
}
