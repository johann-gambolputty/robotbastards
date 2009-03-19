using System;
using System.Collections.Generic;
using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Renderers;
using Poc1.Universe.Interfaces.Planets.Renderers.Patches;
using Poc1.Universe.Planets.Spherical.Renderers.Patches;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Planets.Renderers
{
	/// <summary>
	/// Abstract base class for rendering planetary terrain
	/// </summary>
	public class PlanetTerrainPatchRenderer : AbstractPlanetEnvironmentRenderer, IPlanetTerrainRenderer
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="technique">Technique used for rendering patches</param>
		public PlanetTerrainPatchRenderer( ITechnique technique )
		{
			Arguments.CheckNotNull( technique, "technique" );
			m_Technique = technique;
			m_PatchRenderer = new DelegateRenderable( RenderPatches );
		}

		#region IPlanetTerrainRenderer Members

		/// <summary>
		/// Refreshes the terrain
		/// </summary>
		public virtual void Refresh( )
		{
			m_RootPatches.Clear( );
			m_Vertices.Clear( );
			GC.Collect( );
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders the terrain
		/// </summary>
		/// <param name="context">Terrain rendering context</param>
		public override void Render( IRenderContext context )
		{
			if ( Planet == null )
			{
				throw new InvalidOperationException( "Can't render terrain patches without first setting planet object" );
			}

			GameProfiles.Game.Rendering.PlanetRendering.TerrainRendering.Begin( );

			UpdatePatches( UniCamera.Current );

			m_Vertices.VertexBuffer.Begin( );
			context.ApplyTechnique( m_Technique, m_PatchRenderer );
			m_Vertices.VertexBuffer.End( );

			GameProfiles.Game.Rendering.PlanetRendering.TerrainRendering.End( );

			//if ( DebugInfo.ShowPatchInfo )
			//{
			//    m_Patches[ 0 ].DebugRender( );
			//}

			if ( DebugInfo.ShowPendingTerrainBuildItemCount )
			{
				DebugText.Write( "Pending terrain build items: " + TerrainPatchBuilder.PendingBuildItems );
			}

			if ( DebugInfo.ShowTerrainLeafNodeCount )
			{
				int leafCount = 0;
				foreach ( TerrainPatch patch in m_RootPatches )
				{
					leafCount += patch.CountNodesWithVertexData( );
				}

				DebugText.Write( "Terrain leaf nodes: {0}", leafCount );
			}
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Gets the terrain patch vertex buffer manager
		/// </summary>
		protected TerrainPatchVertices Vertices
		{
			get { return m_Vertices; }
		}

		/// <summary>
		/// Gets the list of root patches
		/// </summary>
		protected List<TerrainPatch> RootPatches
		{
			get { return m_RootPatches; }
		}

		/// <summary>
		/// Assigns this to a planet
		/// </summary>
		protected override void AssignToPlanet( IPlanetRenderer renderer, bool remove )
		{
			renderer.TerrainRenderer = remove ? null : this;
		}

		#endregion

		#region Private Members

		private readonly ITechnique				m_Technique;
		private readonly TerrainPatchVertices	m_Vertices = new TerrainPatchVertices( );
		private readonly List<TerrainPatch>		m_RootPatches = new List<TerrainPatch>( );
		private readonly IRenderable			m_PatchRenderer;

		/// <summary>
		/// Updates patches prior to rendering
		/// </summary>
		/// <param name="camera">Camera that LOD is calculated relative to</param>
		private void UpdatePatches( IUniCamera camera )
		{
			Point3 localPos = Units.RenderUnits.MakeRelativePoint( Planet.Transform.Position, camera.Position );

			ITerrainPatchGenerator generator = ( ITerrainPatchGenerator )Planet.PlanetModel.TerrainModel;
			foreach ( TerrainPatch patch in m_RootPatches )
			{
				patch.UpdateLod( localPos, generator, camera );
			}
			foreach ( TerrainPatch patch in m_RootPatches )
			{
				patch.Update( camera, generator );
			}
		}

		/// <summary>
		/// Renders root patches
		/// </summary>
		/// <param name="context">Rendering context</param>
		private void RenderPatches( IRenderContext context )
		{
			foreach ( TerrainPatch rootPatch in m_RootPatches )
			{
				rootPatch.Render( context );
			}
		}

		#endregion

	}
}
