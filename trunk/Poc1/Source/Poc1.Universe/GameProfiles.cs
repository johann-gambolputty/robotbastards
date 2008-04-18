using Rb.Core.Utils;

namespace Poc1.Universe
{
	/// <summary>
	/// 
	/// </summary>
	public class GameProfiles
	{
		public readonly static RootSection Game;

		public class RootSection : ProfileSection
		{
			public readonly RenderingSection Rendering;
			public readonly ProfileSection CloudGeneration;

			#region Setup

			public RootSection( ) :
				base( "Game" )
			{
				Rendering = new RenderingSection( this );
				CloudGeneration = new ProfileSection( this, "Cloud Generation" );
			}

			#endregion
		}

		public class PlanetRenderingSection : ProfileSection
		{
			public readonly ProfileSection FlatPlanetRendering;
			public readonly ProfileSection TerrainRendering;
			public readonly ProfileSection CloudRendering;
			
			public PlanetRenderingSection( ProfileSection parent ) :
				base( parent, "Planet Rendering" )
			{
				FlatPlanetRendering = new ProfileSection( this, "Flat Planet Rendering" );
				TerrainRendering = new ProfileSection( this, "Terrain Rendering" );
				CloudRendering = new ProfileSection( this, "Cloud Rendering" );
			}
		}

		public class RenderingSection : ProfileSection
		{
			public readonly ProfileSection StarSphereRendering;
			public readonly PlanetRenderingSection PlanetRendering;

			public RenderingSection( ProfileSection parent ) :
				base( parent, "Rendering" )
			{
				PlanetRendering = new PlanetRenderingSection( this );
				StarSphereRendering = new ProfileSection(this, "Star Sphere Rendering");
			}
		}

		#region Private Members

		static GameProfiles( )
		{
			Game = new RootSection( );
		}

		#endregion
	}
}