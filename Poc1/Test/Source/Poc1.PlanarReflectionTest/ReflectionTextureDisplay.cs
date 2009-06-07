using System.Drawing;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using RbGraphics=Rb.Rendering.Graphics;

namespace Poc1.PlanarReflectionTest
{
	/// <summary>
	/// Shows the current reflection texture
	/// </summary>
	public class ReflectionTextureDisplay : AbstractRenderable<ReflectionsRenderContext>
	{
		public ReflectionTextureDisplay( )
		{
			m_Sampler = RbGraphics.Factory.CreateTexture2dSampler( );
		}

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public override void Render( ReflectionsRenderContext context )
		{
			if ( context.RenderingReflections || !m_Show )
			{
				return;
			}
			m_Sampler.Texture = context.ReflectionsRenderTarget.Texture;

			Rectangle viewport = RbGraphics.Renderer.Viewport;
			float minX = m_X * viewport.Width;
			float minY = m_Y * viewport.Height;
			float maxX = ( m_X + m_Width ) * viewport.Width;
			float maxY = ( m_Y + m_Height ) * viewport.Height;

			m_Sampler.Begin( );

			RbGraphics.Renderer.Push2d( );

			RbGraphics.Draw.BeginPrimitiveList( PrimitiveType.QuadList );
			RbGraphics.Draw.AddVertexData( VertexFieldSemantic.Texture0, 0, 1 );
			RbGraphics.Draw.AddVertexData( VertexFieldSemantic.Position, minX, minY );
			RbGraphics.Draw.AddVertexData( VertexFieldSemantic.Texture0, 1, 1 );
			RbGraphics.Draw.AddVertexData( VertexFieldSemantic.Position, maxX, minY );
			RbGraphics.Draw.AddVertexData( VertexFieldSemantic.Texture0, 1, 0 );
			RbGraphics.Draw.AddVertexData( VertexFieldSemantic.Position, maxX, maxY );
			RbGraphics.Draw.AddVertexData( VertexFieldSemantic.Texture0, 0, 0 );
			RbGraphics.Draw.AddVertexData( VertexFieldSemantic.Position, minX, maxY );
			RbGraphics.Draw.EndPrimitiveList( );

			RbGraphics.Renderer.Pop2d( );

			m_Sampler.End( );
		}

		#region Private Members

		private readonly ITexture2dSampler m_Sampler;
		private float m_X = 0.0f;
		private float m_Y = 0.0f;
		private float m_Width = 0.3f;
		private float m_Height = 0.3f;
		private bool m_Show = true;

		#endregion
	}
}
