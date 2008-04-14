using System.Drawing;
using System.Drawing.Imaging;
using Poc1.Universe.Classes;
using Poc1.Universe.Classes.Cameras;
using Rb.Assets;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Tao.OpenGl;
using Graphics=Rb.Rendering.Graphics;
using Rectangle=System.Drawing.Rectangle;

namespace Poc1.Universe.OpenGl
{
	[RenderingLibraryType]
	public class OpenGlSpherePlanetRenderer : SpherePlanetRenderer
	{
		public OpenGlSpherePlanetRenderer( SpherePlanet planet ) :
			base( planet )
		{
			IEffect effect = ( IEffect )AssetManager.Instance.Load( "Effects/Planets/terrestrialPlanet.cgfx" );
			TechniqueSelector selector = new TechniqueSelector( effect, "DefaultTechnique" );

			m_PlanetTextureParam = effect.Parameters[ "TerrainSampler" ];

			int res = 256;
			ICubeMapTexture planetTexture = Graphics.Factory.CreateCubeMapTexture( );
			planetTexture.Build
				(
					GeneratePlanetTextureFace( PlanetMapFace.PosX, res, PixelFormat.Format24bppRgb ),
					GeneratePlanetTextureFace( PlanetMapFace.NegX, res, PixelFormat.Format24bppRgb ),
					GeneratePlanetTextureFace( PlanetMapFace.PosY, res, PixelFormat.Format24bppRgb ),
					GeneratePlanetTextureFace( PlanetMapFace.NegY, res, PixelFormat.Format24bppRgb ),
					GeneratePlanetTextureFace( PlanetMapFace.PosZ, res, PixelFormat.Format24bppRgb ),
					GeneratePlanetTextureFace( PlanetMapFace.NegZ, res, PixelFormat.Format24bppRgb ),
					true
				);

			int bmpIndex = 0;
			foreach ( Bitmap bitmap in planetTexture.ToBitmaps( ) )
			{
				bitmap.Save( planet.Name + " Planet Texture " + bmpIndex++ + ".jpg", ImageFormat.Jpeg );
			}
			m_PlanetTexture = planetTexture;

			m_Technique = selector;

			Graphics.Draw.StartCache( );
			RenderSphere( 8 );
			m_PlanetGeometry = Graphics.Draw.StopCache( );
		}


		private unsafe static Bitmap GeneratePlanetTextureFace( PlanetMapFace face, int res, PixelFormat format )
		{
			Bitmap bmp = new Bitmap( res, res, format );
			BitmapData bmpData = bmp.LockBits( new Rectangle( 0, 0, res, res ), ImageLockMode.WriteOnly, format );

			byte* curRow = ( byte* )bmpData.Scan0;

			IPlanetTerrainGenerator gen = new TestNoisePlanetTerrainGenerator( );
		//	IPlanetTerrainGenerator gen = new TestStPlanetTerrainGenerator( );
		//	IPlanetTerrainGenerator gen = new TestFacePlanetTerrainGenerator( );
			gen.GenerateSide( face, curRow, res, res, bmpData.Stride );

			bmp.UnlockBits( bmpData );
			return bmp;
		}


		private readonly IEffectParameter	m_PlanetTextureParam;
		private readonly ITexture			m_PlanetTexture;
		private readonly ITechnique			m_Technique;
		private readonly IRenderable		m_PlanetGeometry;

		private delegate void UvToPointDelegate( float x, float y );

		private static void RenderCubeFace( int subdivisions, bool cw, UvToPointDelegate uvToPoint )
		{
			float inc = 2.0f / subdivisions;
			float curY = -1;
			for ( int y = 0; y < subdivisions; ++y, curY += inc )
			{
				float curX = -1;
				for ( int x = 0; x < subdivisions; ++x, curX += inc )
				{
					uvToPoint( curX, curY );
					if ( cw )
					{
						uvToPoint( curX + inc, curY );
						uvToPoint( curX + inc, curY + inc );
						uvToPoint( curX, curY + inc );
					}
					else
					{
						uvToPoint( curX, curY + inc );
						uvToPoint( curX + inc, curY + inc );
						uvToPoint( curX + inc, curY );
					}
				}
			}
		}

		private float PlanetRenderRadius
		{
			get { return UniCamera.ToAstroRenderUnits( Planet.Radius ); }
		}

		public void UvSpherePoint( float x, float y, float z )
		{
			float invLength = 1.0f / Functions.Sqrt( x * x + y * y + z * z );
			float length = PlanetRenderRadius * invLength;

			Gl.glNormal3f( x * invLength, y * invLength, z * invLength );
			Gl.glVertex3f( x * length, y * length, z * length );
		}

		public void RenderSphere( int subdivisions )
		{
			Gl.glBegin( Gl.GL_QUADS );
			RenderCubeFace( subdivisions, false, delegate( float x, float y ) { UvSpherePoint( x, 1, y ); } );
			RenderCubeFace( subdivisions, true, delegate( float x, float y ) { UvSpherePoint( x, -1, y ); } );
			RenderCubeFace( subdivisions, true, delegate( float x, float y ) { UvSpherePoint( x, y, 1 ); } );
			RenderCubeFace( subdivisions, false, delegate( float x, float y ) { UvSpherePoint( x, y, -1 ); } );
			RenderCubeFace( subdivisions, true, delegate( float x, float y ) { UvSpherePoint( 1, x, y ); } );
			RenderCubeFace( subdivisions, false, delegate( float x, float y ) { UvSpherePoint( -1, x, y ); } );
			Gl.glEnd( );
		}

		public override void Render( IRenderContext context )
		{
			if ( Planet.EnableTerrainRendering )
			{
				ms_TerrainRenderer.Render( context, Planet, m_PlanetTexture );
			}
			else
			{
				UniCamera.PushAstroRenderTransform( TransformType.LocalToWorld, Planet.Transform );

				m_PlanetTextureParam.Set( m_PlanetTexture );
				context.ApplyTechnique( m_Technique, m_PlanetGeometry );

				Graphics.Renderer.PopTransform( TransformType.LocalToWorld );
			}

		}

		private readonly static PlanetTerrainRenderer ms_TerrainRenderer = new PlanetTerrainRenderer( );
	}
}
