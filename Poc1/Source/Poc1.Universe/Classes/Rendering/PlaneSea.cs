using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Classes.Rendering
{
	class PlaneSea
	{
		/// <summary>
		/// Creates the sea mesh
		/// </summary>
		/// <param name="width">Mesh width</param>
		/// <param name="depth">Mesh depth</param>
		/// <param name="res">Mesh resolution</param>
		public unsafe PlaneSea( float width, float depth, int res )
		{
			VertexBufferFormat vbFormat = new VertexBufferFormat( );
			vbFormat.Add( VertexFieldSemantic.Position, VertexFieldElementTypeId.Float32, 3 );
			vbFormat.Add( VertexFieldSemantic.Normal, VertexFieldElementTypeId.Float32, 3 );

			m_Vertices = Graphics.Factory.CreateVertexBuffer( );
			m_Vertices.Create( vbFormat, res * res );

			using ( IVertexBufferLock vbLock = m_Vertices.Lock( false, false ) )
			{
				GenerateMeshVertices( ( Vertex* )vbLock.Bytes, width, depth, res );
			}

			m_Indices = Graphics.Factory.CreateIndexBuffer( );
			m_Indices.Create( CreateMeshIndices( res ), true );
		}

		/// <summary>
		/// Gets/sets the 
		/// </summary>
		public float SeaLevel
		{
			get { return m_SeaLevel; }
			set { m_SeaLevel = value; }
		}

		#region Private Members

		#region Vertex Struct

		private struct Vertex
		{

			public Point3 Position
			{
				set
				{
					m_X = value.X;
					m_Y = value.Y;
					m_Z = value.Z;
				}
			}

			public Vector3 Normal
			{
				set
				{
					m_Nx = value.X;
					m_Ny = value.Y;
					m_Nz = value.Z;
				}
			}

			private float m_X;
			private float m_Y;
			private float m_Z;
			private float m_Nx;
			private float m_Ny;
			private float m_Nz;
		}

		#endregion

		private float m_SeaLevel;
		private readonly IVertexBuffer m_Vertices;
		private readonly IIndexBuffer m_Indices;


		private static ushort[] CreateMeshIndices( int res )
		{
			ushort[] indices = new ushort[ res * res * 6 ];

			int triRes = res - 1;
			int index = 0;
			ushort baseIndex = 0;
			for ( int row = 0; row < triRes; ++row )
			{
				for ( int col = 0; col < triRes; ++col, ++baseIndex )
				{
					indices[ index++ ] = baseIndex;
					indices[ index++ ] = ( ushort )( baseIndex + 1 );
					indices[ index++ ] = ( ushort )( baseIndex + res );

					indices[ index++ ] = ( ushort )( baseIndex + 1 );
					indices[ index++ ] = ( ushort )( baseIndex + 1 + res );
					indices[ index++ ] = ( ushort )( baseIndex + res );
				}
			}


			return indices;
		}

		private static unsafe void GenerateMeshVertices( Vertex* vertices, float width, float depth, int res )
		{
			float incX = width / ( res - 1 );
			float incZ = depth / ( res - 1 );

			Vertex* curVertex = vertices;

			float z = -depth / 2;
			for ( int row = 0; row < res; ++row, z += incZ )
			{
				float x = -width / 2;
				for ( int col = 0; col < res; ++col, x += incX, ++curVertex )
				{
					curVertex->Position = new Point3( x, 0, z );
					curVertex->Normal = Vector3.YAxis;
				}
			}
		}

		#endregion
	}
}
