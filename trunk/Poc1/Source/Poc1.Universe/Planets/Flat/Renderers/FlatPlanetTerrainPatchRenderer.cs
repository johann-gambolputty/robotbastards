using System;
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Planets.Renderers;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Planets.Flat.Renderers
{
	/// <summary>
	/// Terrain patch renderer for flat planets
	/// </summary>
	public class FlatPlanetTerrainPatchRenderer : PlanetTerrainPatchRenderer
	{
		#region Protected Members

		/// <summary>
		/// Creates a technique that is applied to render terrain patches
		/// </summary>
		protected override ITechnique CreatePatchTechnique( IPlanet planet )
		{
			throw new NotImplementedException( );
		}

		#endregion
	}
}
