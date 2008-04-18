using System;
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
	public class SpherePlanetRenderer : IRenderable, IDisposable
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="planet">Planet to render</param>
		public SpherePlanetRenderer( SpherePlanet planet )
		{
			m_Planet = planet;
			m_Terrain = new SphereTerrain( );
			m_TerrainPatches = new SphereTerrainPatches( m_Terrain );

			//	Load in flat planet effect
			IEffect flatEffect = ( IEffect )AssetManager.Instance.Load( "Effects/Planets/terrestrialPlanet.cgfx" );
			m_PlanetTechnique = new TechniqueSelector( flatEffect, "DefaultTechnique" );

			//	Load in cloud rendering effect
			IEffect cloudEffect = ( IEffect )AssetManager.Instance.Load( "Effects/Planets/cloudLayer.cgfx" );
			m_CloudTechnique = new TechniqueSelector( cloudEffect, "DefaultTechnique" );

			//	Generate planet terrain texture
			m_PlanetTexture = m_Terrain.CreatePlanetTexture( 512 );

			int index = 0;
			foreach ( Bitmap bmp in m_PlanetTexture.ToBitmaps( ) )
			{
				bmp.Save( m_Planet.Name + " Planet Texture " + index++ + ".jpg", ImageFormat.Jpeg );
			}

			//	Generate cloud textures
			m_CloudGenerator = new SphereCloudsGenerator( 128 );

			//	Generate cached sphere for rendering the planet
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
			m_CloudGenerator.Update( );

			if ( Planet.EnableTerrainRendering )
			{
				m_TerrainPatches.Render( context, Planet, m_PlanetTexture );
			}
			else
			{
				UniCamera.PushAstroRenderTransform( TransformType.LocalToWorld, Planet.Transform );

				m_PlanetTechnique.Effect.Parameters[ "TerrainSampler" ].Set( m_PlanetTexture );
				m_CloudTechnique.Effect.Parameters[ "CloudTexture" ].Set( m_CloudGenerator.CurrentCloudTexture );
				m_CloudTechnique.Effect.Parameters[ "NextCloudTexture" ].Set( m_CloudGenerator.NextCloudTexture );
				m_CloudTechnique.Effect.Parameters[ "CloudBlend" ].Set( m_CloudGenerator.Blend );
				m_PlanetTechnique.Effect.Parameters[ "CloudTransform" ].Set( m_CloudOffsetTransform );
				context.ApplyTechnique( m_PlanetTechnique, m_PlanetGeometry );

				float cloudLayerScale = 1.01f;
				Graphics.Renderer.Scale( TransformType.LocalToWorld, cloudLayerScale, cloudLayerScale, cloudLayerScale );
				m_CloudTechnique.Effect.Parameters[ "CloudTexture" ].Set( m_CloudGenerator.CurrentCloudTexture );
				m_CloudTechnique.Effect.Parameters[ "NextCloudTexture" ].Set( m_CloudGenerator.NextCloudTexture );
				m_CloudTechnique.Effect.Parameters[ "CloudBlend" ].Set( m_CloudGenerator.Blend );
				m_CloudTechnique.Effect.Parameters[ "CloudTransform" ].Set( m_CloudOffsetTransform );
				context.ApplyTechnique( m_CloudTechnique, m_PlanetGeometry );

				Graphics.Renderer.PopTransform( TransformType.LocalToWorld );
			}

			m_CloudAngle = Utils.Wrap( m_CloudAngle + Constants.DegreesToRadians * 0.01f, 0, Constants.TwoPi );
			m_CloudOffsetTransform.SetXRotation( m_CloudAngle );
		}

		#endregion

		#region Private Members

		private readonly SphereCloudsGenerator m_CloudGenerator;
		private readonly ICubeMapTexture m_PlanetTexture;
		private readonly ITechnique m_PlanetTechnique;
		private readonly ITechnique m_CloudTechnique;
		private readonly IRenderable m_PlanetGeometry;
		private float m_CloudAngle;
		private readonly Matrix44 m_CloudOffsetTransform = new Matrix44( );
		private readonly SphereTerrain m_Terrain;
		private readonly SphereTerrainPatches m_TerrainPatches;
		private readonly SpherePlanet m_Planet;

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Clears out any graphics resources
		/// </summary>
		public void Dispose( )
		{
			m_CloudGenerator.Dispose( );
			m_PlanetTexture.Dispose( );
		}

		#endregion
	}
}
