
using Goo.Core.Mvc;

namespace Goo.Core.Environment
{
	/// <summary>
	/// Maintains a registry of all available controllers
	/// </summary>
	public interface IControllerFactoryRegistry
	{
		/// <summary>
		/// Gets all controller factories
		/// </summary>
		IControllerFactory[] Factories
		{
			get;
		}

		/// <summary>
		/// Registers a the specified controller factory
		/// </summary>
		/// <param name="controllerFactory">Controller factory</param>
		void RegisterControllerFactory( IControllerFactory controllerFactory );

		/// <summary>
		/// Unregisters the specified controller factory
		/// </summary>
		void UnregisterControllerFactory( IControllerFactory controllerFactory );
	}
}
