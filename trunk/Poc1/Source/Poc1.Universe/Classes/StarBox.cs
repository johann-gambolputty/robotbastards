using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Poc1.Universe.Classes.Cameras;
using Rb.Assets;
using Rb.Assets.Interfaces;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.Universe.Classes
{
	/// <summary>
	/// Sky box rendering
	/// </summary>
	public class StarBox : IRenderable
	{
		public StarBox( )
		{
			float hDim = 5.0f;
			Vertex[] vertices = new Vertex[ 8 ]
				{
					new Vertex( -hDim, -hDim, -hDim ),
					new Vertex( +hDim, -hDim, -hDim ),
					new Vertex( +hDim, +hDim, -hDim ),
					new Vertex( -hDim, +hDim, -hDim ),
												
					new Vertex( -hDim, -hDim, +hDim ),
					new Vertex( +hDim, -hDim, +hDim ),
					new Vertex( +hDim, +hDim, +hDim ),
					new Vertex( -hDim, +hDim, +hDim )
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
			m_Vb = Graphics.Factory.CreateVertexBuffer( );
			m_Vb.Create( vbData );

			m_Ib = Graphics.Factory.CreateIndexBuffer( );
			m_Ib.Create( indices, true );

			m_RState = Graphics.Factory.CreateRenderState( );
			m_RState.CullFaces = false;
			m_RState.FaceRenderMode = PolygonRenderMode.Fill;
			m_RState.DepthWrite = false;
			m_RState.DepthTest = false;

			ICubeMapTexture texture = Graphics.Factory.CreateCubeMapTexture( );
			LoadStarBoxTextures( texture, "Textures/StarField0/" );


			int imageIndex = 0;
			foreach ( Bitmap bmp in texture.ToBitmaps( ) )
			{
				bmp.Save( "StarBoxCubeMapFace" + imageIndex++ + ".bmp", ImageFormat.Bmp );
			}

			m_Sampler = Graphics.Factory.CreateCubeMapTextureSampler( );
			m_Sampler.Texture = texture;
		}

		/// <summary>
		/// Loads star box textures from a given folder into a cube map texture
		/// </summary>
		private static void LoadStarBoxTextures( ICubeMapTexture texture, string path )
		{
			texture.Build
				(
					LoadCubeMapFace( path + "starBox_rt.jpg" ),
					LoadCubeMapFace( path + "starBox_lf.jpg" ),
					LoadCubeMapFace( path + "starBox_up.jpg" ),
					LoadCubeMapFace( path + "starBox_dn.jpg" ),
					LoadCubeMapFace( path + "starBox_fr.jpg" ),
					LoadCubeMapFace( path + "starBox_bk.jpg" ),
					true
				);
		}

		private static Bitmap LoadCubeMapFace( string path )
		{
			ILocation location = Locations.NewLocation( path );
			if ( location == null )
			{
				throw new ArgumentException( string.Format( "Failed to find asset location \"{0}\"", path ) );
			}
			using ( Stream stream = ( ( IStreamSource )location ).Open( ) )
			{
				return ( Bitmap )Image.FromStream( stream );
			}
		}

		private readonly IRenderState			m_RState;
		private readonly IVertexBuffer			m_Vb;
		private readonly IIndexBuffer			m_Ib;
		private readonly ICubeMapTextureSampler	m_Sampler;

		private class Vertex
		{
			public Vertex( float x, float y, float z )
			{
				m_Pos = new Point3( x, y, z );
				m_Normal = new Vector3( x, y, z ).MakeNormal( );
			}

			[VertexField( VertexFieldSemantic.Position ) ]
			private Point3 m_Pos;

			[VertexField( VertexFieldSemantic.Normal )]
			private Vector3 m_Normal;

		}

		#region IRenderable Members

		/// <summary>
		/// Renders the star box
		/// </summary>
		public void Render( IRenderContext context )
		{
			Matrix44 mat = new Matrix44( UniCamera.Current.Frame );
			mat.Transpose( );
			Graphics.Renderer.PushTransform( TransformType.Texture0, mat );
			Graphics.Renderer.PushRenderState( m_RState );
			m_Sampler.Begin( );
			m_Vb.Begin( );
			m_Ib.Draw( PrimitiveType.TriList );
			m_Vb.End( );
			m_Sampler.End( );
			Graphics.Renderer.PopRenderState( );
			Graphics.Renderer.PopTransform( TransformType.Texture0 );
		}

		#endregion
	}
}
