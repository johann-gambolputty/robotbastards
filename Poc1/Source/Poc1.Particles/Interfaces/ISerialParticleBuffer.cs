
namespace Poc1.Particles.Interfaces
{
	/// <summary>
	/// A particle buffer interface where particle data is stored for processing in serial
	/// </summary>
	public unsafe interface ISerialParticleBuffer : IParticleBuffer
	{
		/// <summary>
		/// Gets a pointer to the first value of the named field stored in the buffer
		/// </summary>
		/// <remarks>
		/// Buffer must be prepared (<see cref="IParticleBuffer.Prepare"/>) prior to calling this method
		/// </remarks>
		byte* GetField( string name );
		
		/// <summary>
		/// Gets a pointer to the first value of the named field stored in the buffer, for an indexed particle
		/// </summary>
		/// <remarks>
		/// Buffer must be prepared (<see cref="IParticleBuffer.Prepare"/>) prior to calling this method
		/// </remarks>
		byte* GetField( string name, int particleIndex );
	}
}
