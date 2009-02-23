using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Poc1.Bob.Core.Interfaces.Biomes.Views;
using Poc1.Tools.TerrainTextures.Core;
using Rb.Common.Controls.Forms.Graphs;
using Rb.Common.Controls.Graphs.Classes;
using Rb.Common.Controls.Graphs.Classes.Sources;
using Rb.Common.Controls.Utils;
using Rb.Core.Utils;
using Rectangle=System.Drawing.Rectangle;

namespace Poc1.Bob.Controls.Biomes
{
	public partial class TerrainTypeTextureItemControl : UserControl, ITerrainTypeTextureView
	{
		#region Events

		/// <summary>
		/// Event raised when this control is selected
		/// </summary>
		public event Action<TerrainTypeTextureItemControl> Selected;

		/// <summary>
		/// Event raised when the underlying terrain type is modified by this control
		/// </summary>
		public event Action<TerrainTypeTextureItemControl> Changed;
		
		#endregion Events

		/// <summary>
		/// Sets up this control
		/// </summary>
		public TerrainTypeTextureItemControl( )
		{
			InitializeComponent( );

			TerrainType = new TerrainType( );

			( ( Bitmap )deleteTerrainButton.Image ).MakeTransparent( );

			Color dark = GraphUtils.ScaleColour( Color.LightSteelBlue, 0.9f );
			Color light = Color.White;

			m_Blends = new ColorBlend( 3 );
			m_Blends.Colors[ 0 ] = light; m_Blends.Positions[ 0 ] = 0;
			m_Blends.Colors[ 1 ] = light; m_Blends.Positions[ 1 ] = 0.75f;
			m_Blends.Colors[ 2 ] = dark; m_Blends.Positions[ 2 ] = 1;

			m_SelectedBlends = new ColorBlend( 3 );
			m_SelectedBlends.Colors[ 0 ] = dark; m_SelectedBlends.Positions[ 0 ] = 0;
			m_SelectedBlends.Colors[ 1 ] = dark; m_SelectedBlends.Positions[ 1 ] = 0.75f;
			m_SelectedBlends.Colors[ 2 ] = light; m_SelectedBlends.Positions[ 2 ] = 1;
		}

		/// <summary>
		/// Sets up this control
		/// </summary>
		/// <param name="altitudeGraph">Altitude distribution graph</param>
		/// <param name="slopeGraph">Slope distribution graph</param>
		public void Initialise( GraphControl altitudeGraph, GraphControl slopeGraph )
		{
			Arguments.CheckNotNull( altitudeGraph, "altitudeGraph" );
			Arguments.CheckNotNull( slopeGraph, "slopeGraph" );
			m_SlopeGraphComponent = new GraphComponent( "", new GraphX2dSourceFunction1dAdapter( TerrainType.SlopeDistribution ) );
			m_AltitudeGraphComponent = new GraphComponent( "", new GraphX2dSourceFunction1dAdapter( TerrainType.AltitudeDistribution ) );

			m_SlopeGraphComponent.Source.GraphChanged += OnGraphChanged;
			m_AltitudeGraphComponent.Source.GraphChanged += OnGraphChanged;

			m_AltitudeGraph = altitudeGraph;
			m_SlopeGraph = slopeGraph;

			m_SlopeGraph.AddGraphComponent( m_SlopeGraphComponent );
			m_AltitudeGraph.AddGraphComponent( m_AltitudeGraphComponent );

			selectedCheckBox.Checked = true;
		}

		/// <summary>
		/// Uninitialises this control
		/// </summary>
		public void Uninitialise( )
		{
			if ( m_AltitudeGraph == null )
			{
				throw new InvalidOperationException( "Control has not yet been initialised" );
			}
			m_AltitudeGraph.RemoveGraphComponent( m_AltitudeGraphComponent );
			m_SlopeGraph.RemoveGraphComponent( m_SlopeGraphComponent );
		}

		#region Private Members
		
		private GraphControl m_AltitudeGraph;
		private GraphControl m_SlopeGraph;
		private TerrainType m_TerrainType;
		private readonly ColorBlend m_Blends;
		private readonly ColorBlend m_SelectedBlends;
		private GraphComponent m_AltitudeGraphComponent;
		private GraphComponent m_SlopeGraphComponent;
		private readonly Pen m_BorderPen = new Pen( Color.FromArgb( 0xa0, 0x00, 0x00, 0x20 ), 2.0f );
		private readonly Pen m_HighlightPen = new Pen( Color.FromArgb( 0xa0, 0x00, 0x00, 0xff ), 2.0f );

		/// <summary>
		/// Raises the Changed event
		/// </summary>
		private void OnTerrainTypeChanged( )
		{
			if ( Changed != null )
			{
				Changed( this );
			}
		}

		/// <summary>
		/// Called when either slope or altitude graph changes
		/// </summary>
		private void OnGraphChanged( object sender, EventArgs args )
		{
			Invalidate( );
		}

