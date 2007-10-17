
using Rb.Core.Maths;

namespace Poc0.Core.Objects
{
	/// <summary>
	/// Interface for objects that have an oriented
	/// </summary>
	public interface IOriented
	{
		//	TODO: AP: Proper 3D orientation

		/// <summary>
		/// Currrent angle
		/// </summary>
		float Angle
		{
			get; set;
		}

		/// <summary>
		/// Forward vector
		/// </summary>
		Vector3 Forward
		{
			get;
		}

		/// <summary>
		/// Right vector
		/// </summary>
		Vector3 Right
		{
			get;
		}

		/// <summary>
		/// Up vector
		/// </summary>
		Vector3 Up
		{
			get;
		}
	}
}
