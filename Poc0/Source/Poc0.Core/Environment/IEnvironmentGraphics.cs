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
			m_Grid = new GridCell[ width, height ];
		}

		public GridCell this[ int x, int y ]
		{
			get { return m_Grid[ x, y ]; }
			set { m_Grid[ x, y ] = value; }
		}

		private readonly GridCell[,] m_Grid;

		public class CellGeometryGroup
		{
			public CellGeometryGroup( VertexBufferData vertices, AssetHandle[] textures )
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

			private readonly VertexBufferData m_Vertices;
			private readonly AssetHandle[] m_Textures;
		}

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
