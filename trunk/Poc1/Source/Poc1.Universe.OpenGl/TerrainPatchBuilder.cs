
using System;
using System.Collections.Generic;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.OpenGl
{
	internal class TerrainPatchBuilder
	{

		private class PatchVbLock : IVertexBufferLock
		{
			public PatchVbLock( TerrainPatchBuilder builder, IVertexBufferLock vbLock, int level )
			{
				m_Builder = builder;
				m_Lock = vbLock;
				m_Level = level;
			}


			private readonly IVertexBufferLock m_Lock;
			private readonly TerrainPatchBuilder m_Builder;
			private readonly int m_Level;

			#region IVertexBufferLock Members

			public int FirstVertexIndex
			{
				get { return m_Lock.FirstVertexIndex; }
			}

			public int VertexCount
			{
				get { return m_Lock.VertexCount; }
			}

			public unsafe byte* Bytes
			{
				get { return m_Lock.Bytes; }
			}

			public unsafe void Write<T>( int offset, IEnumerable<T> vertices )
			{
				m_Lock.Write( offset, vertices );
			}

			public unsafe System.IO.Stream ToStream( )
			{
				return m_Lock.ToStream( );
			}

			#endregion

			#region IDisposable Members

			public void Dispose( )
			{
				m_Builder.ReleasePatchVertices( m_Level, FirstVertexIndex );
				m_Lock.Dispose( );
			}

			#endregion
		}

		private class Lod
		{
			public Lod( int size )
			{
				IndexBufferFormat format = new IndexBufferFormat( IndexBufferIndexSize.Int32 );
				m_IndexBuffer = Graphics.Factory.CreateIndexBuffer( format, size );
			}

			public IIndexBuffer IndexBuffer
			{
				get { return m_IndexBuffer; }
			}

			public List<int> VbPool
			{
				get { return m_VbPool; }
			}

			private readonly IIndexBuffer m_IndexBuffer;
			private readonly List<int> m_VbPool = new List<int>( );
		}

		public IVertexBufferLock AllocatePatchVertices( int level )
		{
			if ( m_Lods[ level ].VbPool.Count == 0 )
			{
				return null;
			}
			List< int > levelIndexes = m_Lods[ level ].VbPool;
			int firstIndex = levelIndexes[ 0 ];
			levelIndexes.RemoveAt( 0 );
			return new PatchVbLock( this, m_Buffer.Lock( firstIndex, NumberOfLevelVertices( level ), false, true ), level );
		}

		public void ReleasePatchVertices( int level, int index )
		{
			m_Levels[ level ].Add( index );
		}

		private const int MaxLodLevels = 4;

		public static int GetLevelSize( int level )
		{
			switch ( level )
			{
				case 0: return 129;
				case 1: return 65;
				case 2: return 31;
				case 3: return 15;
			}
			throw new ArgumentException( "Level is out of range - " + level, "level" );
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
				case 0: return 4;
				case 1: return 8;
				case 2: return 16;
				case 3: return 16;
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

			m_Buffer = Graphics.Factory.CreateVertexBuffer( format, numVertices );

			m_Lods = new List<Lod>( );
			int curVertexIndex = 0;
			for ( int level = 0; level < MaxLodLevels; ++level )
			{
				int numLevelVertices = NumberOfLevelVertices( level );
				int poolSize = LevelPoolSize( level );
				m_Levels[ level ] = new List<int>( );
				for ( int poolIndex = 0; poolIndex < poolSize; ++poolIndex )
				{

					m_Levels[ level ].Add( curVertexIndex );
					curVertexIndex += numLevelVertices;
				}
			}
		}

		private readonly Lod[] m_Lods = new Lod[ MaxLodLevels ]; 
		private readonly IVertexBuffer m_Buffer;
	}
}
