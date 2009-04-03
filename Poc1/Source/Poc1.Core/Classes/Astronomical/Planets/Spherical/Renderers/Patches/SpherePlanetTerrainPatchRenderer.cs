using Poc1.Core.Classes.Astronomical.Planets.Renderers.Patches;
using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets;
using Poc1.Core.Interfaces.Astronomical.Planets.Renderers.Patches;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical;
using Rb.Core.Maths;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers.Patches
{
	/// <summary>
	/// Terrain renderer for spherical planets
	/// </summary>
	public class SpherePlanetTerrainPatchRenderer : AbstractPlanetTerrainPatchRenderer
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="terrainGenerator">Terrain generator</param>
		public SpherePlanetTerrainPatchRenderer( ITerrainPatchGenerator terrainGenerator ) :
			base( terrainGenerator )
		{	
		}

		/// <summary>
		/// Refreshes the renderer (empties terrain patch quad trees)
		/// </summary>
		public override void Refresh( IPlanet planet )
		{
			base.Refresh( planet );
			Units.Metres radius = ( ( ISpherePlanet )planet ).Model.Radius.ToMetres;
			float uvRes = ( float )( radius / 5000.0 );
			CreateCubePatches( 20, 1, uvRes );
		}

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
