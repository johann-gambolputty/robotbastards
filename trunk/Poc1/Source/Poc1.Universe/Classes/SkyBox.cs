using System.Drawing;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.Universe.Classes
{
	/// <summary>
	/// Sky box rendering
	/// </summary>
	public class SkyBox : IRenderable
	{
		public SkyBox( )
		{
			float hDim = 100;
			Vertex[] vertices = new Vertex[ 8 ]
				{
					new Vertex( -hDim, -hDim, -hDim, Color.Red ),
					new Vertex( +hDim, -hDim, -hDim, Color.Red ),
					new Vertex( +hDim, +hDim, -hDim, Color.Red ),
					new Vertex( -hDim, +hDim, -hDim, Color.Red ),
					
					new Vertex( -hDim, -hDim, +hDim, Color.Black ),
					new Vertex( +hDim, -hDim, +hDim, Color.Black ),
					new Vertex( +hDim, +hDim, +hDim, Color.Black ),
					new Vertex( -hDim, +hDim, +hDim, Color.Black )
				};

			int[] indices = new int[]
				{
				//	Tri 0		Tri 1
					0, 1, 3,	1, 2, 3,	//	-z
					7, 6, 4,	6, 5, 4,	//	+z
					4, 5, 0,	5, 1, 0,	//	+y
					6, 7, 2,	7, 3, 2,	//	-y
					0, 3, 4,	3, 7, 4,	//	-x
					5, 6, 1,	6, 2, 1		//	+x
				};

			VertexBufferData vbData = VertexBufferData.FromVertexCollection( vertices );
			vbData.Format.Static = true;
			m_Vb = Graphics.Factory.CreateVertexBuffer( vbData );


			IndexBufferData ibData = IndexBufferData.FromIndexArray( indices );
			ibData.Format.Static = true;
			m_Ib = Graphics.Factory.CreateIndexBuffer( ibData );

			m_RState = Graphics.Factory.CreateRenderState( );
			m_RState.CullFaces = false;
			m_RState.FaceRenderMode = PolygonRenderMode.Fill;
		}

		private readonly IRenderState	m_RState;
		private readonly IVertexBuffer	m_Vb;
		private readonly IIndexBuffer	m_Ib;

		private class Vertex
		{
			public Vertex( float x, float y, float z, Color colour )
			{
				m_Pos = new Point3( x, y, z );
				m_Colour = colour;
			}

			[VertexField( VertexFieldSemantic.Position ) ]
			private Point3 m_Pos;

			[VertexField( VertexFieldSemantic.Diffuse )]
			private Color m_Colour;
		}

		#region IRenderable Members

		public void Render( IRenderContext context )
		{
			Graphics.Renderer.PushTransform( TransformType.WorldToView, Matrix44.Identity );
			Graphics.Renderer.PushRenderState( m_RState );
			m_Vb.Begin( );
			m_Ib.Draw( PrimitiveType.TriList );
			m_Vb.End( );
			Graphics.Renderer.PopRenderState( );
			Graphics.Renderer.PopTransform( TransformType.WorldToView );
		}

		#endregion
	}
}
