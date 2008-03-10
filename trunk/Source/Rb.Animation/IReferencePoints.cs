
namespace Rb.Animation
{
	/// <summary>
	/// Manages a set of reference points
	/// </summary>
	public interface IReferencePoints
	{
		/// <summary>
		/// Gets a named reference point
		/// </summary>
		/// <param name="name">The reference point name to look up</param>
		/// <returns>Returns the named reference point</returns>
		/// <exception cref="System.ArgumentException">Thrown if the named reference point does not exist</exception>
		IReferencePoint this[ string name ]
		{
			get;
		}
	}
}
