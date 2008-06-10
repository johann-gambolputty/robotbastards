using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Rb.NiceControls.Graph
{
	public partial class GraphControl : UserControl, IGraphControl
	{
		#region Public Members
		
		public GraphControl( )
		{
			InitializeComponent( );
			Color dark = Color.LightSteelBlue;
			Color light = Color.White;

			m_Blends = new ColorBlend( 4 );
			m_Blends.Colors[ 0 ] = dark; m_Blends.Positions[ 0 ] = 0;
			m_Blends.Colors[ 1 ] = light; m_Blends.Positions[ 1 ] = 0.1f;
			m_Blends.Colors[ 2 ] = light; m_Blends.Positions[ 2 ] = 0.9f;
			m_Blends.Colors[ 3 ] = dark; m_Blends.Positions[ 3 ] = 1;

			m_GridColour = dark;

			Graph = new LineGraph( );
		}

		#region Graph margins and dimensions

		/// <summary>
		/// Gets/sets the margin at the left of the graph area
		/// </summary>
		public int GraphLeftMargin
		{
			get { return m_GraphLeftMargin; }
			set { m_GraphLeftMargin = value; }
		}

		/// <summary>
		/// Gets/sets the margin at the right of the graph area
		/// </summary>
		public int GraphRightMargin
		{
			get { return m_GraphRightMargin; }
			set { m_GraphRightMargin = value; }
		}

		/// <summary>
		/// Gets/sets the margin at the top of the graph area
		/// </summary>
		public int GraphTopMargin
		{
			get { return m_GraphTopMargin; }
			set { m_GraphTopMargin = value; }
		}

		/// <summary>
		/// Gets/sets the margin at the bottom of the graph area
		/// </summary>
		public int GraphBottomMargin
		{
			get { return m_GraphBottomMargin; }
			set { m_GraphBottomMargin = value; }
		}

		/// <summary>
		/// Gets the width of the graph (control width minus graph margins)
		/// </summary>
		public int GraphWidth
		{
			get { return Width - ( GraphLeftMargin + GraphRightMargin ); }
		}
		
		/// <summary>
		/// Gets the height of the graph (control height minus graph margins)
		/// </summary>
		public int GraphHeight
		{
			get { return Height - ( GraphTopMargin + GraphBottomMargin ); }
		}


		#endregion

		#region Subdivisions

		/// <summary>
		/// Gets/sets the number of horizontal subdivisions displayed on the graph
		/// </summary>
		public int HorizontalSubdivisions
		{
			get { return m_HorizontalSubdivisions; }
			set { m_HorizontalSubdivisions = value; }
		}

		/// <summary>
		/// Gets/sets the number of horizontal subdivisions displayed on the graph
		/// </summary>
		public int VerticalSubdivisions
		{
			get { return m_VerticalSubdivisions; }
			set { m_VerticalSubdivisions = value; }
		}

		#endregion

		/// <summary>
		/// Gets/sets the graph being edited by this control
		/// </summary>
		public IGraph Graph
		{
			get { return m_Graph; }
			set
			{
				if ( m_InputHandler != null )
				{
					m_InputHandler.Detach( this );
				}
				m_Graph = value;
				if ( m_Graph != null )
				{
					m_InputHandler = m_Graph.CreateInputHandler( );
					m_InputHandler.Attach( this );
				}
			}
		}

		#endregion

		#region Private Members
		
		private int					m_HorizontalSubdivisions	= 10;
		private int					m_VerticalSubdivisions		= 10;

		private int 				m_GraphLeftMargin			= 2;
		private int 				m_GraphRightMargin			= 2;
		private int 				m_GraphTopMargin			= 2;
		private int 				m_GraphBottomMargin			= 2;
		private readonly ColorBlend	m_Blends;
		private readonly Color		m_GridColour;
		private IGraph				m_Graph;
		private IGraphInputHandler	m_InputHandler;

		#endregion

		#region IGraphControl Members

		/// <summary>
		/// Maps a client y coordinate to a graph y coordinate
		/// </summary>
		public float ClientXToGraphX( int x )
		{
			float fX = ( x - GraphLeftMargin ) / ( float )GraphWidth;
			return fX;
		}

		/// <summary>
		/// Maps a client y coordinate to a graph y coordinate
		/// </summary>
		public float ClientYToGraphY( int y )
		{
			float fY = ( ( GraphHeight - y ) - GraphBottomMargin ) / ( float )GraphHeight;
			return fY;
		}

		/// <summary>
		/// Maps a client point to a graph point
		/// </summary>
		public PointF ClientToGraph( Point pt )
		{
			return new PointF( ClientXToGraphX( pt.X ), ClientYToGraphY( pt.Y ) );
		}

		/// <summary>
		/// Maps a graph point to a client point
		/// </summary>
		public PointF GraphToClient( float x, float y )
		{
			float cX = GraphLeftMargin + ( x * GraphWidth );
			float cY = GraphBottomMargin + ( ( 1 - y ) * GraphHeight );
			return new PointF( cX, cY );
		}

		/// <summary>
		/// Gets the underlying control
		/// </summary>
		public Control BaseControl
		{
			get { return this; }
		}

		#endregion

		#region Control Events

		private void GraphControl_Paint( object sender, PaintEventArgs e )
		{
			//	Draw the grid background
			Rectangle bounds = new Rectangle( 0, 0, Width, Height );
			if ( Enabled )
			{
				using ( LinearGradientBrush fillBrush = new LinearGradientBrush( bounds, Color.Black, Color.White, 90 ) )
				{
					fillBrush.InterpolationColors = m_Blends;
					e.Graphics.FillRectangle( fillBrush, bounds );
				}
			}
			else
			{
				e.Graphics.FillRectangle( SystemBrushes.Control, bounds );
			}

			//	Draw the grid lines
			using ( Pen pen = new Pen( Enabled ? m_GridColour : Color.LightGray, 1.0f ) )
			{
				float x = GraphLeftMargin;
				float incX = ( GraphWidth - 1 ) / ( ( float )HorizontalSubdivisions - 1 );
				for ( int col = 0; col < HorizontalSubdivisions; ++col, x += incX )
				{
					e.Graphics.DrawLine( pen, x, GraphLeftMargin, x, GraphHeight );
				}

				float y = GraphBottomMargin;
				float incY = ( GraphHeight - 1 ) / ( ( float )VerticalSubdivisions - 1 );
				for ( int row = 0; row < VerticalSubdivisions; ++row, y += incY )
				{
					e.Graphics.DrawLine( pen, GraphLeftMargin, y, GraphWidth, y );
				}
			}

			if ( !Enabled )
			{
				return;
			}
			if ( m_Graph != null )
			{
				Rectangle graphBounds = new Rectangle( GraphLeftMargin, GraphRightMargin, GraphWidth, GraphHeight );
				m_Graph.Render( graphBounds, e.Graphics );
			}
		}

		private void GraphControl_Load( object sender, System.EventArgs e )
		{
			DoubleBuffered = true;
		}

		#endregion

	}
}
