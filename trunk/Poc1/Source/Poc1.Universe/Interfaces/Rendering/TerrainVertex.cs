using System.Runtime.InteropServices;
using Rb.Core.Maths;

namespace Poc1.Universe.Interfaces.Rendering
{
	/// <summary>
	/// Vertex type used for terrain rendering
	/// </summary>
	[StructLayout( LayoutKind.Sequential, Pack = 0 )]
	public struct TerrainVertex
	{
		/// <summary>
		/// Sets the vertex position
		/// </summary>
		public void SetPosition( float x, float y, float z )
		{
			m_X = x;
			m_Y = y;
			m_Z = z;
		}

		/// <summary>
		/// Sets the vertex normal
		/// </summary>
		public void SetNormal( float x, float y, float z )
		{
			m_NormalX = x;
			m_NormalY = y;
			m_NormalZ = z;
		}

		/// <summary>
		/// Gets/sets the vertex position
		/// </summary>
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

		/// <summary>
		/// Gets/sets the vertex normal
		/// </summary>
		public Vector3 Normal
		{
			get { return new Vector3( m_NormalX, m_NormalY, m_NormalZ ); }
			set
			{
				m_NormalX = value.X;
				m_NormalY = value.Y;
				m_NormalZ = value.Z;
			}
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
