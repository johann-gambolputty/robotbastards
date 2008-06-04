
using Poc1.Particles.Interfaces;

namespace Poc1.Particles.Classes
{
	class ParticleBuffer : IParticleBuffer
	{
		#region IParticleBuffer Members

		public unsafe int Stride
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		public unsafe int MaximumNumberOfParticles
		{
			get
			{
				throw new System.Exception("The method or operation is not implemented.");
			}
			set
			{
				throw new System.Exception("The method or operation is not implemented.");
			}
		}

		public unsafe void AddParticle()
		{
			throw new System.Exception("The method or operation is not implemented.");
		}

		public unsafe void RemoveParticle(int particleIndex)
		{
			throw new System.Exception("The method or operation is not implemented.");
		}

		public unsafe int NumActiveParticles
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		public unsafe int[] ActiveParticleBufferIndexes
		{
			get { throw new System.Exception("The method or operation is not implemented."); }
		}

		public unsafe void AddField(string name, System.Type type, int numElements)
		{
			throw new System.Exception("The method or operation is not implemented.");
		}

		public unsafe System.IDisposable Pin()
		{
			throw new System.Exception("The method or operation is not implemented.");
		}

		public unsafe byte* GetField(string name)
		{
			throw new System.Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}
