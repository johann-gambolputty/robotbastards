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

			int res = 128;
			//	IPlanetTerrainGenerator terrainGenerator = new TestStPlanetTerrainGenerator( );
			//	IPlanetTerrainGenerator terrainGenerator = new TestFacePlanetTerrainGenerator( );
			//	IPlanetTerrainGenerator terrainGenerator = new TestCloudGenerator( );
			IPlanetTerrainGenerator terrainGenerator = new TestNoisePlanetTerrainGenerator( );
			ICubeMapTexture planetTexture = Graphics.Factory.CreateCubeMapTexture( );
			planetTexture.Build
				(
					GeneratePlanetTextureFace( terrainGenerator, PlanetMapFace.PosX, res ),
					GeneratePlanetTextureFace( terrainGenerator, PlanetMapFace.NegX, res ),
					GeneratePlanetTextureFace( terrainGenerator, PlanetMapFace.PosY, res ),
					GeneratePlanetTextureFace( terrainGenerator, PlanetMapFace.NegY, res ),
					GeneratePlanetTextureFace( terrainGenerator, PlanetMapFace.PosZ, res ),
					GeneratePlanetTextureFace( terrainGenerator, PlanetMapFace.NegZ, res ),
					true
				);

			int cloudRes = 128;
			ICubeMapTexture cloudTexture = Graphics.Factory.CreateCubeMapTexture( );
			IPlanetTerrainGenerator cloudGenerator = new TestCloudGenerator( );
			cloudTexture.Build
			(
				GeneratePlanetCloudTextureFace( cloudGenerator, PlanetMapFace.PosX, cloudRes ),
				GeneratePlanetCloudTextureFace( cloudGenerator, PlanetMapFace.NegX, cloudRes ),
				GeneratePlanetCloudTextureFace( cloudGenerator, PlanetMapFace.PosY, cloudRes ),
				GeneratePlanetCloudTextureFace( cloudGenerator, PlanetMapFace.NegY, cloudRes ),
				GeneratePlanetCloudTextureFace( cloudGenerator, PlanetMapFace.PosZ, cloudRes ),
				GeneratePlanetCloudTextureFace( cloudGenerator, PlanetMapFace.NegZ, cloudRes ),
				true
			);

			int bmpIndex = 0;
			foreach ( Bitmap bitmap in planetTexture.ToBitmaps( ) )
			{
				bitmap.Save( planet.Name + " Planet Texture " + bmpIndex++ + ".jpg", ImageFormat.Jpeg );
			}
			m_PlanetTexture = planetTexture;
			m_CloudTexture = cloudTexture;

			m_PlanetTechnique = selector;
			
			IEffect cloudEffect = ( IEffect )AssetManager.Instance.Load( "Effects/Planets/cloudLayer.cgfx" );
			m_CloudTechnique = new TechniqueSelector( cloudEffect, "DefaultTechnique" );

			Graphics.Draw.StartCache( );
			RenderSphere( 8 );
			m_PlanetGeometry = Graphics.Draw.StopCache( );
		}

		private unsafe static Bitmap GeneratePlanetCloudTextureFace( IPlanetTerrainGenerator gen, PlanetMapFace face, int res )
		{
			PixelFormat format = gen.CubeMapFormat;
			Bitmap bmp = new Bitmap( res, res, format );
			BitmapData bmpData = bmp.LockBits( new Rectangle( 0, 0, res, res ), ImageLockMode.WriteOnly, format );

			gen.GenerateSide( face, ( byte* )bmpData.Scan0, res, res, bmpData.Stride );

			bmp.UnlockBits( bmpData );
			return bmp;
		}

		private unsafe static Bitmap GeneratePlanetTextureFace( IPlanetTerrainGenerator gen, PlanetMapFace face, int res )
		{
			PixelFormat format = gen.CubeMapFormat;
			Bitmap bmp = new Bitmap( res, res, format );
			BitmapData bmpData = bmp.LockBits( new Rectangle( 0, 0, res, res ), ImageLockMode.WriteOnly, format );

			gen.GenerateSide( face, ( byte* )bmpData.Scan0, res, res, bmpData.Stride );

			bmp.UnlockBits( bmpData );
			return bmp;
		}


		private readonly ITexture			m_PlanetTexture;
		private readonly ITexture			m_CloudTexture;
		private readonly ITechnique			m_PlanetTechnique;
		private readonly ITechnique			m_CloudTechnique;
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

				m_PlanetTechnique.Effect.Parameters[ "TerrainSampler" ].Set( m_PlanetTexture );
				m_PlanetTechnique.Effect.Parameters[ "CloudSampler" ].Set( m_CloudTexture );
				m_PlanetTechnique.Effect.Parameters[ "CloudTransform" ].Set( m_CloudOffsetTransform );
				context.ApplyTechnique( m_PlanetTechnique, m_PlanetGeometry );

				float cloudLayerScale = 1.01f;
				Graphics.Renderer.Scale( TransformType.LocalToWorld, cloudLayerScale, cloudLayerScale, cloudLayerScale );
				m_CloudTechnique.Effect.Parameters[ "CloudSampler" ].Set( m_CloudTexture );
				m_CloudTechnique.Effect.Parameters[ "CloudTransform" ].Set( m_CloudOffsetTransform );
				context.ApplyTechnique( m_CloudTechnique, m_PlanetGeometry );

				Graphics.Renderer.PopTransform( TransformType.LocalToWorld );
			}

			m_CloudAngle = Utils.Wrap( m_CloudAngle + Constants.DegreesToRadians * 0.01f, 0, Constants.TwoPi );
			m_CloudOffsetTransform.SetXRotation( m_CloudAngle );
		}

		private float m_CloudAngle;
		private readonly Matrix44 m_CloudOffsetTransform = new Matrix44( );
		private readonly static PlanetTerrainRenderer ms_TerrainRenderer = new PlanetTerrainRenderer( );
	}
}
