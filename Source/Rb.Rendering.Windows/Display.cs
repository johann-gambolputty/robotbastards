using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Rb.Rendering.Windows
{
	public partial class Display : UserControl
	{
		#region	Control designer properties
		
		/// <summary>
		/// Sets the colour depth of the control
		/// </summary>
		[ Category( "Rendering properties" ), Description( "Colour bits" ) ]
		public byte ColourBits
		{
			get { return m_ColourBits; }
			set { m_ColourBits = value; }
		}

		/// <summary>
		/// Sets the number of depth bits used by the control
		/// </summary>
		[ Category( "Rendering properties" ), Description( "Depth bits" ) ]
		public byte DepthBits
		{
			get { return m_DepthBits; }
			set { m_DepthBits = value; }
		}
		
		/// <summary>
		/// Sets the number of stencil bits used by the control
		/// </summary>
		[ Category( "Rendering properties" ), Description( "Stencil bits" ) ]
		public byte StencilBits
		{
			get { return m_StencilBits; }
			set { m_StencilBits = value; }
		}

		/// <summary>
		/// Sets continuous rendering. If ContinuousRendering is true, the control will be invalidated repeatedly by a rendering timer at 30fps
		/// </summary>
		[ Category( "Rendering properties" ), Description( "Sets control to always invalidate itself" ) ]
		public bool ContinuousRendering
		{
			get { return m_ContinuousRendering; }
			set { m_ContinuousRendering = value; }
		}

		#endregion

		/// <summary>
		/// Display construciton
		/// </summary>
		public Display( )
		{
			if ( Renderer.Exists )
			{
				m_Setup = ( IWindowsDisplaySetup )RenderFactory.Inst.CreateDisplaySetup( );
			}

			if ( m_Setup != null )
			{
				SetStyle( m_Setup.AddStyles, true );
				SetStyle( m_Setup.RemoveStyles, false );
			}

			InitializeComponent( );
			
			// TODO: Add any initialization after the InitComponent call

			if ( ( m_ContinuousRendering ) && ( !DesignMode ) )
			{
				m_RenderingTimer			= new Timer( );
				m_RenderingTimer.Tick		+= new EventHandler( RenderTick );
				m_RenderingTimer.Interval	= ( int )( 100.0f / 30.0f );
				m_RenderingTimer.Enabled	= true;
				m_RenderingTimer.Start( );
			}
		}

		private void Display_Load( object sender, System.EventArgs e )
		{
			if ( m_Setup != null )
			{
				m_Setup.Create( this, m_ColourBits, m_DepthBits, m_StencilBits );
			}
		}

		private void Display_Paint( object sender, PaintEventArgs e )
		{
			if ( m_Setup == null )
			{
				e.Graphics.Clear( Color.White );
				return;
			}

			if ( DesignMode )
			{
				if ( m_DesignImage == null )
				{
					m_DesignImage = m_Setup.CreateDesignImage( );
				}

				e.Graphics.Clear( Color.White );

				if ( m_DesignImage != null )
				{
					e.Graphics.DrawImage( m_DesignImage, new Point( 0, 0 ) );
				}

				e.Graphics.DrawRectangle( new Pen( Color.Black, 2 ), 1, 1, Width - 2, Height - 2 );
			}
			else
			{
				if ( BeginPaint( ) )
				{
					Draw( );
					EndPaint( );
				}
			}

			m_AlreadyInvalidated = false;
		}

		/// <summary>
		/// Does nothing - rendering of everything, including background, is done through the renderer
		/// </summary>
		protected override void OnPaintBackground( PaintEventArgs pevent )
		{
			//	Don't do anything
		}

		/// <summary>
		/// Begins rendering
		/// </summary>
		protected virtual bool BeginPaint( )
		{
			return m_Setup.BeginPaint( this );
		}

		/// <summary>
		/// Draws the contents of the control
		/// </summary>
		protected virtual void Draw( )
		{
			Renderer.Inst.ClearVerticalGradient( Color.LightSkyBlue, Color.Black );
		}

		/// <summary>
		/// Ends rendering
		/// </summary>
		protected virtual void EndPaint( )
		{
			m_Setup.EndPaint( this );
		}

		/// <summary>
		/// Rendering timer tick callback. Invalidates the control
		/// </summary>
		private void RenderTick( object sender, EventArgs args )
		{
			if ( !m_AlreadyInvalidated )
			{
				Invalidate( );
				m_AlreadyInvalidated = true;
			}
		}

		#region	Private stuff

		private byte 					m_StencilBits			= 0;
		private byte 					m_DepthBits				= 24;
		private byte 					m_ColourBits			= 32;
		private IWindowsDisplaySetup	m_Setup					= null;
		private Image					m_DesignImage			= null;
		private bool					m_ContinuousRendering	= true;
		private bool					m_AlreadyInvalidated	= false;
		private Timer					m_RenderingTimer;

		#endregion

	}
}
