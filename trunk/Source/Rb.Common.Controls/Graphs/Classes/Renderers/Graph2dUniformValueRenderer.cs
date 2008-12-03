
using System;
using System.Drawing;
using Rb.Common.Controls.Graphs.Classes.Controllers;
using Rb.Common.Controls.Graphs.Classes.Sources;
using Rb.Common.Controls.Graphs.Interfaces;

namespace Rb.Common.Controls.Graphs.Classes.Renderers
{
	/// <summary>
	/// Renderer for <see cref="Graph2dSourceUniformValue"/> graphs
	/// </summary>
	public class Graph2dUniformValueRenderer : IGraph2dRenderer
	{
		#region IGraph2dRenderer Members

		/// <summary>
		/// Gets/sets the graph colour
		/// </summary>
		public Color Colour
		{
			get { return m_Pen.Color; }
			set { m_Pen.Color = value; }
		}

		/// <summary>
		/// Renders a graph using this renderer
		/// </summary>
		public void Render( IGraphCanvas graphics, GraphTransform transform, IGraph2dSource data, PointF cursorDataPt, bool enabled )
		{
			Graph2dSourceUniformValue uniformData = ( Graph2dSourceUniformValue )data;

			if ( uniformData.FixedAxis == Graph2dSourceUniformValue.Axis.X )
			{
				float dataX = transform.DataToScreen( new PointF( uniformData.Value, 0 ) ).X;
				
				//	TODO: Need to 
				if ( Math.Abs( cursorDataPt.X - uniformData.Value ) < transform.ScreenToDataXScale * Graph2dUniformValueController.ScreenSelectionTolerance )
				{
					graphics.DrawLine( s_HighlightPen, dataX, transform.DrawBounds.Top, dataX, transform.DrawBounds.Bottom );
				}
				graphics.DrawLine( m_Pen, dataX, transform.DrawBounds.Top, dataX, transform.DrawBounds.Bottom );

			}
			else
			{
				float dataY = transform.DataToScreen( new PointF( 0, uniformData.Value ) ).Y;
				if ( Math.Abs( cursorDataPt.Y - uniformData.Value ) < transform.ScreenToDataYScale * Graph2dUniformValueController.ScreenSelectionTolerance )
				{
					graphics.DrawLine( s_HighlightPen, transform.DrawBounds.Left, dataY, transform.DrawBounds.Right, dataY );
				}
				graphics.DrawLine( m_Pen, transform.DrawBounds.Left, dataY, transform.DrawBounds.Right, dataY );
				
			}
		}

		#endregion

		#region Private Members

		private readonly Pen m_Pen = new Pen( Color.Red );

		private static readonly Pen s_HighlightPen = new Pen( Color.DarkSlateBlue, 4 );

		#endregion
	}
}
