using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Poc1.Particles.Interfaces;

namespace Poc1.Particles.Classes
{
	public class SerialParticleBuffer : ISerialParticleBuffer
	{
		public SerialParticleBuffer( )
		{
		}

		public SerialParticleBuffer( int maximumNumberOfParticles )
		{
			MaximumNumberOfParticles = maximumNumberOfParticles;
		}

		#region IParticleBuffer Members

		/// <summary>
		/// Gets the stride, in bytes, between particle field values in the buffer
		/// </summary>
		public int Stride
		{
			get { return m_Stride; }
			set { m_Stride = value; }
		}

		/// <summary>
		/// Gets/sets the maximum number of particles. The default for this is 256
		/// </summary>
		public int MaximumNumberOfParticles
		{
			get { return m_MaximumNumberOfParticles; }
			set
			{
				if ( m_ActiveParticleIndexes.Count > 0 )
				{
					throw new InvalidOperationException( "Can't resize the particle buffer while there are active particles" );
				}

				m_MaximumNumberOfParticles = value;
				m_BuildBuffer = true;
			}
		}

		/// <summary>
		/// Gets the number of particles in the buffer
		/// </summary>
		public int NumActiveParticles
		{
			get { return m_ActiveParticleIndexes.Count; }
		}

		/// <summary>
		/// Gets the positions in the particle buffer of each active particle in the buffer
		/// </summary>
		/// <remarks>
		/// The length of the returned array may not be equal to the number of active particles. Use
		/// <see cref="NumActiveParticles"/> instead.
		/// </remarks>
		public IList<int> ActiveParticleBufferIndexes
		{
			get { return m_ActiveParticleIndexes; }
		}

		/// <summary>
		/// Adds a particle to the buffer. Returns the index of the particle
		/// </summary>
		public int AddParticle( )
		{
			int particleOffset;
			if ( m_FreeParticleIndexes.Count > 0 )
			{
				particleOffset = m_FreeParticleIndexes[ 0 ];
				m_FreeParticleIndexes.RemoveAt( 0 );
			}
			else if ( m_ActiveParticleIndexes.Count > 0 )
			{
				//	No free particles - let's recycle the oldest
				particleOffset = m_ActiveParticleIndexes[ 0 ];
				m_ActiveParticleIndexes.RemoveAt( 0 );
			}
			else
			{
				throw new InvalidOperationException( "Both free particle and active particle lists are empty" );
			}

			m_ActiveParticleIndexes.Add( particleOffset );
			int particleIndex = m_ActiveParticleIndexes.Count - 1;
			InitializeParticle( particleOffset );
			return particleIndex;
		}

		private unsafe void InitializeParticle( int particleOffset )
		{
			fixed ( byte* bufferBytes = m_Buffer )	//	Not necessary, because m_Buffer is pinned, but gets around compiler error
			{
				byte* particleMem = bufferBytes + particleOffset;
				foreach ( FieldInfo field in m_Fields.Values )
				{
					byte* fieldMem = particleMem + field.Offset;
					object[] defaultValues = field.DefaultValues;
					switch ( field.Type )
					{
						case ParticleFieldType.Int32 :
							{
								for ( int i = 0; i < field.NumElements; ++i )
								{
									( ( int* )fieldMem )[ i ] = ( int )defaultValues[ i ];
								}
								break;
							}
						case ParticleFieldType.Float32 :
							{
								for ( int i = 0; i < field.NumElements; ++i )
								{
									( ( float* )fieldMem )[ i ] = ( float )defaultValues[ i ];
								}
								break;
							}
						default :
							{
								throw new NotSupportedException( "Unsupported field type " + field.Type );
							}
					}
				}
			}	//	TODO: AP: Check that the buffer remains pinned after this scope
		}


		/// <summary>
		/// Marks a particle for removal from the buffer. The index of the particle is in the range [0,NumActiveParticles)
		/// </summary>
		public void MarkParticleForRemoval( int particleIndex )
		{
			m_FreeParticleIndexes.Add( m_ActiveParticleIndexes[ particleIndex ] );
			m_ActiveParticleIndexes[ particleIndex ] = -1;
		}

		/// <summary>
		/// Removes all particles from the buffer that were marked by <see cref="MarkParticleForRemoval"/>
		/// </summary>
		public void RemoveMarkedParticles( )
		{
			for ( int index = 0; index < m_ActiveParticleIndexes.Count; )
			{
				if ( m_ActiveParticleIndexes[ index ] == -1 )
				{
					m_ActiveParticleIndexes.RemoveAt( index );
				}
				else
				{
					++index;
				}
			}
		}

