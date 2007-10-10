using Poc0.LevelEditor.Core;
using Rb.Rendering;
using Rb.Tools.LevelEditor.Core;
using Tao.OpenGl;
using Rb.Core.Maths;

namespace Poc0.LevelEditor.Rendering.OpenGl
{
	/// <summary>
	/// Rendering for level geometry in the level editor
	/// </summary>
	public class OpenGlLevelGeometry : LevelGeometry
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public OpenGlLevelGeometry( EditorScene scene ) :
			base( scene )
		{
			m_WallState = Graphics.Factory.NewRenderState( );
			m_WallState.DisableLighting( );
			m_WallState.SetColour( System.Drawing.Color.DarkOrange );

			m_FloorState = Graphics.Factory.NewRenderState( );
			m_FloorState.DisableLighting( );
			m_FloorState.SetColour( System.Drawing.Color.Blue );
		}

		#region Protected members

		/// <summary>
		/// Renders the level geometry in 3D
		/// </summary>
		/// <param name="csg">Level geometry CSG object</param>
		protected override void Render3d( IRenderContext context, Csg csg )
		{
			if ( csg == null )
			{
				return;
			}

			//	Render walls
			Graphics.Renderer.PushRenderState( m_WallState );

			Gl.glBegin( Gl.GL_QUADS );
			RenderWall( context, csg.Root );
			Gl.glEnd( );
			
			Graphics.Renderer.PopRenderState( );
			
			//	Render floors
			Graphics.Renderer.PushRenderState( m_FloorState );

			//Gl.glBegin( Gl.GL_TRIANGLES );
			//RenderFloors( context, Walls );
			//Gl.glEnd( );

			Graphics.Renderer.PopRenderState( );
		}

		#endregion

		private readonly RenderState m_WallState;
		private readonly RenderState m_FloorState;

		private void RenderWall( IRenderContext context, Csg.BspNode node )
		{
			if ( node == null )
			{
				return;
			}
			Point2 start = node.Edge.P0;
			Point2 end = node.Edge.P1;

			float height = 10;

			Gl.glVertex3f( start.X, 0, start.Y );
			Gl.glVertex3f( end.X, 0, end.Y );
			Gl.glVertex3f( end.X, height, end.Y );
			Gl.glVertex3f( start.X, height, start.Y );

			RenderWall( context, node.InFront );
			RenderWall( context, node.Behind );
		}
	}
}
