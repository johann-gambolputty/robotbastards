using System;
using System.Runtime.Serialization;
using Poc0.Core.Environment;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Textures;
using Rb.World;
using Rb.World.Services;
using Tao.OpenGl;
using Rb.Rendering.Lights;

namespace Poc0.Core.Rendering.OpenGl
{
	/// <summary>
	/// Handles environment rendering in OpenGL
	/// </summary>
	[Serializable, RenderingLibraryType]
	public class OpenGlEnvironmentGraphics : IEnvironmentGraphics, ISceneObject, IDeserializationCallback
	{
		/// <summary>
		/// Sets up render states
		/// </summary>
		public OpenGlEnvironmentGraphics( )
		{
			m_WallState = Graphics.Factory.NewRenderState( );
			m_WallState.DisableLighting( );
			m_WallState.SetColour( System.Drawing.Color.DarkOrange );
			m_WallState.EnableCap( RenderStateFlag.Texture2d | RenderStateFlag.Texture2dUnit0 );

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
		
		#region IEnvironmentGraphics Members

		/// <summary>
		/// Builds this object
		/// </summary>
		/// <param name="data">Source data</param>
		public void Build( EnvironmentGraphicsData data )
		{
			GraphicsLog.Assert( m_Scene != null, "Expected environment graphics to be attached to a scene prior to Build()" );

			m_Data = data;
			m_Grid = new Cell[ data.Width, data.Height ];
			for ( int y = 0; y < data.Height; ++y )
			{
				for ( int x = 0; x < data.Width; ++x )
				{
					m_Grid[ x, y ] = new Cell( m_Scene, data[ x, y ] );
				}
			}
		}

		#endregion

		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to a scene
		/// </summary>
		public void AddedToScene( Scene scene )
		{
			m_Scene = scene;
			scene.Renderables.Add( this );
		}

		/// <summary>
		/// Called when this object removed from a scene
		/// </summary>
		public void RemovedFromScene( Scene scene )
		{
			m_Scene = null;
			scene.Renderables.Remove( this );
		}

		#endregion

		#region Private members

		[NonSerialized]
		private Cell[,] m_Grid;
		private readonly RenderState m_WallState;
		private readonly RenderState m_FloorState;
		private EnvironmentGraphicsData m_Data;
		private Scene m_Scene;

		/// <summary>
		/// A geometry group that shares a texture set
		/// </summary>
		private class GeometryGroup
		{
			/// <summary>
			/// Sets up this group
			/// </summary>
			/// <param name="src">Source data</param>
			public GeometryGroup( EnvironmentGraphicsData.CellGeometryGroup src )
			{
				m_Indices = src.Indices;
				m_Technique = src.Technique;
				m_Textures = src.Textures;
			}

			/// <summary>
			/// Renders this group
			/// </summary>
			/// <param name="context">Rendering context</param>
			public void Render( IRenderContext context )
			{
				context.ApplyTechnique( m_Technique, RenderGeometry );
			}

			/// <summary>
			/// Renders group geometry
			/// </summary>
			/// <param name="context">Rendering context</param>
			private void RenderGeometry( IRenderContext context )
			{
				foreach ( ITexture2d texture in m_Textures )
				{
					Graphics.Renderer.BindTexture( texture );
				}

				Gl.glDrawElements( Gl.GL_TRIANGLES, m_Indices.Length, Gl.GL_UNSIGNED_INT, m_Indices );

				foreach ( ITexture2d texture in m_Textures )
				{
					Graphics.Renderer.UnbindTexture( texture );
				}

			}

			private readonly ITechnique m_Technique;
			private readonly ITexture2d[] m_Textures;
			private readonly int[] m_Indices;
		}

		/// <summary>
		/// A grid cell, containing an array of geometry groups
		/// </summary>
		private class Cell
		{
			/// <summary>
			/// Cell setup
			/// </summary>
			/// <param name="scene">Scene data</param>
			/// <param name="src">Cell source data</param>
			public Cell( Scene scene, EnvironmentGraphicsData.GridCell src )
			{
				m_Scene = scene;
				m_VertexBuffer = Graphics.Factory.NewVertexBuffer( src.VertexData );
				m_Groups = new GeometryGroup[ src.Groups.Count ];
				for ( int groupIndex = 0; groupIndex < m_Groups.Length; ++groupIndex )
				{
					m_Groups[ groupIndex ] = new GeometryGroup( src.Groups[ groupIndex ] );
				}
			}

			/// <summary>
			/// Renders this cell
			/// </summary>
			/// <param name="context">Rendering context</param>
			public void Render( IRenderContext context )
			{
				Graphics.Renderer.PushTransform( Transform.LocalToWorld, Matrix44.Identity );

				//	TODO: AP: Fix lighting
				ILightingService lighting = m_Scene.GetService< ILightingService >( );
				foreach ( ILight light in lighting.Lights )
				{
					Graphics.Renderer.AddLight( light );
				}

				m_VertexBuffer.Begin( );
				foreach ( GeometryGroup group in m_Groups )
				{
					group.Render( context );
				}
				m_VertexBuffer.End( );

				Graphics.Renderer.ClearLights( );

				Graphics.Renderer.PopTransform( Transform.LocalToWorld );
			}

			private readonly Scene m_Scene;
			private readonly GeometryGroup[] m_Groups;
			private readonly IVertexBuffer m_VertexBuffer;
		}



		#endregion

		#region IDeserializationCallback Members

		/// <summary>
		/// Called when the entire object graph for the scene has been deserialized
		/// </summary>
		/// <param name="sender">Sender</param>
		public void OnDeserialization( object sender )
		{
			Build( m_Data );
		}

		#endregion
	}
}

