using System;
using System.Collections.Generic;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.OpenGl
{
	internal class TerrainPatchBuilder
	{
		#region PatchVbLock Private Class

		public class PatchVertexRange
		{
			public PatchVertexRange( IVertexBuffer buffer, int firstIndex, int count )
			{
				m_Buffer = buffer;
				m_FirstIndex = firstIndex;
				m_Count = count;
			}

			public int FirstVertexOffset
			{
				get { return m_FirstIndex; }
			}

			public IVertexBufferLock Lock( )
			{
				return m_Buffer.Lock( m_FirstIndex, m_Count, false, true );
			}

			#region Private Members

			private readonly IVertexBuffer m_Buffer;
			private readonly int m_FirstIndex;
			private readonly int m_Count;

			#endregion

		}

		#endregion

		#region Lod Private Class

		private class Lod
		{
			public List<int> VbPool
			{
				get { return m_VbPool; }
			}

			private readonly List<int> m_VbPool = new List<int>( );
		}

		#endregion

		public void BeginPatchRendering( )
		{
			m_Buffer.Begin( );
		}

		public void EndPatchRendering( )
		{
			m_Buffer.End( );
		}

		public PatchVertexRange AllocatePatchVertices( int level )
		{
			if ( m_LodLevels[ level ].VbPool.Count == 0 )
			{
				return null;
			}
			List< int > levelIndexes = m_LodLevels[ level ].VbPool;
			int firstIndex = levelIndexes[ 0 ];
			levelIndexes.RemoveAt( 0 );
			return new PatchVertexRange( m_Buffer, firstIndex, NumberOfLevelVertices( level ) );
		}

		public void ReleasePatchVertices( int level, int index )
		{
			m_LodLevels[ level ].VbPool.Add( index );
		}

		public const int MaxLodLevels = 4;
		private readonly static int MaxSize = ( int )Functions.Pow( 2, MaxLodLevels - 1 );

		public static int GetLevelSize( int level )
		{
			return ( MaxSize / ( int )Functions.Pow( 2, level ) ) + 1;
		}

		private static int NumberOfLevelVertices( int level )
		{
			int size = GetLevelSize( level );
			return size * size;
		}

		private static int LevelPoolSize( int level )
		{
			switch ( level )
			{
				case 0: return 32;
				case 1: return 64;
				case 2: return 64;
				case 3: return 64;
			}
			throw new ArgumentException( "Level is out of range - " + level, "level" );
		}

		public TerrainPatchBuilder( )
		{
			int numVertices = 0;
			for ( int level = 0; level < MaxLodLevels; ++level )
			{
				numVertices += LevelPoolSize( level ) * NumberOfLevelVertices( level );
			}

			VertexBufferFormat format = new VertexBufferFormat( );
			format.Add( VertexFieldSemantic.Position, VertexFieldElementTypeId.Float32, 3 );

			GraphicsLog.Info( "Allocating terrain patch builder VBO: {0} vertices in format {1}", numVertices, format );

			m_Buffer = Graphics.Factory.CreateVertexBuffer( );
			m_Buffer.Create( format, numVertices );

			int curVertexIndex = 0;
			for ( int level = 0; level < MaxLodLevels; ++level )
			{
				Lod newLod = new Lod( );
				m_LodLevels[ level ] = newLod;

				int numLevelVertices = NumberOfLevelVertices( level );
				int poolSize = LevelPoolSize( level );
				for ( int poolIndex = 0; poolIndex < poolSize; ++poolIndex )
				{
					newLod.VbPool.Add( curVertexIndex );
					curVertexIndex += numLevelVertices;
				}
			}
		}

		private readonly Lod[] m_LodLevels = new Lod[ MaxLodLevels ]; 
		private readonly IVertexBuffer m_Buffer;

	}
}
