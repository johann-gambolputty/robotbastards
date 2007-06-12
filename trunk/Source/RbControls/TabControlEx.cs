using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RbControls
{
	public class TabControlEx : System.Windows.Forms.TabControl
	{
		private System.ComponentModel.IContainer components = null;

		public TabControlEx( )
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			Padding = new Point( Padding.X + 2, Padding.Y + 2 );
            DrawMode = TabDrawMode.OwnerDrawFixed;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		private int m_CloseButtonWidth	= 8;
		private int m_CloseButtonHeight = 8;

		private int m_MouseOverIndex = -1;

		private const int kTabXOffset = 2;
		private const int kTabYOffset = 2;

		private Rectangle GetCloseButtonRect( int tabIndex )
		{
			Rectangle pageTabRect = GetTabRect( tabIndex );
			return new Rectangle( pageTabRect.Left, pageTabRect.Top, m_CloseButtonWidth, m_CloseButtonHeight );
		}

		protected void DrawCloseButton( TabPage page, DrawItemEventArgs args )
		{
			System.Drawing.Pen pen		= System.Drawing.SystemPens.ControlDark;
			System.Drawing.Pen hiPen	= m_MouseOverIndex == args.Index ? System.Drawing.SystemPens.ControlDarkDark : System.Drawing.SystemPens.ControlLight;

			Rectangle closeButtonRect	= GetCloseButtonRect( args.Index );
			closeButtonRect.Offset( kTabXOffset, kTabYOffset );

			args.Graphics.DrawLine( hiPen, closeButtonRect.Left, closeButtonRect.Top, closeButtonRect.Right, closeButtonRect.Bottom );
			args.Graphics.DrawLine( hiPen, closeButtonRect.Right, closeButtonRect.Top, closeButtonRect.Left, closeButtonRect.Bottom );

			closeButtonRect.Offset( 1, 0 );
			args.Graphics.DrawLine( pen, closeButtonRect.Left, closeButtonRect.Top, closeButtonRect.Right, closeButtonRect.Bottom );
			args.Graphics.DrawLine( pen, closeButtonRect.Right, closeButtonRect.Top, closeButtonRect.Left, closeButtonRect.Bottom );
		}

		protected override void OnMouseMove( MouseEventArgs e )
		{
			base.OnMouseMove( e );

			int mouseOverIndex = -1;
			for ( int index = 0; index < TabPages.Count; ++index )
			{
				Rectangle closeButtonRect = GetCloseButtonRect( index );
				if ( closeButtonRect.Contains( e.X, e.Y ) )
				{
					mouseOverIndex = index;
					break;
				}
			}

			if ( m_MouseOverIndex != mouseOverIndex )
			{
				Invalidate( );
				m_MouseOverIndex = mouseOverIndex;
			}
		}

		protected override void OnMouseDown( MouseEventArgs e )
		{
			base.OnMouseDown( e );
			if ( m_MouseOverIndex != -1 )
			{
				TabPages.RemoveAt( m_MouseOverIndex );
			}
		}



		protected override void OnDrawItem( DrawItemEventArgs args )
		{
			base.OnDrawItem( args );

			TabPage		page		= TabPages[ args.Index ];
			Rectangle	pageTabRect	= GetTabRect( args.Index );

			DrawCloseButton( page, args );

			System.Drawing.Brush textBrush = System.Drawing.SystemBrushes.ControlText;
			args.Graphics.DrawString( page.Text, Font, textBrush, pageTabRect.Left + m_CloseButtonWidth + kTabXOffset + 2, pageTabRect.Top + Padding.Y );
		}

	}
}

