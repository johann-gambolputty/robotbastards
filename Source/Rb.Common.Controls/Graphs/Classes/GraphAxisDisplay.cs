using System.Drawing;
using Rb.Common.Controls.Graphs.Interfaces;

namespace Rb.Common.Controls.Graphs.Classes
{
	/// <summary>
	/// Determines how a graph axis is displayed
	/// </summary>
	public class GraphAxisDisplay
	{
		#region Public Members

		/// <summary>
		/// Stringifies a graph axis value
		/// </summary>
		public delegate string ValueToStringDelegate( float value );

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="title">Axis title</param>
		public GraphAxisDisplay( string title )
		{
			m_Title = title;
		}

		/// <summary>
		/// Gets/sets the title of the axis
		/// </summary>
		public string Title
		{
			get { return m_Title; }
			set { m_Title = value; }
		}

		/// <summary>
		/// Gets/sets the title display flag
		/// </summary>
		public bool DisplayTitle
		{
			get { return m_DisplayTitle; }
			set { m_DisplayTitle = value; }
		}

		/// <summary>
		/// Gets/sets the vertical title flag
		/// </summary>
		public bool VerticalTitleText
		{
			get { return m_VerticalTitleText; }
			set { m_VerticalTitleText = value; }
		}

		/// <summary>
		/// Gets/sets the font used to render the graph title
		/// </summary>
		public Font TitleFont
		{
			get { return m_TitleFont; }
			set { m_TitleFont = value; }
		}

		/// <summary>
		/// Gets/sets the brush used to render the axis title
		/// </summary>
		public Brush TitleFontBrush
		{
			get { return m_TitleFontBrush; }
			set { m_TitleFontBrush = value; }
		}

		/// <summary>
		/// Gets/sets the font used to render tick values
		/// </summary>
		public Font TickFont
		{
			get { return m_TickFont; }
			set { m_TickFont = value; }
		}

		/// <summary>
		/// Gets/sets the brush used to render tick values
		/// </summary>
		public Brush TickFontBrush
		{
			get { return m_TickFontBrush; }
			set { m_TickFontBrush = value; }
		}

		/// <summary>
		/// Gets/sets the pen used to render graph lines
		/// </summary>
		public Pen LinePen
		{
			get { return m_LinePen; }
			set { m_LinePen = value; }
		}

		/// <summary>
		/// Gets/sets the pen used to render the line at graph value 0. If null, the line pen is used
		/// </summary>
		public Pen ZeroLinePen
		{
			get { return m_ZeroLinePen; }
			set { m_ZeroLinePen = value; }
		}

		public float LineStep
		{
			get { return m_LineStep; }
			set { m_LineStep = value; }
		}

		/// <summary>
		/// Gets/sets the delegate used to stringify axis values
		/// </summary>
		public ValueToStringDelegate ValueToString
		{
			get { return m_ValueToString; }
			set { m_ValueToString = value; }
		}

		/// <summary>
		/// Draws the axis horizontally
		/// </summary>
		public void DrawHorizontal( IGraphCanvas canvas, Rectangle drawBounds, RectangleF dataBounds )
		{
			if ( m_TickFont == null )
			{
				return;
			}
			float cellSize = m_LineStep;
			float dataX = ( int )( dataBounds.Left / cellSize ) * cellSize;
			float drawY = ( ( ( -dataBounds.Top / dataBounds.Height ) * drawBounds.Height ) + drawBounds.Top );
			if ( drawY < m_TickFont.Height )
			{
				drawY = m_TickFont.Height;
			}
			int widthInCells = ( int )( dataBounds.Width / cellSize ) + 1;
			for ( int cellX = 0; cellX < widthInCells; ++cellX )
			{
				float drawX = ( ( ( dataX - dataBounds.Left ) / dataBounds.Width ) * drawBounds.Width ) + drawBounds.Left;
				canvas.DrawText( m_TickFont, m_TickFontBrush, drawX, GraphUtils.InvY( drawY, drawBounds ), AxisValueToString( dataX ) );

				dataX += cellSize;
			}
		}
		
		/// <summary>
		/// Draws the axis vertically
		/// </summary>
		public void DrawVertical( IGraphCanvas canvas, Rectangle drawBounds, RectangleF dataBounds )
		{
			if ( m_TickFont == null )
			{
				return;
			}
			float cellSize = m_LineStep;
			float dataY = ( int )( dataBounds.Top / cellSize ) * cellSize;
			int heightInCells = ( int )( dataBounds.Height / cellSize ) + 1;
			
			float drawX = ( ( -dataBounds.Left / dataBounds.Width ) * drawBounds.Width ) + drawBounds.Left;
			if ( drawX < 2 )
			{
				drawX = 2;
			}
			for ( int cellY = 0; cellY < heightInCells; ++cellY )
			{
				float drawY = ( ( ( dataY - dataBounds.Top ) / dataBounds.Height ) * drawBounds.Height ) + drawBounds.Top;
				canvas.DrawText( m_TickFont, m_TickFontBrush, drawX, GraphUtils.InvY( drawY, drawBounds ), AxisValueToString( dataY ) );

				dataY += cellSize;
			}
		}
		
		/// <summary>
		/// Calls m_ValueToString, handling the case when it's null
		/// </summary>
		public string AxisValueToString( float value )
		{
			return m_ValueToString == null ? value.ToString( "G2" ) : m_ValueToString( value );
		}

		#endregion

		#region Private Members

		private string					m_Title;
		private bool					m_DisplayTitle;
		private bool					m_VerticalTitleText;
		private Font					m_TitleFont			= new Font( "Arial", 6 );
		private Brush					m_TitleFontBrush	= Brushes.DarkBlue;
		private Font					m_TickFont			= new Font( "Arial", 6 );
		private Brush					m_TickFontBrush		= Brushes.DarkBlue;
		private Pen						m_LinePen			= new Pen( Color.FromArgb( 0x20, 0x00, 0x00, 0xa0 ), 1.0f );
		private Pen						m_ZeroLinePen		= Pens.DarkBlue;
		private float					m_LineStep			= 1.0f;
		private ValueToStringDelegate	m_ValueToString;

		#endregion
	}
}
