
namespace Poc1.Universe.Interfaces
{
	/// <summary>
	/// Body interface
	/// </summary>
	public interface IBody
	{
		/// <summary>
		/// Gets/sets the name of this entity
		/// </summary>
		string Name
		{
			get; set;
		}

		/// <summary>
		/// Gets the transform for this body
		/// </summary>
		UniTransform Transform
		{
			get;
		}
	}
}
