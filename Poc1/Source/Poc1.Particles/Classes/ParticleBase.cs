
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Poc1.Particles.Interfaces;
using Rb.Core.Maths;

namespace Poc1.Particles.Classes
{
	public unsafe class ParticleFields
	{
		public int Stride
		{
			get { return m_ParticleSize; }
		}

		public int MaxParticleCount
		{
			get { return m_MaxParticles; }
			set
			{
				m_MaxParticles = value;
				m_BuildBuffer = true;
			}
		}

		public IDisposable Pin( )
		{
			GCHandle handle = GCHandle.Alloc( m_Buffer, GCHandleType.Pinned );
			return new BufferHandle( handle );
		}

		public void AddField( string name, Type type, int numElements )
		{
			m_BuildBuffer = true;
			FieldInfo field = new FieldInfo( type, numElements, m_ParticleSize );
			m_Fields.Add( name, field );
			m_ParticleSize += field.Size;
		}

		public byte* GetField( string name )
		{
			if ( m_BuildBuffer )
			{
				if ( m_Buffer == null )
				{
					m_Buffer = new byte[ Stride * MaxParticleCount ];
				}
				else
				{
					throw new NotImplementedException( "Rebuild of existing buffer not implemented" );
				}
			}

			FieldInfo field = m_Fields[ name ];
			fixed( byte* bufferBytes = m_Buffer )	//	Not necessary, because m_Buffer is pinned, but gets around compiler error
			{
				return bufferBytes + field.Offset;
			}
		}

		#region Private Members

		#region BufferHandle Class

		private class BufferHandle : IDisposable
		{
			public BufferHandle( GCHandle handle )
			{
				m_Handle = handle;
			}

			#region IDisposable Members

			public void Dispose( )
			{
				m_Handle.Free( );
			}

			#endregion

			#region Private Members

			private readonly GCHandle m_Handle;

			#endregion
		}

		#endregion

		#region FieldInfo Class

		private class FieldInfo
		{
			public FieldInfo( Type type, int numElements, int offset )
			{
				m_Size = Marshal.SizeOf( type ) * numElements;
				m_Offset = offset;
			}

			public int Size
			{
				get { return m_Size; }
			}

			public int Offset
			{
				get { return m_Offset; }
			}

			private readonly int m_Size;
			private readonly int m_Offset;
		}

		#endregion

		#endregion

		private int m_MaxParticles;
		private bool m_BuildBuffer;
		private byte[] m_Buffer;
		private readonly Dictionary<string, FieldInfo> m_Fields = new Dictionary<string, FieldInfo>( );
		private int m_ParticleSize;
	}


	/// <summary>
	/// Simple inert particle
	/// </summary>
	public class ParticleBase
	{
		public const string Position = "pos";
		public const string Velocity = "vel";
		public const string Age = "age";


		//public Point3 Position
		//{
		//    get { return m_Position; }
		//    set { m_Position = value; }
		//}

		//public int Age
		//{
		//    get { return m_Age; }
		//    set { m_Age = value; }
		//}

		#region Private Members

		private Point3	m_Position;
		private int		m_Age;

		#endregion
	}

	public unsafe struct ParticleFieldIterator
	{
		public ParticleFieldIterator( IParticleBuffer pBuffer, string fieldName )
		{
			m_Pos = pBuffer.GetField( fieldName );
			m_Stride = pBuffer.Stride;
		}

		public void Set( Point3 src )
		{
			float* dst = GetFloatPtr( );
			dst[ 0 ] = src.X;
			dst[ 1 ] = src.Y;
			dst[ 2 ] = src.Z;
		}

		public void Set( int val )
		{
			int* dst = GetIntPtr( );
			dst[ 0 ] = val;
		}

		public void Set( float val )
		{
			float* dst = GetFloatPtr( );
			dst[ 0 ] = val;
		}

		public Point3 GetPosition( )
		{
			float* src = GetFloatPtr( );
			return new Point3( src[ 0 ], src[ 1 ], src[ 2 ] );
		}

		public float* GetFloatPtr( )
		{
			float* val = ( float* )m_Pos;
			m_Pos += m_Stride;
			return val;
		}

		public int* GetIntPtr( )
		{
			int* val = ( int* )m_Pos;
			m_Pos += m_Stride;
			return val;
		}

		private byte* m_Pos;
		private readonly int m_Stride;
	}
}
