using System.Drawing;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Cameras;
using RbGraphics = Rb.Rendering.Graphics;


namespace Poc1.TerrainPatchTest
{
	class QuadPatchTree : IRenderable
	{
		public const float Width = 128;
		public const float Height = 128;

		public QuadPatchTree( Terrain terrain )
		{
			terrain.SetTerrainArea( Width, Height );
			m_Vertices = new QuadPatchVertices( );
			m_Root = new QuadPatch( terrain, m_Vertices, Color.Black, -Width / 2, -Height / 2, Width, Height );
		}

		public float CameraDistanceToPatch
		{
			get { return m_CameraDistanceToPatch; }
			set { m_CameraDistanceToPatch = value; }
		}

		public bool UseCameraDist
		{
			get { return m_UseCameraDist; }
			set { m_UseCameraDist = value; }
		}

		#region IRenderable Members

		public void Render( IRenderContext context )
		{
			ICamera3 camera = ( ( ICamera3 )RbGraphics.Renderer.Camera );
			if ( UseCameraDist )
			{
				RbGraphics.Fonts.DebugFont.Write( 0, 0, Color.White, "Using actual camera distance" );
				m_Root.UpdateLod( camera.Frame.Translation );
			}
			else
			{
				RbGraphics.Fonts.DebugFont.Write( 0, 0, Color.White, "Using simulated camera distance {0}", CameraDistanceToPatch );
				m_Root.UpdateLod( CameraDistanceToPatch );
			}

			m_Root.Update( ( IProjectionCamera )camera );

			m_Vertices.VertexBuffer.Begin( );
			m_Root.Render( );
			m_Vertices.VertexBuffer.End( );
		}

		#endregion

		private float m_CameraDistanceToPatch = 360.0f;
		private bool m_UseCameraDist;
		private QuadPatchVertices m_Vertices;
		private QuadPatch m_Root;
	}
}
