using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;


namespace RbControls
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public class Display : System.Windows.Forms.UserControl
	{

		/// <summary>
		/// Accepts cursor keys
		/// </summary>
		protected override bool IsInputKey( Keys keyData )
		{
			if ( ( keyData == Keys.Up ) || ( keyData == Keys.Down ) || ( keyData == Keys.Left ) || ( keyData == Keys.Right ) )
			{
				return true;
			}
			return base.IsInputKey( keyData );
		}

		/// <summary>
		/// Override control creation parameters to support OpenGL requirements
		/// </summary>
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams result = base.CreateParams;

				if ( m_Context != null )
				{
					result.ClassStyle |= m_Context.ClassStyles;
				}

				return result;
			}
		}

		#region	Control designer properties
		
		/// <summary>
		/// Sets the colour depth of the control
		/// </summary>
		[ Category( "Rendering properties" ), Description( "Colour bits" ) ]
		public byte ColourBits
		{
			get
			{
				return m_ColourBits;
			}
			set
			{
				m_ColourBits = value;
			}
		}

		/// <summary>
		/// Sets the number of depth bits used by the control
		/// </summary>
		[ Category( "Rendering properties" ), Description( "Depth bits" ) ]
		public byte DepthBits
		{
			get
			{
				return m_DepthBits;
			}
			set
			{
				m_DepthBits = value;
			}
		}
		
		/// <summary>
		/// Sets the number of stencil bits used by the control
		/// </summary>
		[ Category( "Rendering properties" ), Description( "Stencil bits" ) ]
		public byte StencilBits
		{
			get
			{
				return m_StencilBits;
			}
			set
			{
				m_StencilBits = value;
			}
		}

		/// <summary>
		/// Sets continuous rendering. If ContinuousRendering is true, the control will be invalidated repeatedly by a rendering timer at 30fps
		/// </summary>
		[ Category( "Rendering properties" ), Description( "Sets control to always invalidate itself" ) ]
		public bool ContinuousRendering
		{
			get
			{
				return m_ContinuousRendering;
			}
			set
			{
				m_ContinuousRendering = value;
			}
		}

		#endregion


		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Creates a control rendering context, using RbEngine.Rendering.Renderer.CreateControlContext(). Sets up the control styles from that,
		/// then does the usual InitializeComponent() thing
		/// </summary>
		public Display()
		{
			if ( RbEngine.Rendering.Renderer.Exists )
			{
				m_Context = RbEngine.Rendering.Renderer.Inst.CreateControlContext( this );
			}

			if ( m_Context != null )
			{
				SetStyle( m_Context.AddStyles, true );
				SetStyle( m_Context.RemoveStyles, false );
			}

			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

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

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// Display
			// 
			this.Name = "Display";
			this.Size = new System.Drawing.Size(232, 200);
			this.Load += new System.EventHandler(this.Display_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Display_Paint);

		}
		#endregion


		#region	Private stuff

		private byte									m_StencilBits			= 0;
		private byte									m_DepthBits				= 24;
		private byte									m_ColourBits			= 32;
		private RbEngine.Rendering.ControlRenderContext	m_Context				= null;
		private System.Drawing.Image					m_DesignImage			= null;
		private bool									m_ContinuousRendering	= true;
		private bool									m_AlreadyInvalidated	= false;
		private Timer									m_RenderingTimer;

		#endregion

		private void Display_Load(object sender, System.EventArgs e)
		{
			if ( m_Context != null )
			{
				m_Context.Create( this, m_ColourBits, m_DepthBits, m_StencilBits );
			}
		}

		private void Display_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if ( m_Context == null )
			{
				e.Graphics.Clear( System.Drawing.Color.White );
				return;
			}

			if ( DesignMode )
			{
				if ( m_DesignImage == null )
				{
					m_DesignImage = m_Context.CreateDesignImage( );
				}

				e.Graphics.Clear( System.Drawing.Color.White );

				if ( m_DesignImage != null )
				{
					e.Graphics.DrawImage( m_DesignImage, new Point( 0, 0 ) );
				}

				e.Graphics.DrawRectangle( new System.Drawing.Pen( System.Drawing.Color.Black, 2 ), 1, 1, Width - 2, Height - 2 );
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
		/// Sets up the control rendering viewport
		/// </summary>
		protected void SetupViewport( )
		{
			RbEngine.Rendering.Renderer.Inst.SetViewport( 0, 0, Width, Height );
		}

		/// <summary>
		/// Begins rendering
		/// </summary>
		protected virtual bool BeginPaint( )
		{
			return m_Context.BeginPaint( this );
		}

		/// <summary>
		/// Draws the contents of the control
		/// </summary>
		protected virtual void Draw( )
		{
			SetupViewport( );

			RbEngine.Rendering.Renderer renderer = RbEngine.Rendering.Renderer.Inst;

			renderer.ClearVerticalGradient( System.Drawing.Color.LightSkyBlue, System.Drawing.Color.Black );
		}

		/// <summary>
		/// Ends rendering
		/// </summary>
		protected virtual void EndPaint( )
		{
			m_Context.EndPaint( );
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

	}
}
