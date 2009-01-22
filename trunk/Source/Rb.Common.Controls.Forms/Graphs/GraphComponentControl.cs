using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Rb.Common.Controls.Graphs.Classes;
using Rb.Common.Controls.Utils;

namespace Rb.Common.Controls.Forms.Graphs
{
	public partial class GraphComponentControl : UserControl
	{
		public GraphComponentControl( GraphComponent graphComponent )
		{
			InitializeComponent( );

			m_GraphComponent = graphComponent;

			graphComponent.Source.GraphChanged += OnGraphChanged;

			enableCheckbox.Checked = !GraphComponent.Source.Disabled;
			graphNameLabel.Text = GraphComponent.Name;

			Color dark = GraphUtils.ScaleColour( Color.LightSteelBlue, 0.9f );
			Color light = Color.White;

			m_Blends = GraphUtils.CreateColourBlend( light, 0, light, 0.75f, dark, 1 );
			m_SelectedBlends = GraphUtils.CreateColourBlend( dark, 0, dark, 0.75f, light, 1 );
		}

		/// <summary>
		/// Gets/sets the selected state of this control. Changes the selected
		/// state of the associated graph component.
		/// </summary>
		public bool Selected
		{
			get { return m_Selected; }
			set
			{
				bool changed = m_Selected != value;
				m_Selected = value;
				if ( changed )
				{
					GraphComponent.Source.Selected = m_Selected;
					Invalidate( );
				}
			}
		}

		/// <summary>
		/// Gets the associated graph component
		/// </summary>
		public GraphComponent GraphComponent
		{
			get { return m_GraphComponent; }
		}

		private readonly GraphComponent m_GraphComponent;
		private bool m_Selected;
		private readonly ColorBlend m_Blends;
		private readonly ColorBlend m_SelectedBlends;
		private readonly Pen m_BorderPen = new Pen( Color.FromArgb( 0xa0, 0x00, 0x00, 0x20 ), 2.0f );
		private readonly Pen m_HighlightPen = new Pen( Color.FromArgb( 0xa0, 0x00, 0x00, 0xff ), 2.0f );
		private GraphControl m_GraphControl;

		private void OnGraphChanged( object sender, EventArgs args )
		{
			m_Selected = GraphComponent.Source.Selected;
			Invalidate( );
		}

		private void GraphComponentControl_Paint( object sender, PaintEventArgs e )
		{
			Rectangle bounds = DisplayRectangle;
			bounds.Inflate( -1, -1 );
			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			using ( GraphicsPath path = DrawingHelpers.CreateRoundedRectanglePath( bounds, 4 ) )
			{
				using ( LinearGradientBrush fillBrush = new LinearGradientBrush( bounds, Color.Black, Color.White, 90 ) )
				{
					fillBrush.InterpolationColors = GraphComponent.Source.Selected ? m_SelectedBlends : m_Blends;
					e.Graphics.FillPath( Enabled ? fillBrush : SystemBrushes.Control, path );
				}

				e.Graphics.DrawPath( GraphComponent.Source.Highlighted ? m_HighlightPen : m_BorderPen, path );
			}

			if ( GraphComponent.Renderer == null )
			{
				return;
			}

			int hHeight = 12;
			int midY = bounds.Top + hHeight + 2;

			int radius = ( hHeight - 6 );

			Rectangle colourKeyBounds = new Rectangle( bounds.Right - ( radius * 2 + 4 ), midY - radius, radius * 2, radius * 2 );
			using ( Brush colourKeyBrush = new SolidBrush( GraphComponent.Renderer.Colour ) )
			{
				e.Graphics.FillEllipse( colourKeyBrush, colourKeyBounds );
			}
			e.Graphics.DrawEllipse( Pens.Black, colourKeyBounds );
		}

		/// <summary>
		/// Gets/sets the associated graph control
		/// </summary>
		public GraphControl AssociatedGraphControl
		{
			get { return m_GraphControl; }
			set
			{
				if ( m_GraphControl != null )
				{
					m_GraphControl.MouseMove -= OnAssociatedGraphControlMouseMove;
				}
				m_GraphControl = value;
				if ( m_GraphControl != null )
				{
					m_GraphControl.MouseMove += OnAssociatedGraphControlMouseMove;
				}
			}
		}

		private void OnAssociatedGraphControlMouseMove( object sender, MouseEventArgs e )
		{
			PointF cursorDataPt = m_GraphControl.Transform.ScreenToData( e.Location );

			valueLabel.Text = "Value: " + m_GraphComponent.Source.GetDisplayValueAt( cursorDataPt.X, cursorDataPt.Y );
			Invalidate( );
		}

		private void GraphComponentControl_Click( object sender, EventArgs e )
		{
			Selected = !Selected;
		}

		private void graphNameLabel_Click( object sender, EventArgs e )
		{
			Selected = !Selected;
		}

		private void valueLabel_Click( object sender, EventArgs e )
		{
			Selected = !Selected;
		}

		private void enableCheckbox_CheckedChanged( object sender, EventArgs e )
		{
			GraphComponent.Source.Disabled = !enableCheckbox.Checked;
		}

		private void GraphComponentControl_Resize(object sender, EventArgs e)
		{
			Invalidate( );
		}

		private void GraphComponentControl_Load(object sender, EventArgs e)
		{
			DoubleBuffered = true;
		}

	}
}