		/// <summary>
		/// Adds a field to the particle definition
		/// </summary>
		public void AddField( string name, ParticleFieldType type, int numElements, object defaultValue )
		{
			FieldInfo field;
			if ( m_Fields.TryGetValue( name, out field ) )
			{
				if ( type != field.Type )
				{
					throw new ArgumentException( string.Format( "Field \"{0}\" was already defined using type {1}, not type {2}", name, field.Type, type ), "type" );
				}
				if ( field.NumElements != numElements )
				{
					throw new ArgumentException( string.Format( "Field \"{0}\" was already defined with {1} elements of type {2}, not {3}", name, field.NumElements, type, numElements ), "numElements" );
				}
				return;
			}

			m_BuildBuffer = true;
			field = new FieldInfo( type, numElements, m_Stride, defaultValue );
			m_Fields.Add( name, field );
			m_Stride += field.Size;
		}

		/// <summary>
		/// Pins the buffer, and returns a disposable object that unpins it. This must be done before calling GetField()
		/// </summary>
		public IDisposable Prepare( )
		{
			if ( m_BuildBuffer )
			{
				ResetFreeList( );
				m_Buffer = new byte[ Stride * MaximumNumberOfParticles ];
				m_BuildBuffer = false;
			}

			GCHandle handle = GCHandle.Alloc( m_Buffer, GCHandleType.Pinned );
			return new BufferHandle( handle );
		}

		/// <summary>
		/// Gets a pointer to the first value of the named field stored in the buffer
		/// </summary>
		/// <remarks>
		/// Buffer must be prepared (<see cref="IParticleBuffer.Prepare"/>) prior to calling this method
		/// </remarks>
		public unsafe byte* GetField( string name )
		{
			if ( m_BuildBuffer )
			{
				throw new InvalidOperationException( "Tried to access a field before the buffer was rebuilt" );
			}
			FieldInfo field;
			if ( !m_Fields.TryGetValue( name, out field ) )
			{
				throw new ArgumentException( string.Format( "Field \"{0}\" does not exist - all fields must be specified in the Attach() method of particle system components", name ), "name" );
			}
			byte* result;
			fixed( byte* bufferBytes = m_Buffer )	//	Not necessary, because m_Buffer is pinned, but gets around compiler error
			{
				result = bufferBytes + field.Offset;
			}	//	TODO: AP: Check that the buffer remains pinned after this scope

			return result;
		}
		

		/// <summary>
		/// Gets a pointer to the first value of the named field stored in the buffer
		/// </summary>
		/// <remarks>
		/// Buffer must be prepared (<see cref="IParticleBuffer.Prepare"/>) prior to calling this method
		/// </remarks>
		public unsafe byte* GetField( string name, int particleIndex )
		{
			return GetField( name ) + m_ActiveParticleIndexes[ particleIndex ];
		}

		#endregion

		#region Private Members

		private int m_Stride;
		private int m_MaximumNumberOfParticles;
		private readonly List<int> m_ActiveParticleIndexes = new List<int>( );
		private readonly List<int> m_FreeParticleIndexes = new List<int>( );
		private bool m_BuildBuffer;
		private byte[] m_Buffer;
		private readonly Dictionary<string, FieldInfo> m_Fields = new Dictionary<string, FieldInfo>( );
		
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

		private static int ParticleFieldTypeSize( ParticleFieldType type )
		{
			switch ( type )
			{
				case ParticleFieldType.Int32 : return sizeof( int );
				case ParticleFieldType.Float32 : return sizeof( float );
			}
			throw new NotSupportedException( "Unsupported field type " + type );
		}

		private class FieldInfo
		{
			public FieldInfo( ParticleFieldType type, int numElements, int offset, object defaultValue )
			{
				m_Type = type;
				m_NumElements = numElements;
				m_Size = ParticleFieldTypeSize( type ) * numElements;
				m_Offset = offset;
				m_DefaultValues = new object[ numElements ];

				if ( defaultValue is Array )
				{
					object[] src = ( object[] )defaultValue;
					for ( int i = 0; i < numElements; ++i )
					{
						m_DefaultValues[ i ] = src[ i ];
					}
				}
				else
				{
					m_DefaultValues = new object[ numElements ];
					for ( int i = 0; i < numElements; ++i )
					{
						m_DefaultValues[ i ] = defaultValue;
					}
				}
			}

			public ParticleFieldType Type
			{
				get { return m_Type; }
			}

			public int Size
			{
				get { return m_Size; }
			}

			public int Offset
			{
				get { return m_Offset; }
			}

			public object[] DefaultValues
			{
				get { return m_DefaultValues; }
			}

			public int NumElements
			{
				get { return m_NumElements; }
			}

			private readonly ParticleFieldType m_Type;
			private readonly int m_Size;
			private readonly int m_Offset;
			private readonly int m_NumElements;
			private readonly object[] m_DefaultValues;
		}

		#endregion
		
		private void ResetFreeList( )
		{
			m_FreeParticleIndexes.Capacity = MaximumNumberOfParticles;
			m_ActiveParticleIndexes.Capacity = MaximumNumberOfParticles;

			m_ActiveParticleIndexes.Clear( );
			m_FreeParticleIndexes.Clear( );
			for ( int i = 0; i < m_MaximumNumberOfParticles; ++i )
			{
				m_FreeParticleIndexes.Add( i * m_Stride );
			}
		}

		#endregion

	}
}
