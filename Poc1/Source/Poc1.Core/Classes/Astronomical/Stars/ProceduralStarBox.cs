
using System;
using System.Drawing;
using Poc1.Core.Classes.Profiling;
using Poc1.Core.Interfaces.Rendering;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Graphics=Rb.Rendering.Graphics;
using Rb.Assets;

namespace Poc1.Core.Classes.Astronomical.Stars
{
	/// <summary>
	/// Renders stars using a shader
	/// </summary>
	public class ProceduralStarBox : IRenderable<IUniRenderContext>
	{
		#region Construction

		/// <summary>
		/// Builds the star box (dome, sphere, thing)
		/// </summary>
		public ProceduralStarBox( ) :
			this( "Effects/Planets/procStars.cgfx", "Star Fields/Default/" )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="starEffectPath">Path to the star effect</param>
		/// <param name="starTexturePath">Path to the star textures</param>
		public unsafe ProceduralStarBox( string starEffectPath, string starTexturePath )
		{
			Arguments.CheckNotNullOrEmpty( starEffectPath, "starEffectPath" );
			Arguments.CheckNotNullOrEmpty( starTexturePath, "starTexturePath" );
			EffectAssetHandle effect = new EffectAssetHandle( starEffectPath, false );
			m_Technique = new TechniqueSelector( effect, "DefaultTechnique" );

			Draw.ISurface surface = Graphics.Draw.NewSurface( Graphics.Draw.NewBrush( Color.Black ), null );
			surface.FaceBrush.State.CullFaces = false;
			surface.FaceBrush.State.DepthWrite = false;
			surface.FaceBrush.State.DepthTest = false;

			Graphics.Draw.StartCache( );
			Graphics.Draw.Sphere( surface, Point3.Origin, 20, 20, 20 );
			m_Box = Graphics.Draw.StopCache( );

			m_NoiseTexture = ( ITexture3d )AssetManager.Instance.Load( "Terrain/Tiled3dNoise.noise3d.texture" );

			Texture3dData randomTexture = new Texture3dData( 32, 32, 32, TextureFormat.R8G8B8 );
			fixed ( byte* texels = randomTexture.Bytes )
			{
				Random rnd = new Random( );
				byte* curTexel = texels;
				int maxTexIndex = randomTexture.Width * randomTexture.Height * randomTexture.Depth;
				for ( int texIndex = 0; texIndex < maxTexIndex; ++texIndex )
				{
					*( curTexel++ ) = ( byte )( rnd.Next( ) & 0xff );
					*( curTexel++ ) = ( byte )( rnd.Next( ) & 0xff );
					*( curTexel++ ) = ( byte )( rnd.Next( ) & 0xff );
				}

				//float fMul = 255.0f / ( float )( randomTexture.Width - 1 );
				//curTexel = texels;
				//for ( int x = 0; x < randomTexture.Width; ++x )
				//{
				//    byte bX = ( byte )Utils.Clamp( x * fMul, 0, 255.0f );
				//    for ( int y = 0; y < randomTexture.Height; ++y )
				//    {
				//        byte bY = ( byte )Utils.Clamp( y * fMul, 0, 255.0f );
				//        for ( int z = 0; z < randomTexture.Depth; ++z )
				//        {
				//            byte bZ = ( byte )Utils.Clamp( z * fMul, 0, 255.0f );
				//            *( curTexel++ ) = bX;
				//            *( curTexel++ ) = bY;
				//            *( curTexel++ ) = bZ;
				//        }
				//    }
				//}
			}

			m_RandomTexture = Graphics.Factory.CreateTexture3d( );
			m_RandomTexture.Create( randomTexture );
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders the star box
		/// </summary>
		public void Render( IUniRenderContext context )
		{
			GameProfiles.Game.Rendering.StarSphereRendering.Begin( );
			Graphics.Renderer.PushTransform( TransformType.Texture0, context.Camera.InverseFrame );
			m_Technique.Effect.Parameters[ "NoiseTile" ].Set( m_NoiseTexture );
			m_Technique.Effect.Parameters[ "RandomTexture" ].Set( m_RandomTexture );
			m_Technique.Apply( context, m_Box );
			Graphics.Renderer.PopTransform( TransformType.Texture0 );
			GameProfiles.Game.Rendering.StarSphereRendering.End( );
		}

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			if ( context is IUniRenderContext )
			{
				Render( ( IUniRenderContext )context );
				return;
			}
			throw new NotSupportedException( "Rendering from standard render context is not supported - use an IUniRenderContext" );
		}

		#endregion

		#region Private Members

		private readonly ITechnique m_Technique;
		private readonly IRenderable m_Box;
		private readonly ITexture3d m_RandomTexture;
		private readonly ITexture3d m_NoiseTexture;

		#endregion
	}
}
