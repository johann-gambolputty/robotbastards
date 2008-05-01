using System.Runtime.InteropServices;
using Rb.Core.Maths;

namespace Poc1.TerrainPatchTest
{
	[StructLayout( LayoutKind.Sequential )]
	internal struct PatchVertex
	{
		public Point3 Position
		{
			get { return new Point3( m_X, m_Y, m_Z );}
			set
			{
				m_X = value.X;
				m_Y = value.Y;
				m_Z = value.Z;
			}
		}

		private float m_X;
		private float m_Y;
		private float m_Z;
	}
}
