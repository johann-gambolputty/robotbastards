using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Ocean renderer using planar geometry
	/// </summary>
	public class PlaneOceanRenderer : OceanRenderer
	{
		/// <summary>
		/// Creates the sea mesh
		/// </summary>
		/// <param name="width">Mesh width</param>
		/// <param name="depth">Mesh depth</param>
		/// <param name="res">Mesh resolution</param>
		public unsafe PlaneOceanRenderer( float width, float depth, int res ) :
			base( "Effects/Planets/planeOcean.cgfx" )
		{
			VertexBufferFormat vbFormat = CreateOceanVertexBufferFormat( );
			m_Vertices = Graphics.Factory.CreateVertexBuffer( );
			m_Vertices.Create( vbFormat, res * res );

			using ( IVertexBufferLock vbLock = m_Vertices.Lock( false, true ) )
			{
				GenerateMeshVertices( ( Vertex* )vbLock.Bytes, width, depth, res );
			}

			m_Indices = Graphics.Factory.CreateIndexBuffer( );
			m_Indices.Create( CreateMeshIndices( res ), true );
		}

		/// <summary>
		/// Gets/sets the sea level
		/// </summary>
		public float SeaLevel
		{
			get { return m_SeaLevel; }
			set { m_SeaLevel = value; }
		}

		public override void Render( IRenderContext context )
		{
			Graphics.Renderer.PushTransform( TransformType.LocalToWorld );
			Graphics.Renderer.Translate( TransformType.LocalToWorld, 0, SeaLevel, 0 );

			base.Render( context );

			Graphics.Renderer.PopTransform( TransformType.LocalToWorld );
		}

		#region Protected Members

		/// <summary>
		/// Renders ocean geometry
		/// </summary>
		/// <param name="context">Rendering context</param>
		protected override void RenderOcean( IRenderContext context )
		{
			m_Vertices.Begin( );
			m_Indices.Draw( PrimitiveType.TriList );
			m_Vertices.End( );
		}
		
		#endregion

		#region Private Members

		private float m_SeaLevel = 100;
		private readonly IVertexBuffer m_Vertices;
		private readonly IIndexBuffer m_Indices;

		private static ushort[] CreateMeshIndices( int res )
		{
			int triRes = res - 1;
			ushort[] indices = new ushort[ triRes * triRes * 6 ];

			int index = 0;
			for ( int row = 0; row < triRes; ++row )
			{
				ushort baseIndex = unchecked( ( ushort )( row * res ) );
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
					curVertex->Position = new Point3( x, 32, z );
					curVertex->Normal = Vector3.YAxis;
				}
			}
		}

		#endregion
	}
}
