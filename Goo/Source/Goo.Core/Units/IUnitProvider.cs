
namespace Goo.Core.Units
{
	/// <summary>
	/// Loads units
	/// </summary>
	public interface IUnitProvider
	{
		/// <summary>
		/// Loads the named unit
		/// </summary>
		/// <param name="name">Unit name</param>
		/// <returns>Returns the loaded unit</returns>
		IUnit Load( string name );
	}
}
