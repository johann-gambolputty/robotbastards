using Poc0.Core.Environment;

namespace Poc0.LevelEditor.Core.Geometry
{
	/// <summary>
	/// Builds an <see cref="IEnvironmentCollisions"/> implementation
	/// </summary>
	internal class EnvironmentCollisionsBuilder
	{
		/// <summary>
		/// Builds an <see cref="IEnvironmentCollisions"/> object
		/// </summary>
		public static IEnvironmentCollisions Build( LevelGeometry level, float expansion )
		{
			Csg2.Node srcNode = expansion == 0 ? Csg2.Build( level.ObstaclePolygons ) : Csg2.BuildExpansion( level.ObstaclePolygons, 1.5f );

			EnvironmentCollisions.Node node = Build( srcNode );
			EnvironmentCollisions impl = new EnvironmentCollisions( node );
			return impl;
		}

		/// <summary>
		/// Builds the collision BSP tree from a source CSG tree
		/// </summary>
		private static EnvironmentCollisions.Node Build( Csg2.Node srcNode )
		{
			//	Note: Source node edges and child node assignments are reversed, because the source BSP is built from obstacle polys, wheras we want the floor BSP...
			EnvironmentCollisions.Node newNode = new EnvironmentCollisions.Node( srcNode.Edge.End, srcNode.Edge.Start );

			if ( srcNode.InFront != null )
			{
				newNode.Behind = Build( srcNode.InFront );
			}
			if ( srcNode.Behind != null )
			{
				newNode.InFront = Build( srcNode.Behind );
			}

			return newNode;
		}
	}
}
