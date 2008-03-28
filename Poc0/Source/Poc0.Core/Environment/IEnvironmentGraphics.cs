using System;
using System.Collections.Generic;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc0.Core.Environment
{
	/// <summary>
	/// Data used to create environment graphics
	/// </summary>
	[Serializable]
	public class EnvironmentGraphicsData
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="width">Grid width</param>
		/// <param name="height">Grid height</param>
		public EnvironmentGraphicsData( int width, int height )
		{
			m_Width = width;
			m_Height = height;
			m_Grid = new GridCell[ width, height ];
		}

		/// <summary>
		/// Grid width
		/// </summary>
		public int Width
		{
			get { return m_Width; }
		}

		/// <summary>
		/// Grid height
		/// </summary>
		public int Height
		{
			get { return m_Height; }
		}

		/// <summary>
		/// Gets a grid cell
		/// </summary>
		/// <param name="x">Grid cell X coordinate</param>
		/// <param name="y">Grid cell Y coordinate</param>
		/// <returns>Returns the cell at the (x,y) coordinate</returns>
		public GridCell this[ int x, int y ]
		{
			get { return m_Grid[ x, y ]; }
			set { m_Grid[ x, y ] = value; }
		}

		/// <summary>
		/// Cell geometry group
		/// </summary>
		[Serializable]
		public class CellGeometryGroup
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			/// <param name="indices">Index buffer</param>
			/// <param name="technique">Rendering technique</param>
			/// <param name="textures">Textures</param>
			public CellGeometryGroup( int[] indices, ITechnique technique, ITexture2d[] textures )
			{
				m_Indices = indices;
				m_Textures = textures;
				m_Technique = technique;
			}

			/// <summary>
			/// Gets the group vertex buffer
			/// </summary>
			public int[] Indices
			{
				get { return m_Indices; }
			}

			/// <summary>
			/// Gets the group texture set
			/// </summary>
			public ITexture2d[] Textures
			{
				get { return m_Textures; }
			}

			/// <summary>
			/// Gets the group technique
			/// </summary>
			public ITechnique Technique
			{
				get { return m_Technique; }
			}

			private readonly ITechnique m_Technique;
			private readonly int[] m_Indices;
			private readonly ITexture2d[] m_Textures;
		}

		/// <summary>
		/// Grid cell
		/// </summary>
		[Serializable]
		public class GridCell
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			public GridCell( VertexBufferData vertices )
			{
				m_Vertices = vertices;
			}

			/// <summary>
			/// Gets vertex buffer data for all geometry in this cell
			/// </summary>
			public VertexBufferData VertexData
			{
				get { return m_Vertices; }
			}

			/// <summary>
			/// Gets the list of geometry groups making up this cell
			/// </summary>
			public List<CellGeometryGroup> Groups
			{
				get { return m_Groups; }
			}

			private readonly VertexBufferData m_Vertices;
			private readonly List<CellGeometryGroup> m_Groups = new List<CellGeometryGroup>( );
		}


		private readonly int m_Width;
		private readonly int m_Height;
		private readonly GridCell[,] m_Grid;

	}

	/// <summary>
	/// Environment graphics interface
	/// </summary>
	[RenderingLibraryType]
	public interface IEnvironmentGraphics : IRenderable
	{
		/// <summary>
		/// Builds this object
		/// </summary>
		/// <param name="data">Source data</param>
		void Build( EnvironmentGraphicsData data );
	}

}
