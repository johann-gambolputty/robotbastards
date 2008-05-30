using Rb.Core.Maths;

namespace Poc1.Particles.Interfaces
{
	/// <summary>
	/// Base particle interface
	/// </summary>
	public interface IParticle
	{
		/// <summary>
		/// Particle position
		/// </summary>
		Point3 Position
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the age of the particle
		/// </summary>
		int Age
		{
			get; set;
		}
	}
}
