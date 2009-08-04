
namespace Goo.Core.Mvc
{
	/// <summary>
	/// Controller factory
	/// </summary>
	public interface IControllerFactory
	{
		/// <summary>
		/// Creates a controller and its view
		/// </summary>
		IController Create( ControllerInitializationContext initContext );
	}
}
