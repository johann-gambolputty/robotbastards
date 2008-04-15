using System.Runtime.InteropServices;
using Rb.Core.Maths;

namespace Poc1.Universe.OpenGl
{
	[StructLayout( LayoutKind.Sequential, Pack = 0 )]
	internal struct TerrainVertex
	{
		public void SetPosition( float x, float y, float z )
		{
			m_X = x;
			m_Y = y;
			m_Z = z;
		}

		public void SetNormal( float x, float y, float z )
		{
			m_NormalX = x;
			m_NormalY = y;
			m_NormalZ = z;
		}

		public Point3 Position
		{
			get { return new Point3( m_X, m_Y, m_Z ); }
		}

		#region Private Members

		private float m_X;
		private float m_Y;
		private float m_Z;

		private float m_NormalX;
		private float m_NormalY;
		private float m_NormalZ;

		#endregion
	}
}
