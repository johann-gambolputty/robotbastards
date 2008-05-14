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
			m_Terrain = new SphereTerrain( planet );
			
			//	Load in flat planet effect
			IEffect flatEffect = ( IEffect )AssetManager.Instance.Load( "Effects/Planets/terrestrialPlanet.cgfx" );
			m_PlanetTechnique = new TechniqueSelector( flatEffect, "DefaultTechnique" );

			//	Load in cloud rendering effect
			IEffect cloudEffect = ( IEffect )AssetManager.Instance.Load( "Effects/Planets/cloudLayer.cgfx" );
			m_CloudTechnique = new TechniqueSelector( cloudEffect, "DefaultTechnique" );
			
			//	Generate planet terrain texture
			m_PlanetTexture = m_Terrain.CreatePlanetTexture( 256 );

			int index = 0;
			foreach ( Bitmap bmp in m_PlanetTexture.ToBitmaps( ) )
			{
			    bmp.Save( m_Planet.Name + " Planet Texture " + index++ + ".jpg", ImageFormat.Jpeg );
			}

			m_TerrainPatches = new SphereTerrainQuadPatches( planet, m_Terrain );

			//	Generate cloud textures
			m_CloudGenerator = new SphereCloudsGenerator( 256 );

			//	Generate cached sphere for rendering the planet
			Graphics.Draw.StartCache( );
			Graphics.Draw.Sphere( null, Point3.Origin, ( float )UniUnits.AstroRenderUnits.FromUniUnits( Planet.Radius ), 40, 40 );
			m_PlanetGeometry = Graphics.Draw.StopCache( );

			Graphics.Draw.StartCache( );
			long cloudRadius = Planet.Radius + UniUnits.Metres.ToUniUnits( 5000 );
			Graphics.Draw.Sphere( null, Point3.Origin, ( float )UniUnits.RenderUnits.FromUniUnits( cloudRadius ), 40, 40 );
			m_CloudShell = Graphics.Draw.StopCache( );
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
				UniCamera.PushRenderTransform( TransformType.LocalToWorld, m_Planet.Transform );

				GameProfiles.Game.Rendering.PlanetRendering.TerrainRendering.Begin( );
				m_TerrainPatches.Render( context );
				GameProfiles.Game.Rendering.PlanetRendering.TerrainRendering.End( );

				GameProfiles.Game.Rendering.PlanetRendering.CloudRendering.Begin( );
				m_CloudTechnique.Effect.Parameters[ "CloudBlend" ].Set( m_CloudGenerator.Blend );
				m_CloudTechnique.Effect.Parameters[ "CloudTransform" ].Set( m_CloudOffsetTransform );
				m_CloudTechnique.Effect.Parameters[ "CloudTexture" ].Set( m_CloudGenerator.CurrentCloudTexture );
				m_CloudTechnique.Effect.Parameters[ "NextCloudTexture" ].Set( m_CloudGenerator.NextCloudTexture );
				context.ApplyTechnique( m_CloudTechnique, m_CloudShell );
				GameProfiles.Game.Rendering.PlanetRendering.CloudRendering.End( );

				Graphics.Renderer.PopTransform( TransformType.LocalToWorld );
			}
			else
			{
				UniCamera.PushAstroRenderTransform( TransformType.LocalToWorld, Planet.Transform );

				GameProfiles.Game.Rendering.PlanetRendering.FlatPlanetRendering.Begin( );
				m_PlanetTechnique.Effect.Parameters[ "TerrainSampler" ].Set( m_PlanetTexture );
				m_PlanetTechnique.Effect.Parameters[ "CloudBlend" ].Set( m_CloudGenerator.Blend );
				m_PlanetTechnique.Effect.Parameters[ "CloudTransform" ].Set( m_CloudOffsetTransform );
				m_PlanetTechnique.Effect.Parameters[ "CloudTexture" ].Set( m_CloudGenerator.CurrentCloudTexture );
				m_PlanetTechnique.Effect.Parameters[ "NextCloudTexture" ].Set( m_CloudGenerator.NextCloudTexture );
				context.ApplyTechnique( m_PlanetTechnique, m_PlanetGeometry );
				GameProfiles.Game.Rendering.PlanetRendering.FlatPlanetRendering.End( );

				GameProfiles.Game.Rendering.PlanetRendering.CloudRendering.Begin( );
				float cloudLayerScale = 1.05f;
				Graphics.Renderer.Scale( TransformType.LocalToWorld, cloudLayerScale, cloudLayerScale, cloudLayerScale );
				m_CloudTechnique.Effect.Parameters[ "CloudTexture" ].Set( m_CloudGenerator.CurrentCloudTexture );
				m_CloudTechnique.Effect.Parameters[ "NextCloudTexture" ].Set( m_CloudGenerator.NextCloudTexture );
				m_CloudTechnique.Effect.Parameters[ "CloudBlend" ].Set( m_CloudGenerator.Blend );
				m_CloudTechnique.Effect.Parameters[ "CloudTransform" ].Set( m_CloudOffsetTransform );
				context.ApplyTechnique( m_CloudTechnique, m_PlanetGeometry );
				GameProfiles.Game.Rendering.PlanetRendering.CloudRendering.End( );

				Graphics.Renderer.PopTransform( TransformType.LocalToWorld );
			}

			m_CloudAngle = Utils.Wrap( m_CloudAngle + Constants.DegreesToRadians * 0.001f, 0, Constants.TwoPi );
			m_CloudOffsetTransform.SetXRotation( m_CloudAngle );
		}

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

		#region Private Members

		private readonly SphereCloudsGenerator m_CloudGenerator;
		private readonly ICubeMapTexture m_PlanetTexture;
		private readonly ITechnique m_PlanetTechnique;
		private readonly ITechnique m_CloudTechnique;
		private readonly IRenderable m_PlanetGeometry;
		private readonly Matrix44 m_CloudOffsetTransform = new Matrix44( );
		private readonly SphereTerrain m_Terrain;
		private readonly IRenderable m_TerrainPatches;
		private readonly IRenderable m_CloudShell;
		private readonly SpherePlanet m_Planet;

		private float m_CloudAngle;

		#endregion

	}
}
