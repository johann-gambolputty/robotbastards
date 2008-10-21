using System;
using System.Drawing;
using System.Windows.Forms;
using Poc1.Universe.Interfaces.Planets.Models;
using Rb.Assets;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using RbGraphics = Rb.Rendering.Graphics;

namespace Poc1.PlanetBuilder
{
	public partial class TerrainVisualiserControl : UserControl
	{
		public TerrainVisualiserControl( )
		{
			InitializeComponent( );
			m_Texture = RbGraphics.Factory.CreateTexture2d( );

			IEffect effect = ( IEffect )AssetManager.Instance.Load( @"Effects\Planets\TerrainVisualiser.cgfx" );
			m_Technique = new TechniqueSelector( effect, "ShowTerrainPropertiesTechnique" );

			terrainDisplay.OnRender += RenderTerrain;

			resolutionComboBox.Items.Add( "Fit to window" );
			resolutionComboBox.Items.Add( 2048 );
			resolutionComboBox.Items.Add( 1024 );
			resolutionComboBox.Items.Add( 512 );
			resolutionComboBox.Items.Add( 256 );
			resolutionComboBox.Items.Add( 128 );
			resolutionComboBox.Items.Add( 64 );
			resolutionComboBox.Items.Add( 32 );
			resolutionComboBox.Items.Add( 16 );
			resolutionComboBox.Items.Add( 8 );
			resolutionComboBox.Items.Add( 4 );
			resolutionComboBox.SelectedIndex = 0;
		}



		/// <summary>
		/// Gets/sets the terrain model visualised by this control
		/// </summary>
		public IPlanetTerrainModel TerrainModel
		{
			get { return m_TerrainModel; }
			set
			{
				if ( m_TerrainModel != null )
				{
					m_TerrainModel.ModelChanged -= TerrainModelChanged;
				}
				m_TerrainModel = value;
				displayPanel.BackgroundImage = null;
				if ( m_TerrainModel != null )
				{
					m_TerrainModel.ModelChanged += TerrainModelChanged;
					TerrainModelChanged( this, null );
				}
			}
		}

		#region Private Members

		private bool m_TextureBuilding;
		private bool m_RebuildTexture;
		private readonly ITexture2d m_Texture;
		private readonly TechniqueSelector m_Technique;
		private IPlanetTerrainModel m_TerrainModel;

		/// <summary>
		/// Gets the selected width in the resolution combo box
		/// </summary>
		private int SelectedWidth
		{
			get
			{
				if ( resolutionComboBox.SelectedItem is string )
				{
					return displayPanel.Width;
				}
				return ( int )resolutionComboBox.SelectedItem;
			}
		}

		/// <summary>
		/// Gets the selected height in the resolution combo box
		/// </summary>
		private int SelectedHeight
		{
			get
			{
				if ( resolutionComboBox.SelectedItem is string )
				{
					return displayPanel.Height;
				}
				return ( int )resolutionComboBox.SelectedItem;
			}
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Renders the terrain
		/// </summary>
		private void RenderTerrain( IRenderContext context )
		{
			RbGraphics.Renderer.Push2d( );
			m_Technique.Effect.Parameters[ "MarbleFaceTexture" ].Set( m_Texture );
			m_Technique.Effect.Parameters[ "ShowSlopes" ].Set( showSlopesRadioButton.Checked );
			m_Technique.Effect.Parameters[ "ShowHeights" ].Set( showHeightsRadioButton.Checked );
			m_Technique.Apply( context, RenderTerrainBitmap );
			RbGraphics.Renderer.Pop2d( );
		}

		/// <summary>
		/// Renders the terrain bimap
		/// </summary>
		private void RenderTerrainBitmap( IRenderContext context )
		{
			RbGraphics.Draw.BeginPrimitiveList( PrimitiveType.QuadList );
			RbGraphics.Draw.AddVertexData( VertexFieldSemantic.Position, 0, 0 );
			RbGraphics.Draw.AddVertexData( VertexFieldSemantic.Texture0, 0, 0 );
			RbGraphics.Draw.AddVertexData( VertexFieldSemantic.Position, terrainDisplay.Width, 0 );
			RbGraphics.Draw.AddVertexData( VertexFieldSemantic.Texture0, 1, 0 );
			RbGraphics.Draw.AddVertexData( VertexFieldSemantic.Position, terrainDisplay.Width, terrainDisplay.Height );
			RbGraphics.Draw.AddVertexData( VertexFieldSemantic.Texture0, 1, 1 );
			RbGraphics.Draw.AddVertexData( VertexFieldSemantic.Position, 0, terrainDisplay.Height );
			RbGraphics.Draw.AddVertexData( VertexFieldSemantic.Texture0, 0, 1 );
			RbGraphics.Draw.EndPrimitiveList( );
		}

		/// <summary>
		/// Event handler for IPlanetTerrainModel.ModelChanged
		/// </summary>
		private void TerrainModelChanged( object sender, EventArgs args )
		{
			if ( m_TextureBuilding )
			{
				m_RebuildTexture = true;
			}
			else
			{
				QueueTextureBuildRequest( );
			}
		}

		/// <summary>
		/// Queues up a request to rebuild the displayed texture
		/// </summary>
		private void QueueTextureBuildRequest( )
		{
			if ( TerrainModel == null )
			{
				return;
			}
			if ( m_TextureBuilding )
			{
				throw new InvalidOperationException( "Tried to queue up a texture build request when one was already pending" );
			}
			m_TextureBuilding = true;
			TerrainVisualiserBitmapBuilder.AddRequest( m_TerrainModel, SelectedWidth, SelectedHeight, TerrainModelBitmapBuilt );
		}

		/// <summary>
		/// Work complete ballback for TerrainVisualiserBitmapBuilder.AddRequest
		/// </summary>
		private void TerrainModelBitmapBuilt( Bitmap bmp )
		{
			m_TextureBuilding = false;
			m_Texture.Create( bmp, false );
			if ( m_RebuildTexture )
			{
				m_RebuildTexture = false;
				QueueTextureBuildRequest( );
			}

			terrainDisplay.Invalidate( );
		}

		private void resolutionComboBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			TerrainModelChanged( sender, e );
		}

		private void showHeightsRadioButton_CheckedChanged( object sender, EventArgs e )
		{
			terrainDisplay.Invalidate( );
		}

		private void showSlopesRadioButton_CheckedChanged( object sender, EventArgs e )
		{
			terrainDisplay.Invalidate( );
		}

		private void showTerrainTypesRadioButton_CheckedChanged( object sender, EventArgs e )
		{
			terrainDisplay.Invalidate( );
		}

		#endregion

	}
}
