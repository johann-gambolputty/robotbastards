
using Rb.Core.Maths;

namespace Poc1.Particles.Interfaces
{
	/// <summary>
	/// Sets up spawn states for new particles
	/// </summary>
	public interface ISpawnStateGenerator
	{


		/// <summary>
		/// Generates the next spawn state
		/// </summary>
		void Next( );

		/// <summary>
		/// Gets the initial position of the current particle
		/// </summary>
		Point3 InitialPosition
		{
			get;
		}

		/// <summary>
		/// Gets the initial direction of the current particle
		/// </summary>
		Vector3 InitialDirection
		{
			get;
		}

		/// <summary>
		/// Gets the initial age of the current particle
		/// </summary>
		int InitialAge
		{
			get;
		}

		/// <summary>
		/// Gets the initial size of the current particle
		/// </summary>
		int InitialSize
		{
			get;
		}
	}
}
