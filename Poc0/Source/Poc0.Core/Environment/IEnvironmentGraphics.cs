using System;
using System.Collections.Generic;
using Rb.Core.Assets;
using Rb.Rendering;

namespace Poc0.Core.Environment
{
	[Serializable]
	public class EnvironmentGraphicsData
	{
		public EnvironmentGraphicsData( int width, int height )
		{
			m_Width = width;
			m_Height = height;
			m_Grid = new GridCell[ width, height ];
		}

		public int Width
		{
			get { return m_Width; }
		}

		public int Height
		{
			get { return m_Height; }
		}

		public GridCell this[ int x, int y ]
		{
			get { return m_Grid[ x, y ]; }
			set { m_Grid[ x, y ] = value; }
		}

		private readonly int m_Width;
		private readonly int m_Height;
		private readonly GridCell[,] m_Grid;

		[Serializable]
		public class CellGeometryGroup
		{
			public CellGeometryGroup( VertexBufferData vertices, ITechnique technique, AssetHandle[] textures )
			{
				m_Vertices = vertices;
				m_Textures = textures;
			}

			public VertexBufferData Vertices
			{
				get { return m_Vertices; }
			}

			public AssetHandle[] Textures
			{
				get { return m_Textures; }
			}

			private readonly ITechnique m_Technique;
			private readonly VertexBufferData m_Vertices;
			private readonly AssetHandle[] m_Textures;
		}

		[Serializable]
		public class GridCell
		{
			public List<CellGeometryGroup> Groups
			{
				get { return m_Groups; }
			}

			private readonly List<CellGeometryGroup> m_Groups = new List<CellGeometryGroup>( );
		}
	}

	[RenderingLibraryType]
	public interface IEnvironmentGraphics : IRenderable
	{
		void Build( EnvironmentGraphicsData data );
	}
}