		/// <summary>
		/// Returns true if either altitude or slope graph components are selected
		/// </summary>
		private bool GraphSelected
		{
			get
			{
				if ( ( m_AltitudeGraphComponent != null ) && ( m_AltitudeGraphComponent.Source != null ) && ( m_AltitudeGraphComponent.Source.Selected ) )
				{
					return true;
				}
				if ( ( m_SlopeGraphComponent != null ) && ( m_SlopeGraphComponent.Source != null ) && ( m_SlopeGraphComponent.Source.Selected ) )
				{
					return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Returns true if either altitude or slope graph components are highlighted
		/// </summary>
		private bool GraphHighlighted
		{
			get
			{
				if ( ( m_AltitudeGraphComponent != null ) && ( m_AltitudeGraphComponent.Source != null ) && ( m_AltitudeGraphComponent.Source.Highlighted ) )
				{
					return true;
				}
				if ( ( m_SlopeGraphComponent != null ) && ( m_SlopeGraphComponent.Source != null ) && ( m_SlopeGraphComponent.Source.Highlighted ) )
				{
					return true;
				}
				return false;
			}
		}

		#region Event Handlers

		private void TerrainTypeControl_Paint( object sender, PaintEventArgs e )
		{
			Rectangle bounds = DisplayRectangle;
			bounds.Inflate( -1, -1 );
			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			using ( GraphicsPath path = DrawingHelpers.CreateRoundedRectanglePath( bounds, 4 ) )
			{
				using ( LinearGradientBrush fillBrush = new LinearGradientBrush( bounds, Color.Black, Color.White, 90 ) )
				{
					fillBrush.InterpolationColors = m_Blends;
					e.Graphics.FillPath( Enabled ? fillBrush : SystemBrushes.Control, path );
				}

				Pen borderPen = GraphHighlighted ? m_HighlightPen : m_BorderPen;
				e.Graphics.DrawPath( borderPen, path );
			}

			//if ( m_GraphComponent.Renderer == null )
			//{
			//    return;
			//}

			//int hHeight = 12;
			//int midY = bounds.Top + hHeight + 2;

			//int radius = ( hHeight - 6 );

			//Rectangle colourKeyBounds = new Rectangle( bounds.Right - ( radius * 2 + 4 ), midY - radius, radius * 2, radius * 2 );
			//using ( Brush colourKeyBrush = new SolidBrush( m_GraphComponent.Renderer.Colour ) )
			//{
			//    e.Graphics.FillEllipse( colourKeyBrush, colourKeyBounds );
			//}
			//e.Graphics.DrawEllipse( Pens.Black, colourKeyBounds );
		}

		private void deleteTerrainButton_Click( object sender, EventArgs e )
		{
			if ( RemoveTerrainType != null )
			{
				RemoveTerrainType( this );
			}
		}

		private void setTextureButton_Click( object sender, EventArgs e )
		{
			OpenFileDialog openDlg = new OpenFileDialog( );
			openDlg.Filter = "Image files (*.jpg, *.bmp)|*.JPG;*.BMP|All Files (*.*)|*.*";
			if ( openDlg.ShowDialog( ) != DialogResult.OK )
			{
				return;
			}

			TerrainType.LoadBitmap( openDlg.FileName );
			setTextureButton.Image = TerrainType.Texture == null ? null : ( Bitmap )TerrainType.Texture.Clone( );
			if ( string.IsNullOrEmpty( TerrainType.Name ) )
			{
				TerrainType.Name = System.IO.Path.GetFileNameWithoutExtension( openDlg.FileName );
				
				//	TODO: AP: Should be managed by controller
				nameTextBox.Text = TerrainType.Name;
			}
			OnTerrainTypeChanged( );
		}

		private void showInGraphsCheckBox_CheckedChanged( object sender, EventArgs e )
		{
			if ( m_AltitudeGraphComponent != null )
			{
				m_AltitudeGraphComponent.Source.Disabled = !selectedCheckBox.Checked;
			}
			if ( m_SlopeGraphComponent != null )
			{
				m_SlopeGraphComponent.Source.Disabled = !selectedCheckBox.Checked;
			}
		}

		private void nameTextBox_TextChanged( object sender, EventArgs e )
		{
			TerrainType.Name = nameTextBox.Text;
			if ( m_SlopeGraphComponent != null )
			{
				m_SlopeGraphComponent.Name = TerrainType.Name;
			}
			if ( m_AltitudeGraphComponent != null )
			{
				m_AltitudeGraphComponent.Name = TerrainType.Name;
			}
		}

		private void TerrainTypeControl_Resize( object sender, EventArgs e )
		{
			Invalidate( );
		} 

		#endregion Event Handlers
 
		#endregion Private Members

		#region ITerrainTypeView Members

		/// <summary>
		/// Event, raised when the user requests that the terrain type be removed
		/// </summary>
		public event ActionDelegates.Action<ITerrainTypeView> RemoveTerrainType;

		/// <summary>
		/// Gets the terrain type associated with this control
		/// </summary>
		public TerrainType TerrainType
		{
			get { return m_TerrainType; }
			set { m_TerrainType = value; }
		}

		#endregion
	}
}
