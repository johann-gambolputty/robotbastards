using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Rb.Common.Controls.Graphs.Classes;
using Rb.Common.Controls.Graphs.Interfaces;
using Rb.Common.Controls.Utils;
using Rb.Core.Utils;

namespace Rb.Common.Controls.Forms.Graphs
{
	/// <summary>
	/// Graph control
	/// </summary>
	public partial class GraphControl : UserControl
	{
		/// <summary>
		/// Initializes the graph control
		/// </summary>
		public GraphControl( )
		{
			m_XAxis = new GraphAxisDisplay( "X" );
			m_XAxis.LinePen = m_GridPen;
			m_YAxis = new GraphAxisDisplay( "Y" );
			m_YAxis.LinePen = m_GridPen;
			InitializeComponent( );
			MouseWheel += GraphControl_MouseWheel;

			//	Set both default and current data window
			DefaultDataWindow = new RectangleF( -0.05f, -0.05f, 1.1f, 1.1f );

			Color dark = GraphUtils.ScaleColour( Color.LightSteelBlue, 0.9f );
			Color light = Color.White;

			m_Blends = new ColorBlend( 4 );
			m_Blends.Colors[ 0 ] = dark; m_Blends.Positions[ 0 ] = 0;
			m_Blends.Colors[ 1 ] = light; m_Blends.Positions[ 1 ] = 0.05f;
			m_Blends.Colors[ 2 ] = light; m_Blends.Positions[ 2 ] = 0.95f;
			m_Blends.Colors[ 3 ] = dark; m_Blends.Positions[ 3 ] = 1;
		}

		/// <summary>
		/// Gets the X axis setup
		/// </summary>
		[Browsable( true )]
		[TypeConverter( typeof( ExpandableStructConverter ) )]
		public GraphAxisDisplay XAxis
		{
			get { return m_XAxis; }
			set { m_XAxis = value; }
		}

		/// <summary>
		/// Gets the Y axis setup
		/// </summary>
		[Browsable( true )]
		[TypeConverter( typeof( ExpandableStructConverter ) )]
		public GraphAxisDisplay YAxis
		{
			get { return m_YAxis; }
			set { m_YAxis = value; }
		}

		/// <summary>
		/// Gets/sets the data area/control size relation flag. If enabled, the data area resizes when the control resizes
		/// </summary>
		[Browsable( true )]
		[Description( "If enabled, the data area resizes when the control resizes" )]
		public bool RelateControlSizeToDataArea
		{
			get { return m_RelateControlSizeToDataArea; }
			set { m_RelateControlSizeToDataArea = value; }
		}

		/// <summary>
		/// Gets/sets the data window
		/// </summary>
		[Browsable( true )]
		[TypeConverter( typeof( ExpandableStructConverter ) )]
		public RectangleF DataWindow
		{
			get { return m_Transform.DataBounds; }
			set { m_Transform.DataBounds = value; }
		}

		/// <summary>
		/// Gets/sets the default data window. When the user clicks on the default
		/// zoom button, the data window returns to this area
		/// </summary>
		/// <remarks>
		/// Setting the default data window changes the current data window to the same value.
		/// </remarks>
		[Browsable( true )]
		[TypeConverter( typeof( ExpandableStructConverter ) )]
		public RectangleF DefaultDataWindow
		{
			get { return m_DefaultDataBounds; }
			set
			{
				m_DefaultDataBounds = value;
				DataWindow = value;
			}
		}

		/// <summary>
		/// Gets the current graph transform
		/// </summary>
		[Browsable( false )]
		public GraphTransform Transform
		{
			get { return m_Transform; }
		}

		/// <summary>
		/// Gets the list of graph components being displayed by this control
		/// </summary>
		[Browsable( false )]
		public IEnumerable<GraphComponent> GraphComponents
		{
			get { return m_GraphComponents; }
		}

		/// <summary>
		/// Adds a graph component to the control
		/// </summary>
		public GraphComponent AddGraphComponent( GraphComponent component )
		{
			if ( component == null )
			{
				throw new ArgumentNullException( "component" );
			}
			m_GraphComponents.Add( component );
			component.Source.GraphChanged += delegate { Invalidate( ); };
			Invalidate( );
			return component;
		}

		/// <summary>
		/// Removes a graph component frmo the control
		/// </summary>
		/// <param name="component"></param>
		public void RemoveGraphComponent( GraphComponent component )
		{
			if ( component != null )
			{
				m_GraphComponents.Remove( component );
				Invalidate( );
			}
		}

		/// <summary>
		/// Shows the specified X position in the window
		/// </summary>
		public void ShowDataX( float x )
		{
			RectangleF dataBounds = m_Transform.DataBounds;
			dataBounds.X = x - dataBounds.Width;
			m_Transform.DataBounds = dataBounds;
			Invalidate( );
		}

