using System;
using System.ComponentModel;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using Rb.Rendering;

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
		[ Category( "Rendering properties" ), Description( "Sets control to invalidate itself after RenderInterval milliseconds" ) ]
		public bool ContinuousRendering
		{
			get { return m_ContinuousRendering; }
			set
			{
				m_ContinuousRendering = value;
				if ( !DesignMode )
				{
					if ( m_ContinuousRendering )
					{
						m_RenderingTimer.Start( );
					}
					else
					{
						m_RenderingTimer.Stop( );
					}
				}
			}
		}

		/// <summary>
		/// Continuous rendering interval
		/// </summary>
		[ Category( "Rendering properties" ), Description("Milliseconds between frame renders, if ContinuousRendering is true" ) ]
		public int RenderInterval
		{
			get { return m_RenderingTimer.Interval; }
			set { m_RenderingTimer.Interval = value; }
		}

		#endregion

		/// <summary>
		/// Display construciton
		/// </summary>
		public Display( )
		{
			if ( m_Setup != null )
			{
				SetStyle( m_Setup.AddStyles, true );
				SetStyle( m_Setup.RemoveStyles, false );
			}

			InitializeComponent( );

			m_RenderingTimer = new Timer( );
			m_RenderingTimer.Interval = 1;
			if ( ( m_ContinuousRendering ) && ( !DesignMode ) )
			{
				m_RenderingTimer.Tick		+= RenderTick;
				m_RenderingTimer.Enabled	= true;
				m_RenderingTimer.Start( );
			}
		}

		/// <summary>
		/// Overrides the control's class style parameters.
		/// </summary>
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ClassStyle = cp.ClassStyle | ( m_Setup == null ? 0 : m_Setup.ClassStyles );
				return cp;
			}
		}

		private void Display_Load( object sender, EventArgs e )
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
				if ( DesignMode )
				{
					if ( m_DesignImage == null )
					{
						m_DesignImage = Properties.Resources.Blessed;
					}

					e.Graphics.Clear( Color.White );

					if ( m_DesignImage != null )
					{
						e.Graphics.DrawImage( m_DesignImage, 0, 0, Width, Height );
					}

					e.Graphics.DrawRectangle( new Pen( Color.Black, 2 ), 1, 1, Width - 2, Height - 2 );
				}
				else
				{
					e.Graphics.Clear( Color.White );
				}
			}
			else
			{
				if ( BeginPaint( ) )
				{
					Draw( );
					EndPaint( );
				}

				m_AlreadyInvalidated = false;
			}
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
			if ( OnBeginPaint != null )
			{
				OnBeginPaint( this, null );
			}
			return m_Setup.BeginPaint( this );
		}

		/// <summary>
		/// Draws the contents of the control
		/// </summary>
		protected virtual void Draw( )
		{
			if ( m_Viewers.Count == 0 )
			{
				Graphics.Renderer.ClearDepth( 1.0f );
				Graphics.Renderer.ClearColourToVerticalGradient( Color.DarkSeaGreen, Color.Black );
			}
			else
			{
				foreach ( Viewer viewer in Viewers )
				{
					viewer.Render( );
				}
			}
		}

		/// <summary>
		/// Ends rendering
		/// </summary>
		protected virtual void EndPaint( )
		{
			m_Setup.EndPaint( this );
			if ( OnEndPaint != null )
			{
				OnEndPaint( this, null );
			}
		}

		public event EventHandler OnBeginPaint;
		public event EventHandler OnEndPaint;

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

        #region Viewers

		/// <summary>
		/// Gets the first viewer under the specified cursor position
		/// </summary>
		/// <param name="x">Cursor x position</param>
		/// <param name="y">Cursor y position</param>
		/// <returns>Returns the first viewer that contains (x,y), or null</returns>
		public Viewer GetViewerUnderCursor( int x, int y )
		{
			float fX = x / ( float )Width;
			float fY = y / ( float )Height;
			foreach ( Viewer viewer in m_Viewers )
			{
				if ( viewer.ViewRect.Contains( fX, fY ) )
				{
					return viewer;
				}
			}
			return null;
		}

        /// <summary>
        /// Gets the collection of viewers
        /// </summary>
	    public IEnumerable< Viewer > Viewers
	    {
	        get { return m_Viewers; }
	    }

        /// <summary>
        /// Adds a viewer
        /// </summary>
        /// <param name="viewer">Viewer to add</param>
        public void AddViewer( Viewer viewer )
        {
            m_Viewers.Add( viewer );
        	viewer.Control = this;
        }

        /// <summary>
        /// Removes a viewer
        /// </summary>
        /// <param name="viewer">Viewer to remove</param>
        public void RemoveViewer( Viewer viewer )
        {
            m_Viewers.Remove( viewer );
        	viewer.Control = null;
        }

        #endregion

        #region	Private stuff

        private byte 							m_StencilBits			= 0;
		private byte 							m_DepthBits				= 24;
		private byte 							m_ColourBits			= 32;
		private readonly IWindowsDisplaySetup	m_Setup					= CreateSetupData( );
		private Image							m_DesignImage			= null;
		private bool							m_ContinuousRendering	= true;
		private bool							m_AlreadyInvalidated	= false;
		private readonly List< Viewer > 		m_Viewers				= new List< Viewer >( );
		private readonly Timer					m_RenderingTimer;

		/// <summary>
		/// Creates setup data from the render factory, if it's available
		/// </summary>
		private static IWindowsDisplaySetup CreateSetupData( )
		{
			if ( Graphics.Factory != null )
			{
				return ( IWindowsDisplaySetup )Graphics.Factory.CreateDisplaySetup( );
			}
			return null;
		}

		#endregion

	}
}
