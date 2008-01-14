
namespace Rb.Rendering.Contracts.Objects
{
	/// <summary>
	/// A data source for effect parameters
	/// </summary>
	public interface IEffectDataSource
	{
		/// <summary>
		/// Gets/sets the value that will be transferred to effect parameters
		/// </summary>
		object Value
		{
			get; set;
		}
	}
}
