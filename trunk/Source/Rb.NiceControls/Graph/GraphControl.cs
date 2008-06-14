using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Rb.Core.Maths;
using Rectangle=System.Drawing.Rectangle;

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
		/// Gets/sets multiple graphs flag
		/// </summary>
		public bool AllowMultipleGraphs
		{
			get { return m_AllowMultipleGraphs; }
			set { m_AllowMultipleGraphs = value; }
		}

		/// <summary>
		/// Returns the set of graph input handlers attached to this control
		/// </summary>
		public IEnumerable<IGraphInputHandler> GraphInputHandlers
		{
			get { return m_Handlers; }
		}

		/// <summary>
		/// Removes all graphs from the control
		/// </summary>
		public void ClearGraphs( )
		{
			foreach ( IGraphInputHandler handler in m_Handlers )
			{
				handler.Detach( this );
			}
			m_Handlers.Clear( );
			Invalidate( );
		}

		/// <summary>
		/// Adds a graph input handler to the control
		/// </summary>
		public void AddGraph( IGraphInputHandler handler )
		{
			if ( handler == null )
			{
				throw new ArgumentNullException( "handler" );
			}
			if ( !AllowMultipleGraphs )
			{
				ClearGraphs( );
			}
			handler.Attach( this );
			m_Handlers.Add( handler );
			Invalidate( );
		}

		/// <summary>
		/// Removes a function from the control (must be wrapped in a <see cref="GraphInputHandler"/>)
		/// </summary>
		public void RemoveFunction( IFunction1d function )
		{
			for ( int handlerIndex = 0; handlerIndex < m_Handlers.Count; )
			{
				GraphInputHandler gHandler = m_Handlers[ handlerIndex ] as GraphInputHandler;
				if ( ( gHandler != null ) && ( gHandler.Function == function ) )
				{
					m_Handlers.RemoveAt( handlerIndex );
				}
				else
				{
					++handlerIndex;
				}
			}
			Invalidate( );
		}

		/// <summary>
		/// Removes a graph input handler from the control
		/// </summary>
		public void RemoveGraph( IGraphInputHandler handler )
		{
			if ( handler == null )
			{
				throw new ArgumentNullException( "handler" );
			}
			if ( m_Handlers.Contains( handler ) )
			{
				handler.Detach( this );
				m_Handlers.Remove( handler );
				Invalidate( );
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
		private bool				m_AllowMultipleGraphs;
		private readonly List<IGraphInputHandler> m_Handlers = new List<IGraphInputHandler>( );



		#endregion

		#region IGraphControl Members

		/// <summary>
		/// Event, invoked when the graph changes
		/// </summary>
		public event EventHandler GraphChanged;
		
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

		/// <summary>
		/// Invokes the GraphChanged event, and invalidated the control
		/// </summary>
		public void OnGraphChanged( )
		{
			if ( GraphChanged != null )
			{
				GraphChanged( this, null );
			}
			Invalidate( );
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
			Rectangle graphBounds = new Rectangle( GraphLeftMargin, GraphRightMargin, GraphWidth, GraphHeight );
			foreach ( IGraphInputHandler handler in m_Handlers )
			{
				handler.Render( graphBounds, e.Graphics );
			}
		}

		private void GraphControl_Load( object sender, EventArgs e )
		{
			DoubleBuffered = true;
		}

		#endregion

	}
}
