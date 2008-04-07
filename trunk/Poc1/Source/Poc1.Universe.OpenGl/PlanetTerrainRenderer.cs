
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
			m_Patch = new TerrainPatch( );
			m_Patch.Build( m_Builder );
		}

		public void Render( IRenderContext context )
		{
		}

		private TerrainPatch m_Patch;

		private TerrainPatchBuilder m_Builder = new TerrainPatchBuilder( );
	}
}
