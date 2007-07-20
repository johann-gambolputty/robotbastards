using System;
using System.Drawing;
using Poc0.LevelEditor.Core;
using Rb.Rendering;
using Rb.Rendering.OpenGl;
using Tao.OpenGl;

namespace Poc0.LevelEditor.Rendering.OpenGl
{
	public class OpenGlTileGrid2dRenderer : TileGrid2dRenderer
	{
		public OpenGlTileGrid2dRenderer( TileGrid grid )
		{
			Grid = grid;
			m_TileRenderState = RenderFactory.Instance.NewRenderState( );
			m_TileRenderState
				.DisableCap( RenderStateFlag.DepthTest )
				//.EnableCap( RenderStateFlag.Blend )
				//.SetBlendMode( BlendFactor.SrcAlpha, BlendFactor.OneMinusSrcAlpha )
				.SetPolygonRenderingMode( PolygonRenderMode.Fill )
				.EnableCap( RenderStateFlag.Texture2d )
				.DisableLighting( );

			m_TileTextureSampler.MinFilter = TextureFilter.LinearTexel;
			m_TileTextureSampler.MagFilter = TextureFilter.LinearTexel;
		}

		private readonly RenderState			m_TileRenderState;
		private readonly OpenGlTextureSampler2d m_TileTextureSampler	= new OpenGlTextureSampler2d( );
		private const int						InvalidDisplayList		= -1;
		private int								m_DisplayList			= InvalidDisplayList;

		/// <summary>
		/// Creates a display list for the given tile grid
		/// </summary>
		private static int CreateDisplayList( TileGrid grid )
		{
			int displayList = Gl.glGenLists( 1 );
			Gl.glNewList( displayList, Gl.GL_COMPILE );

			int screenY = 0;
			int screenXIncrement = 32;
			int screenYIncrement = 32;

			float invTexWidth	= 1.0f / grid.Set.DisplayTexture.Width;
			float invTexHeight	= 1.0f / grid.Set.DisplayTexture.Height;

			Gl.glBegin( Gl.GL_QUADS );
			
			for ( int tileY = 0; tileY < grid.Height; ++tileY, screenY += screenYIncrement )
			{
				int screenX = 0;
				for ( int tileX = 0; tileX < grid.Width; ++tileX, screenX += screenXIncrement )
				{
					Rectangle rect = grid[ tileX, tileY ].TileType.TextureRectangle;

					//	TODO: AP: Should store normalised texture rectangle in tile type
					float minU = rect.Left * invTexWidth;
					float maxU = rect.Right * invTexWidth;
					float minV = rect.Top * invTexHeight;
					float maxV = rect.Bottom * invTexHeight;

					Gl.glTexCoord2f( minU, minV );
					Gl.glVertex2i( screenX, screenY );

					Gl.glTexCoord2f( maxU, minV );
					Gl.glVertex2i( screenX + screenXIncrement, screenY );
					
					Gl.glTexCoord2f( maxU, maxV );
					Gl.glVertex2i(screenX + screenXIncrement, screenY + screenXIncrement );

					Gl.glTexCoord2f( minU, maxV );
					Gl.glVertex2i( screenX, screenY + screenYIncrement );
				}
			}

			Gl.glEnd( );

			Gl.glEndList( );

			return displayList;
		}


		/// <summary>
		/// Renders the specified tile grid
		/// </summary>
		public override void Render( IRenderContext context )
		{
			if ( Grid == null )
			{
				return;
			}

			if ( m_DisplayList == InvalidDisplayList )
			{
				m_DisplayList = CreateDisplayList( Grid );
			}

			Renderer.Instance.PushRenderState( m_TileRenderState );
			m_TileTextureSampler.Texture = ( OpenGlTexture2d )Grid.Set.DisplayTexture;
			m_TileTextureSampler.Begin( );

			Gl.glCallList( m_DisplayList );

			Gl.glPopMatrix( );

			m_TileTextureSampler.End( );
			Renderer.Instance.PopRenderState( );
		}
	}
}
