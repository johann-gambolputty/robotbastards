using Rb.Core.Maths;

namespace Poc0.Core.Environment
{
	public class Collision
	{
		public Collision( Point3 pt, Vector3 normal, float dist, float t )
		{
			m_Point = pt;
			m_Normal = normal;
			m_Distance = dist;
			m_T = t;
		}

		#region Public properties

		public Point3 CollisionPoint
		{
			get { return m_Point; }
		}

		public Vector3 CollisionNormal
		{
			get { return m_Normal; }
		}

		public float DistanceToCollision
		{
			get { return m_Distance; }
		}

		public float T
		{
			get { return m_T; }
		}

		#endregion

		private readonly Point3 m_Point;
		private readonly Vector3 m_Normal;
		private readonly float m_Distance;
		private readonly float m_T;
	}

	public interface IEnvironmentCollisions
	{
		bool IsPointInObstacle( Point3 pt );

		Collision CheckMovement( Point3 pos, Vector3 move );
	}
}