		/// <summary>
		/// Moves the data window by a given vector
		/// </summary>
		public void MoveDataWindow( float deltaX, float deltaY )
		{
			RectangleF dataBounds = m_Transform.DataBounds;
			dataBounds.Offset( deltaX, deltaY );
			m_Transform.DataBounds = dataBounds;
			Invalidate( );
		}

		#region Private Members

		private bool							m_RelateControlSizeToDataArea;
		private readonly List<GraphComponent>	m_GraphComponents	= new List<GraphComponent>( );
		private Pen								m_BorderPen			= new Pen( Color.FromArgb( 0xa0, 0x00, 0x00, 0xa0 ), 2.0f );
		private Pen								m_GridPen			= new Pen( Color.FromArgb( 0x20, 0x00, 0x00, 0xa0 ), 1.0f );
		private GraphAxisDisplay				m_XAxis;
		private GraphAxisDisplay				m_YAxis;
		private Point							m_LastMousePos;
		private bool							m_MouseDownProcessed;
        private readonly ColorBlend				m_Blends;
		private RectangleF						m_DefaultDataBounds;
		private readonly GraphTransform			m_Transform			= new GraphTransform( new Rectangle( ),new RectangleF( 0, 0, 2, 2 )  );
		private bool							m_Invalidated;
		private Font							m_InfoFont			= new Font( "Courier New", 8 );
		private bool							m_DrawInfoBox;
		
		private static Pen 						ControlDarkPen		= new Pen( GraphUtils.ScaleColour( SystemColors.Control, 0.95f ) );
		private static Pen 						ControlLightPen		= new Pen( GraphUtils.ScaleColour( SystemColors.Control, 1.05f ) );

		/// <summary>
		/// Invalidates the control
		/// </summary>
		private new void Invalidate( )
		{
			//	TODO: AP: Probably unnecessary (only one wm_paint should be on the message queue at any one time anyway)
			if ( !m_Invalidated )
			{
				m_Invalidated = true;
				base.Invalidate( );
			}
		}

		#endregion

		#region Event Handlers

		private Pen ActiveGridPen
		{
			get { return Enabled ? m_GridPen : ControlLightPen; }
		}

		private Pen ActiveBorderPen
		{
			get { return Enabled ? m_BorderPen : ControlDarkPen; }
		}

		private Pen ActiveAxisPen
		{
			get { return Enabled ? Pens.DarkBlue : ControlDarkPen; }
		}

	//	private int m_InfoAreaWidth = 60;

		private Rectangle DataDisplayArea
		{
			get
			{
				Rectangle rect = DisplayRectangle;
				rect.Inflate( -3, -3 );
			//	rect.Inflate( -m_InfoAreaWidth / 2, -3 );
			//	rect.X += m_InfoAreaWidth / 2;
				return rect;
			}
		}

		private void DrawInfoBox( Graphics graphics, Font font, Point cursorPos, bool showAll )
		{
			if ( !m_DrawInfoBox )
			{
				return;
			}
			//	Measure the box
			const int maximumTitleWidth = 80;
			const int maximumValueWidth = 60;

			int width = maximumTitleWidth + maximumValueWidth + 4;
			int height = font.Height * ( 1 + m_GraphComponents.Count );

			PointF dataCursorPos = m_Transform.ScreenToData( cursorPos );

			int x = cursorPos.X + 20;
			int y = cursorPos.Y;

			if ( ( x + width ) > Width )
			{
				x = Width - width;
			}
			if ( ( y + height ) > Height )
			{
				y = Height - height;
			}

			Color darkColour = Color.FromArgb( 0xa0, Color.SteelBlue );
			Color lightColour = Color.FromArgb( 0xa0, Color.LightSteelBlue );
			Rectangle boxRect = new Rectangle( x - 2, y - 2, width + 4, height + 4 );
			using ( LinearGradientBrush brush = new LinearGradientBrush( boxRect, darkColour, lightColour, 90 ) )
			{
				graphics.FillRectangle( brush, boxRect );

				using ( LinearGradientBrush shadowBrush = new LinearGradientBrush( boxRect, GraphUtils.ScaleColour( darkColour, 0.5f ), GraphUtils.ScaleColour( lightColour, 0.5f ), 90 ) )
				{
					graphics.FillRectangle( shadowBrush, x - 4, y - 4, 2, height + 2 );
					graphics.FillRectangle( shadowBrush, x - 2, y - 4, width + 2, 2 );
				}
			}

			RectangleF titleLayoutRect = new RectangleF( x, y, maximumTitleWidth, font.Height );
			RectangleF valueLayoutRect = new RectangleF( x + maximumTitleWidth + 4, y, maximumValueWidth, font.Height );
			StringFormat format = new StringFormat( StringFormat.GenericDefault );
			format.Trimming = StringTrimming.EllipsisCharacter;
			float dataTolerance = m_Transform.ScreenToDataYScale * 4;

			graphics.DrawString( string.Format( "{0}: {1}, {2}: {3}", XAxis.Title, XAxis.AxisValueToString( dataCursorPos.X ), m_YAxis.Title, m_YAxis.AxisValueToString( dataCursorPos.Y ) ), font, Brushes.Black, titleLayoutRect.X, titleLayoutRect.Y );
			titleLayoutRect.Y += font.Height;
			valueLayoutRect.Y += font.Height;
			using ( Font boldFont = new Font( font, FontStyle.Bold ) )
			{
				foreach ( GraphComponent graphComponent in m_GraphComponents )
				{
					if ( !showAll )
					{
						if ( !graphComponent.Source.IsHit( dataCursorPos.X, dataCursorPos.Y, dataTolerance ) )
						{
							continue;
						}
					}
					Font useFont = graphComponent.Source.Highlighted ? boldFont : font;
					graphics.DrawString( graphComponent.Name, useFont, Brushes.Black, titleLayoutRect, format );
					graphics.DrawString( graphComponent.Source.GetDisplayValueAt( dataCursorPos.X, dataCursorPos.Y ), font, Brushes.Black, valueLayoutRect );
					titleLayoutRect.Y += font.Height;
					valueLayoutRect.Y += font.Height;
				}
			}
		}

