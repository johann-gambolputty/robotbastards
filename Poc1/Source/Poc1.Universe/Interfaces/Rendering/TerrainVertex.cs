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
		public unsafe void CopyTo( TerrainVertex* vertex, Vector3 offset )
		{
			vertex->m_X = m_X + offset.X;
			vertex->m_Y = m_Y + offset.Y;
			vertex->m_Z = m_Z + offset.Z;

			vertex->m_NormalX = m_NormalX;
			vertex->m_NormalY = m_NormalY;
			vertex->m_NormalZ = m_NormalZ;

			vertex->m_U = m_U;
			vertex->m_V = m_V;

			vertex->m_Slope = m_Slope;
			vertex->m_Elevation = m_Elevation;
		}

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

		/// <summary>
		/// Gets/sets the vertex terrain UV coordinate
		/// </summary>
		public Point2 TerrainUv
		{
			get { return new Point2( m_U, m_V ); }
			set
			{
				m_U = value.X;
				m_V = value.Y;
			}
		}

		/// <summary>
		/// Gets/sets vertex x coordinate
		/// </summary>
		public float X
		{
			get { return m_X; }
			set { m_X = value; }
		}

		/// <summary>
		/// Gets/sets vertex y coordinate
		/// </summary>
		public float Y
		{
			get { return m_Y; }
			set { m_Y = value; }
		}

		/// <summary>
		/// Gets/sets vertex z coordinate
		/// </summary>
		public float Z
		{
			get { return m_Z; }
			set { m_Z = value; }
		}

		/// <summary>
		/// Gets/sets vertex normal x component
		/// </summary>
		public float NormalX
		{
			get { return m_NormalX; }
			set { m_NormalX = value; }
		}

		/// <summary>
		/// Gets/sets vertex normal y component
		/// </summary>
		public float NormalY
		{
			get { return m_NormalY; }
			set { m_NormalY = value; }
		}

		/// <summary>
		/// Gets/sets vertex normal z component
		/// </summary>
		public float NormalZ
		{
			get { return m_NormalZ; }
			set { m_NormalZ = value; }
		}

		public float Slope
		{
			get { return m_Slope; }
			set { m_Slope = value; }
		}

		public float Elevation
		{
			get { return m_Elevation; }
			set { m_Elevation = value; }
		}

		#region Private Members

		private float m_X;
		private float m_Y;
		private float m_Z;

		private float m_NormalX;
		private float m_NormalY;
		private float m_NormalZ;

		private float m_U;
		private float m_V;

		private float m_Slope;
		private float m_Elevation;

		#endregion
	}
}
