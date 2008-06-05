using System.Collections.Generic;
using Poc1.Particles.Interfaces;
using Rb.Core.Maths;

namespace Poc1.Particles.Classes
{
	public unsafe class SerialParticleFieldIterator
	{
		public SerialParticleFieldIterator( ISerialParticleBuffer pBuffer, string fieldName )
		{
			m_Index = 0;
			if ( pBuffer.NumActiveParticles > 0 )
			{
				m_ActiveParticles = pBuffer.ActiveParticleBufferIndexes;
				m_Buffer = pBuffer.GetField( fieldName );
				m_Pos = m_Buffer + m_ActiveParticles[ m_Index ];
			}
			else
			{
				m_ActiveParticles = null;
				m_Buffer = null;
				m_Pos = null;
			}
		}

		public void MoveNext( )
		{
			m_Pos = m_Buffer + m_ActiveParticles[ m_Index++ ];
		}

		public float FloatValue
		{
			get { return *FloatPtr; }
			set { *FloatPtr = value; }
		}

		public int IntValue
		{
			get { return *IntPtr; }
			set { *IntPtr = value; }
		}

		public Point3 Point3Value
		{
			get
			{
				float* src = FloatPtr;
				return new Point3( src[ 0 ], src[ 1 ], src[ 2 ] );
			}
			set
			{
				float* dst = FloatPtr;
				dst[ 0 ] = value.X;
				dst[ 1 ] = value.Y;
				dst[ 2 ] = value.Z;
			}
		}

		public Vector3 Vector3Value
		{
			get
			{
				float* src = FloatPtr;
				return new Vector3( src[ 0 ], src[ 1 ], src[ 2 ] );
			}
			set
			{
				float* dst = FloatPtr;
				dst[ 0 ] = value.X;
				dst[ 1 ] = value.Y;
				dst[ 2 ] = value.Z;
			}
		}

		public int* IntPtr
		{
			get { return ( int* )m_Pos; }
		}

		public float* FloatPtr
		{
			get { return ( float* )m_Pos; }
		}

		private readonly IList<int> m_ActiveParticles;
		private readonly byte* m_Buffer;
		private int m_Index;
		private byte* m_Pos;
	}
}
