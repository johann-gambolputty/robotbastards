using Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers.Marble;
using Poc1.Core.Classes.Profiling;
using Poc1.Core.Interfaces.Astronomical.Planets.Renderers.Terrain.PackTextures;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Renderers;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Renderers.Marble;
using Poc1.Core.Interfaces.Rendering;
using Rb.Assets;
using Rb.Core.Threading;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers
{
	/// <summary>
	/// Renders homogenous procedural terrain from afar as a cube-mapped sphere
	/// </summary>
	public class SpherePlanetHomogenousProceduralMarbleRenderer : AbstractSpherePlanetMarbleRenderer
	{

		/// <summary>
		/// Setup constructor
		/// </summary>
		public SpherePlanetHomogenousProceduralMarbleRenderer( )
		{
			m_TextureBuilder = new SpherePlanetMarbleTextureBuilder( );
			IEffect effect = ( IEffect )AssetManager.Instance.Load( "Effects/Planets/marbleSpherePlanet.cgfx" );
			m_Technique = new TechniqueSelector( effect, "DefaultTechnique" );
		}

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public override void Render( IUniRenderContext context )
		{
			if ( context.CurrentPass != UniRenderPass.FarObjects )
			{
				return;
			}
			using ( GameProfiles.Game.Rendering.PlanetRendering.FlatPlanetRendering.Enter( ) )
			{
				IRenderable geometry = Geometry;
				UpdateMarbleTexture( );
				ISpherePlanetCloudRenderer cloudRenderer = Planet.Renderer.GetRenderer<ISpherePlanetCloudRenderer>( );
				if ( cloudRenderer != null )
				{
					cloudRenderer.SetupCloudEffect( m_Technique.Effect );
				}
				if ( m_MarbleTexture != null )
				{
					m_Technique.Effect.Parameters[ "MarbleTexture" ].Set( m_MarbleTexture );
				}
				ITerrainPackTextureProvider packTextures = Planet.Renderer.GetRenderer<ITerrainPackTextureProvider>( );
				if ( packTextures == null )
				{
					return;
				}
				m_Technique.Effect.Parameters[ "TerrainPackTexture" ].Set( packTextures.PackTexture );
				m_Technique.Effect.Parameters[ "TerrainTypeTexture" ].Set( packTextures.LookupTexture );
				m_Technique.Apply( context, geometry );
			}
		}

		#region Private Members

		private ITexture m_MarbleTexture;
		private bool m_MarbleTextureBuilding;
		private readonly TechniqueSelector m_Technique;
		private readonly ISpherePlanetMarbleTextureBuilder m_TextureBuilder;

		/// <summary>
		/// Called when the texture builder has finished building the marble texture
		/// </summary>
		private void OnMarbleTextureBuilt( ITexture marbleTexture )
		{
			m_MarbleTexture = marbleTexture;
			m_MarbleTextureBuilding = false;
		}

		/// <summary>
		/// Updates the marble texture, if necessary
		/// </summary>
		private void UpdateMarbleTexture( )
		{
			bool rebuildRequired = m_MarbleTexture == null || m_TextureBuilder.RequiresRebuild( Planet );
			if ( rebuildRequired && !m_MarbleTextureBuilding )
			{
				m_MarbleTextureBuilding = true;
				m_TextureBuilder.QueueBuild( ExtendedThreadPool.Instance, Planet, OnMarbleTextureBuilt );
			}
		}

		#endregion
	}
}
