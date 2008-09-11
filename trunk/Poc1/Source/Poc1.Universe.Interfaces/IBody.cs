
namespace Poc1.Universe.Interfaces
{
	/// <summary>
	/// Body interface
	/// </summary>
	public interface IBody
	{
		/// <summary>
		/// Gets the transform for this body
		/// </summary>
		UniTransform Transform
		{
			get;
		}
	}
}
