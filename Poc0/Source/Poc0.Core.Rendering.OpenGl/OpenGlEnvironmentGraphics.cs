using System;
using System.Runtime.Serialization;
using Poc0.Core.Environment;
using Rb.Rendering;
using Rb.World;
using Tao.OpenGl;

namespace Poc0.Core.Rendering.OpenGl
{
	[Serializable, RenderingLibraryType]
	public class OpenGlEnvironmentGraphics : IEnvironmentGraphics, ISceneObject
	{
		private readonly RenderState m_WallState;
		private readonly RenderState m_FloorState;

		/// <summary>
		/// Sets up render states
		/// </summary>
		public OpenGlEnvironmentGraphics( )
		{
			m_WallState = Graphics.Factory.NewRenderState( );
			m_WallState.DisableLighting( );
			m_WallState.SetColour( System.Drawing.Color.DarkOrange );

			m_FloorState = Graphics.Factory.NewRenderState( );
			m_FloorState.DisableLighting( );
			m_FloorState.SetColour( System.Drawing.Color.Blue );
		}

		/// <summary>
		/// Renders the environment
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			//	Render walls
			Graphics.Renderer.PushRenderState( m_WallState );

			foreach ( Cell cell in m_Grid )
			{
				cell.Render( context );
			}
			
			Graphics.Renderer.PopRenderState( );
		}

		#region Private members

		[NonSerialized]
		private Cell[,] m_Grid;

		private class GeometryGroup
		{
			public GeometryGroup( EnvironmentGraphicsData.CellGeometryGroup src )
			{
				m_NumTris = src.Vertices.NumVertices / 3;
				m_Vertices = Graphics.Factory.NewVertexBuffer( src.Vertices );
			}

			public void Render( IRenderContext context )
			{
				m_Vertices.Begin( );
				Gl.glDrawArrays( Gl.GL_TRIANGLES, 0, m_NumTris * 3 );
				m_Vertices.End( );
			}

			private readonly int m_NumTris;
			private readonly IVertexBuffer m_Vertices;
		}

		private class Cell
		{
			public Cell( EnvironmentGraphicsData.GridCell src )
			{
				m_Groups = new GeometryGroup[ src.Groups.Count ];
				for ( int groupIndex = 0; groupIndex < m_Groups.Length; ++groupIndex )
				{
					m_Groups[ groupIndex ] = new GeometryGroup( src.Groups[ groupIndex ] );
				}
			}

			public void Render( IRenderContext context )
			{
				foreach ( GeometryGroup group in m_Groups )
				{
					group.Render( context );
				}
			}

			private readonly GeometryGroup[] m_Groups;
		}

		private EnvironmentGraphicsData m_Data;

		[OnDeserialized]
		public void OnDeserialized( StreamingContext context )
		{
			Build( m_Data );
		}

		#endregion

		#region IEnvironmentGraphics Members

		public void Build( EnvironmentGraphicsData data )
		{
			m_Data = data;
			m_Grid = new Cell[ data.Width, data.Height ];
			for ( int y = 0; y < data.Height; ++y )
			{
				for ( int x = 0; x < data.Width; ++x )
				{
					m_Grid[ x, y ] = new Cell( data[ x, y ] );
				}
			}
		}

		#endregion

		#region ISceneObject Members

		public void AddedToScene( Scene scene )
		{
			scene.Renderables.Add( this );
		}

		public void RemovedFromScene( Scene scene )
		{
			scene.Renderables.Remove( this );
		}

		#endregion
	}
}