		private void GraphControl_Paint( object sender, PaintEventArgs e )
		{
			m_Invalidated = false;
			m_Transform.DrawBounds = DataDisplayArea;

			//	If the data grid is effectively empty, then early out
			if ( m_Transform.DataBounds.Width < 0.00001f || m_Transform.DataBounds.Height < 0.00001f )
			{
				return;
			}

			GraphGraphicsCanvas canvas = new GraphGraphicsCanvas( e.Graphics );
			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

			//	Draw graph borders
			Rectangle bounds = DisplayRectangle;
			bounds.Inflate( -1, -1 );
			if ( bounds.Width <= 1 && bounds.Height <= 1 )
			{
				return;
			}
			using ( GraphicsPath path = DrawingHelpers.CreateRoundedRectanglePath( bounds, 4 ) )
			{
				using ( LinearGradientBrush fillBrush = new LinearGradientBrush( bounds, Color.Black, Color.White, 90 ) )
				{
					fillBrush.InterpolationColors = m_Blends;
					e.Graphics.FillPath( Enabled ? fillBrush : SystemBrushes.Control, path );
				}

				e.Graphics.DrawPath( ActiveBorderPen, path );
			}

			bounds = DataDisplayArea;
			e.Graphics.Clip = new Region( bounds );

			//	Draw graph grid
			GraphUtils.DrawGrid( canvas, ActiveGridPen, ActiveAxisPen, bounds, m_Transform.DataBounds, m_XAxis.LineStep, m_YAxis.LineStep );
			
			if ( Enabled )
			{
				using ( Pen pen = new Pen( Color.Gray, 1.0f ) )
				{
					pen.DashStyle = DashStyle.Dash;
					e.Graphics.DrawLine( pen, m_LastMousePos.X, 0, m_LastMousePos.X, Height );
					e.Graphics.DrawLine( pen, 0, m_LastMousePos.Y, Width, m_LastMousePos.Y );
				}
			}

			//	Draw attached graphs
			PointF lastCursorDataPos = m_Transform.ScreenToData( m_LastMousePos );
			foreach ( GraphComponent component in m_GraphComponents )
			{
				if ( component.Renderer != null && component.Source != null )
				{
					component.Renderer.Render( canvas, m_Transform, component.Source, lastCursorDataPos, Enabled );
				}
			}

			//	Draw graph axis
			if ( Enabled )
			{
				m_XAxis.DrawHorizontal( canvas, bounds, m_Transform.DataBounds );
				m_YAxis.DrawVertical( canvas, bounds, m_Transform.DataBounds );
			}

			DrawInfoBox( e.Graphics, m_InfoFont, m_LastMousePos, true );
		}

		private void GraphControl_Load( object sender, EventArgs e )
		{
			DoubleBuffered = true;
			((Bitmap)actualSizeButton.Image).MakeTransparent(Color.Magenta);
		}

