using System;
using System.Collections.Generic;

namespace Poc1.Particles.Interfaces
{
	public enum ParticleFieldType
	{
		Int32,
		Float32
	}

	/// <summary>
	/// Interface defining creation and management of dynamic particle fields
	/// </summary>
	public interface IParticleBuffer
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
		IList<int> ActiveParticleBufferIndexes
		{
			get;
		}

		/// <summary>
		/// Adds a particle to the buffer. Returns the index of the particle
		/// </summary>
		int AddParticle( );


		/// <summary>
		/// Prepares the buffer prior to update. Returns an optional disposable object that can be used to unprepare the buffer post-update
		/// </summary>
		IDisposable Prepare( );

		/// <summary>
		/// Marks a particle for removal from the buffer. The index of the particle is in the range [0,NumActiveParticles)
		/// </summary>
		void MarkParticleForRemoval( int particleIndex );

		/// <summary>
		/// Removes all particles from the buffer that were marked by <see cref="MarkParticleForRemoval"/>
		/// </summary>
		void RemoveMarkedParticles( );

		/// <summary>
		/// Adds a field to the particle definition
		/// </summary>
		void AddField( string name, ParticleFieldType type, int numElements, object defaultValue );

	}
}
