using Rb.Core.Maths;

namespace Poc0.LevelEditor.Core
{

	public class LevelVertex
	{
		public LevelVertex( Point2 pt )
		{
			m_Position = pt;
			m_StartEdge = null;
			m_EndEdge = null;
		}

		public LevelEdge StartEdge
		{
			get { return m_StartEdge; }
			set { m_StartEdge = value; }
		}

		public LevelEdge EndEdge
		{
			get { return m_EndEdge; }
			set { m_EndEdge = value; }
		}

		public Point2 Position
		{
			get { return m_Position; }
			set { m_Position = value; }
		}

		private Point2		m_Position;
		private LevelEdge	m_StartEdge;
		private LevelEdge	m_EndEdge;
	}
}
