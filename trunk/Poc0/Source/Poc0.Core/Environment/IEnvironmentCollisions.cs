using Rb.Core.Maths;

namespace Poc0.Core.Environment
{
	public class Collision
	{
		public Collision( Point3 pt, Vector3 normal, float dist )
		{
			m_Point = pt;
			m_Normal = normal;
			m_Distance = dist;
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

		#endregion

		private readonly Point3 m_Point;
		private readonly Vector3 m_Normal;
		private readonly float m_Distance;
	}

	public interface IEnvironmentCollisions
	{
		Collision CheckMovement( Point3 pos, Vector3 move );
	}
}
