using System.Drawing;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Assets;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using RbGraphics = Rb.Rendering.Graphics;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Planetary terrain for spherical planets
	/// </summary>
	public class SpherePlanetTerrain : IPlanetTerrain
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="planet">Planet to generate terrain for</param>
		public SpherePlanetTerrain( SpherePlanet planet )
		{
			m_Planet = planet;
			m_PlanetTexture = RbGraphics.Factory.CreateCubeMapTextureSampler( );

			IEffect effect = ( IEffect )AssetManager.Instance.Load( "Effects/Planets/terrestrialPlanet.cgfx" );
			m_FlatTechnique = new TechniqueSelector( effect, "DefaultTechnique" );

			RbGraphics.Draw.StartCache( );
			RbGraphics.Draw.Sphere( null, Point3.Origin, 1.0f );
			m_PlanetShape = RbGraphics.Draw.StopCache( );
		}

		#region IPlanetTerrain Members

		/// <summary>
		/// Generates the position and normal from a point on a terrain patch
		/// </summary>
		public unsafe void MakeTerrainVertexFromPatchPoint( Point3 patchPt, TerrainVertex* vertex )
		{
			float invLength = 1.0f / Functions.Sqrt( patchPt.X * patchPt.X + patchPt.Y * patchPt.Y + patchPt.Z * patchPt.Z );
			float toSurface = invLength * PlanetStandardRadius;
			vertex->SetPosition( patchPt.X * toSurface, patchPt.Y * toSurface, patchPt.Z * toSurface );
			vertex->SetNormal( patchPt.X * invLength, patchPt.Y * invLength, patchPt.Z * invLength );
		}

		/// <summary>
		/// Renders the planet using textures only (no terrain)
		/// </summary>
		public void RenderFlat( IRenderContext context )
		{
			m_PlanetTexture.Begin( );
			m_FlatTechnique.Apply( context, m_PlanetShape );
			m_PlanetTexture.End( );
		}

		#endregion

		#region Private Members

		private const float PlanetStandardRadius = 32;

		private readonly SpherePlanet m_Planet;
		private readonly Bitmap[] m_Faces = new Bitmap[ 6 ];
		private readonly ICubeMapTextureSampler m_PlanetTexture;
		private readonly IRenderable m_PlanetShape;
		private readonly ITechnique m_FlatTechnique;

		#endregion
	}
}
