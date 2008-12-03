using System.Drawing;

namespace Rb.Common.Controls.Graphs.Interfaces
{
	/// <summary>
	/// Stores the transformation from data to screen space
	/// </summary>
	public class GraphTransform
	{
		/// <summary>
		/// Graph transform setup
		/// </summary>
		/// <param name="drawBounds">Drawing bounds</param>
		/// <param name="dataBounds">Data bounds</param>
		public GraphTransform( Rectangle drawBounds, RectangleF dataBounds )
		{
			m_DrawBounds = drawBounds;
			m_DataBounds = dataBounds;
		}

		/// <summary>
		/// Gets the drawing area
		/// </summary>
		public Rectangle DrawBounds
		{
			get { return m_DrawBounds; }
			set { m_DrawBounds = value; }
		}

		/// <summary>
		/// Gets the data area
		/// </summary>
		public RectangleF DataBounds
		{
			get { return m_DataBounds; }
			set { m_DataBounds = value; }
		}

		/// <summary>
		/// Gets the scale factor to transform x coordinates from screen to data space
		/// </summary>
		public float ScreenToDataXScale
		{
			get { return ( DataBounds.Width / DrawBounds.Width ); }
		}

		/// <summary>
		/// Gets the scale factor to transform y coordinates from screen to data space
		/// </summary>
		public float ScreenToDataYScale
		{
			get { return ( DataBounds.Height / DrawBounds.Height ); }
		}

		/// <summary>
		/// Transforms a screen point to a data point
		/// </summary>
		public PointF ScreenToData( Point pt )
		{
			float dataX = ( ( pt.X - DrawBounds.Left ) * ScreenToDataXScale ) + DataBounds.Left;
			float dataY = ( ( ( DrawBounds.Height - pt.Y ) - DrawBounds.Top ) * ScreenToDataYScale ) + DataBounds.Top;

			return new PointF( dataX, dataY );
		}

		/// <summary>
		/// Transforms a data point to a screen point
		/// </summary>
		public Point DataToScreen( PointF dataPt )
		{
			float screenX = ( dataPt.X - DataBounds.Left ) * ( DrawBounds.Width / DataBounds.Width ) + DrawBounds.Left;
			float screenY = DrawBounds.Height - ( ( ( dataPt.Y - DataBounds.Top ) * ( DrawBounds.Height / DataBounds.Height ) ) + DrawBounds.Top );

			return new Point( ( int )screenX, ( int )screenY );
		}

		#region Private Members

		private Rectangle m_DrawBounds;
		private RectangleF m_DataBounds;

		#endregion

	}
}
