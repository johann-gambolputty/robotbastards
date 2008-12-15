using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Interfaces.Biomes.Views;
using Rb.Common.Controls.Graphs.Classes;

namespace Poc1.Bob.Controls.Biomes
{
	public partial class BiomeManagerControl : UserControl, IBiomeManagerView
	{
		public BiomeManagerControl( )
		{
			InitializeComponent( );

			OnNewBiome( );

			Color dark = GraphUtils.ScaleColour( Color.LightSteelBlue, 0.9f );
			Color light = Color.White;

			m_Blends = new ColorBlend( 3 );
			m_Blends.Colors[ 0 ] = light; m_Blends.Positions[ 0 ] = 0;
			m_Blends.Colors[ 1 ] = light; m_Blends.Positions[ 1 ] = 0.75f;
			m_Blends.Colors[ 2 ] = dark; m_Blends.Positions[ 2 ] = 1;
		}

		#region IBiomeManagerView Members

		/// <summary>
		/// Gets/sets the biome that this view is displaying. Can set to null (no biome shown)
		/// </summary>
		public BiomeModel CurrentBiome
		{
			get { return m_Biome; }
			set
			{
				if ( m_Biome != value )
				{
					m_Biome = value;
					OnNewBiome( );
				}
			}
		}

		/// <summary>
		/// Gets the sub-view showing terrain textures
		/// </summary>
		public IBiomeTerrainTextureView TerrainTextureView
		{
			get { return ( IBiomeTerrainTextureView )texturingTabPage.Controls[ 0 ]; }
		}

		#endregion

		#region Private Members

		private BiomeModel m_Biome;
		private readonly ColorBlend m_Blends;

		private void OnNewBiome( )
		{
			biomeTabControl.Enabled = ( m_Biome != null );
		}

		private void biomeTabControl_DrawItem( object sender, DrawItemEventArgs e )
		{
			//	TODO: AP: Looks shit
			TabPage page = biomeTabControl.TabPages[ e.Index ];
			Rectangle tabBounds = biomeTabControl.GetTabRect( e.Index );

			using ( GraphicsPath path = CreateTabPath( tabBounds.Left, tabBounds.Top, tabBounds.Width, tabBounds.Height, 4 ) )
			{
				using ( LinearGradientBrush fillBrush = new LinearGradientBrush( tabBounds, Color.Black, Color.White, 90 ) )
				{
					fillBrush.InterpolationColors = m_Blends;
					e.Graphics.FillPath( Enabled ? fillBrush : SystemBrushes.Control, path );
				}
			}

			StringFormat format = new StringFormat( );
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			e.Graphics.DrawString( page.Text, Font, Brushes.Black, tabBounds, format );
		}

		/// <summary>
		/// Creates a path for a tab header
		/// </summary>
		private static GraphicsPath CreateTabPath( float x, float y, float w, float h, float radius )
		{
			GraphicsPath path = new GraphicsPath( );

			float diameter = radius * 2;
			float endX = x + w;
			float endY = y + h;
			path.AddArc( x, y, diameter, diameter, 180, 90 );
			path.AddLine( x + diameter, y, endX, y );
			path.AddLine( endX, y, endX, endY );
			path.AddLine( endX, endY, x, endY );
			path.AddArc( x, endY, diameter, diameter, 90, 90 );
			path.CloseFigure( );

			return path;
		}

		#endregion
	}
}
