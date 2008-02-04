
namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// If an object is selected that supports this interface, the SelectedObject property is selected instead.
	/// </summary>
	/// <remarks>
	/// This is for modifier objects, like position editors. When a modifier object is selected, what should
	/// actually be selected is the modifier owner.
	/// See <see cref="SelectionSet.ApplySelect(object, bool)"/> for use of this interface
	/// </remarks>
	public interface ISelectionModifier
	{
		/// <summary>
		/// Gets the object to be selected
		/// </summary>
		object SelectedObject
		{
			get;
		}
	}
}
