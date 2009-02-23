
namespace Rb.Core.Components
{
    /// <summary>
    /// Child object interface
    /// </summary>
    public interface IComponent
	{
		/// <summary>
		/// Gets/sets the composite object that contains this component
		/// </summary>
		/// <remarks>
		/// If the owner is set, the component should be added to the component
		/// list of the specified composite, and removed from the component list
		/// of the previous owner.
		/// </remarks>
		IComposite Owner
		{
			get; set;
		}
    }
}
