
using System;
using System.Collections.Generic;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.OpenGl
{
	internal class TerrainPatchBuilder
	{
		#region PatchVbLock Private Class

		private class PatchVbLock : IVertexBufferLock
		{
			public PatchVbLock( TerrainPatchBuilder builder, IVertexBufferLock vbLock, int level )
			{
				m_Builder = builder;
				m_Lock = vbLock;
				m_Level = level;
			}

			#region Private Members

			private readonly IVertexBufferLock m_Lock;
			private readonly TerrainPatchBuilder m_Builder;
			private readonly int m_Level;

			#endregion

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

		public IVertexBufferLock AllocatePatchVertices( int level )
		{
			if ( m_LodLevels[ level ].VbPool.Count == 0 )
			{
				return null;
			}
			List< int > levelIndexes = m_LodLevels[ level ].VbPool;
			int firstIndex = levelIndexes[ 0 ];
			levelIndexes.RemoveAt( 0 );
			return new PatchVbLock( this, m_Buffer.Lock( firstIndex, NumberOfLevelVertices( level ), false, true ), level );
		}

		public void ReleasePatchVertices( int level, int index )
		{
			m_LodLevels[ level ].VbPool.Add( index );
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
				case 0: return 1;
				case 1: return 1;
				case 2: return 1;
				case 3: return 1;
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