		private void GraphControl_MouseMove( object sender, MouseEventArgs e )
		{
			PointF lastDataPoint = m_Transform.ScreenToData( m_LastMousePos );
			PointF dataPoint = m_Transform.ScreenToData( e.Location );

			if ( ( ( e.Button & MouseButtons.Left ) != 0 ) && !m_MouseDownProcessed )
			{
				float deltaX = lastDataPoint.X - dataPoint.X;
				float deltaY = lastDataPoint.Y - dataPoint.Y;
				MoveDataWindow( deltaX, deltaY );
			}

			foreach ( GraphComponent component in m_GraphComponents )
			{
				if ( component.Controller != null )
				{
					component.Controller.OnMouseMove( component.Source, m_Transform, lastDataPoint.X, lastDataPoint.Y, dataPoint.X, dataPoint.Y );
				}
				if ( component.Source.IsHit( dataPoint.X, dataPoint.Y, 0.1f ) )
				{
					component.Source.Highlighted = true;
				}
				else
				{
					component.Source.Highlighted = false;
				}
			}

			m_LastMousePos = e.Location;

			Invalidate( );
		}

		private void GraphControl_MouseWheel( object sender, MouseEventArgs e )
		{
			RectangleF dataBounds = m_Transform.DataBounds;
			float delta = Math.Max( dataBounds.Width, dataBounds.Height ) / 10;
			if ( e.Delta < 0 )
			{
				delta = -delta;
			}
			dataBounds.Inflate( delta, delta );
			if ( dataBounds.Width > 0 && dataBounds.Height > 0 )
			{
				m_Transform.DataBounds = dataBounds;
				Invalidate( );
			}
		}
		
		private void GraphControl_MouseDown( object sender, MouseEventArgs e )
		{
			PointF dataPoint = m_Transform.ScreenToData( e.Location );
			float tolerance = m_Transform.ScreenToDataYScale * 3;

			m_MouseDownProcessed = false;
			foreach ( GraphComponent component in m_GraphComponents )
			{
				if ( component.Controller != null )
				{
					if ( ( e.Button & MouseButtons.Left ) != 0 )
					{
						m_MouseDownProcessed |= component.Controller.OnMouseLeftButton( component.Source, m_Transform, dataPoint.X, dataPoint.Y, true );
					}
					else if ( ( e.Button & MouseButtons.Right ) != 0 )
					{
						m_MouseDownProcessed |= component.Controller.OnMouseRightButton( component.Source, m_Transform, dataPoint.X, dataPoint.Y, true );
					}
				}

				if ( ( e.Button & MouseButtons.Left ) != 0 )
				{
					component.Source.Selected = false;	//	TODO: If shift not held
					if ( component.Source.IsHit( dataPoint.X, dataPoint.Y, tolerance ) )
					{
						m_MouseDownProcessed = true;
						component.Source.Selected = !component.Source.Selected;
					}
					else
					{
						component.Source.Selected = false;
					}
				}

				if ( m_MouseDownProcessed )
				{
					break;
				}
			}
		}

		private void GraphControl_MouseUp( object sender, MouseEventArgs e )
		{
			PointF dataPoint = m_Transform.ScreenToData( e.Location );
			m_MouseDownProcessed = false;

			//	Send mouse button event to all graph controllers
			foreach ( GraphComponent component in m_GraphComponents )
			{
				if ( component.Controller != null )
				{
					if ( ( e.Button & MouseButtons.Left ) != 0 )
					{
						component.Controller.OnMouseLeftButton( component.Source, m_Transform, dataPoint.X, dataPoint.Y, false );
					}
					else if ( ( e.Button & MouseButtons.Right ) != 0 )
					{
						component.Controller.OnMouseRightButton( component.Source, m_Transform, dataPoint.X, dataPoint.Y, false );
					}
				}
			}
		}

		private void actualSizeButton_Click( object sender, EventArgs e )
		{
			//	Return to the default zoom value
			RectangleF src = m_Transform.DataBounds;
			m_Transform.DataBounds = new RectangleF( src.X, src.Y, DefaultDataWindow.Width, DefaultDataWindow.Height );
			Invalidate( );
		}
		private void GraphControl_Resize( object sender, EventArgs e )
		{
			if ( !RelateControlSizeToDataArea )
			{
				Invalidate( );
				return;
			}
			//	Change the size of the data area, by the change in size of the display area
			Rectangle oldDisplayArea = m_Transform.DrawBounds;
			Rectangle currentDisplayArea = DataDisplayArea;

			if ( oldDisplayArea.IsEmpty )
			{
				//	Control is resizing but has not yet been rendered
				return;
			}

			RectangleF dataBounds = m_Transform.DataBounds;
			dataBounds.Width += ( currentDisplayArea.Width - oldDisplayArea.Width ) * m_Transform.ScreenToDataXScale;
			dataBounds.Height += ( currentDisplayArea.Height - oldDisplayArea.Height ) * m_Transform.ScreenToDataYScale;
			m_Transform.DataBounds = dataBounds;
			Invalidate( );
		}

		private void GraphControl_MouseEnter( object sender, EventArgs e )
		{
			m_DrawInfoBox = true;
			Invalidate( );
		}

		private void GraphControl_MouseLeave( object sender, EventArgs e )
		{
			m_DrawInfoBox = false;
			Invalidate( );
		}

		#endregion

	}
}
