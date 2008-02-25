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
		public static IEnvironmentCollisions Build( LevelGeometry level )
		{
			Csg2.Node srcNode = Csg2.BuildExpansion( level.ObstaclePolygons, 1.0f );

			EnvironmentCollisions.Node node = Build( srcNode );
			EnvironmentCollisions impl = new EnvironmentCollisions( node );
			return impl;
		}

		/// <summary>
		/// Builds the collision BSP tree from a source CSG tree
		/// </summary>
		private static EnvironmentCollisions.Node Build( Csg2.Node srcNode )
		{
			EnvironmentCollisions.Node newNode = new EnvironmentCollisions.Node( srcNode.Edge.Start, srcNode.Edge.End );

			if ( srcNode.InFront != null )
			{
				newNode.InFront = Build( srcNode.InFront );
			}
			if ( srcNode.Behind != null )
			{
				newNode.Behind = Build( srcNode.Behind );
			}

			return newNode;
		}
	}
}
