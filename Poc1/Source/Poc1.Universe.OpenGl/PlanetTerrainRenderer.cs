
using System.Collections.Generic;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.OpenGl
{
	/// <summary>
	/// Only one planet at a time can be close enough to render using terrain patches. This class manages the rendering of the planet
	/// </summary>
	public class PlanetTerrainRenderer
	{
		public PlanetTerrainRenderer( )
		{
			TerrainPatch lastPatch = null;
			for ( int i = 0; i < 2; ++i )
			{
				TerrainPatch newPatch = new TerrainPatch( );
				newPatch.Frame.Translate( 0, 0, i * TerrainPatch.PatchHeight );
				newPatch.TopPatch = lastPatch;
				newPatch.Build( m_Builder );
				m_Patches.Add( newPatch );
				lastPatch = newPatch;
			}

			m_PatchRenderState = Graphics.Factory.CreateRenderState( );
			m_PatchRenderState.DepthTest = false;
			m_PatchRenderState.FaceRenderMode = PolygonRenderMode.Lines;
		}

		private IRenderState m_PatchRenderState;

		public void Render( IRenderContext context )
		{
			m_PatchRenderState.Begin( );
			m_Builder.BeginPatchRendering( );

			foreach ( TerrainPatch patch in m_Patches )
			{
				patch.Render( );
			}
			m_Builder.EndPatchRendering( );
			m_PatchRenderState.End( );
		}

		private readonly List<TerrainPatch> m_Patches = new List<TerrainPatch>( );
		private TerrainPatchBuilder m_Builder = new TerrainPatchBuilder( );
	}
}
