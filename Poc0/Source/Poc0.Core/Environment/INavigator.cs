using Rb.Core.Maths;

namespace Poc0.Core.Environment
{
	interface IPathFollower
	{
		Point3 MoveForward( float distance );

		float DistanceAlongPath
		{
			get;
		}

		bool AtStartOfPath
		{
			get;
		}

		bool AtEndOfPath
		{
			get;
		}
	}

	interface IPath
	{
		IPathFollower CreateFollower( );
	}

	interface INavigator
	{
		IPath CreatePath( Matrix44 startFrame, Matrix44 endFrame );
	}
}
