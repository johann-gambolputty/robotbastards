using System;
using System.Drawing;
using System.Windows.Forms;
using Poc1.Universe.Interfaces.Planets.Models;
using Rb.Rendering.Interfaces.Objects;
using RbGraphics = Rb.Rendering.Graphics;

namespace Poc1.PlanetBuilder
{
	public partial class TerrainVisualiserControl : UserControl
	{
		public TerrainVisualiserControl( )
		{
			InitializeComponent( );
			m_Sampler = RbGraphics.Factory.CreateTexture2dSampler( );
			m_Sampler.Texture = RbGraphics.Factory.CreateTexture2d( );
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
					int width = 256;
					int height = 256;
					TerrainVisualiserBitmapBuilder.AddRequest( m_TerrainModel, width, height, TerrainModelBitmapBuilt );
				}
			}
		}

		#region Private Members

		private ITexture2dSampler m_Sampler;
		private IPlanetTerrainModel m_TerrainModel;

		#endregion

		#region Event Handlers

		/// <summary>
		/// Renders the terrain
		/// </summary>
		private void terrainDisplay_OnRender( object sender, EventArgs e )
		{
			m_Sampler.Begin( );
			RbGraphics.Renderer.Push2d( );
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
			RbGraphics.Renderer.Pop2d( );
			m_Sampler.End( );
		}

		/// <summary>
		/// Event handler for IPlanetTerrainModel.ModelChanged
		/// </summary>
		private void TerrainModelChanged( object sender, EventArgs args )
		{
			TerrainVisualiserBitmapBuilder.AddRequest( m_TerrainModel, displayPanel.Width, displayPanel.Height, TerrainModelBitmapBuilt );
		}

		/// <summary>
		/// Work complete ballback for TerrainVisualiserBitmapBuilder.AddRequest
		/// </summary>
		private void TerrainModelBitmapBuilt( Bitmap bmp )
		{
			m_Sampler.Texture.Create( bmp, true );
		}

		#endregion

	}
}
