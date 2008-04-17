using System.Drawing;
using System.Drawing.Imaging;
using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Classes.Rendering;
using Rb.Assets;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Class for rendering spherical planets
	/// </summary>
	public class SpherePlanetRenderer : IRenderable
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="planet">Planet to render</param>
		public SpherePlanetRenderer( SpherePlanet planet )
		{
			m_Planet = planet;

			IEffect flatEffect = ( IEffect )AssetManager.Instance.Load( "Effects/Planets/terrestrialPlanet.cgfx" );
			m_PlanetTechnique = new TechniqueSelector( flatEffect, "DefaultTechnique" );

			IEffect cloudEffect = ( IEffect )AssetManager.Instance.Load( "Effects/Planets/cloudLayer.cgfx" );
			m_CloudTechnique = new TechniqueSelector( cloudEffect, "DefaultTechnique" );

			m_Terrain = new SphereTerrain( );
			m_PlanetTexture = m_Terrain.CreatePlanetTexture( 128 );

			int index = 0;
			foreach ( Bitmap bmp in m_PlanetTexture.ToBitmaps( ) )
			{
				bmp.Save( m_Planet.Name + " Planet Texture " + index++ + ".jpg", ImageFormat.Jpeg );
			}

			m_CloudTexture = new SphereCloudsGenerator( ).CreateCloudTexture( 128 );
			index = 0;
			foreach ( Bitmap bmp in m_CloudTexture.ToBitmaps( ) )
			{
				bmp.Save( m_Planet.Name + " Cloud Texture " + index++ + ".jpg", ImageFormat.Jpeg );
			}

			Graphics.Draw.StartCache( );
			Graphics.Draw.Sphere( null, Point3.Origin, UniCamera.ToAstroRenderUnits( Planet.Radius ), 20, 20 );
			m_PlanetGeometry = Graphics.Draw.StopCache( );
		}

		/// <summary>
		/// Gets the planet being rendered
		/// </summary>
		public SpherePlanet Planet
		{
			get { return m_Planet; }
		}

		#region IRenderable Members

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			if ( Planet.EnableTerrainRendering )
			{
			//	ms_TerrainRenderer.Render( context, Planet, m_PlanetTexture );
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

		#endregion

		#region Private Members

		private readonly ICubeMapTexture m_PlanetTexture;
		private readonly ICubeMapTexture m_CloudTexture;
		private readonly ITechnique m_PlanetTechnique;
		private readonly ITechnique m_CloudTechnique;
		private readonly IRenderable m_PlanetGeometry;
		private float m_CloudAngle;
		private readonly Matrix44 m_CloudOffsetTransform = new Matrix44( );
		private readonly SphereTerrain m_Terrain;
		private readonly SpherePlanet m_Planet;

		#endregion
	}
}
