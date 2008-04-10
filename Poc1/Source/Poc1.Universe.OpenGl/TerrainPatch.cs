using System.Collections.Generic;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.OpenGl
{
	internal class TerrainPatch
	{
		public const float PatchWidth = 32;
		public const float PatchHeight = PatchWidth;

		private Point3 m_TopLeft;
		private Point3 m_TopRight;
		private Point3 m_BottomLeft;
		private Point3 m_BottomRight;
		private Vector3 m_PatchXDir;
		private Vector3 m_PatchZDir;
		private float m_PatchWidth;
		private float m_PatchHeight;

		public void SetBounds( Point3 topLeft, Point3 topRight, Point3 bottomLeft, Point3 bottomRight )
		{
			m_TopLeft = topLeft;
			m_TopRight = topRight;
			m_BottomLeft = bottomLeft;
			m_BottomRight = bottomRight;
			m_PatchXDir = m_TopRight - m_TopLeft;
			m_PatchZDir = m_BottomLeft - m_TopLeft;
			m_PatchWidth = m_PatchXDir.Length;
			m_PatchHeight = m_PatchZDir.Length;
			m_PatchXDir /= m_PatchWidth;
			m_PatchZDir /= m_PatchHeight;
		}

		private enum Side
		{
			Left,
			Top,
			Right,
			Bottom
		}

		private int VertexOffset
		{
			get { return m_Offset; }
		}

		private static Side OppositeSide( Side side )
		{
			switch ( side )
			{
				case Side.Left		: return Side.Right;
				case Side.Top		: return Side.Bottom;
				case Side.Right		: return Side.Left;
				case Side.Bottom	: return Side.Top;
			}

			return Side.Right;
		}

		private void GetSideVars( Side side, out int firstIndex, out int nextIndexOffset, int offset )
		{
			switch ( side )
			{
				default:
				case Side.Left	:
					firstIndex = VertexOffset + offset;
					nextIndexOffset = m_Size;
					break;
				case Side.Top	:
					firstIndex = VertexOffset + m_Size * offset;
					nextIndexOffset = 1;
					break;
				case Side.Right	:
					firstIndex = VertexOffset + m_Size - ( offset + 1 );
					nextIndexOffset = m_Size;
					break;
				case Side.Bottom :
					firstIndex = VertexOffset + m_Size * ( m_Size - ( offset + 1 ) );
					nextIndexOffset = 1;
					break;
			}
		}

		private bool BuildConnectingStripIndexBuffer( TerrainPatch neighbour, Side side, List<int> indices )
		{
			if ( neighbour == null )
			{
				return false;
			}

			if ( LodLevel <= neighbour.LodLevel )
			{
				//	Current patch has higher detail than neighbour patch
				return false;
			}
			int index;
			int nextIndexOffset;
			int neighbourIndex;
			int neighbourNextIndexOffset;

			GetSideVars( side, out index, out nextIndexOffset, 1 );
			neighbour.GetSideVars( OppositeSide( side ), out neighbourIndex, out neighbourNextIndexOffset, 0 );

			int startError = ( int )Functions.Pow( 2, LodLevel - neighbour.LodLevel );
			int error = startError;
			for ( int count = 1; count < neighbour.m_Size; ++count )
			{
				if ( error == 0 )
				{
					indices.Add( neighbourIndex );
					indices.Add( index );
					indices.Add( index + nextIndexOffset );

					index += nextIndexOffset;
					error = startError;
				}
				indices.Add( neighbourIndex );
				indices.Add( index );
				indices.Add( neighbourIndex + neighbourNextIndexOffset );

				neighbourIndex += neighbourNextIndexOffset;
				--error;
			}
			indices.Add( neighbourIndex );
			indices.Add( index );
			indices.Add( index + nextIndexOffset );

			return true;
		}

		private IIndexBuffer BuildIndexBuffer( )
		{
			int size = m_Size;
			int offset = VertexOffset;
			int numIndices = ( size - 1 ) * ( size - 1 ) * 6;
			List<int> indices = new List<int>( numIndices );

			bool addedLeftStrip = BuildConnectingStripIndexBuffer( LeftPatch, Side.Left, indices );
			bool addedTopStrip = BuildConnectingStripIndexBuffer( TopPatch, Side.Top, indices );
			bool addedRightStrip = BuildConnectingStripIndexBuffer( RightPatch, Side.Right, indices );
			bool addedBottomStrip = BuildConnectingStripIndexBuffer( BottomPatch, Side.Bottom, indices );

			int startRow = addedTopStrip ? 1 : 0;
			int endRow = addedBottomStrip ? size - 2 : size - 1;
			int startCol = addedLeftStrip ? 1 : 0;
			int endCol = addedRightStrip ? size - 2 : size - 1;

			for ( int row = startRow; row < endRow; ++row )
			{
				int index = offset + ( row * size ) + startCol;
				for ( int col = startCol; col < endCol; ++col )
				{
					indices.Add( index );
					indices.Add( index + 1 );
					indices.Add( index + size );

					indices.Add( index + 1 );
					indices.Add( index + 1 + size );
					indices.Add( index + size );

					++index;
				}
			}

			IndexBufferData data = new IndexBufferData( new IndexBufferFormat( IndexBufferIndexSize.Int32 ), indices.ToArray( ) );
			IIndexBuffer buffer = Graphics.Factory.CreateIndexBuffer( data );

			return buffer;
		}

		private void DestroyIndexBuffer( )
		{
			if ( m_IndexBuffer != null )
			{
				m_IndexBuffer.Dispose( );
				m_IndexBuffer = null;
			}
		}

		private TerrainPatchBuilder.PatchVertexRange m_VbRange;

		public void PreBuild( TerrainPatchBuilder builder )
		{
			DestroyIndexBuffer( );
			m_VbRange = builder.AllocatePatchVertices( m_Lod );
			m_Size = TerrainPatchBuilder.GetLevelSize( m_Lod );
			m_Offset = m_VbRange.FirstVertexOffset;
		}

		public unsafe void Build( TerrainPatchBuilder builder )
		{
			using ( IVertexBufferLock vbLock = m_VbRange.Lock( ) )
			{
				m_IndexBuffer = BuildIndexBuffer( );
				TerrainVertex* curVertex = ( TerrainVertex* )vbLock.Bytes;

				Vector3 xInc = m_PatchXDir * ( m_PatchWidth / ( m_Size - 1 ) );
				Vector3 zInc = m_PatchZDir * ( m_PatchHeight / ( m_Size - 1 ) );

				Point3 rowStart = m_TopLeft;
				for ( int row = 0; row < m_Size; ++row )
				{
					Point3 curPt = rowStart;
					for ( int col = 0; col < m_Size; ++col )
					{
						Point3 rlPt = Point3.Origin + ( curPt - Point3.Origin ).MakeNormal( ) * 64;

						curVertex->X = rlPt.X;
						curVertex->Y = rlPt.Y;// +Functions.Sin( col * Constants.TwoPi / size ) * yScale;
						curVertex->Z = rlPt.Z;
						++curVertex;
						curPt += xInc;
					}

					rowStart += zInc;
				}
			}
		}

		public int LodLevel
		{
			get { return m_Lod; }
			set { m_Lod = value; }
		}

		public TerrainPatch LeftPatch
		{
			get { return m_LeftPatch; }
			set { m_LeftPatch = value; }
		}

		public TerrainPatch TopPatch
		{
			get { return m_TopPatch; }
			set { m_TopPatch = value; }
		}

		public TerrainPatch RightPatch
		{
			get { return m_RightPatch; }
			set { m_RightPatch = value; }
		}

		public TerrainPatch BottomPatch
		{
			get { return m_BottomPatch; }
			set { m_BottomPatch = value; }
		}

		public static void LinkTopAndBottomPatches( TerrainPatch topPatch, TerrainPatch bottomPatch )
		{
			if ( topPatch != null )
			{
				topPatch.BottomPatch = bottomPatch;
			}
			if ( bottomPatch != null )
			{
				bottomPatch.TopPatch = topPatch;
			}
		}

		public static void LinkLeftAndRightPatches( TerrainPatch leftPatch, TerrainPatch rightPatch )
		{
			if ( leftPatch != null )
			{
				leftPatch.RightPatch = rightPatch;
			}
			if ( rightPatch != null )
			{
				rightPatch.LeftPatch = leftPatch;
			}
		}

		public void Render( )
		{
			m_IndexBuffer.Draw( PrimitiveType.TriList );
		}

		private int m_Size;
		private int m_Offset;
		private IIndexBuffer m_IndexBuffer;
		private int m_Lod = 3;
		private TerrainPatch m_LeftPatch;
		private TerrainPatch m_TopPatch;
		private TerrainPatch m_RightPatch;
		private TerrainPatch m_BottomPatch;
	}
}
