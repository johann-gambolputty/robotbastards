
using Rb.Core.Maths;

namespace Poc0.LevelEditor.Core
{
	public class LevelEdge
	{
		public LevelEdge( LevelVertex start, LevelVertex end, LevelPolygon owner, bool doubleSided )
		{
			m_Start = start;
			m_End = end;
			m_Owner = owner;
			m_DoubleSided = doubleSided;

			m_Start.StartEdge = this;
			m_End.EndEdge = this;
		}

		public bool IsDoubleSided
		{
			get { return m_DoubleSided; }
		}

		public LevelPolygon Polygon
		{
			get { return m_Owner; }
		}

		public LevelVertex Start
		{
			get { return m_Start; }
			set { m_Start = value; }
		}

		public LevelVertex End
		{
			get { return m_End; }
			set { m_End = value; }
		}

		public LevelEdge PreviousEdge
		{
			get { return m_PrevEdge; }
			set { m_PrevEdge = value; }
		}

		public LevelEdge NextEdge
		{
			get { return m_NextEdge; }
			set { m_NextEdge = value; }
		}

		public static void LinkEdges( LevelEdge edge0, LevelEdge edge1 )
		{
			edge0.NextEdge = edge1;
			edge1.PreviousEdge = edge0;
		}

		public float SqrDistanceTo( Point2 pt )
		{
			Vector2 lineVec = m_End.Position - m_Start.Position;
			float sqrLength = lineVec.SqrLength;
			float t = 0.0f;

			if ( sqrLength != 0.0f )
			{
				t = Utils.Clamp( ( pt - m_Start.Position ).Dot( lineVec ) / sqrLength, 0.0f, 1.0f );
			}

			Point2 closestPt = m_Start.Position + ( lineVec * t );
			return closestPt.SqrDistanceTo( pt );
		}

		private LevelEdge m_PrevEdge;
		private LevelEdge m_NextEdge;
		private LevelVertex m_Start;
		private LevelVertex m_End;
		private readonly bool m_DoubleSided;
		private readonly LevelPolygon m_Owner;
	}
}
