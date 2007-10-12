using System;
using System.Runtime.Serialization;
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
	[Serializable]
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

		#region Serialization

		/// <summary>
		/// Called when this object is deserialized
		/// </summary>
		/// <param name="context">Deserialization context</param>
		[OnDeserialized]
		public void OnDeserialized( StreamingContext context )
		{
			DestroyDisplayList( );
		}

		#endregion

		#region Protected members

		/// <summary>
		/// Renders the level geometry in 3D
		/// </summary>
		/// <param name="context">Rendering context</param>
		/// <param name="csg">Level geometry CSG object</param>
		protected override void Render3d( IRenderContext context, Csg csg )
		{
			if ( csg == null )
			{
				return;
			}
			if ( m_DisplayList == -1 )
			{
				RebuildDisplayList( );
			}

			//	Render walls
			Graphics.Renderer.PushRenderState( m_WallState );
			Gl.glCallList( m_DisplayList );
			Graphics.Renderer.PopRenderState( );
			
			//	Render floors
			//Graphics.Renderer.PushRenderState( m_FloorState );

			//Gl.glBegin( Gl.GL_TRIANGLES );
			//RenderFloors( context, Walls );
			//Gl.glEnd( );

			//Graphics.Renderer.PopRenderState( );
		}

		/// <summary>
		/// Called when level geometry has changed
		/// </summary>
		protected override void OnGeometryChanged( object sender, EventArgs args )
		{
			RebuildDisplayList( );
			base.OnGeometryChanged( sender, args );
		}


		#endregion

		[NonSerialized]
		private int m_DisplayList = -1;
		private readonly RenderState m_WallState;
		private readonly RenderState m_FloorState;

		/// <summary>
		/// Renders the current node's walls
		/// </summary>
		/// <param name="node">Current node</param>
		private void RenderWall( Csg.BspNode node )
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

			Gl.glVertex3f( end.X, height, end.Y );
			Gl.glVertex3f( start.X, height, start.Y );
			Gl.glVertex3f( start.X, 0, start.Y );

			RenderWall( node.InFront );
			RenderWall( node.Behind );
		}

		/// <summary>
		/// Rebuilds the current display list
		/// </summary>
		private void RebuildDisplayList( )
		{
			DestroyDisplayList( );

			if ( Csg != null )
			{
				m_DisplayList = Gl.glGenLists( 1 );
				Gl.glNewList( m_DisplayList, Gl.GL_COMPILE );

				Gl.glBegin( Gl.GL_TRIANGLES );
				RenderWall( Csg.Root );
				Gl.glEnd( );

				Gl.glEndList( );
			}
		}

		/// <summary>
		/// Destroys the current display list
		/// </summary>
		private void DestroyDisplayList( )
		{
			if ( m_DisplayList != -1 )
			{
				Gl.glDeleteLists( m_DisplayList, 1 );
			}
			m_DisplayList = -1;
		}
	}
}
