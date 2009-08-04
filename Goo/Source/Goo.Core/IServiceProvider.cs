
namespace Goo.Core
{
	/// <summary>
	/// Interface for objects that can provide services
	/// </summary>
	public interface IServiceProvider
	{
		/// <summary>
		/// Gets all registered services
		/// </summary>
		object[] Services
		{
			get;
		}
	
		/// <summary>
		/// Adds a service object
		/// </summary>
		/// <param name="service">Service object to add</param>
		TService AddService<TService>( TService service );

		/// <summary>
		/// Removes a service object
		/// </summary>
		/// <param name="service">Service object to remove</param>
		void RemoveService( object service );

		/// <summary>
		/// Gets a service belonging to this object by its type
		/// </summary>
		/// <typeparam name="TService">Service type</typeparam>
		/// <returns>Returns a service of the specified type. Returns null if no such service exists.</returns>
		TService GetService<TService>( );

		/// <summary>
		/// Gets a service belonging to this object by its type
		/// </summary>
		/// <typeparam name="TService">Service type</typeparam>
		/// <returns>Returns a service of the specified type. Throws a KeyNotFoundException if no such service exists.</returns>
		TService EnsureGetService<TService>( );
	}
}
