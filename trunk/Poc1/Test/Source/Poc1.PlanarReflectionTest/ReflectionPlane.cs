using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Textures;
using Rb.Assets;
using Poc1.Tools.Waves;

namespace Poc1.PlanarReflectionTest
{
	/// <summary>
	/// Reflective plane
	/// </summary>
	public class ReflectionPlane : AbstractRenderable<ReflectionsRenderContext>
	{
		public ReflectionPlane( )
		{
			m_Technique = new TechniqueSelector( "PlanarReflectionTest/reflectionTest.cgfx", true, "DefaultTechnique" );

			using ( WaveAnimation animation = ( WaveAnimation )AssetManager.Instance.Load( "PlanarReflectionTest/SimpleWater.waves.bin" ) )
			{
				m_WaveAnimation = new AnimatedTexture2d( animation.ToTextures( true ), 5.0f );
			}
		}

		/// <summary>
		/// Renders the plane
		/// </summary>
		/// <param name="context">Rendering context</param>
		public override void Render( ReflectionsRenderContext context )
		{
			if ( context.RenderingReflections )
			{
				return;
			}

			m_WaveAnimation.UpdateAnimation( context.RenderTime );

			m_Technique.Effect.Parameters[ "ReflectionsTexture" ].Set( context.ReflectionsRenderTarget.Texture );
			m_Technique.Effect.Parameters[ "OceanTexture0" ].Set( m_WaveAnimation.SourceTexture );
			m_Technique.Effect.Parameters[ "OceanTexture1" ].Set( m_WaveAnimation.DestinationTexture );
			m_Technique.Effect.Parameters[ "OceanTextureT" ].Set( m_WaveAnimation.LocalT ); 
			
			context.ApplyTechnique( m_Technique, RenderPlane );
		}

		#region Private Members

		private readonly AnimatedTexture2d m_WaveAnimation;
		private readonly TechniqueSelector m_Technique;
		private float m_Min = -20;
		private float m_Max = 20;

		/// <summary>
		/// Renders the plane
		/// </summary>
		private void RenderPlane( IRenderContext context )
		{
			float min = m_Min;
			float max = m_Max;

			Graphics.Draw.BeginPrimitiveList( PrimitiveType.QuadList );

			Graphics.Draw.AddVertexData( VertexFieldSemantic.Texture0, 0, 0 );
			Graphics.Draw.AddVertexData( VertexFieldSemantic.Position, min, 0, min );

			Graphics.Draw.AddVertexData( VertexFieldSemantic.Texture0, 1, 0 );
			Graphics.Draw.AddVertexData( VertexFieldSemantic.Position, max, 0, min );

			Graphics.Draw.AddVertexData( VertexFieldSemantic.Texture0, 1, 1 );
			Graphics.Draw.AddVertexData( VertexFieldSemantic.Position, max, 0, max );

			Graphics.Draw.AddVertexData( VertexFieldSemantic.Texture0, 0, 1 );
			Graphics.Draw.AddVertexData( VertexFieldSemantic.Position, min, 0, max );

			Graphics.Draw.EndPrimitiveList( );
		}

		#endregion
	}
}
