using System;
using Rb.Core.Maths;

namespace Poc0.Core.Environment
{
	[Serializable]
	public class EnvironmentCollisions : IEnvironmentCollisions
	{
		[Serializable]
		public class Node
		{
			public Node( Point2 start, Point2 end )
			{
				m_Plane = new Plane2( start, end );
			}

			public Node InFront
			{
				get { return m_InFront; }
				set { m_InFront = value; }
			}

			public Node Behind
			{
				get { return m_Behind; }
				set { m_Behind = value; }
			}

			public bool IsInside( Point3 pt )
			{
				if ( m_Plane.ClassifyPoint( pt.X, pt.Z, 0.01f ) == PlaneClassification.InFront )
				{
					return ( m_InFront == null ) || ( m_InFront.IsInside( pt ) );
				}
				return ( m_Behind != null ) && ( m_Behind.IsInside( pt ) );
			}

			public bool Check( Point2 start, Point2 end, float startT, float endT, ref float collisionT, ref Vector2 collisionNormal )
			{
				Line2Intersection intersection = Intersections2.GetLinePlaneIntersection( start, end, m_Plane );
				if ( intersection == null )
				{
					PlaneClassification edgeClass = m_Plane.ClassifyPoint( start, 0.01f );
					if ( edgeClass == PlaneClassification.On )
					{
						edgeClass = m_Plane.ClassifyPoint( end, 0.01f );
					}
					if ( edgeClass == PlaneClassification.InFront )
					{
						return ( m_InFront != null ) && m_InFront.Check( start, end, startT, endT, ref collisionT, ref collisionNormal );
					}
					else if ( edgeClass == PlaneClassification.Behind )
					{
						if ( m_Behind == null )
						{
							//	Line crossed through obstacle
							collisionT = startT;
							collisionNormal = m_Plane.Normal;
							return true;
						}
						return m_Behind.Check( start, end, startT, endT, ref collisionT, ref collisionNormal );
					}

					return false;
				}
				float intersectionT = startT + ( endT - startT ) * ( intersection.Distance / start.DistanceTo( end ) );
				if ( m_InFront != null )
				{
					if ( m_InFront.Check( start, intersection.IntersectionPosition, startT, intersectionT, ref collisionT, ref collisionNormal ) )
					{
						return true;
					}
				}
				if ( m_Behind != null )
				{
					if ( m_Behind.Check( intersection.IntersectionPosition, end, intersectionT, endT, ref collisionT, ref collisionNormal ) )
					{
						return true;
					}
				}
				else
				{
					collisionT = intersectionT;
					collisionNormal = m_Plane.Normal;
					return true;
				}

				return false;
			}

			private readonly Plane2 m_Plane;
			private Node m_Behind;
			private Node m_InFront;
		}

		public EnvironmentCollisions( Node root )
		{
			m_Root = root;
		}

		#region IEnvironmentCollisions Members

		public bool IsPointInObstacle( Point3 pt )
		{
			return ( m_Root == null ) ? false : !m_Root.IsInside( pt );
		}

		public Collision CheckMovement( Point3 pos,  Vector3 move )
		{
			if ( m_Root == null )
			{
				return null;
			}
			Point3 end3 = pos + move;
			Point2 start = new Point2( pos.X, pos.Z );
			Point2 end = new Point2( end3.X, end3.Z );
			float collisionT = 0;
			Vector2 collisionNormal = Vector2.XAxis;
			if ( !m_Root.Check( start, end, 0, 1, ref collisionT, ref collisionNormal ) )
			{
				return null;
			}

			return new Collision( pos + move * collisionT, new Vector3( collisionNormal.X, 0, collisionNormal.Y ), move.Length * collisionT, collisionT );
		}

		#endregion

		private readonly Node m_Root;
	}
}
