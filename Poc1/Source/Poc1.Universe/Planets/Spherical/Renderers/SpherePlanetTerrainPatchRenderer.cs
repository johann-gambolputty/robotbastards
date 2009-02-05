using Poc1.Universe.Interfaces.Planets.Spherical;
using Poc1.Universe.Interfaces.Planets.Spherical.Renderers;
using Poc1.Universe.Planets.Renderers;
using Poc1.Universe.Planets.Spherical.Renderers.Patches;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Planets.Spherical.Renderers
{
	/// <summary>
	/// Terrain renderer for spherical planets
	/// </summary>
	public class SpherePlanetTerrainPatchRenderer : PlanetTerrainPatchRenderer, ISpherePlanetTerrainRenderer
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SpherePlanetTerrainPatchRenderer( ) :
			base( "Effects/Planets/terrestrialPlanetTerrain.cgfx" )
		{
		}

		#region Public Members

		/// <summary>
		/// Gets the owner planet
		/// </summary>
		public ISpherePlanet SpherePlanet
		{
			get { return ( ISpherePlanet )Planet; }
		}

		#endregion

		#region IPlanetTerrainRenderer Members

		/// <summary>
		/// Refreshes the renderer (empties terrain patch quad trees)
		/// </summary>
		public override void Refresh( )
		{
			base.Refresh( );
			if ( Planet == null )
			{
				return;
			}
			float uvRes = ( float )( SpherePlanet.PlanetModel.Radius.ToMetres / 5000.0 );
			CreateCubePatches( 20, 1, uvRes );
		}

		#endregion

		#region Protected Members
		protected override void SetupTerrainEffect( IEffect effect )
		{
			base.SetupTerrainEffect( effect );
			effect.Parameters[ "PlanetRadius" ].Set( SpherePlanet.PlanetModel.Radius.ToRenderUnits );
		}
		
		#endregion

		#region Private Members

		/// <summary>
		/// Adds patches that cover the side of a cube
		/// </summary>
		/// <param name="res">Patch resolution (res*res patches cover the cube side)</param>
		/// <param name="topLeft">The top left corner of the cube side</param>
		/// <param name="topRight">The top right corner of the cube side</param>
		/// <param name="bottomLeft">The bottom left corner of the cube side</param>
		/// <param name="uvRes">The UV resolution of the cube patch</param>
		private void AddCubeSidePatches( int res, Point3 topLeft, Point3 topRight, Point3 bottomLeft, float uvRes )
		{
			Vector3 xAxis = ( topRight - topLeft );
			Vector3 yAxis = ( bottomLeft - topLeft );
			Vector3 xInc = xAxis / res;
			Vector3 yInc = yAxis / res;
			Point3 rowStart = topLeft;

			for ( int row = 0; row < res; ++row )
			{
				Point3 curPos = rowStart;
				for ( int col = 0; col < res; ++col )
				{
					TerrainPatch newPatch = new TerrainPatch( Vertices, curPos, xInc, yInc, new Point2( 0, 0 ), uvRes );
					RootPatches.Add( newPatch );

					curPos += xInc;
				}
				rowStart += yInc;
			}
		}

		/// <summary>
		/// Creates 8 corner points for a cube with a half-width of hDim
		/// </summary>
		private static Point3[] CreateCubeCorners( float hDim )
		{
			Point3[] corners = new Point3[]
				{
					new Point3( -hDim, -hDim, -hDim ),
					new Point3( +hDim, -hDim, -hDim ),
					new Point3( +hDim, +hDim, -hDim ),
					new Point3( -hDim, +hDim, -hDim ),
					
					new Point3( -hDim, -hDim, +hDim ),
					new Point3( +hDim, -hDim, +hDim ),
					new Point3( +hDim, +hDim, +hDim ),
					new Point3( -hDim, +hDim, +hDim ),
				};
			return corners;
		}

		/// <summary>
		/// Creates patches on each side of a cube
		/// </summary>
		private void CreateCubePatches( float hDim, int res, float uvRes )
		{
			Point3[] corners = CreateCubeCorners( hDim );

			AddCubeSidePatches( res, corners[ 7 ], corners[ 6 ], corners[ 4 ], uvRes );	//	+z
			AddCubeSidePatches( res, corners[ 0 ], corners[ 1 ], corners[ 3 ], uvRes );	//	-z
			AddCubeSidePatches( res, corners[ 4 ], corners[ 5 ], corners[ 0 ], uvRes );	//	+y
			AddCubeSidePatches( res, corners[ 6 ], corners[ 7 ], corners[ 2 ], uvRes );	//	-y
			AddCubeSidePatches( res, corners[ 5 ], corners[ 6 ], corners[ 1 ], uvRes );	//	+x
			AddCubeSidePatches( res, corners[ 0 ], corners[ 3 ], corners[ 4 ], uvRes );	//	-x
		}

		#endregion
	}
}
