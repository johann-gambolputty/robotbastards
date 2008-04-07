
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
			m_TestPatch = new TerrainPatch( );
			m_TestPatch.Build( m_Builder );
		}

		public void Render( IRenderContext context )
		{
			m_TestPatch.Render( context );
		}

		private readonly TerrainPatch m_TestPatch;
		private readonly TerrainPatchBuilder m_Builder = new TerrainPatchBuilder( );
	}
}
