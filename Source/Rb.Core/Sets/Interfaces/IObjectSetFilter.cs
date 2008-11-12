
namespace Rb.Core.Sets.Interfaces
{
	/// <summary>
	/// Object set filter
	/// </summary>
	public interface IObjectSetFilter
	{
		/// <summary>
		/// Filters the objects in a set
		/// </summary>
		/// <param name="set">Set to filter</param>
		/// <param name="resultSet">Set to store filtered objects</param>
		void Filter( IObjectSet set, IObjectSet resultSet );
	}
}
