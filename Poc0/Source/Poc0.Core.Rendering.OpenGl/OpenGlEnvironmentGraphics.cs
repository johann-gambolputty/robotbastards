using Poc0.Core.Environment;
using Rb.Core.Maths;
using Rb.Rendering;
using Tao.OpenGl;

namespace Poc0.Core.Rendering.OpenGl
{
	public class OpenGlEnvironmentGraphics : EnvironmentGraphics
	{
		private readonly RenderState m_WallState;
		private readonly RenderState m_FloorState;

		public OpenGlEnvironmentGraphics( )
		{
			m_WallState = Graphics.Factory.NewRenderState( );
			m_WallState.DisableLighting( );
			m_WallState.SetDepthTest( DepthTestPass.Always );
			m_WallState.SetColour( System.Drawing.Color.DarkOrange );

			m_FloorState = Graphics.Factory.NewRenderState( );
			m_FloorState.DisableLighting( );
			m_FloorState.SetDepthTest( DepthTestPass.Always );
			m_FloorState.SetColour( System.Drawing.Color.Blue );
		}

		public override void Render( IRenderContext context )
		{
			if ( Walls == null )
			{
				return;
			}

			Graphics.Renderer.PushRenderState( m_WallState );

			Gl.glBegin( Gl.GL_QUADS );

			RenderWall( context, Walls );

			Gl.glEnd( );
			
			Graphics.Renderer.PopRenderState( );
			
			Graphics.Renderer.PushRenderState( m_FloorState );

			Gl.glBegin( Gl.GL_TRIANGLES );
			RenderFloors( context, Walls );
			Gl.glEnd( );

			Graphics.Renderer.PopRenderState( );
		}

		#region Private members

		private static void RenderFloors( IRenderContext context, WallNode wall )
		{
			if ( wall.Floor != null )
			{
				Point2[] points = wall.Floor.Points;
				Point2 basePos = points[ 0 ];
				float height = wall.Floor.Height;
				for ( int vertexIndex = 1; vertexIndex < points.Length - 1; ++vertexIndex )
				{
					Gl.glVertex3f( basePos.X, height, basePos.Y );
					Gl.glVertex3f( points[ vertexIndex ].X, height, points[ vertexIndex ].Y );
					Gl.glVertex3f( points[ vertexIndex + 1 ].X, height, points[ vertexIndex + 1 ].Y );
				}
			}
			if ( wall.InFront != null )
			{
				RenderFloors( context, wall.InFront );
			}
			if ( wall.Behind != null )
			{
				RenderFloors( context, wall.Behind );
			}
		}

		private static void RenderWall( IRenderContext context, WallNode wall )
		{
			Gl.glVertex3f( wall.Start.X, 0, wall.Start.Y );
			Gl.glVertex3f( wall.End.X, 0, wall.End.Y );
			Gl.glVertex3f( wall.End.X, wall.Height, wall.End.Y );
			Gl.glVertex3f( wall.Start.X, wall.Height, wall.Start.Y );

			if ( wall.Floor != null )
			{
				//	TODO: AP: ... Render floor ...
			}
			if ( wall.InFront != null )
			{
				RenderWall( context, wall.InFront );
			}
			if ( wall.Behind != null )
			{
				RenderWall( context, wall.Behind );
			}
		}

		#endregion
	}
}
