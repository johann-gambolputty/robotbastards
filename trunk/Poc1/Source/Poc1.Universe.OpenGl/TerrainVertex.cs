using System.Runtime.InteropServices;
using Rb.Core.Maths;

namespace Poc1.Universe.OpenGl
{
	[StructLayout( LayoutKind.Sequential, Pack = 0 )]
	internal struct TerrainVertex
	{
		public Point3 Position
		{
			get { return new Point3( m_X, m_Y, m_Z ); }
			set
			{
				m_X = value.X;
				m_Y = value.Y;
				m_Z = value.Z;
			}
		}

		public TerrainVertex( Point3 position )
		{
			m_X = position.X;
			m_Y = position.Y;
			m_Z = position.Z;
		}

		public float X
		{
			get { return m_X; }
			set { m_X = value; }
		}

		public float Y
		{
			get { return m_Y; }
			set { m_Y = value; }
		}

		public float Z
		{
			get { return m_Z; }
			set { m_Z = value; }
		}

		private float m_X;
		private float m_Y;
		private float m_Z;
	}
}
