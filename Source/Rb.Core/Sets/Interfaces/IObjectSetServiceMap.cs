
namespace Rb.Core.Sets.Interfaces
{
	/// <summary>
	/// Stores a set of services, accessible by type
	/// </summary>
	public interface IObjectSetServiceMap
	{
		/// <summary>
		/// Adds a service to this set
		/// </summary>
		/// <param name="service">Service to add</param>
		/// <exception cref="System.ArgumentNullException">Thrown if service is null</exception>
		void AddService( IObjectSetService service );

		/// <summary>
		/// Removes a service from this set
		/// </summary>
		/// <param name="service">Service to remove</param>
		/// <exception cref="System.ArgumentNullException">Thrown if service is null</exception>
		void RemoveService( IObjectSetService service );

		/// <summary>
		/// Gets a service attached to this set, by its type. Throws an ArgumentException if the service does not exist
		/// </summary>
		/// <typeparam name="T">Type of service to retrieve</typeparam>
		/// <returns>Returns the typed service</returns>
		/// <exception cref="System.ArgumentException">Thrown if T is not a valid service</exception>
		T SafeService<T>( ) where T : class;

		/// <summary>
		/// Gets a service attached to this set, by its type
		/// </summary>
		/// <typeparam name="T">Type of service to retrieve</typeparam>
		/// <returns>Returns the typed service, or null if no such service exists</returns>
		T Service<T>( ) where T : class;
	}
}
