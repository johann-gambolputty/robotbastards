
using System.Collections.Generic;
using Rb.Core.Maths;

namespace Poc0.Core.Environment
{
	class LinePathFollower : IPathFollower
	{
		public LinePathFollower( LinePath path )
		{
		}

		#region IPathFollower Members

		public Point3 MoveForward( float distance )
		{
			throw new System.Exception("The method or operation is not implemented.");
		}

		public float DistanceAlongPath
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		public bool AtStartOfPath
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		public bool AtEndOfPath
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		#endregion
	}

	enum PathFollowDirection
	{
		Forwards,
		Backwards
	}

	class MultiplePathFollower : IPathFollower
	{
		public void AddPath( IPath path, PathFollowDirection direction )
		{
			m_Followers.Add( path.CreateFollower( ) );
		}

		private readonly List< IPathFollower > m_Followers = new List< IPathFollower >( );
		private Point3 m_CurPoint;

		#region IPathFollower Members

		public Point3 MoveForward( float distance )
		{
			if ( m_Followers.Count == 0 )
			{
				return m_CurPoint;
			}

			IPathFollower follower = m_Followers[ 0 ];
			m_CurPoint = follower.MoveForward( distance );
			
			if ( follower.AtEndOfPath )
			{
				//	TODO: AP: Move along next path by distance remainder
				m_Followers.RemoveAt( 0 );
			}

			return m_CurPoint;
		}

		public float DistanceAlongPath
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		public bool AtStartOfPath
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		public bool AtEndOfPath
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		#endregion
	}

	class LinePath : IPath
	{
		public LinePath( Point3[] points, bool loop )
		{
			m_Points = points;
			m_Loop = loop;
		}

		public bool Loop
		{
			get { return m_Loop; }
		}

		public Point3[] Points
		{
			get { return m_Points; }
		}

		#region IPath Members

		public IPathFollower CreateFollower( )
		{
			throw new System.Exception("The method or operation is not implemented.");
		}

		#endregion

		#region Private members

		private readonly Point3[] m_Points;
		private readonly bool m_Loop;

		#endregion
	}

	class Navigator : INavigator
	{
		#region INavigator Members

		public IPath CreatePath( Matrix44 startFrame, Matrix44 endFrame )
		{
			throw new System.Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}
