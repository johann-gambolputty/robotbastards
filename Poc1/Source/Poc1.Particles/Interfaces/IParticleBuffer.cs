using System;

namespace Poc1.Particles.Interfaces
{
	/// <summary>
	/// Interface defining creation and management of dynamic particle fields
	/// </summary>
	public unsafe interface IParticleBuffer
	{
		/// <summary>
		/// Gets the stride, in bytes, between particle field values in the buffer
		/// </summary>
		int Stride
		{
			get;
		}

		/// <summary>
		/// Gets/sets the maximum number of particles. The default for this is 256
		/// </summary>
		int MaximumNumberOfParticles
		{
			get; set;
		}

		/// <summary>
		/// Adds a particle to the buffer. Returns the index of the particle
		/// </summary>
		void AddParticle( );

		/// <summary>
		/// Removes a particle from the buffer. The index of the particle is in the range [0,NumActiveParticles)
		/// </summary>
		void RemoveParticle( int particleIndex );

		/// <summary>
		/// Gets the number of particles in the 
		/// </summary>
		int NumActiveParticles
		{
			get;
		}

		/// <summary>
		/// Gets the positions in the particle buffer of each active particle in the buffer
		/// </summary>
		/// <remarks>
		/// The length of the returned array may not be equal to the number of active particles. Use
		/// <see cref="NumActiveParticles"/> instead.
		/// </remarks>
		int[] ActiveParticleBufferIndexes
		{
			get;
		}

		/// <summary>
		/// Adds a field to the particle definition
		/// </summary>
		void AddField( string name, Type type, int numElements );

		/// <summary>
		/// Pins the buffer, and returns a disposable object that unpins it. This must be done before calling GetField()
		/// </summary>
		IDisposable Pin( );

		/// <summary>
		/// Gets a pointer to the first value of the named field stored in the buffer
		/// </summary>
		byte* GetField( string name );
	}
}
